using System.Collections.Generic;
using SfSdk.Constants;

namespace SfSdk.ResponseData
{
    internal class LoginResponse : IResponse
    {
        private readonly int _gold;
        private readonly int _mushrooms;
        private readonly string _sessionId;
        private readonly int _silver;

        public LoginResponse(IList<string> savegameParts, string sessionId)
        {
            _sessionId = sessionId;
            _gold = int.Parse(savegameParts[(int) SF.SgGold])/100;
            _silver = int.Parse(savegameParts[(int) SF.SgGold])%100;
            _mushrooms = int.Parse(savegameParts[(int) SF.SgMush]);
        }

        public int Gold
        {
            get { return _gold; }
        }

        public int Silver
        {
            get { return _silver; }
        }

        public int Mushrooms
        {
            get { return _mushrooms; }
        }

        public string SessionId
        {
            get { return _sessionId; }
        }
    }
}