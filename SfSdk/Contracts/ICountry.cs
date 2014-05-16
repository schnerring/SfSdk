// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICountry.cs" company="">
//   
// </copyright>
// <summary>
//   Contains information about a country where S&F is available.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SfSdk.Contracts
{
    using System.Collections.Generic;

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