using System;
using SfSdk.Constants;

namespace SfSdk.ResponseData
{
    /// <summary>
    ///     The reponse type returned on <see cref="SF.ActScreenChar" />, <see cref="SF.RespPlayerScreen" />.
    /// </summary>
    internal class CharacterResponse : ResponseBase
    {
        private readonly string _comment;
        private readonly string _guild;
        private readonly Savegame _savegame;

        /// <summary>
        ///     Creates a new character response.
        /// </summary>
        /// <param name="args">The response arguments.</param>
        public CharacterResponse(string[] args) : base(args)
        {
            if (args.Length < 3) throw new NotImplementedException();

            string[] savegameParts = ("0/" + args[0]).Split('/');
            _savegame = new Savegame(savegameParts);
            _comment = args[1];
            _guild = args[2];
        }

        /// <summary>
        ///     The character's comment.
        /// </summary>
        public string Comment
        {
            get { return _comment; }
        }

        /// <summary>
        ///     The character's guild.
        /// </summary>
        public string Guild
        {
            get { return _guild; }
        }

        /// <summary>
        ///     The character's savegame.
        /// </summary>
        public Savegame Savegame
        {
            get { return _savegame; }
        }
    }
}