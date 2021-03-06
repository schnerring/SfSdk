using System.Collections.Generic;

namespace SfSdk.Contracts
{
    /// <summary>
    ///     Contains information about a country where S&amp;F is available.
    /// </summary>
    public interface ICountry
    {
        /// <summary>
        ///     Gets the country's name
        /// </summary>
        string CountryName { get; }

        /// <summary>
        ///     Gets the servers.
        /// </summary>
        IList<IServer> Servers { get; }
    }
}