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

            // Act
            var loginSucceded =
                await
                sut.LoginAsync(TestConstants.ValidUsername, TestConstants.ValidPasswordHash,
                               TestConstants.ValidServerUri);

            // Assert
            loginSucceded.Should().BeTrue();
        }

        [Fact]
        public async Task LoginAsyncReturnsFalseIfLoginFails()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());

            // Act
            var loginSucceded =
                await
                sut.LoginAsync(TestConstants.ValidUsername, TestConstants.InvalidPasswordHash,
                               TestConstants.ValidServerUri);

            // Assert
            loginSucceded.Should().BeFalse();
        }

        [Fact]
        public void LogoutAsyncThrowsExceptionIfSessionIsNotLoggedIn()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            Func<Task> a = async () => await sut.LogoutAsync();

            // Act / Assert
            a.ShouldThrow<SessionLoggedOutException>()
             .Where(e => e.Message == "LogoutAsync requires to be logged in.");
        }

        [Fact]
        public async Task LogoutAsyncReturnsTrueIfSessionIsLoggedIn()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            await sut.LoginAsync(TestConstants.ValidUsername,
                                 TestConstants.ValidPasswordHash,
                                 TestConstants.ValidServerUri);

            // Act
            var logoutSucceeded = await sut.LogoutAsync();

            // Assert
            // TODO how to test the false case?
            logoutSucceeded.Should().BeTrue();
        }

        [Fact]
        public void MyCharacterAsyncThrowsExceptionIfSessionIsNotLoggedIn()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            Func<Task> a = async () => await sut.MyCharacterAsync();

            // Act / Assert
            a.ShouldThrow<SessionLoggedOutException>()
             .Where(e => e.Message == "MyCharacterAsync requires to be logged in.");
        }

        [Fact]
        public async Task MyCharacterAsyncReturnsICharacterIfSessionIsLoggedIn()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            await sut.LoginAsync(TestConstants.ValidUsername,
                                 TestConstants.ValidPasswordHash,
                                 TestConstants.ValidServerUri);

            // Act
            var myCharacter = await sut.MyCharacterAsync();

            // Assert
            // TODO how to test null value for Character
            myCharacter.Should().NotBeNull();
        }

        [Fact]
        public void RequestCharacterAsyncThrowsExceptionIfUsernameIsNull()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            Func<Task> a = async () => await sut.RequestCharacterAsync(null);

            // Act / Assert
            a.ShouldThrow<ArgumentNullException>()
             .Where(e => e.ParamName == "username");
        }

        [Fact]
        public void RequestCharacterAsyncThrowsExceptionIfSessionIsNotLoggedIn()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            Func<Task> a = async () => await sut.RequestCharacterAsync("Foo");

            // Act / Assert
            a.ShouldThrow<SessionLoggedOutException>()
             .Where(e => e.Message == "RequestCharacterAsync requires to be logged in.");
        }

        [Fact]
        public async Task RequestCharacterAsyncReturnsICharacterIfSessionIsLoggedIn()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            await sut.LoginAsync(TestConstants.ValidUsername,
                                 TestConstants.ValidPasswordHash,
                                 TestConstants.ValidServerUri);

            // Act
            var myCharacter = await sut.RequestCharacterAsync("Foo");

            // Assert
            // TODO how to test null value for Character
            myCharacter.Should().NotBeNull();
        }

        [Fact]
        public void HallOfFameAsyncThrowsExceptionIfSessionIsNotLoggedIn()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            Func<Task> a = async () => await sut.HallOfFameAsync();

            // Act / Assert
            a.ShouldThrow<SessionLoggedOutException>()
             .Where(e => e.Message == "HallOfFameAsync requires to be logged in.");
        }

        [Fact]
        public async Task HallOfFameAsyncReturnsShouldReturn15ICharactersIfSessionIsLoggedIn()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            await sut.LoginAsync(TestConstants.ValidUsername,
                                 TestConstants.ValidPasswordHash,
                                 TestConstants.ValidServerUri);

            // Act
            var characters = await sut.HallOfFameAsync();

            // Assert
            // TODO how to test null value for Character
            characters.Should().HaveCount(c => c == 15);
        }

        // TODO: private code paths?
    }
}