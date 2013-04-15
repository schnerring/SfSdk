using System;
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
        [Fact]
        public async Task GetCountriesAsyncReturns25Countries()
        {
            // Arrange / Act
            IEnumerable<ICountry> countries = await new CountryProvider().GetCountriesAsync();

            // Assert
            countries.Should().HaveCount(25);
        }

        [Fact]
        public async Task CountriesCollectionContainsProperNames()
        {
            // Arrange
            var countryNames = new[]
                {
                    "Deutschland",
                    "Poland",
                    "Czech Republic",
                    "France",
                    "Spain",
                    "Hungary",
                    "United States of America",
                    "Italy",
                    "Portugal",
                    "Greece",
                    "Turkey",
                    "Netherlands",
                    "United Arabian Emirates",
                    "Brazil",
                    "Chile",
                    "Denmark",
                    "Great Britain",
                    "Sweden",
                    "Canada",
                    "India",
                    "Japan",
                    "Mexico",
                    "Rumania",
                    "Russia",
                    "Slovakia"
                };

            // Act
            IEnumerable<ICountry> countries = await new CountryProvider().GetCountriesAsync();

            // Assert
            countries.Select(c => c.Name).Should().Contain(countryNames);
        }

        [Fact]
        public async Task CountriesCollectionContainsProperUris()
        {
            // Arrange
            IEnumerable<Uri> countryUris = new[]
                {
                    "http://www.sfgame.de/",
                    "http://www.sfgame.pl/",
                    "http://www.sfgame.cz/",
                    "http://www.sfgame.fr/",
                    "http://www.sfgame.es/",
                    "http://www.sfgame.hu/",
                    "http://www.sfgame.us/",
                    "http://www.sfgame.it/",
                    "http://www.sfgame.com.pt/",
                    "http://www.sfgame.gr/",
                    "http://www.sfgame.web.tr/",
                    "http://www.sfgame.nl/",
                    "http://www.sfgame.ae/",
                    "http://www.sfgame.com.br/",
                    "http://www.sfgame.cl/",
                    "http://www.sfgame.dk/",
                    "http://www.sfgame.co.uk/",
                    "http://www.sfgame.se/",
                    "http://www.sfgame.ca/",
                    "http://www.sfgame.in/",
                    "http://www.sfgame.jp/",
                    "http://www.sfgame.mx/",
                    "http://www.sfgame.ro/",
                    "http://www.sfgame.ru/",
                    "http://www.sfgame.sk/"
                }.Select(url => new UriBuilder(url).Uri);

            // Act
            IEnumerable<ICountry> countries = await new CountryProvider().GetCountriesAsync();

            // Assert
            countries.Select(c => c.Uri).Should().Contain(countryUris);
        }

        [Fact(Skip = "Interrupts Network connection, reactivate when needed")]
        public async Task GetCountriesAsyncThrowsExceptionWithoutNetworkConnection()
        {
            // Arrange
            TestHelpers.Disconnect();
            var provider = new CountryProvider();

            // Act / Assert
            await TestHelpers.ThrowsAsync<NotImplementedException>(async () => await provider.GetCountriesAsync());
            TestHelpers.Connect();
        }
    }
}