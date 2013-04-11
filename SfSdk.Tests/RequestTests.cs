using System;
using System.Threading.Tasks;
using FluentAssertions;
using SfSdk.Enums;
using Xunit;

namespace SfSdk.Tests
{
    public class RequestTests : IUseFixture<TestAccountFixture>
    {
        private const string EmptySessionId = "00000000000000000000000000000000";
        private string _passwordHash;
        private Uri _serverUri;
        private string _username;

        public void SetFixture(TestAccountFixture data)
        {
            _username = data.Username;
            _passwordHash = data.PasswordHash;
            _serverUri = data.ServerUri;
        }

        [Fact]
        public void ConstructorThrowsExceptionIfSessionIdIsNull()
        {
            // Arrange / Act
            Action a = () => new Request(null, _serverUri, SfAction.Album);

            // Assert
            a.ShouldThrow<ArgumentNullException>().Where(e => e.ParamName == "sessionId");
        }

        [Fact]
        public void ConstructorThrowsExceptionIfServerUriIsNull()
        {
            // Arrange / Act
            Action a = () => new Request(EmptySessionId, null, SfAction.Album);

            // Assert
            a.ShouldThrow<ArgumentNullException>().Where(e => e.ParamName == "serverUri");
        }

        [Fact]
        public void ConstructorThrowsExceptionIfSessionIdHasInvalidLength()
        {
            // Arrange / Act
            const string invalidSessionid = "000";
            Action a = () => new Request(invalidSessionid, _serverUri, SfAction.Album);

            // Assert
            a.ShouldThrow<ArgumentException>().Where(e => e.ParamName == "sessionId");
        }

        [Fact(Skip = "Interrupts Network connection, reactivate when needed")]
        public async Task ExecuteAsyncThrowsExceptionWithoutNetworkConnection()
        {
            TestHelpers.Disconnect();

            // Arrange
            // Login in this case
            var request = new Request("00000000000000000000000000000000", _serverUri, SfAction.Login,
                                      new[] { _username, _passwordHash, "v1.70&random=%2" });

            // Act / Assert
            await TestHelpers.ThrowsAsync<NotImplementedException>(async () => await request.ExecuteAsync());

            TestHelpers.Connect();
        }
    }
}