﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SfSdk.Constants;
using SfSdk.Contracts;
using SfSdk.Data;
using SfSdk.Logging;
using SfSdk.Response;

namespace SfSdk.Providers
{
    /// <summary>
    ///     ItemProvider for S&amp;F items.
    /// </summary>
    internal class ItemProvider
    {
        private static readonly ILog Log = LogManager.GetLog(typeof (ItemProvider));

        private readonly Uri _imageServerUri;
        private readonly Dictionary<SF, string> _languageDict;
        private readonly Dictionary<int, string> _urlDict;

        private static readonly Dictionary<Uri, Dictionary<int, string>> UrlDicts = new Dictionary<Uri, Dictionary<int, string>>();
        private static readonly Dictionary<Uri, Dictionary<SF, string>> ConfigurationDicts = new Dictionary<Uri, Dictionary<SF, string>>();
        private static readonly Dictionary<Uri, Dictionary<SF, string>> LanguageDicts = new Dictionary<Uri, Dictionary<SF, string>>();

        /// <summary>
        ///     Creates a new instance of type <see cref="ItemProvider" /> and defines its resources.
        /// </summary>
        /// <param name="serverUri">The server <see cref="Uri" />.</param>
        /// <exception cref="ArgumentNullException">When serverUri is null.</exception>
        public ItemProvider(Uri serverUri)
        {
            if (serverUri == null) throw new ArgumentNullException("serverUri");

            Log.Info("Resource download started.");

            if (!ConfigurationDicts.ContainsKey(serverUri))
                ConfigurationDicts.Add(serverUri, new ConfigurationResourceProvider().GetResources(serverUri));
            var configurationDict = ConfigurationDicts[serverUri];

            if (!LanguageDicts.ContainsKey(serverUri))
                LanguageDicts.Add(serverUri, new LanguageResourceProvider().GetResources(serverUri));
            _languageDict = LanguageDicts[serverUri]; 

            Log.Info("Resource download finished.");
            
            _imageServerUri = new Uri(configurationDict[SF.CfgImgUrl]);

            if (UrlDicts.ContainsKey(serverUri))
            {
                _urlDict = UrlDicts[serverUri];
                return;
            }

            _urlDict = new Dictionary<int, string>();
            UrlDicts.Add(serverUri, _urlDict);

            var paramCensored = false;
            if (configurationDict.ContainsKey(SF.CfgCensored))
                paramCensored = int.Parse(configurationDict[SF.CfgCensored]) != 0;

            DefineMonsters(paramCensored);
            DefineItems();
        }

        /// <summary>
        ///     Creates monster items.
        /// </summary>
        /// <param name="scrapbookContent">The scrapbook's content.</param>
        /// <returns>The scrapbook's monster items.</returns>
        public IEnumerable<IScrapbookItem> CreateMonsterItems(List<int> scrapbookContent)
        {
            var result = new List<IScrapbookItem>();

            for (int page = 0; page <= 62; ++page)
                for (int i = 0; i < 4; ++i)
                {
                    var monsterItem = new MonsterItem
                    {
                        Id = (int) SF.ImgOppimgMonster + page*4 + i,
                        ContentId = (int) SF.CntAlbumMonster + i,
                        HasItem = scrapbookContent[(page*4) + i] == 1,
                        Text = page*4 + i >= 220
                            ? _languageDict[SF.TxtNewMonsterNames + page*4 + i - 220]
                            : _languageDict[SF.TxtMonsterName + page*4 + i]
                    };
                    monsterItem.ImageUri = GetImageUri(monsterItem.Id);
                    result.Add(monsterItem);
                }

            return result;
        }

