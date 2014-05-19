using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SfSdk.Contracts;
using SfSdk.Data;

namespace SfSdk.Providers
{
    /// <summary>
    ///     A service to receive information about countries where S&amp;F is available.
    /// </summary>
    public class CountryProvider : ICountryProvider
    {
        /// <summary>
        ///     Returns all the countries from the Servers file.
        /// </summary>
        /// <returns>A <see cref="IEnumerable{T}" /> where T: <see cref="ICountry" />.</returns>
        public async Task<IEnumerable<ICountry>> GetCountriesAsync()
        {
            string jsonData = File.ReadAllText("Servers.json");
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<List<Country>>(jsonData));
        }
    }
}