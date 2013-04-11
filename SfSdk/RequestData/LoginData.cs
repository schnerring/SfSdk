using System.Collections.Generic;
using SfSdk.Enums;

namespace SfSdk.RequestData
{
    internal class LoginData
    {
        private readonly int _gold;
        private readonly int _mushrooms;
        private readonly string _sessionId;
        private readonly int _silver;

        public LoginData(string sessionId, IList<string> savegameParts)
        {
            _sessionId = sessionId;
            _gold = int.Parse(savegameParts[(int) SfSavegame.Gold])/100;
            _silver = int.Parse(savegameParts[(int) SfSavegame.Gold])%100;
            _mushrooms = int.Parse(savegameParts[(int) SfSavegame.Mushrooms]);
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