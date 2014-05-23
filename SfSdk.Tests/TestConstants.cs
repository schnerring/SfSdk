using System;

namespace SfSdk.Tests
{
    public class TestConstants
    {
        public const string ValidUsername = "Username";
        public const string InvalidUsername = "InvalidUsername";
        public const string ValidPasswordHash = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        public const string InvalidPasswordHash = "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb";
        public const string ValidSessionId = "00000000000000000000000000000000";
        public const string InvalidSessionId = "000";
        public static readonly Uri ValidServerUri = new Uri("http://s25.sfgame.de/");
        public static readonly Uri ValidImageServerUri = new Uri("http://img.playa-games.com/res/sfgame/");
        public static readonly Uri ValidCountryServerUri = new Uri("http://www.sfgame.de/");
        public static readonly int[] ValidAlbumContent = new int[3200];
        
        public const string ExistingSuccess = "187"; // SF.RespLogoutSuccess
        public const string NonExistingSuccess = "000";
        public const string ExistingError = "E006"; // SF.ErrLoginFailed
        public const string NonExistingError = "E000";

        public const string ValidSavegameString = "1912230455/75298/1367106812/1362182092/837135956/4/0/5/572/2200/360/55499/-1/1204/14/4/5/3/103/103/4/103/3/1/1/1/0/6/1/2/17/21/30/20/24/0/0/0/0/0/0/0/2/0/0/2/2/1365774494/6/1001/3/0/5/2/3/0/0/0/26/0/0/0/0/0/0/0/0/0/0/0/0/0/5/1001/2/0/5/4/3/0/0/0/24/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/1/1001/4/14/0/0/0/0/0/0/1/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/1365774068/5/5/5/6/4/2/-98/-11/-64/17/11/7/105/420/105/0/1001/9/0/3/1/2/3/0/0/154/0/0/1001/2/0/2/1/4/2/0/0/25/0/0/1001/6/0/1/4/2/4/0/0/154/0/44/774/91/53/51/42/0/1363718552/5/1001/6/0/5/2/3/2/0/0/131/0/7/1001/9/0/1/5/4/3/0/0/141/0/4/1001/7/0/1/2/3/3/0/0/147/0/1/1002/11/33/1/4/3/0/0/0/96/0/4/1001/4/0/5/2/4/0/0/0/45/0/4/1003/21/0/1/4/3/3/0/0/126/1/1362943332/8/4/0/0/1/5/3/3/0/0/75/0/9/7/0/0/2/4/5/2/0/0/50/0/8/6/0/0/2/5/1/2/0/0/50/0/12/4/0/0/11/4/0/72/10/0/71/0/9/1/0/0/3/5/4/3/0/0/37/1/12/5/0/0/11/5/0/72/10/0/71/0/0/1/8601/3/0/0/0/0/0/0/1362946879/0/0/0/5/4/14/0/0/0/0/0/1365763903/5580/0/0/0/0/0/0/0/1362182092/5/0/3/13/0/1425/399/0/1/9/0/0/2/1362776202/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/98807380/98807380/98807380/0/0/0/0/1367106812/";
    }
}