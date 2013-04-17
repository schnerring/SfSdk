using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using SfSdk.Contracts;
using SfSdk.Data;
using Xunit;
using FluentAssertions;

namespace SfSdk.Tests.Data
{
    public class ServerTests
    {
        private const string ValidUrl = "http://www.sfgame.de/";

        [Fact]
        public void CreateServersAsyncThrowsExceptionIfCountryIsNull()
        {
            // Arrange
            Func<Task> sut = async () => await Server.CreateServersAsync(null);
            
            // Act / Assert
            sut.ShouldThrow<ArgumentNullException>().Where(e => e.ParamName == "country");
        }

        [Fact]
        public void CreateServersAsyncThrowsExceptionIfCountrysUriIsNull()
        {
            // Arrange
            var countryMock = new Mock<ICountry>();
            countryMock.Setup(c => c.Uri).Returns(null as Uri);
            Func<Task> sut = async () => await Server.CreateServersAsync(countryMock.Object);

            // Act / Assert
            sut.ShouldThrow<ArgumentException>()
               .Where(e => e.Message.StartsWith("The country's URI must not be null.") && e.ParamName == "country");
        }

        [Fact]
        public async Task CreateServersAsyncReturnsResultsWithValidArguments()
        {
            // Arrange
            var countryMock = new Mock<ICountry>();
            countryMock.Setup(c => c.Uri).Returns(new Uri(ValidUrl));
            Func<Task<IEnumerable<IServer>>> sut = async () => await Server.CreateServersAsync(countryMock.Object);

            // Act
            var servers = await sut();

            // Assert
            servers.Should().HaveCount(c => c > 0);
        }
    }
}