        /// <summary>
        ///     Creates valuable items.
        /// </summary>
        /// <param name="scrapbookContent">The scrapbook's content.</param>
        /// <returns>The scrapbook's valuable items.</returns>
        public IEnumerable<IScrapbookItem> CreateValuableItems(List<int> scrapbookContent)
        {
            var result = new List<IScrapbookItem>();

            for (int page = 0; page <= 25; ++page)
            {
                for (int i = 0; i < 4; ++i)
                {
                    var itemsToAdd = new List<IScrapbookItem>();

                    if (page <= 5)
                        if (page < 5 || i <= 0)
                            itemsToAdd.AddRange(CreateMultipleItems<ValuableItem>(scrapbookContent, i, 300 + page*20 + i*5,
                                8, 1 + page*4 + i, 0));
                        else continue;
                    else if (page <= 7)
                        itemsToAdd.Add(CreateEpicItem<ValuableItem>(scrapbookContent, i, 510 + (page - 6)*4 + i, 8,
                            50 + (page - 6)*4 + i, 0));
                    else if (page <= 11)
                        itemsToAdd.AddRange(CreateMultipleItems<ValuableItem>(scrapbookContent, i, 526 + (page - 8)*20 + i*5,
                            9, 1 + (page - 8)*4 + i, 0));
                    else if (page <= 13)
                        itemsToAdd.Add(CreateEpicItem<ValuableItem>(scrapbookContent, i, 686 + (page - 12)*4 + i, 9,
                            50 + (page - 12)*4 + i, 0));
                    else if (page <= 23)
                        if (page < 23 || i <= 0)
                            itemsToAdd.Add(CreateEpicItem<ValuableItem>(scrapbookContent, i, 702 + (page - 14)*4 + i, 10,
                                1 + (page - 14)*4 + i, 0));
                        else continue;
                    else if (page <= 25)
                        itemsToAdd.Add(CreateEpicItem<ValuableItem>(scrapbookContent, i, 760 + 16 + (page - 24)*4 + i, 10,
                            50 + (page - 24)*4 + i, 0));

                    result.AddRange(itemsToAdd);
                }
            }

            return result;
        }

