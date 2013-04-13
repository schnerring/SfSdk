using System;
using System.Threading.Tasks;
using FluentAssertions;
using SfSdk.Constants;
using SfSdk.ResponseData;
using Xunit;

namespace SfSdk.Tests
{
    public class RequestTests : IUseFixture<TestAccountFixture>
    {
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
        public void ConstructorThrowsExceptionIfSessionIdIsNull()
        {
            // Arrange
            Action a = () => new Request(null, _serverUri, SF.ActAccountCreate);

            // Act / Assert
            a.ShouldThrow<ArgumentNullException>().Where(e => e.ParamName == "sessionId");
        }

        [Fact]
        public void ConstructorThrowsExceptionIfServerUriIsNull()
        {
            // Arrange
            Action a = () => new Request(EmptySessionId, null, SF.ActAccountCreate);

            // Act / Assert
            a.ShouldThrow<ArgumentNullException>().Where(e => e.ParamName == "serverUri");
        }

        [Fact]
        public void ConstructorThrowsExceptionIfSessionIdHasInvalidLength()
        {
            // Arrange
            const string invalidSessionid = "000";
            Action a = () => new Request(invalidSessionid, _serverUri, SF.ActAccountCreate);

            // Act / Assert
            a.ShouldThrow<ArgumentException>().Where(e => e.ParamName == "sessionId");
        }

        [Fact]
        public async Task ExecuteAsyncReturnsIResultIfSuccessful()
        {
            // Arrange
            // Login in this case
            var loginRequest = new Request(EmptySessionId, _serverUri, SF.ActLogin,
                                           new[] {_username, _passwordHash, "v1.70&random=%2"});

            // Act
            var result = await loginRequest.ExecuteAsync();
            
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
            var loginRequest = new Request(EmptySessionId, _serverUri, SF.ActLogin,
                                           new[] { _username, _passwordHash, "v1.70&random=%2" });

            // Act
            var result = await loginRequest.ExecuteAsync();
            var oldSessionId = ((LoginResponse) result.Response).SessionId;

            // to request a new session id, so the old one expires
            await loginRequest.ExecuteAsync();

            var otherRequest = new Request(oldSessionId, _serverUri, SF.ActScreenChar);
            result = await otherRequest.ExecuteAsync();

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
            var request = new Request(EmptySessionId, _serverUri, SF.ActLogin,
                                      new[] { _username, _passwordHash, "v1.70&random=%2" });

            // Act / Assert
            await TestHelpers.ThrowsAsync<NotImplementedException>(async () => await request.ExecuteAsync());

            TestHelpers.Connect();
        }
    }
}