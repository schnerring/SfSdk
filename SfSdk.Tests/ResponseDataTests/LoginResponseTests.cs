using System.Security.Authentication;
using System.Threading.Tasks;
using FluentAssertions;
using SfSdk.ResponseData;
using Xunit;

namespace SfSdk.Tests.ResponseDataTests
{
    public class LoginResponseTests : IUseFixture<LoginResponseFixture>
    {
        private Task<Session> _session;

        public void SetFixture(LoginResponseFixture data)
        {
            _session = data.LoginAsync();
        }

        [Fact]
        public async void Test1()
        {
            // Arrange
            var s = await _session;

            // Act
//            var sut = new LoginResponse();
//            s.Gold.Should().BeGreaterOrEqualTo(0);
        }
    }

    public class LoginResponseFixture : TestAccountFixture
    {
        public async Task<Session> LoginAsync()
        {
            var session = new Session();
            var loginSuccessful = await session.LoginAsync(Username, PasswordHash, ServerUri);
            if (!loginSuccessful)
                throw new InvalidCredentialException(
                    "Please check the server status and the credentials in your TestAccount.txt file.");
            return session;
        }
    }
}