        /// <summary>
        ///     Creates warrior items.
        /// </summary>
        /// <param name="scrapbookContent">The scrapbook's content.</param>
        /// <returns>The scrapbook's warrior items.</returns>
        public IEnumerable<IScrapbookItem> CreateWarriorItems(List<int> scrapbookContent)
        {
            var result = new List<IScrapbookItem>();

            for (int page = 0; page <= 39; ++page)
            {
                for (int i = 0; i < 4; ++i)
                {
                    var itemsToAdd = new List<IScrapbookItem>();

                    if (page <= 7)
                        if (page < 7 || i <= 1)
                            itemsToAdd.AddRange(CreateMultipleItems<WarriorItem>(scrapbookContent, i,
                                776 + 16 + page*20 + i*5, 1, 1 + page*4 + i, 1));
                        else continue;
                    else if (page <= 9)
                        itemsToAdd.Add(CreateEpicItem<WarriorItem>(scrapbookContent, i, 1076 + 16 + (page - 8)*4 + i, 1,
                            50 + (page - 8)*4 + i, 1));
                    else if (page <= 12)
                        if (page < 12 || i <= 1)
                            itemsToAdd.AddRange(CreateMultipleItems<WarriorItem>(scrapbookContent, i,
                                1092 + 16 + (page - 10)*20 + i*5, 2, 1 + (page - 10)*4 + i, 1));
                        else continue;
                    else if (page <= 14)
                        itemsToAdd.Add(CreateEpicItem<WarriorItem>(scrapbookContent, i, 1192 + 16 + (page - 13)*4 + i, 2,
                            50 + (page - 13)*4 + i, 1));
                    else if (page <= 17)
                        if (page < 17 || i <= 1)
                            itemsToAdd.AddRange(CreateMultipleItems<WarriorItem>(scrapbookContent, i,
                                1208 + 16 + (page - 15)*20 + i*5, 3, 1 + (page - 15)*4 + i, 1));
                        else continue;
                    else if (page <= 19)
                        itemsToAdd.Add(CreateEpicItem<WarriorItem>(scrapbookContent, i, 1308 + 16 + (page - 18)*4 + i, 3,
                            50 + (page - 18)*4 + i, 1));
                    else if (page <= 22)
                        if (page < 22 || i <= 1)
                            itemsToAdd.AddRange(CreateMultipleItems<WarriorItem>(scrapbookContent, i,
                                1324 + 16 + (page - 20)*20 + i*5, 4, 1 + (page - 20)*4 + i, 1));
                        else continue;
                    else if (page <= 24)
                        itemsToAdd.Add(CreateEpicItem<WarriorItem>(scrapbookContent, i, 1424 + 16 + (page - 23)*4 + i, 4,
                            50 + (page - 23)*4 + i, 1));
                    else if (page <= 27)
                        if (page < 27 || i <= 1)
                            itemsToAdd.AddRange(CreateMultipleItems<WarriorItem>(scrapbookContent, i,
                                1440 + 16 + (page - 25)*20 + i*5, 5, 1 + (page - 25)*4 + i, 1));
                        else continue;
                    else if (page <= 29)
                        itemsToAdd.Add(CreateEpicItem<WarriorItem>(scrapbookContent, i, 1540 + 16 + (page - 28)*4 + i, 5,
                            50 + (page - 28)*4 + i, 1));
                    else if (page <= 32)
                        if (page < 32 || i <= 1)
                            itemsToAdd.AddRange(CreateMultipleItems<WarriorItem>(scrapbookContent, i,
                                1556 + 16 + (page - 30)*20 + (i*5), 6, 1 + (page - 30)*4 + i, 1));
                        else continue;
                    else if (page <= 34)
                        itemsToAdd.Add(CreateEpicItem<WarriorItem>(scrapbookContent, i, 1656 + 16 + (page - 33)*4 + i, 6,
                            50 + (page - 33)*4 + i, 1));
                    else if (page <= 37)
                        if (page < 37 || i <= 1)
                            itemsToAdd.AddRange(CreateMultipleItems<WarriorItem>(scrapbookContent, i,
                                1672 + 16 + (page - 35)*20 + i*5, 7, 1 + (page - 35)*4 + i, 1));
                        else continue;
                    else if (page <= 39)
                        itemsToAdd.Add(CreateEpicItem<WarriorItem>(scrapbookContent, i, 1772 + 16 + (page - 38)*4 + i, 7,
                            50 + (page - 38)*4 + i, 1));

                    result.AddRange(itemsToAdd);
                }
            }

            return result;
        }

