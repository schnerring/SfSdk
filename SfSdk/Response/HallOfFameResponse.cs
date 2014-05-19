using System;
using System.Collections.Generic;
using SfSdk.Constants;

namespace SfSdk.Response
{
    /// <summary>
    ///     The reponse type returned on <see cref="SF.ActScreenChar" />, <see cref="SF.RespPlayerScreen" />.<br />
    ///     Triggered by action <see cref="SF.ActScreenChar" />, <see cref="SF.ActRequestChar" />.
    /// </summary>
    internal interface IHallOfFameResponse : IResponse
    {
        /// <summary>
        ///     The predicate used for seaching the hall of fame.
        /// </summary>
        string SearchString { get; }
        
        /// <summary>
        ///     The result set of characters.
        /// </summary>
        IEnumerable<Tuple<int, string, string, int, int>> Characters { get; }
    }

    /// <summary>
    ///     The reponse type returned on <see cref="SF.ActScreenChar" />, <see cref="SF.RespPlayerScreen" />.<br />
    ///     Triggered by action <see cref="SF.ActScreenChar" />, <see cref="SF.ActRequestChar" />.
    /// </summary>
    internal class HallOfFameResponse : ResponseBase, IHallOfFameResponse
    {
        private readonly string _searchString;

        private readonly IList<Tuple<int, string, string, int, int>> _characters =
            new List<Tuple<int, string, string, int, int>>();

        /// <summary>
        ///     Creates a new hall of fame response.
        /// </summary>
        /// <param name="args">The response arguments.</param>
        /// <exception cref="ArgumentException">When the arguments have not a minimum length of 1.</exception>
        public HallOfFameResponse(string[] args) : base(args)
        {
            if (Args.Length < 1) throw new ArgumentException("The arguments must have a minimum length of 1.", "args");
            if (Args.Length > 1) _searchString = args[1];
            var charInfo = args[0].Split('/');
            var i = 0;
            while (i < charInfo.Length - 1)
            {
                var rank = Math.Abs(int.Parse(charInfo[i]));
                i++;
                var username = charInfo[i];
                i++;
                var guild = charInfo[i];
                i++;
                var level = Math.Abs(int.Parse(charInfo[i]));
                i++;
                var honor = int.Parse(charInfo[i]);
                i++;
                _characters.Add(new Tuple<int, string, string, int, int>(rank, username, guild, level, honor));
            }
        }

        public string SearchString
        {
            get { return _searchString; }
        }

        public IEnumerable<Tuple<int, string, string, int, int>> Characters
        {
            get { return _characters; }
        }
    }
}