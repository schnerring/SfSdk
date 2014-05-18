// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScrapbookResponse.cs" company="">
//   
// </copyright>
// <summary>
//   The reponse type returned on <see cref="SF.RespAlbum" />.<br />
//   Triggered by action <see cref="SF.ActAlbum" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using SfSdk.Constants;
using SfSdk.Providers;

namespace SfSdk.Response
{
    /// <summary>
    ///     The reponse type returned on <see cref="SF.RespAlbum" />.<br />
    ///     Triggered by action <see cref="SF.ActAlbum" />.
    /// </summary>
    internal interface IScrapbookResponse : IResponse
    {
    }

    /// <summary>
    ///     The response type returned on <see cref="SF.RespAlbum" />.<br />
    ///     Triggered by action <see cref="SF.ActAlbum" />.
    /// </summary>
    internal class ScrapbookResponse : ResponseBase, IScrapbookResponse
    {
        private static Dictionary<int, string> LanguageResourceDict;

        public ScrapbookResponse(string[] args, Uri serverUri) : base(args)
        {
            if (Args.Length < 1) throw new ArgumentException("The arguments must have a minimum length of 1.", "args");
            if (serverUri == null) throw new ArgumentNullException("serverUri");

            LanguageResourceDict = new LanguageResourceProvider().GetLanguageResources(serverUri);

            byte[] byteArray = Convert.FromBase64String(Args.First());
            var array = new List<int>();

            int i = 0;
            while (i < byteArray.Length)
            {
                array.Add((byteArray[i] & 128)/128);
                array.Add((byteArray[i] & 64)/64);
                array.Add((byteArray[i] & 32)/32);
                array.Add((byteArray[i] & 16)/16);
                array.Add((byteArray[i] & 8)/8);
                array.Add((byteArray[i] & 4)/4);
                array.Add((byteArray[i] & 2)/2);
                array.Add(byteArray[i] & 1);
                ++i;
            }

            var categoryCount = new[] {0, 0, 0, 0, 0};
            var categoryMax = new[] {252, 246, 506, 348, 348};
            int contentCount = 0;
            int contentMax = categoryMax.Sum(c => c);

            i = 0;
            while (i < array.Count)
            {
                if (array[i] == 1)
                {
                    if (i < 300)
                    {
                        ++categoryCount[0];
                    }
                    else if (i < 792)
                    {
                        ++categoryCount[1];
                    }
                    else if (i < 1804)
                    {
                        ++categoryCount[2];
                    }
                    else if (i < 2500)
                    {
                        ++categoryCount[3];
                    }
                    else
                    {
                        ++categoryCount[4];
                    }
                    ++contentCount;
                }
                ++i;
            }

            if (contentCount > contentMax)
            {
                contentCount = contentMax;
            }

            i = 0;
            while (i < 5)
            {
                if (categoryCount[i] > categoryMax[i])
                {
                    categoryCount[i] = categoryMax[i];
                }
                ++i;
            }

            int itemOnPage;
            var items = new List<IScrapbookItem>();
            for (int monsterPage = 0; monsterPage <= 62; monsterPage++)
            {
                itemOnPage = 0;
                while (itemOnPage < 4)
                {
                    bool hasItem = array[(monsterPage*4) + i] == 1;
                    var monster = new MonsterItem(hasItem);

                    SetContent((int) SF.CntAlbumMonster + i, (int) SF.ImgOppimgMonster + monsterPage*4 + i);

                    monster.Text = monsterPage*4 + 1 >= 220
                        ? LanguageResourceDict[(int) SF.TxtNewMonsterNames + monsterPage*4 + 1 - 220]
                        : LanguageResourceDict[(int) SF.TxtMonsterName + monsterPage*4 + 1];
                    items.Add(monster);
                    ++itemOnPage;
                }
            }

            for (int valuablePage = 0; valuablePage <= 25; valuablePage++)
            {
                itemOnPage = 0;
                while (itemOnPage < 4)
                {
//                    var valuable = new ValuableItem();
                    if (valuablePage <= 5)
                        if (valuablePage < 5 || i <= 0)
                            SetAlbumItems(array, itemOnPage, 300 + valuablePage*20 + i*5, 8, 1 + valuablePage*4 + i, 0);
                        else
                        {
                            string entryText = string.Empty;
                        }
                    else if (valuablePage <= 7)
                        SetAlbumEpic(array, itemOnPage, 510 + (valuablePage - 6)*4 + i, 8,
                            50 + (valuablePage - 6)*4 + i, 0);
                    else if (valuablePage <= 11)
                        SetAlbumItems(array, itemOnPage, 526 + (valuablePage - 8)*20 + (i*5), 9,
                            1 + (valuablePage - 8)*4 + i, 0);
                    else if (valuablePage <= 13)
                        SetAlbumItems(array, itemOnPage, 686 + (valuablePage - 12)*4 + i, 9,
                            50 + (valuablePage - 12)*4 + i, 0);
                    else if (valuablePage <= 23)
                        if (valuablePage < 23 || i <= 0)
                            SetAlbumEpic(array, itemOnPage, 702 + (valuablePage - 14)*4 + i, 10,
                                1 + (valuablePage - 14)*4 + i, 0);
                        else
                        {
                            string entryText = string.Empty;
                        }
                    else if (valuablePage <= 25)
                        SetAlbumEpic(array, itemOnPage, 760 + 16 + (valuablePage - 24)*4 + i, 10,
                            50 + (valuablePage - 24)*4 + i, 0);
                    ++itemOnPage;
                }
            }
        }