        /// <summary>
        ///     Creates mage or scout items.
        /// </summary>
        /// <param name="scrapbookContent">The scrapbook's content.</param>
        /// <typeparam name="TScrapbookItem">A concrete type of IMageItem or IScoutItem.</typeparam>
        /// <returns>The scrapbook's mage or scout items.</returns>
        /// <exception cref="ArgumentException">
        ///     When TScrapbookItem is not of type <see cref="IMageItem" /> or
        ///     <see cref="IScoutItem" />.
        /// </exception>
        public IEnumerable<IScrapbookItem> CreateMageOrScoutItems<TScrapbookItem>(List<int> scrapbookContent)
            where TScrapbookItem : new()
        {
            var result = new List<IScrapbookItem>();

            var isMageOrScoutType =
                typeof (TScrapbookItem).GetInterfaces().Any(i => i == typeof (IMageItem) || i == typeof (IScoutItem));

            if (!isMageOrScoutType)
                throw new ArgumentException("TScrapbookItem be of type MageItem or ScoutItem.");

            int hunterOffs = 0;
            int scrapbookCat = 3;
            if (typeof (TScrapbookItem) == typeof (ScoutItem))
            {
                hunterOffs = 696 + 16;
                scrapbookCat = 4;
            }

            for (int page = 0; page <= 29; ++page)
            {
                for (int i = 0; i < 4; ++i)
                {
                    var itemsToAdd = new List<IScrapbookItem>();

                    if (page <= 2)
                        if (page < 2 || i <= 1)
                            itemsToAdd.AddRange(CreateMultipleItems<TScrapbookItem>(scrapbookContent, i,
                                1788 + hunterOffs + page*20 + i*5, 1, 1 + page*4 + i, scrapbookCat - 1));
                        else continue;
                    else if (page <= 4)
                        itemsToAdd.Add(CreateEpicItem<TScrapbookItem>(scrapbookContent, i,
                            1888 + hunterOffs + (page - 3)*4 + i, 1, 50 + (page - 3)*4 + i, scrapbookCat - 1));
                    else if (page <= 7)
                        if (page < 7 || i <= 1)
                            itemsToAdd.AddRange(CreateMultipleItems<TScrapbookItem>(scrapbookContent, i,
                                1904 + hunterOffs + (page - 5)*20 + i*5, 3, 1 + (page - 5)*4 + i, scrapbookCat - 1));
                        else continue;
                    else if (page <= 9)
                        itemsToAdd.Add(CreateEpicItem<TScrapbookItem>(scrapbookContent, i,
                            2004 + hunterOffs + (page - 8)*4 + i, 3, 50 + (page - 8)*4 + i, scrapbookCat - 1));
                    else if (page <= 12)
                        if (page < 12 || i <= 1)
                            itemsToAdd.AddRange(CreateMultipleItems<TScrapbookItem>(scrapbookContent, i,
                                2020 + hunterOffs + (page - 10)*20 + i*5, 4, 1 + (page - 10)*4 + i, scrapbookCat - 1));
                        else continue;
                    else if (page <= 14)
                        itemsToAdd.Add(CreateEpicItem<TScrapbookItem>(scrapbookContent, i,
                            2120 + hunterOffs + (page - 13)*4 + i, 4, 50 + (page - 13)*4 + i, scrapbookCat - 1));
                    else if (page <= 17)
                        if (page < 17 || i <= 1)
                            itemsToAdd.AddRange(CreateMultipleItems<TScrapbookItem>(scrapbookContent, i,
                                2136 + hunterOffs + (page - 15)*20 + i*5, 5, 1 + (page - 15)*4 + i, scrapbookCat - 1));
                        else continue;
                    else if (page <= 19)
                        itemsToAdd.Add(CreateEpicItem<TScrapbookItem>(scrapbookContent, i,
                            2236 + hunterOffs + (page - 18)*4 + i, 5, 50 + (page - 18)*4 + i, scrapbookCat - 1));
                    else if (page <= 22)
                        if (page < 22 || i <= 1)
                            itemsToAdd.AddRange(CreateMultipleItems<TScrapbookItem>(scrapbookContent, i,
                                2252 + hunterOffs + (page - 20)*20 + i*5, 6, 1 + (page - 20)*4 + i, scrapbookCat - 1));
                        else continue;
                    else if (page <= 24)
                        itemsToAdd.Add(CreateEpicItem<TScrapbookItem>(scrapbookContent, i,
                            2352 + hunterOffs + (page - 23)*4 + i, 6, 50 + (page - 23)*4 + i, scrapbookCat - 1));
                    else if (page <= 27)
                        if (page < 27 || i <= 1)
                            itemsToAdd.AddRange(CreateMultipleItems<TScrapbookItem>(scrapbookContent, i,
                                2368 + hunterOffs + (page - 25)*20 + i*5, 7, 1 + (page - 25)*4 + i, scrapbookCat - 1));
                        else continue;
                    else if (page <= 29)
                        itemsToAdd.Add(CreateEpicItem<TScrapbookItem>(scrapbookContent, i,
                            2468 + hunterOffs + (page - 28)*4 + i, 7, 50 + (page - 28)*4 + i, scrapbookCat - 1));

                    result.AddRange(itemsToAdd);
                }
            }

            return result;
        }

