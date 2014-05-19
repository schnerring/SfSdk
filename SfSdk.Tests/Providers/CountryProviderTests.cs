using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using SfSdk.Contracts;
using SfSdk.Providers;
using Xunit;

namespace SfSdk.Tests.Providers
{
    public class CountryProviderTests
    {
        [Fact] // Needs internet
        public async Task GetCountriesAsyncReturnsCountries()
        {
            // Arrange / Act
            var countries = await new CountryProvider().GetCountriesAsync();

            // Assert
            countries.Count().Should().BeGreaterOrEqualTo(1);
        }

        [Fact] // Needs internet
        public async Task CountriesCollectionContainsProperNames()
        {
            // Arrange
            const string MustContain = "Deutschland";

            // Act
            IEnumerable<ICountry> countries = await new CountryProvider().GetCountriesAsync();

            // Assert
            countries.FirstOrDefault(c => c.CountryName == MustContain).Should().NotBeNull();
        }

        [Fact]
        public async Task CountryShouldContainServers()
        {
            // Arrange / Act
            IEnumerable<ICountry> countries = await new CountryProvider().GetCountriesAsync();

            // Assert
            countries.First().Servers.Count.Should().BeGreaterOrEqualTo(1);
        }
    }
}