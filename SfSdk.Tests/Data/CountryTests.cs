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
            Func<Task<ICountry>> sut =
                async () => await Country.CreateAsync("Germany", TestConstants.ValidCountryServerUri);

            // Act
            var country = await sut();

            // Assert
            country.Should().NotBeNull();
            country.Name.Should().Be("Germany");
            country.Uri.Should().Be(TestConstants.ValidCountryServerUri);
        }

        [Fact]
        public async Task CreateAsyncReturnsICountryWithNameEqualNull()
        {
            // Arrange
            Func<Task<ICountry>> sut = async () => await Country.CreateAsync(null, TestConstants.ValidCountryServerUri);

            // Act
            var country = await sut();

            // Assert
            country.Should().NotBeNull();
            country.Name.Should().Be(TestConstants.ValidCountryServerUri.AbsoluteUri);
            country.Uri.Should().Be(TestConstants.ValidCountryServerUri);
        }
    }
}