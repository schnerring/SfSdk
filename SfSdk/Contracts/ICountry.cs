using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SfSdk.Contracts
{
    public interface ICountry : ISerializable
    {
        /// <summary>
        ///     The country's name.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     The country's server URI.
        /// </summary>
        Uri Uri { get; }

        /// <summary>
        ///     The countries game servers.
        /// </summary>
        IList<IServer> Servers { get; }
    }
}