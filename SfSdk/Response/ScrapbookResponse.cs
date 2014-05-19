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
using SfSdk.Contracts;
using SfSdk.Data;
using SfSdk.Logging;
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
        private static readonly ILog Log = LogManager.GetLog(typeof (ScrapbookResponse));

        private readonly ItemProvider _itemProvider;

        public ScrapbookResponse(string[] args, Uri serverUri) : base(args)
        {
            if (Args.Length < 1) throw new ArgumentException("The arguments must have a minimum length of 1.", "args");
            if (serverUri == null) throw new ArgumentNullException("serverUri");
            
            _itemProvider = new ItemProvider(serverUri);

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

            var items = new List<IScrapbookItem>();
            for (int monsterPage = 0; monsterPage <= 62; monsterPage++)
            {
                for (int itemOnPage = 0; itemOnPage < 4; ++itemOnPage)
                {
                    bool hasItem = array[(monsterPage * 4) + itemOnPage] == 1;
                    var monster = new MonsterItem()
                    {
                        HasItem = hasItem,
                        ImageUri =
                            _itemProvider.GetImageUri((int) SF.CntAlbumMonster + itemOnPage,
                                                       (int) SF.ImgOppimgMonster + monsterPage*4 + itemOnPage),
                        Text = monsterPage*4 + 1 >= 220
                            ? _itemProvider.GetMonsterItemName(SF.TxtNewMonsterNames + monsterPage*4 + 1 - 220)
                            : _itemProvider.GetMonsterItemName(SF.TxtMonsterName + monsterPage*4 + 1)
                    };

                    items.Add(monster);
                }
            }

            for (int valuablePage = 0; valuablePage <= 25; valuablePage++)
            {
                for (int itemOnPage = 0; itemOnPage < 4; ++itemOnPage)
                {
                    var itemsToAdd = new List<IScrapbookItem>();

                    if (valuablePage <= 5)
                        if (valuablePage < 5 || itemOnPage <= 0)
                            itemsToAdd.AddRange(CreateMultipleItems<ValuableItem>(array, itemOnPage, 300 + valuablePage*20 + itemOnPage*5, 8, 1 + valuablePage*4 + itemOnPage, 0));
                        else continue;
                    else if (valuablePage <= 7)
                        itemsToAdd.Add(CreateEpicItem<ValuableItem>(array, itemOnPage, 510 + (valuablePage - 6)*4 + itemOnPage, 8, 50 + (valuablePage - 6)*4 + itemOnPage, 0));
                    else if (valuablePage <= 11)
                        itemsToAdd.AddRange(CreateMultipleItems<ValuableItem>(array, itemOnPage, 526 + (valuablePage - 8) * 20 + itemOnPage * 5, 9, 1 + (valuablePage - 8) * 4 + itemOnPage, 0));
                    else if (valuablePage <= 13)
                        itemsToAdd.Add(CreateEpicItem<ValuableItem>(array, itemOnPage, 686 + (valuablePage - 12) * 4 + itemOnPage, 9, 50 + (valuablePage - 12) * 4 + itemOnPage, 0));
                    else if (valuablePage <= 23)
                        if (valuablePage < 23 || itemOnPage <= 0)
                            itemsToAdd.Add(CreateEpicItem<ValuableItem>(array, itemOnPage, 702 + (valuablePage - 14) * 4 + itemOnPage, 10, 1 + (valuablePage - 14) * 4 + itemOnPage, 0));
                        else continue;
                    else if (valuablePage <= 25)
                        itemsToAdd.Add(CreateEpicItem<ValuableItem>(array, itemOnPage, 760 + 16 + (valuablePage - 24) * 4 + itemOnPage, 10, 50 + (valuablePage - 24) * 4 + itemOnPage, 0));
                    
                    items.AddRange(itemsToAdd);
                }
            }

            var monsterItems = items.Where(itm => itm is MonsterItem).ToList();
            var valuableItems = items.Where(itm => itm is ValuableItem).ToList();
        }

        private IEnumerable<IScrapbookItem> CreateMultipleItems<TScrapbookItem>(List<int> albumContent, int itemOnPage, int aOffs, int itemType, int itemPic, int itemClass) where TScrapbookItem: new()
        {
            var results = new List<TScrapbookItem>
            {
                new TScrapbookItem(),
                new TScrapbookItem(),
                new TScrapbookItem(),
                new TScrapbookItem(),
                new TScrapbookItem()
            };

            if (!(results is IEnumerable<IScrapbookItem>))
                throw new ArgumentException("TScrapbookItem must implement IScrapbookItem");
            
            var itemText = _itemProvider.GetItemName(itemType, itemPic, itemClass);
            var i = 0;
            var items = results.Cast<IScrapbookItem>().ToList();
            while (i < 5)
            {
                items[i].HasItem = albumContent[aOffs + i] == 1;
                items[i].Text = itemText;
                ++i;
            }

            if (itemClass > 0)
            {
                --itemClass;
            }

            items[0].ImageUri = _itemProvider.GetImageUri((int)SF.CntAlbumWeapon1 + itemOnPage, _itemProvider.GetItemId(itemType, itemPic, 0, itemClass));
            items[1].ImageUri = _itemProvider.GetImageUri((int)SF.CntAlbumWeapon2 + itemOnPage, _itemProvider.GetItemId(itemType, itemPic, 1, itemClass));
            items[2].ImageUri = _itemProvider.GetImageUri((int)SF.CntAlbumWeapon3 + itemOnPage, _itemProvider.GetItemId(itemType, itemPic, 2, itemClass));
            items[3].ImageUri = _itemProvider.GetImageUri((int)SF.CntAlbumWeapon4 + itemOnPage, _itemProvider.GetItemId(itemType, itemPic, 3, itemClass));
            items[4].ImageUri = _itemProvider.GetImageUri((int)SF.CntAlbumWeapon5 + itemOnPage, _itemProvider.GetItemId(itemType, itemPic, 4, itemClass));

            // enable popup?

            return items;
        }

        private IScrapbookItem CreateEpicItem<TScrapbookItem>(List<int> albumContent, int itemOnPage, int aOffs, int itemType, int itemPic, int itemClass) where TScrapbookItem : new()
        {
            var result = new TScrapbookItem();

            if (!(result is IScrapbookItem))
                throw new ArgumentException("TScrapbookItem must implement IScrapbookItem");

            var item = (IScrapbookItem) result;
            item.HasItem = albumContent[aOffs] == 1;
            item.Text = _itemProvider.GetItemName(itemType, itemPic, itemClass);

            if (item.Text.Contains('|'))
            {
                item.HintText = item.Text.Split('|')[1].Replace('#', '\n');
                item.Text = item.Text.Split('|')[0];
            }

            if (itemClass > 0)
            {
                --itemClass;
            }

            item.ImageUri = _itemProvider.GetImageUri((int)SF.CntAlbumWeaponEpic + itemOnPage, _itemProvider.GetItemId(itemType, itemPic, 0, itemClass));

            // enable popup?

            return item;
        }
    }
}