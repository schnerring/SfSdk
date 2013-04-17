using System;
using SfSdk.Constants;

namespace SfSdk.Response
{
    /// <summary>
    ///     The reponse type returned on <see cref="SF.ActScreenChar" />, <see cref="SF.RespPlayerScreen" />.<br />
    ///     Triggered by action <see cref="SF.ActScreenChar" />, <see cref="SF.ActRequestChar" />.
    /// </summary>
    internal interface ICharacterResponse : IResponseWithSaveGame
    {
        /// <summary>
        ///     The character's comment.
        /// </summary>
        string Comment { get; }

        /// <summary>
        ///     The character's guild.
        /// </summary>
        string Guild { get; }
    }

    /// <summary>
    ///     The reponse type returned on <see cref="SF.ActScreenChar" />, <see cref="SF.RespPlayerScreen" />.<br />
    ///     Triggered by action <see cref="SF.ActScreenChar" />, <see cref="SF.ActRequestChar" />.
    /// </summary>
    internal class CharacterResponse : ResponseWithSavegame, ICharacterResponse
    {
        private readonly string _comment;
        private readonly string _guild;

        /// <summary>
        ///     Creates a new character response.
        /// </summary>
        /// <param name="args">The response arguments.</param>
        public CharacterResponse(string[] args) : base(args)
        {
            if (Args.Length < 3) throw new ArgumentException("The arguments must have a minimum length of 3.", "args");

            _comment = Args[1];
            _guild = Args[2];
        }

        public string Comment
        {
            get { return _comment; }
        }

        public string Guild
        {
            get { return _guild; }
        }
    }
}