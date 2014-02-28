using System;
using System.Collections.Generic;
using System.Linq;
using SfSdk.Constants;

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
    ///     The reponse type returned on <see cref="SF.RespAlbum" />.<br />
    ///     Triggered by action <see cref="SF.ActAlbum" />.
    /// </summary>
    internal class ScrapbookResponse : ResponseBase, IScrapbookResponse
    {
        public ScrapbookResponse(string[] args) : base(args)
        {
            if (Args.Length < 1) throw new ArgumentException("The arguments must have a minimum length of 1.", "args");
            var byteArray = Convert.FromBase64String(Args.First());
            var array = new List<int>();
           
            var i = 0;
            while (i < byteArray.Length)
            {
                array.Add((byteArray[i] & 128)/128);
                array.Add((byteArray[i] & 64)/64);
                array.Add((byteArray[i] & 32)/32);
                array.Add((byteArray[i] & 16)/16);
                array.Add((byteArray[i] & 8)/8);
                array.Add((byteArray[i] & 4)/4);
                array.Add((byteArray[i] & 2)/2);
                array.Add((byteArray[i] & 1));
                i++;
            }

            var categoryCount = new[] { 0, 0, 0, 0, 0 };
            var categoryMax = new[] {252, 246, 506, 348, 348};
            var contentCount = 0;
            var contentMax = categoryMax.Sum(c => c);

            i = 0;
            while (i < array.Count)
            {
                if (array[i] == 1)
                {
                    int category;
                    if (i < 300)
                    {
                        categoryCount[0]++;
                    }
                    else if (i < 792)
                    {
                        categoryCount[1]++;
                    }
                    else if (i < 1804)
                    {
                        categoryCount[2]++;
                    }
                    else if (i < 2500)
                    {
                        categoryCount[3]++;
                    }
                    else
                    {
                        categoryCount[4]++;
                    }
                    contentCount++;
                }
                i++;
            }

            if (contentCount > contentMax) contentCount = contentMax;
            i = 0;
            while (i < 5)
            {
                if (categoryCount[i] > categoryMax[i])
                    categoryCount[i] = categoryMax[i];
                i++;
            }

            int itemOnPage;
            var items = new List<IScrapbookItem>();
            for (int monsterPage = 0; monsterPage <= 62; monsterPage++)
            {
                itemOnPage = 0;
                while (itemOnPage < 4)
                {
                    var monster = new MonsterItem {HasItem = array[monsterPage*4 + i] == 1};
//                    SetContent(SF.CntAlbumMonster + i, SF.ImgOppimgMonster + monsterPage*4 + 1);
//                    monster.Text = monsterPage*4 + 1 >= 220
//                        ? txt[SF.TxtNewMonsterNames + monsterPage*4 + 1]
//                        : txt[SF.TxtMonsterName + monsterPage*4 + 1];
                    items.Add(monster);
                    itemOnPage++;
                }
            }

            var havingMonsters = items.OfType<MonsterItem>().Where(m => m.HasItem).ToList();

            for (int valuablePage = 0; valuablePage <= 25; valuablePage++)
            {
                itemOnPage = 0;
                while (itemOnPage < 4)
                {
                    var valuable = new ValuableItem();
                    if (valuablePage <= 5)
                    {
                        if (valuablePage < 5 || i <= 0)
                        {
                            SetAlbumItems(itemOnPage, valuable, array, 300 + valuablePage*20 + i*5, 8, 1 + valuablePage*4 + i, 0);
                        }
                    }
                    itemOnPage++;
                }
            }
        }

        private void SetAlbumItems(int itemOnPage, IScrapbookItem item, List<int> array, int arg1, int arg2, int arg3, int arg4)
        {
            var intArr = new int[5];
            var having = true;
            var i = 0;
            while (i < 5)
            {
                intArr[i] = array[arg1 + i];
                if (intArr[i] == 1)
                {
                    having = true;
                }
                i++;
            }
            if (having)
            {
//                item.Text = GetItemName(arg2, arg3, arg4);
                if (arg4 > 0)
                {
                    arg4--;
                }
                SetContent(SF.CntAlbumWeapon1 + itemOnPage, GetItemId(arg2, arg3, 0, arg4));
                SetContent(SF.CntAlbumWeapon2 + itemOnPage, GetItemId(arg2, arg3, 1, arg4));
                SetContent(SF.CntAlbumWeapon3 + itemOnPage, GetItemId(arg2, arg3, 2, arg4));
                SetContent(SF.CntAlbumWeapon4 + itemOnPage, GetItemId(arg2, arg3, 3, arg4));
                SetContent(SF.CntAlbumWeapon5 + itemOnPage, GetItemId(arg2, arg3, 4, arg4));

                
            }
        }

        private void SetContent(SF p0, int getItemId)
        {
            
        }

        private int GetItemId(int arg1, int arg2, object arg3, int arg4 = -1)
        {
            int local5 = (int) SF.ItmOffs;
            int local6;
            int local7 = 0;
            int local8;
            int local9 = 0;
            bool local10 = false;
            int local11;
            bool local12 = false;
            Savegame savegame = null;
            if (arg4 < 0)
            {
                if (!(arg3 is Array))
                {
                    savegame = new Savegame(string.Empty);
                }
                local6 = arg1 + arg2*(int) SF.SgItmSize;
                local7 = arg2 + arg1;
                arg1 = savegame.GetValue(local6 + SF.SgItmTyp);
                arg2 = savegame.GetValue(local6 + SF.SgItmPic);
                if (arg4 == -2)
                {
                    local10 = true;
                    local9 = savegame.GetValue(SF.SgClass);
                }
                else if (arg4 == -3)
                {
                    local9 = -arg4 - 2;
                    local12 = true;
                    local10 = true;
                }
                local8 = 0;
                local11 = 0;
                while (local11 < 8)
                {
                    local8 = local8 + savegame.GetValue(local6 + SF.SgItmSchadenMin + local11);
                    local11++;
                }
                local8 = local8%5;
                arg4 = 0;
                while (arg2 >= 1000)
                {
                    arg2 = arg2 - 1000;
                    arg4++;
                }
            }
            else
            {
                local8 = (int) arg3;
            }

            local5 = local5 + arg1*(int) SF.CItemsPerType*5*3;
            local5 = local5 + arg2*5*3;
            local5 = local5 + arg4;
            if (local5 >= (int) SF.ItmMax)
            {
                // this.trc("Fehler: Zu wenige Indizes für Items:", _local5, ">=", this.ITM_MAX, "Typ:", _arg1, "Pic:", _arg2, "Color:", _local8, "Class:", _arg4);
                return 0;
            }
            if (((local10 && arg1 == 0) && local7 > 0) && local7 <= 10)
            {
                if (local7 <= 8)
                {
                    local5 = (int) SF.ImgEmptySlot1 + local7 - 1;
                }
                else
                {
                    if (local9 == 1)
                        if (local7 == 9)
                            local5 = (int)SF.ImgEmptySlot9_1;
                        else
                            if (local12) local5 = (int)SF.ImgNoShield;
                            else local5 = (int)SF.ImgEmptySlot10;
                    else if (local9 == 2)
                        if (local7 == 9) local5 = (int)SF.ImgEmptySlot9_2;
                    else if (local9 == 3)
                        if (local7 == 9) local5 = (int) SF.ImgEmptySlot9_3;
                }
            }
            return local5;
        }
    }

    internal interface IScrapbookItem
    {
        bool HasItem { get; set; }
        string Text { get; set; }
    }

    internal class ScrapbookItemBase: IScrapbookItem
    {
        public bool HasItem { get; set; }
        public string Text { get; set; }
    }

    internal class MonsterItem : ScrapbookItemBase
    {

    }

    internal class ValuableItem : ScrapbookItemBase
    {

    }

    internal class WarriorItem : ScrapbookItemBase
    {

    }

    internal class MageItem : ScrapbookItemBase
    {

    }

    internal class ScoutItem : ScrapbookItemBase
    {

    }
}