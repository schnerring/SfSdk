using System;
using System.Collections.Generic;
using System.Globalization;
using SfSdk.Constants;
using SfSdk.Logging;

namespace SfSdk.Providers
{
    internal class ItemProvider
    {
        private static readonly ILog Log = LogManager.GetLog(typeof (ItemProvider));

        private readonly Uri _imageServerUri;
        private readonly Dictionary<SF, string> _languageResourceDict;
        private readonly Dictionary<int, string> _urlDict = new Dictionary<int, string>();

        public ItemProvider(Uri serverUri)
        {
            if (serverUri == null) throw new ArgumentNullException("serverUri");

            Log.Info("Resource download started.");
            var configurationResources = new ConfigurationResourceProvider().GetResources(serverUri);
            _imageServerUri = new Uri(configurationResources[SF.CfgImgUrl]);
            _languageResourceDict = new LanguageResourceProvider().GetResources(serverUri);
            Log.Info("Resource download finished.");

            bool paramCensored = false;
            if (configurationResources.ContainsKey(SF.CfgCensored))
                paramCensored = int.Parse(configurationResources[SF.CfgCensored]) != 0;

            DefineMonsters(paramCensored);
            DefineItems();
        }

        public int GetItemId(int itemType, int itemPic, int itemColor, int itemClass = -1)
        {
            var itemId = (int)SF.ItmOffs;
            itemId += itemType * (int)SF.CItemsPerType * 5 * 3;
            itemId += itemPic * 5 * 3;
            itemId += itemColor * 3;
            itemId += itemClass;

            if (itemId < (int)SF.ItmMax) return itemId;

            Log.Warn("Error: not enough indices for items: {0} >= {1} Type: {2} Pic: {3} Color: {4} Class: {5}",
                itemId, SF.ItmMax, itemType, itemPic, itemColor, itemClass);
            return 0;
        }

        public string GetItemName(int sgIndex, int sg, int albumMode = -1)
        {
            var itemPic = 0;
            var itemTyp = 0;
            var itemClass = 1;

            var txtBase = 0;
            var txtSuffix = string.Empty;

            if (albumMode >= 0)
            {
                itemTyp = sgIndex;
                itemPic = sg;
                itemClass = albumMode;
            }

            if (itemTyp >= 8)
            {
                switch (itemTyp)
                {
                    case 8:
                        txtBase = (int)SF.TxtItmname8;
                        break;
                    case 9:
                        txtBase = (int)SF.TxtItmname9;
                        break;
                    case 10:
                        txtBase = (int)SF.TxtItmname10;
                        break;
                    case 11:
                        txtBase = (int)SF.TxtItmname11;
                        txtSuffix = string.Empty;
                        break;
                    case 12:
                        txtBase = (int)SF.TxtItmname12;
                        break;
                    case 13:
                        txtBase = (int)SF.TxtItmname13;
                        break;
                    case 14:
                        txtBase = (int)SF.TxtItmname14;
                        break;
                }
            }
            else
            {
                var itemOffs = 0;
                switch (itemTyp)
                {
                    case 1:
                        itemOffs = (int)SF.TxtItmname1_1;
                        break;
                    case 2:
                        itemOffs = (int)SF.TxtItmname2_1;
                        break;
                    case 3:
                        itemOffs = (int)SF.TxtItmname3_1;
                        break;
                    case 4:
                        itemOffs = (int)SF.TxtItmname4_1;
                        break;
                    case 5:
                        itemOffs = (int)SF.TxtItmname5_1;
                        break;
                    case 6:
                        itemOffs = (int)SF.TxtItmname6_1;
                        break;
                    case 7:
                        itemOffs = (int)SF.TxtItmname7_1;
                        break;
                }
                if (itemOffs > 0)
                {
                    itemOffs = (itemOffs - (int)SF.TxtItmname1_1);
                    switch (itemClass)
                    {
                        case 1:
                            txtBase = (int)SF.TxtItmname1_1 + itemOffs;
                            break;
                        case 2:
                            txtBase = (int)SF.TxtItmname1_2 + itemOffs;
                            break;
                        case 3:
                            txtBase = (int)SF.TxtItmname1_3 + itemOffs;
                            break;
                    }
                }
            }

            if (itemPic >= 50 && itemTyp != 14)
            {
                txtBase = txtBase + SF.TxtItmname1_1Epic - SF.TxtItmname1_1;
                itemPic -= 49;
                txtSuffix = string.Empty;
            }

            if (!_languageResourceDict.ContainsKey((SF)txtBase + itemPic - 1))
            {
                return "Unknown Item (base=" + txtBase + ", entry=" + (txtBase + itemPic - 1) + ")";
            }

            if (_languageResourceDict.ContainsKey(SF.TxtItmnameExt))
            {
                if (_languageResourceDict[SF.TxtItmnameExt] == "1")
                {
                    return (string.IsNullOrEmpty(txtSuffix) ? string.Empty : txtSuffix + " ") +
                           _languageResourceDict[(SF)txtBase + itemPic - 1];
                }

                if (_languageResourceDict[SF.TxtItmnameExt] != "2")
                    return _languageResourceDict[(SF)txtBase + itemPic - 1] +
                           (string.IsNullOrEmpty(txtSuffix) ? string.Empty : " " + txtSuffix);
            }

            if (string.IsNullOrEmpty(txtSuffix))
                return _languageResourceDict[(SF)txtBase + itemPic - 1];

            var parts = txtSuffix.Split(new[] { "%1" }, StringSplitOptions.None);
            return string.Join(_languageResourceDict[(SF)txtBase + itemPic - 1], parts);
        }