        private void SetAlbumItems(List<int> albumContent, int itemOnPage, int aOffs, int itemType, int itemPic,
            int itemClass)
        {
            var itemSet = new int[5];
            int i = 0;
            bool hasAnyItem = false;

            while (i < 5)
            {
                itemSet[i] = albumContent[aOffs + i];
                if (itemSet[i] == 1) hasAnyItem = true;
                ++i;
            }

            if (hasAnyItem)
            {
                string entryText = GetItemName(itemType, itemPic, itemClass);
                if (itemClass > 0)
                {
                    --itemClass;
                }
                SetContent((int) SF.CntAlbumWeapon1 + itemOnPage, GetItemId(itemType, itemPic, 0, itemClass));
                SetContent((int) SF.CntAlbumWeapon2 + itemOnPage, GetItemId(itemType, itemPic, 1, itemClass));
                SetContent((int) SF.CntAlbumWeapon3 + itemOnPage, GetItemId(itemType, itemPic, 2, itemClass));
                SetContent((int) SF.CntAlbumWeapon4 + itemOnPage, GetItemId(itemType, itemPic, 3, itemClass));
                SetContent((int) SF.CntAlbumWeapon5 + itemOnPage, GetItemId(itemType, itemPic, 4, itemClass));

                // enable popup?
            }
        }

        private int GetItemId(int itemType, int itemPic, object someObject = null, int itemClass = -1)
        {
            throw new NotImplementedException();
        }

        private string GetItemName(int sgIndex, int sg, int albumMode = -1)
        {
            throw new NotImplementedException();
        }

        private void SetAlbumEpic(List<int> albumContent, int itemOnPage, int aOffs, int itemType, int itemPic,
            int itemClass)
        {
            bool hasItem = albumContent[aOffs] == 1;
            if (hasItem)
            {
                string entryText = GetItemName(itemType, itemPic, itemClass);
                string hintText;
                if (entryText.Contains('|'))
                {
                    hintText = entryText.Split('|')[1].Replace('#', '\n');
                    entryText = entryText.Split('|')[0];
                }

                if (itemClass > 0)
                {
                    --itemClass;
                }

                SetContent((int) SF.CntAlbumWeaponEpic + itemOnPage, GetItemId(itemType, itemPic, 0, itemClass));

                // enable popup?
            }
        }

        private void SetContent(int contentId, int imageId)
        {
        }
    }

    internal interface IScrapbookItem
    {
        bool HasItem { get; }
        string Text { get; }
    }

    internal class ScrapbookItemBase : IScrapbookItem
    {
        public ScrapbookItemBase(bool hasItem)
        {
            HasItem = hasItem;
        }

        public bool HasItem { get; private set; }
        public string Text { get; set; }
    }

    internal class MonsterItem : ScrapbookItemBase
    {
        public MonsterItem(bool hasItem)
            : base(hasItem)
        {
        }
    }

    internal class ValuableItem : ScrapbookItemBase
    {
        public ValuableItem(bool hasItem)
            : base(hasItem)
        {
        }
    }

    internal class WarriorItem : ScrapbookItemBase
    {
        public WarriorItem(bool hasItem)
            : base(hasItem)
        {
        }
    }

    internal class MageItem : ScrapbookItemBase
    {
        public MageItem(bool hasItem)
            : base(hasItem)
        {
        }
    }

    internal class ScoutItem : ScrapbookItemBase
    {
        public ScoutItem(bool hasItem)
            : base(hasItem)
        {
        }
    }
}