        private static int GetItemId(int itemType, int itemPic, int itemColor, int itemClass)
        {
            var itemId = (int) SF.ItmOffs;

            itemId += itemType*(int) SF.CItemsPerType*5*3;
            itemId += itemPic*5*3;
            itemId += itemColor*3;
            itemId += itemClass;

            if (itemId < (int) SF.ItmMax) return itemId;

            Log.Warn("Error: not enough indices for items: {0} >= {1} Type: {2} Pic: {3} Color: {4} Class: {5}",
                itemId, SF.ItmMax, itemType, itemPic, itemColor, itemClass);
            return 0;
        }

        /// <summary>
        ///     Creates inventories based on a <see cref="ISavegame"/>.
        /// </summary>
        /// <param name="sg">The savegame.</param>
        /// <returns>A list of <see cref="IInventoryItem"/>s contained in the savegame.</returns>
        public IInventory CreateInventory(ISavegame sg)
        {
            var result = new Inventory();

            var tmpItemPic = sg.GetValue((int)SF.SgInventoryOffs + (int)SF.SgItmSize * 8 + (int)SF.SgItmPic);
            var tmpItemClass = 0;

            while (tmpItemPic >= 1000)
            {
                tmpItemPic -= 1000;
                ++tmpItemClass;
            }

            for (int i = 0; i < 15; ++i)
            {
                var index = (int)SF.SgInventoryOffs + (int)SF.SgItmSize * i;
                    if (sg.GetValue(index + (int)SF.SgItmTyp) == 0)
                        sg.SetValue(index + (int)SF.SgItmPic, 0);

                var item = new InventoryItem {ContentId = (int) SF.CntCharSlot1 + i};

                if (i == 9 && tmpItemClass >= 1)
                {
                    item.Id = GetArrowIdFromSavegame((int) SF.SgInventoryOffs, 8, sg, tmpItemClass == 1 ? 1 : -1);
                    item.ImageUri = GetImageUri(item.Id);
                }
                else
                {
                    item.Id = GetItemIdFromSavegame((int) SF.SgInventoryOffs, i, sg);
                    item.ImageUri = GetImageUri(item.Id);
                }

                result.AllItems.Add(item);
            }

            return result;
        }

