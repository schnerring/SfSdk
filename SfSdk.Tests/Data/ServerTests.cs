using System;
using System.Threading.Tasks;
using Moq;
using SfSdk.Contracts;
using SfSdk.Data;
using Xunit;

namespace SfSdk.Tests.Data
{
    public class ServerTests
    {
        private const string ValidUrl = "http://www.google.com/";

        [Fact]
        public async Task CreateServersAsyncThrowsExceptionIfCountryIsNull()
        {
            // Arrange / Act / Assert
            await TestHelpers.ThrowsAsync<ArgumentNullException>(async () => await Server.CreateServersAsync(null));
        }

        [Fact]
        public async Task CreateServersAsyncThrowsExceptionIfCountrysUriIsNull()
        {
            // Arrange
            var countryMock = new Mock<ICountry>();
            countryMock.Setup(c => c.Uri).Returns(null as Uri);

            // Act / Assert
            await TestHelpers.ThrowsAsync<ArgumentNullException>(async () => await Server.CreateServersAsync(countryMock.Object));
        }

        [Fact]
        public async Task CreateServersAsyncThrowsNoExceptionWithValidArguments()
        {
            // Arrange
            var countryMock = new Mock<ICountry>();
            countryMock.Setup(c => c.Uri).Returns(new Uri(ValidUrl));

            // Act / Assert
            await TestHelpers.ThrowsNotAsync(async () => await Server.CreateServersAsync(countryMock.Object));
        }
    }
}