using System;
using System.Runtime.Serialization;

namespace SfSdk.Contracts
{
    /// <summary>
    ///     Contains information about a sub-server of a country where S&amp;F is available.
    /// </summary>
    public interface IServer : ISerializable
    {
        /// <summary>
        ///     The game server's name.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     The game server's <see cref="Uri"/>.
        /// </summary>
        Uri Uri { get; }
    }
}