        private static int GetItemIdFromSavegame(int itemType, int itemPic, ISavegame sg)
        {
//            var itemClass = -2;
//            var noShield = false;

            var slotId = itemType + itemPic * (int)SF.SgItmSize;
            var slotNum = itemPic + 1;

            itemType = sg.GetValue(slotId + SF.SgItmTyp);
            itemPic = sg.GetValue(slotId + SF.SgItmPic);
            var ownerClass = sg.GetValue(SF.SgClass);

            var itemColor = 0;
            for (var i = 0; i < 8; ++i) itemColor += sg.GetValue(slotId + SF.SgItmSchadenMin + i);
            itemColor %= 5;
            
            var itemClass = 0;
            while (itemPic >= 1000)
            {
                itemPic -= 1000;
                ++itemClass;
            }

            var itemId = GetItemId(itemType, itemPic, itemColor, itemClass);
            if (itemId == 0) return 0;
            if (itemType != 0 || slotNum <= 0 || slotNum > 10) return itemId;

            if (slotNum <= 8)
            {
                itemId = (int) SF.ImgEmptySlot1 + slotNum - 1;
            }
            else switch ((SfClass)ownerClass)
            {
                case SfClass.Warrior:
                    if (slotNum == 9) itemId = (int) SF.ImgEmptySlot9_1;
//                    else if (noShield) itemId = (int) SF.ImgNoShield;
                    else itemId = (int) SF.ImgEmptySlot10;
                    break;
                case SfClass.Mage:
                    if (slotNum == 9) itemId = (int) SF.ImgEmptySlot9_2;
                    break;
                case SfClass.Scout:
                    if (slotNum == 9) itemId = (int) SF.ImgEmptySlot9_3;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return itemId;
        }

        private static int GetArrowId(int itemClass, int itemPic, int itemColor)
        {
            var arrowId = (int) SF.ArrowOffs;

            arrowId += itemClass*5*100;
            arrowId += itemPic*5;
            arrowId += itemColor;

            if (arrowId < (int) SF.ArrowMax) return arrowId;

            Log.Warn("Error: not enough indices for arrows: {0} >= {1} Pic: {2} Color: {3} Class: {4}",
                arrowId, SF.ItmMax, itemPic, itemColor, itemClass);
            return 0;
        }

        private static int GetArrowIdFromSavegame(int itemClass, int itemPic, ISavegame sg, int colorOverride)
        {
            var slotId = itemClass + itemPic * (int)SF.SgItmSize;
            itemPic = sg.GetValue(slotId + SF.SgItmPic);
            var itemColor = 0;
            for (var i = 0; i < 8; ++i) itemColor += sg.GetValue(slotId + SF.SgItmSchadenMin + i);
            itemColor %= 5;
            itemClass = 0;
            while (itemPic >= 1000)
            {
                itemPic -= 1000;
                ++itemClass;
            }
            --itemClass;
            if (colorOverride >= 0)
                itemColor = colorOverride;

            return GetArrowId(itemClass, itemPic, itemColor);
        }

        private string GetItemName(int itemType, int itemPic, int itemClass)
        {
            int txtBase = 0;
            string txtSuffix = string.Empty;

            if (itemType >= 8)
            {
                switch (itemType)
                {
                    case 8:
                        txtBase = (int) SF.TxtItmname8;
                        break;
                    case 9:
                        txtBase = (int) SF.TxtItmname9;
                        break;
                    case 10:
                        txtBase = (int) SF.TxtItmname10;
                        break;
                    case 11:
                        txtBase = (int) SF.TxtItmname11;
                        txtSuffix = string.Empty;
                        break;
                    case 12:
                        txtBase = (int) SF.TxtItmname12;
                        break;
                    case 13:
                        txtBase = (int) SF.TxtItmname13;
                        break;
                    case 14:
                        txtBase = (int) SF.TxtItmname14;
                        break;
                }
            }
            else
            {
                int itemOffs = 0;
                switch (itemType)
                {
                    case 1:
                        itemOffs = (int) SF.TxtItmname1_1;
                        break;
                    case 2:
                        itemOffs = (int) SF.TxtItmname2_1;
                        break;
                    case 3:
                        itemOffs = (int) SF.TxtItmname3_1;
                        break;
                    case 4:
                        itemOffs = (int) SF.TxtItmname4_1;
                        break;
                    case 5:
                        itemOffs = (int) SF.TxtItmname5_1;
                        break;
                    case 6:
                        itemOffs = (int) SF.TxtItmname6_1;
                        break;
                    case 7:
                        itemOffs = (int) SF.TxtItmname7_1;
                        break;
                }
                if (itemOffs > 0)
                {
                    itemOffs = (itemOffs - (int) SF.TxtItmname1_1);
                    switch (itemClass)
                    {
                        case 1:
                            txtBase = (int) SF.TxtItmname1_1 + itemOffs;
                            break;
                        case 2:
                            txtBase = (int) SF.TxtItmname1_2 + itemOffs;
                            break;
                        case 3:
                            txtBase = (int) SF.TxtItmname1_3 + itemOffs;
                            break;
                    }
                }
            }

            if (itemPic >= 50 && itemType != 14)
            {
                txtBase = txtBase + SF.TxtItmname1_1Epic - SF.TxtItmname1_1;
                itemPic -= 49;
                txtSuffix = string.Empty;
            }

            if (!_languageDict.ContainsKey((SF) txtBase + itemPic - 1))
            {
                return "Unknown Item (base=" + txtBase + ", entry=" + (txtBase + itemPic - 1) + ")";
            }

            if (_languageDict.ContainsKey(SF.TxtItmnameExt))
            {
                if (_languageDict[SF.TxtItmnameExt] == "1")
                {
                    return (string.IsNullOrWhiteSpace(txtSuffix) ? string.Empty : txtSuffix + " ") +
                           _languageDict[(SF) txtBase + itemPic - 1];
                }

                if (_languageDict[SF.TxtItmnameExt] != "2")
                    return _languageDict[(SF) txtBase + itemPic - 1] +
                           (string.IsNullOrWhiteSpace(txtSuffix) ? string.Empty : " " + txtSuffix);
            }

            if (string.IsNullOrWhiteSpace(txtSuffix))
                return _languageDict[(SF) txtBase + itemPic - 1];

            string[] parts = txtSuffix.Split(new[] {"%1"}, StringSplitOptions.None);
            return string.Join(_languageDict[(SF) txtBase + itemPic - 1], parts);
        }

        private Uri GetImageUri(int itemId)
        {
            return !_urlDict.ContainsKey(itemId)
                ? null
                : new Uri(_imageServerUri, _urlDict[itemId]);
        }

        private static string GetItemFile(int itemType, int itemPic, int itemColor, int itemClass)
        {
            string itemFile = "itm";
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
            else if (itemType == 8 || itemType == 9 || itemType == 10 || itemType == 11 || itemType == 12 ||
                     itemType == 13 || itemType == 14)
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

        private void DefineMonsters(bool paramCensored)
        {
            DefineImage(SF.ImgUnknownEnemy, "res/gfx/scr/fight/monster/unknown.jpg");

            for (var k = 0; k < 500; ++k)
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
                    string monsterChecksum =
                        (i.ToString(CultureInfo.InvariantCulture) + "ScriptKiddieLovesToPeek").ToMd5Hash();
                    DefineImage(SF.ImgOppimgMonster + k, "res/gfx/scr/fight/monster/monster" + monsterChecksum + ".jpg");
                }
                else
                {
                    DefineImage(SF.ImgOppimgMonster + k,
                        "res/gfx/scr/fight/monster/monster" + (i + 1).ToString(CultureInfo.InvariantCulture) + ".jpg");
                }
            }
        }

