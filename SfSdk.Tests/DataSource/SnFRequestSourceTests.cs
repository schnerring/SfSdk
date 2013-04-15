using System;
using System.Threading.Tasks;
using FluentAssertions;
using SfSdk.Constants;
using SfSdk.DataSource;
using SfSdk.Request;
using SfSdk.Response;
using Xunit;

namespace SfSdk.Tests.DataSource
{
    public class SnFRequestSourceTests : IUseFixture<TestAccountFixture>
    {
        // TODO implement tests to check returned data for every implemented action.

        private const string EmptySessionId = "00000000000000000000000000000000";
        private string _passwordHash;
        private Uri _serverUri;
        private string _username;

        public void SetFixture(TestAccountFixture f)
        {
            _username = f.Username;
            _passwordHash = f.PasswordHash;
            _serverUri = f.ServerUri;
        }

        [Fact]
        public async Task ExecuteAsyncReturnsIResultIfSuccessful()
        {
            // Arrange
            // Login in this case
            var uriFactory = new SnFUriFactory(EmptySessionId, _serverUri, SF.ActLogin,
                                               new[] {_username, _passwordHash, "v1.70&random=%2"});
            var sut = new SnFRequestSource(uriFactory);

            // Act
            SfResponse result = await sut.RequestAsync();

            // Assert
            result.Should().NotBeNull();
            result.Response.Should().NotBeNull();
            result.Errors.Should().HaveCount(count => count == 0);
        }

        [Fact]
        public async Task ExecuteAsyncReturnsErrorsIfUnsuccessful()
        {
            // Arrange
            // Login in this case
            var uriFactory = new SnFUriFactory(EmptySessionId, _serverUri, SF.ActLogin,
                                                   new[] {_username, _passwordHash, "v1.70&random=%2"});
            var sut = new SnFRequestSource(uriFactory);

            // Act
            SfResponse result = await sut.RequestAsync();
            string oldSessionId = ((LoginResponse) result.Response).SessionId;

            // to request a new session id, so the old one expires
            await sut.RequestAsync();

            var otherUriFactory = new SnFUriFactory(oldSessionId, _serverUri, SF.ActScreenChar);
            sut = new SnFRequestSource(otherUriFactory);
            result = await sut.RequestAsync();

            // Assert
            result.Should().NotBeNull();
            result.Response.Should().BeNull();
            result.Errors.Should().HaveCount(count => count > 0);
        }

        [Fact(Skip = "Interrupts Network connection, reactivate when needed")]
        public async Task ExecuteAsyncThrowsExceptionWithoutNetworkConnection()
        {
            TestHelpers.Disconnect();

            // Arrange
            // Login in this case
            var uriFactory = new SnFUriFactory(EmptySessionId, _serverUri, SF.ActLogin,
                                                   new[] {_username, _passwordHash, "v1.70&random=%2"});
            var sut = new SnFRequestSource(uriFactory);

            // Act / Assert
            await TestHelpers.ThrowsAsync<NotImplementedException>(async () => await sut.RequestAsync());

            TestHelpers.Connect();
        }
    }
}