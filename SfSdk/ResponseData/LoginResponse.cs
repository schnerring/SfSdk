using System;
using SfSdk.Constants;

namespace SfSdk.ResponseData
{
    /// <summary>
    ///     The reponse type returned on <see cref="SF.RespLoginSuccess" />, <see cref="SF.RespLoginSuccessBought" />.
    /// </summary>
    internal class LoginResponse : ResponseBase
    {
        private readonly int _gold;
        private readonly int _mushrooms;
        private readonly Savegame _savegame;
        private readonly string _sessionId;
        private readonly int _silver;

        /// <summary>
        ///     Creates a new login response.
        /// </summary>
        /// <param name="args">The response arguments.</param>
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

        /// <summary>
        ///     The savegame of the logged in account's character.
        /// </summary>
        public Savegame Savegame
        {
            get { return _savegame; }
        }

        /// <summary>
        ///     The gold count of the logged in account.
        /// </summary>
        public int Gold
        {
            get { return _gold; }
        }

        /// <summary>
        ///     The silver count of the logged in account.
        /// </summary>
        public int Silver
        {
            get { return _silver; }
        }

        /// <summary>
        ///     The mushrooms count of the logged in account.
        /// </summary>
        public int Mushrooms
        {
            get { return _mushrooms; }
        }

        /// <summary>
        ///     The currently valid session ID of the logged in account.
        /// </summary>
        public string SessionId
        {
            get { return _sessionId; }
        }
    }
}