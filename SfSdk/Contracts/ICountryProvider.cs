using System.Collections.Generic;
using System.Threading.Tasks;

namespace SfSdk.Contracts
{
    public interface ICountryProvider
    {
        /// <summary>
        ///     Returns all countries where S & F is available.
        /// </summary>
        /// <param name="forceRefresh">Indicates whether the server shall be requeried or not.</param>
        /// <returns>A cached list of ICountry after the first execution.</returns>
        Task<IEnumerable<ICountry>> GetCountriesAsync(bool forceRefresh = false);
    }
}