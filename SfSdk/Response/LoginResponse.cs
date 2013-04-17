using System;
using SfSdk.Constants;

namespace SfSdk.Response
{
    /// <summary>
    ///     The reponse type returned on <see cref="SF.RespLoginSuccess" />, <see cref="SF.RespLoginSuccessBought" />.<br />
    ///     Triggered by action <see cref="SF.ActLogin" />.
    /// </summary>
    internal interface ILoginResponse : IResponseWithSaveGame
    {
        /// <summary>
        ///     The gold count of the logged in account.
        /// </summary>
        int Gold { get; }

        /// <summary>
        ///     The silver count of the logged in account.
        /// </summary>
        int Silver { get; }

        /// <summary>
        ///     The mushrooms count of the logged in account.
        /// </summary>
        int Mushrooms { get; }

        /// <summary>
        ///     The currently valid session ID of the logged in account.
        /// </summary>
        string SessionId { get; }
    }

    /// <summary>
    ///     The reponse type returned on <see cref="SF.RespLoginSuccess" />, <see cref="SF.RespLoginSuccessBought" />.<br />
    ///     Triggered by action <see cref="SF.ActLogin" />.
    /// </summary>
    internal class LoginResponse : ResponseWithSavegame, ILoginResponse
    {
        private readonly int _gold;
        private readonly int _mushrooms;
        private readonly string _sessionId;
        private readonly int _silver;

        /// <summary>
        ///     Creates a new login response.
        /// </summary>
        /// <param name="args">The response arguments.</param>
        public LoginResponse(string[] args) : base(args)
        {
            if (Args.Length < 3) throw new ArgumentException("The arguments must have a minimum length of 3.", "args");

            _sessionId = Args[2];

            _gold = Savegame.GetValue(SF.SgGold)/100;
            _silver = Savegame.GetValue(SF.SgGold)%100;
            _mushrooms = Savegame.GetValue(SF.SgMush);
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