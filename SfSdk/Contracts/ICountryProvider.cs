using System.Collections.Generic;
using System.Threading.Tasks;

namespace SfSdk.Contracts
{
    /// <summary>
    ///     A service to receive information about the countries where S&amp;F is available.
    /// </summary>
    public interface ICountryProvider
    {
        /// <summary>
        ///     Returns all the countries where S&amp;F is available.
        /// </summary>
        /// <param name="forceRefresh">Indicates whether the <see cref="ICountry"/>'s details shall be re-requested or the cached results shall be returned.</param>
        /// <returns>A <see cref="IEnumerable{T}"/> where T: <see cref="ICountry"/>.</returns>
        Task<IEnumerable<ICountry>> GetCountriesAsync(bool forceRefresh = false);
    }
}