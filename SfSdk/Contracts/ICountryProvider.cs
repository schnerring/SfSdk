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
        ///     Returns all the countries from the Servers file.
        /// </summary>
        /// <returns>A <see cref="IEnumerable{T}"/> where T: <see cref="ICountry"/>.</returns>
        Task<IEnumerable<ICountry>> GetCountriesAsync();
    }
}