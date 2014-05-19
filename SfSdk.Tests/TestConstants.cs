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
    }
}