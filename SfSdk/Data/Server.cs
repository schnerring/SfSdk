using System;
using Newtonsoft.Json;
using SfSdk.Contracts;

namespace SfSdk.Data
{
    /// <summary>
    ///     Implements the functionality of creating a new <see cref="IServer" />.
    /// </summary>
    [Serializable]
    internal class Server : IServer
    {
        /// <summary>
        ///     Gets the game server's name.
        /// </summary>
        [JsonProperty]
        public string ServerName { get; private set; }

        /// <summary>
        ///     Gets the server <see cref="Uri" />.
        /// </summary>
        [JsonProperty]
        public Uri ServerUri { get; private set; }
    }
}