        private void DefineItems()
        {
            for (var itemType = 0; itemType <= 14; ++itemType)
            {
                for (var itemPic = 0; itemPic < (int) SF.CItemsPerType; ++itemPic)
                {
                    for (var itemColor = 0; itemColor < 5; ++itemColor)
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
                                for (var itemClass = 0; itemClass < 3; ++itemClass)
                                {
                                    DefineImage((SF) GetItemId(itemType, itemPic, itemColor, itemClass),
                                        GetItemFile(itemType, itemPic, itemColor, itemClass));
                                }
                                break;
                            default:
                                DefineImage((SF) GetItemId(itemType, itemPic, itemColor, 0),
                                    GetItemFile(itemType, itemPic, itemColor, 0));
                                break;
                        }
                    }
                }
            }

            for (int itemType = 0; itemType <= 1; ++itemType)
            {
                for (var itemPic = 0; itemPic < (int) SF.CItemsPerType; ++itemPic)
                {
                    for (var itemColor = 0; itemColor < 5; ++itemColor)
                    {
                        DefineImage((SF) GetArrowId(itemType, itemPic, itemColor),
                            ("res/gfx/itm/1-" + (itemType + 2) + "/shot" + (itemType == 0 ? 2 : 1) + "-" + itemPic + "-" +
                             ((itemPic >= 50 ? (itemType == 0 ? (itemColor == 3 ? 3 : 0) : 0) : itemColor) + 1) + ".png"));
                    }
                }
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

            for (var itemType = 0; itemType < 8; ++itemType)
            {
                DefineImage(SF.ImgEmptySlot1 + itemType, "res/gfx/scr/char/slot" + (itemType + 1) + ".png");
            }

            DefineImage(SF.ImgEmptySlot9_1, "res/gfx/scr/char/slot9_1.png");
            DefineImage(SF.ImgEmptySlot9_2, "res/gfx/scr/char/slot9_2.png");
            DefineImage(SF.ImgEmptySlot9_3, "res/gfx/scr/char/slot9_3.png");
            DefineImage(SF.ImgEmptySlot10, "res/gfx/scr/char/slot10.png");
            DefineImage(SF.ImgNoShield, "res/gfx/itm/no_shield.png");

        }

