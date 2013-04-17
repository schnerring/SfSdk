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
            var sut = new SnFRequestSource(_serverUri);

            // Act
            // Login in this case
            SfResponse result =
                await sut.RequestAsync(EmptySessionId, SF.ActLogin, new[] {_username, _passwordHash, "v1.70&random=%2"});

            // Assert
            result.Should().NotBeNull();
            result.Response.Should().NotBeNull();
            result.Errors.Should().HaveCount(count => count == 0);
        }

        [Fact]
        public async Task ExecuteAsyncReturnsErrorsIfUnsuccessful()
        {
            // Arrange
            var sut = new SnFRequestSource(_serverUri);
            // Login in this case
            Func<Task<SfResponse>> login =
                async () =>
                await sut.RequestAsync(EmptySessionId, SF.ActLogin, new[] { _username, _passwordHash, "v1.70&random=%2" });

            // Act
            SfResponse result = await login();
            string oldSessionId = ((LoginResponse) result.Response).SessionId;

            // to request a new session id, so the old one expires
            await login();

            result = await sut.RequestAsync(oldSessionId, SF.ActScreenChar);

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
            var sut = new SnFRequestSource(_serverUri);

            // Act / Assert
            // Login in this case
            await
                TestHelpers.ThrowsAsync<NotImplementedException>(
                    async () =>
                    await
                    sut.RequestAsync(EmptySessionId, SF.ActLogin, new[] {_username, _passwordHash, "v1.70&random=%2"}));

            TestHelpers.Connect();
        }
    }
}