        public string GetMonsterItemName(SF index)
        {
            return _languageResourceDict[index];
        }

        public Uri GetImageUri(int contentId, int imageId)
        {
            return !_urlDict.ContainsKey(imageId)
                ? null
                : new Uri(_imageServerUri, _urlDict[imageId]);
        }

        private string GetItemFile(int itemType, int itemPic, int itemColor, int itemClass)
        {
            var itemFile = "itm";
            if (itemPic >= 50 && itemType != 14)
            {
                itemColor = 0;
            }

            itemFile += itemType + "-";
            itemFile += itemPic;

            if ((itemType >= 3 && itemType <= 7) || itemType == 1 || itemType == 2)
            {
                itemFile = itemType + "-" + (itemClass + 1) + "/" + itemFile + "-";
                itemFile += (itemColor + 1);
                itemFile += "-" + (itemClass + 1);
            }
            else if (itemType == 8 || itemType == 9 || itemType == 10 || itemType == 11 || itemType == 12 || itemType == 13 || itemType == 14)
            {
                itemFile = itemType + "-1/" + itemFile + "-";
                if (itemType < 10)
                {
                    itemFile += (itemColor + 1) + "-";
                }
                itemFile += "1";
            }
            return "res/gfx/itm/" + itemFile + ".png";
        }

        private int GetArrowId(int itemClass, int itemPic, int itemColor)
        {
            var arrowId = (int) SF.ArrowOffs;

            arrowId += itemClass * 5 * 100;
            arrowId += itemPic * 5;
            arrowId += itemColor;

            if (arrowId < (int) SF.ArrowMax) return arrowId;

            Log.Warn("Error: not enough indices for arrows: {0} >= {1} Pic: {2} Color: {3} Class: {4}",
                arrowId, SF.ItmMax, itemPic, itemColor, itemClass);
            return 0;
        }

        private void DefineMonsters(bool paramCensored)
        {
            DefineImage(SF.ImgUnknownEnemy, "res/gfx/scr/fight/monster/unknown.jpg");

            var k = 0;
            while (k < 500)
            {
                var i = k;

                if (paramCensored)
                {
                    if (i >= 66 && i <= 68) i = 69;
                    if (i == 73) i = 128;
                    if (i >= 117 && i <= 118) i = 69;
                }

                if (i >= 399)
                {
                    var monsterChecksum = (i.ToString(CultureInfo.InvariantCulture) + "ScriptKiddieLovesToPeek").ToMd5Hash();
                    DefineImage(SF.ImgOppimgMonster + k, "res/gfx/scr/fight/monster/monster" + monsterChecksum + ".jpg");
                }
                else
                {
                    DefineImage(SF.ImgOppimgMonster + k, "res/gfx/scr/fight/monster/monster" + (i + 1).ToString(CultureInfo.InvariantCulture) + ".jpg");
                }

                ++k;
            }
        }

