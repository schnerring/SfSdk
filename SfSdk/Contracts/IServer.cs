using System;
using System.Runtime.Serialization;

namespace SfSdk.Contracts
{
    public interface IServer : ISerializable
    {
        /// <summary>
        ///     The game server's name
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     The game server's URI.
        /// </summary>
        Uri Uri { get; }
    }
}