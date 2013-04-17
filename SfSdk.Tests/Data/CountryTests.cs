using System;
using System.Threading.Tasks;
using SfSdk.Contracts;
using SfSdk.Data;
using Xunit;
using FluentAssertions;

namespace SfSdk.Tests.Data
{
    public class CountryTests
    {
        [Fact]
        public void CreateAsyncThrowsExceptionIfUriIsNull()
        {
            // Arrange
            Func<Task> sut = async () => await Country.CreateAsync(null, null);

            // Act / Assert
            sut.ShouldThrow<ArgumentNullException>().Where(e => e.ParamName == "uri");
        }

        [Fact]
        public async Task CreateAsyncReturnsICountryWithValidParameters()
        {
            // Arrange
            var validUri = new UriBuilder("http://www.sfgame.de/").Uri;
            Func<Task<ICountry>> sut = async () => await Country.CreateAsync("Germany", validUri);

            // Act
            var country = await sut();

            // Assert
            country.Should().NotBeNull();
            country.Name.Should().Be("Germany");
            country.Uri.Should().Be(validUri);
        }

        [Fact]
        public async Task CreateAsyncReturnsICountryWithNameEqualNull()
        {
            // Arrange
            var validUri = new UriBuilder("http://www.sfgame.de/").Uri;
            Func<Task<ICountry>> sut = async () => await Country.CreateAsync(null, validUri);

            // Act
            var country = await sut();

            // Assert
            country.Should().NotBeNull();
            country.Name.Should().Be(validUri.AbsoluteUri);
            country.Uri.Should().Be(validUri);
        }
    }
}