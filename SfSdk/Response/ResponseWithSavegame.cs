using System;

namespace SfSdk.Response
{
    /// <summary>
    ///     A response containing a savegame.
    /// </summary>
    internal interface IResponseWithSaveGame
    {
        /// <summary>
        ///     The character's savegame.
        /// </summary>
        ISavegame Savegame { get; }
    }

    /// <summary>
    ///     A base class of type <see cref="IResponse"/> containing the arguments of the response.
    /// </summary>
    internal abstract class ResponseWithSavegame : ResponseBase, IResponseWithSaveGame
    {
        private readonly ISavegame _savegame;

        /// <summary>
        ///     Creates a new response with savegame.
        /// </summary>
        /// <param name="args">The response arguments.</param>
        /// <exception cref="ArgumentException">When the arguments have not a minimum length of 1.</exception>
        protected ResponseWithSavegame(string[] args) : base(args)
        {
            if (Args.Length < 1) throw new ArgumentException("The arguments must have a minimum length of 1.", "args");

            _savegame = new Savegame(Args[0]);
        }

        public ISavegame Savegame
        {
            get { return _savegame; }
        }
    }
}