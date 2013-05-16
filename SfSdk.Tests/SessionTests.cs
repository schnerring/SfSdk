using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace SfSdk.Tests
{
    public class SessionTests
    {
        [Fact]
        public void LoginAsyncThrowsExceptionIfUsernameIsNull()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            Func<Task> a = async () => await sut.LoginAsync(null, null, null);

            // Act / Assert
            a.ShouldThrow<ArgumentException>()
             .Where(e => e.ParamName == "username" && e.Message.StartsWith("Username must not be null or empty."));
        }

        [Fact]
        public void LoginAsyncThrowsExceptionIfUsernameIsEmpty()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            Func<Task> a = async () => await sut.LoginAsync(string.Empty, null, null);

            // Act / Assert
            a.ShouldThrow<ArgumentException>()
             .Where(e => e.ParamName == "username" && e.Message.StartsWith("Username must not be null or empty."));
        }

        [Fact]
        public void LoginAsyncThrowsExceptionIfPasswordHashIsNull()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            Func<Task> a = async () => await sut.LoginAsync(TestConstants.ValidUsername, null, null);

            // Act / Assert
            a.ShouldThrow<ArgumentException>()
             .Where(
                 e =>
                 e.ParamName == "md5PasswordHash" &&
                 e.Message.StartsWith("Password hash must not be null and have a length of 32."));
        }

        [Fact]
        public void LoginAsyncThrowsExceptionIfPasswordHashHasNotALengthOf32()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            Func<Task> a = async () => await sut.LoginAsync(TestConstants.ValidUsername, "a", null);

            // Act / Assert
            a.ShouldThrow<ArgumentException>()
             .Where(
                 e =>
                 e.ParamName == "md5PasswordHash" &&
                 e.Message.StartsWith("Password hash must not be null and have a length of 32."));
        }

        [Fact]
        public void LoginAsyncThrowsExceptionIfServerUriIsNull()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            Func<Task> a =
                async () => await sut.LoginAsync(TestConstants.ValidUsername, TestConstants.ValidPasswordHash, null);

            // Act / Assert
            a.ShouldThrow<ArgumentNullException>()
             .Where(e => e.ParamName == "serverUri");
        }

        [Fact]
        public async Task LoginAsyncReturnsTrueIfLoginSucceeds()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());

            // Act / Assert
            var loginSucceded =
                await
                sut.LoginAsync(TestConstants.ValidUsername, TestConstants.ValidPasswordHash,
                               TestConstants.ValidServerUri);

            // Assert
            loginSucceded.Should().BeTrue();
        }

        [Fact]
        public async Task LoginAsyncReturnsTrueIfLoginFails()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());

            // Act / Assert
            var loginSucceded =
                await
                sut.LoginAsync(TestConstants.ValidUsername, TestConstants.InvalidPasswordHash,
                               TestConstants.ValidServerUri);

            // Assert
            loginSucceded.Should().BeFalse();
        }

        // TODO: LogoutAsync, MyCharacterAsync, RequestCharacterAsync, HallOfFameAsync
    }
}