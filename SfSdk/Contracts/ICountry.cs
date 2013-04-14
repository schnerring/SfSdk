using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SfSdk.Contracts
{
    /// <summary>
    ///     Contains information about a country where S&amp;F is available.
    /// </summary>
    public interface ICountry : ISerializable
    {
        /// <summary>
        ///     The country's name.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     The country's server <see cref="Uri" />.
        /// </summary>
        Uri Uri { get; }

        /// <summary>
        ///     The country's subservers. See: <seealso cref="IServer" />.
        /// </summary>
        IList<IServer> Servers { get; }
    }
}