        private void DefineItems()
        {
            DefineImage(SF.ImgNoShield, "res/gfx/scr/fight/monster/unknown.jpg");

            var itemType = 0;
            while (itemType <= 14)
            {
                var itemPic = 0;
                while (itemPic < (int)SF.CItemsPerType)
                {
                    var itemColor = 0;
                    while (itemColor < 5)
                    {
                        switch (itemType)
                        {
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                                var itemClass = 0;
                                while (itemClass < 3)
                                {
                                    DefineImage((SF)GetItemId(itemType, itemPic, itemColor, itemClass), GetItemFile(itemType, itemPic, itemColor, itemClass));
                                    ++itemClass;
                                }
                                break;
                            default:
                                DefineImage((SF)GetItemId(itemType, itemPic, itemColor, 0), GetItemFile(itemType, itemPic, itemColor, 0));
                                break;
                        }
                        ++itemColor;
                    }
                    ++itemPic;
                }
                ++itemType;
            }

            itemType = 0;
            while (itemType <= 1)
            {
                var itemPic = 0;
                while (itemPic < (int)SF.CItemsPerType)
                {
                    var itemColor = 0;
                    while (itemColor < 5)
                    {
                        DefineImage((SF)GetArrowId(itemType, itemPic, itemColor), ("res/gfx/itm/1-" + (itemType + 2) + "/shot" + (itemType == 0 ? 2 : 1) + "-" + itemPic + "-" + ((itemPic >= 50 ? (itemType == 0 ? (itemColor == 3 ? 3 : 0) : 0) : itemColor) + 1) + ".png"));
                        ++itemColor;
                    }
                    ++itemPic;
                }
                ++itemType;
            }

            DefineImage(SF.ImgWeaponFist, "res/gfx/itm/kampf_faust.png");
            DefineImage(SF.ImgWeaponStonefist, "res/gfx/itm/kampf_steinfaust.png");
            DefineImage(SF.ImgWeaponBone, "res/gfx/itm/kampf_knochen.png");
            DefineImage(SF.ImgWeaponStick, "res/gfx/itm/kampf_stock.png");
            DefineImage(SF.ImgWeaponClaw, "res/gfx/itm/kampf_kralle1.png");
            DefineImage(SF.ImgWeaponClaw2, "res/gfx/itm/kampf_kralle2.png");
            DefineImage(SF.ImgWeaponClaw3, "res/gfx/itm/kampf_kralle3.png");
            DefineImage(SF.ImgWeaponClaw4, "res/gfx/itm/kampf_kralle4.png");
            DefineImage(SF.ImgWeaponSwoosh, "res/gfx/itm/kampf_swoosh1.png");
            DefineImage(SF.ImgWeaponSwoosh2, "res/gfx/itm/kampf_swoosh2.png");
            DefineImage(SF.ImgWeaponSwoosh3, "res/gfx/itm/kampf_swoosh3.png");
            DefineImage(SF.ImgWeaponSplat, "res/gfx/itm/kampf_splat1.png");
            DefineImage(SF.ImgWeaponSplat2, "res/gfx/itm/kampf_splat2.png");
            DefineImage(SF.ImgWeaponSplat3, "res/gfx/itm/kampf_splat3.png");
            DefineImage(SF.ImgWeaponFire, "res/gfx/itm/kampf_feuer1.png");
            DefineImage(SF.ImgWeaponFire2, "res/gfx/itm/kampf_feuer2.png");
            DefineImage(SF.ImgWeaponFire3, "res/gfx/itm/kampf_feuer3.png");
        }

        private void DefineImage(SF actorId, string url)
        {
            if (!_urlDict.ContainsKey((int) actorId))
            {
                _urlDict.Add((int) actorId, url);
                return;
            }
            Log.Warn("Image with the same URL already added!");

        }
    }
}