        private void DefineImage(SF actorId, string url)
        {
            if (!_urlDict.ContainsKey((int) actorId))
            {
                _urlDict.Add((int) actorId, url);
                return;
            }
            else if (_urlDict[(int) actorId] != url)
            {
                
            }
            Log.Warn("Image with the same URL already added: {0}", url);
        }

        private IEnumerable<IScrapbookItem> CreateMultipleItems<TScrapbookItem>(IReadOnlyList<int> scrapbookContent,
            int itemOnPage, int aOffs, int itemType, int itemPic, int itemClass) where TScrapbookItem : new()
        {
            var results = new List<TScrapbookItem>
            {
                new TScrapbookItem(),
                new TScrapbookItem(),
                new TScrapbookItem(),
                new TScrapbookItem(),
                new TScrapbookItem()
            };

            if (!(results is IEnumerable<ScrapbookItemBase>))
                throw new ArgumentException("TScrapbookItem must be derived type of ScrapbookItemBase");

            var items = results.Cast<ScrapbookItemBase>().ToList();

            items[0].ContentId = (int)SF.CntAlbumWeapon1 + itemOnPage;
            items[1].ContentId = (int)SF.CntAlbumWeapon2 + itemOnPage;
            items[2].ContentId = (int)SF.CntAlbumWeapon3 + itemOnPage;
            items[3].ContentId = (int)SF.CntAlbumWeapon4 + itemOnPage;
            items[4].ContentId = (int)SF.CntAlbumWeapon5 + itemOnPage;

            for (var i = 0; i < 5; ++i)
            {
                items[i].HasItem = scrapbookContent[aOffs + i] == 1;
                items[i].Text = GetItemName(itemType, itemPic, itemClass);
            }

            if (itemClass > 0) --itemClass;

            for (var i = 0; i < 5; ++i)
            {
                items[i].Id = GetItemId(itemType, itemPic, i, itemClass);
                items[i].ImageUri = GetImageUri(items[i].Id);
            }

            // enable popup?

            return items;
        }

        private IScrapbookItem CreateEpicItem<TScrapbookItem>(IReadOnlyList<int> scrapbookContent, int itemOnPage, int aOffs,
            int itemType, int itemPic, int itemClass) where TScrapbookItem : new()
        {
            var result = new TScrapbookItem();

            var item = result as ScrapbookItemBase;
            if (item == null)
                throw new ArgumentException("TScrapbookItem must be derived type of ScrapbookItemBase");

            item.HasItem = scrapbookContent[aOffs] == 1;
            item.Text = GetItemName(itemType, itemPic, itemClass);
            item.ContentId = (int)SF.CntAlbumWeaponEpic + itemOnPage;

            if (item.Text.Contains('|'))
            {
                item.HintText = item.Text.Split('|')[1].Replace('#', ' ');
                item.Text = item.Text.Split('|')[0];
            }

            if (itemClass > 0) --itemClass;

            item.Id = GetItemId(itemType, itemPic, 0, itemClass);
            item.ImageUri = GetImageUri(item.Id);

            // enable popup?

            return item;
        }
    }
}