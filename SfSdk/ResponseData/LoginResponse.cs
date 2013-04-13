using System;
using SfSdk.Constants;

namespace SfSdk.ResponseData
{
    internal class LoginResponse : ResponseBase
    {
        private readonly int _gold;
        private readonly int _mushrooms;
        private readonly Savegame _savegame;
        private readonly string _sessionId;
        private readonly int _silver;

        public LoginResponse(string[] args) : base(args)
        {
            if (Args.Length < 3) throw new NotImplementedException();

            var savegameParts = ("0/" + Args[0]).Split('/');
            _savegame = new Savegame(savegameParts);
            _sessionId = Args[2];

            _gold = _savegame.GetValue(SF.SgGold)/100;
            _silver = _savegame.GetValue(SF.SgGold)%100;
            _mushrooms = _savegame.GetValue(SF.SgMush);
        }

        public Savegame Savegame
        {
            get { return _savegame; }
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