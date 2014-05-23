using System;
using System.Threading.Tasks;
using Moq;
using SfSdk.Constants;
using SfSdk.Contracts;
using SfSdk.Data;
using SfSdk.Response;
using Xunit;
using FluentAssertions;

namespace SfSdk.Tests.Data
{
    public class CharacterTests
    {
        private const string ValidUsername = "Foo";

        [Fact]
        public void ConstructorThrowsExceptionIfResponseIsNull()
        {
            // Arrange
            var sessionMock = new Mock<ISession>();
            Action sut = () => new Character(null, ValidUsername, sessionMock.Object, TestConstants.ValidServerUri);

            // Act / Assert
            sut.ShouldThrow<ArgumentNullException>().Where(e => e.ParamName == "response");
        }

        [Fact]
        public void ConstructorThrowsExceptionIfUsernameIsNull()
        {
            // Arrange
            var sessionMock = new Mock<ISession>();
            var characterResponseMock = new Mock<ICharacterResponse>();
            Action sut = () => new Character(characterResponseMock.Object, null, sessionMock.Object, TestConstants.ValidServerUri);

            // Act / Assert
            sut.ShouldThrow<ArgumentException>()
               .Where(e => e.ParamName == "username" && e.Message.StartsWith("Username must not be null or empty."));
        }

        [Fact]
        public void ConstructorThrowsExceptionIfUsernameIsEmpty()
        {
            // Arrange
            var sessionMock = new Mock<ISession>();
            var characterResponseMock = new Mock<ICharacterResponse>();
            Action sut = () => new Character(characterResponseMock.Object, string.Empty, sessionMock.Object, TestConstants.ValidServerUri);

            // Act / Assert
            sut.ShouldThrow<ArgumentException>()
               .Where(e => e.ParamName == "username" && e.Message.StartsWith("Username must not be null or empty."));
        }

        [Fact]
        public void ConstructorThrowsExceptionIfSessionIsNull()
        {
            // Arrange
            var characterResponseMock = new Mock<ICharacterResponse>();
            var savegameMock = new Mock<ISavegame>();
            characterResponseMock.Setup(cr => cr.Savegame).Returns(savegameMock.Object);
            Action sut = () => new Character(characterResponseMock.Object, ValidUsername, null, TestConstants.ValidServerUri);

            // Act / Assert
            sut.ShouldThrow<ArgumentNullException>()
               .Where(e => e.ParamName == "session");
        }

        [Fact]
        public void ConstructorThrowsExceptionIfResponseSavegameIsNull()
        {
            // Arrange
            var sessionMock = new Mock<ISession>();
            var characterResponseMock = new Mock<ICharacterResponse>();
            characterResponseMock.Setup(c => c.Savegame).Returns(null as Savegame);

            Action sut = () => new Character(characterResponseMock.Object, ValidUsername, sessionMock.Object, TestConstants.ValidServerUri);

            // Act / Assert
            sut.ShouldThrow<ArgumentException>()
               .Where(e => e.ParamName == "response" && e.Message.StartsWith("Character response must contain a savegame."));
        }

        [Fact]
        public void ConstructorThrowsExceptionIfCharacterClassIsLessThanOne()
        {
            // Arrange
            var sessionMock = new Mock<ISession>();
            var savegameMock = new Mock<ISavegame>();
            savegameMock.Setup(sg => sg.GetValue<int>(It.IsAny<SF>())).Returns(0);
            savegameMock.Setup(sg => sg.GetValue<int>(It.IsAny<int>())).Returns(0);
            savegameMock.Setup(sg => sg.GetValue(It.IsAny<SF>())).Returns(0);
            savegameMock.Setup(sg => sg.GetValue(It.IsAny<int>())).Returns(0);

            var characterResponseMock = new Mock<ICharacterResponse>();
            characterResponseMock.Setup(c => c.Savegame).Returns(savegameMock.Object);

            Action sut = () => new Character(characterResponseMock.Object, ValidUsername, sessionMock.Object, TestConstants.ValidServerUri);

            // Act / Assert
            sut.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void ConstructorThrowsExceptionIfCharacterClassIsGreaterThanThree()
        {
            // Arrange
            var sessionMock = new Mock<ISession>();
            var savegameMock = new Mock<ISavegame>();
            savegameMock.Setup(sg => sg.GetValue<int>(It.IsAny<SF>())).Returns(4);
            savegameMock.Setup(sg => sg.GetValue<int>(It.IsAny<int>())).Returns(4);
            savegameMock.Setup(sg => sg.GetValue(It.IsAny<SF>())).Returns(4);
            savegameMock.Setup(sg => sg.GetValue(It.IsAny<int>())).Returns(4);

            var characterResponseMock = new Mock<ICharacterResponse>();
            characterResponseMock.Setup(c => c.Savegame).Returns(savegameMock.Object);

            Action sut = () => new Character(characterResponseMock.Object, ValidUsername, sessionMock.Object, TestConstants.ValidServerUri);

            // Act / Assert
            sut.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void ConstructorDoesNotThrowExceptionIfResponseSavegamesValuesAreValid()
        {
            // TODO how to determine a "valid" savegame?
            // Arrange
            var sessionMock = new Mock<ISession>();
            var savegameMock = new Mock<ISavegame>();
            savegameMock.Setup(sg => sg.GetValue<int>(It.IsAny<SF>())).Returns(1);
            savegameMock.Setup(sg => sg.GetValue<int>(It.IsAny<int>())).Returns(1);
            savegameMock.Setup(sg => sg.GetValue(It.IsAny<SF>())).Returns(1);
            savegameMock.Setup(sg => sg.GetValue(It.IsAny<int>())).Returns(1);

            var characterResponseMock = new Mock<ICharacterResponse>();
            characterResponseMock.Setup(c => c.Savegame).Returns(savegameMock.Object);

            Action sut = () => new Character(characterResponseMock.Object, ValidUsername, sessionMock.Object, TestConstants.ValidServerUri);

            // Act / Assert
            sut.ShouldNotThrow<Exception>();
        }

        [Fact]
        public void Constructor2ThrowsExceptionIfUsernameIsNull()
        {
            // Arrange
            var sessionMock = new Mock<ISession>();
            Action sut = () => new Character(0, null, string.Empty, 0, 0, sessionMock.Object);

            // Act / Assert
            sut.ShouldThrow<ArgumentException>()
               .Where(e => e.ParamName == "username" && e.Message.StartsWith("Username must not be null or empty."));
        }

        [Fact]
        public void Constructor2ThrowsExceptionIfUsernameIsEmpty()
        {
            // Arrange
            var sessionMock = new Mock<ISession>();
            Action sut = () => new Character(0, string.Empty, string.Empty, 0, 0, sessionMock.Object);

            // Act / Assert
            sut.ShouldThrow<ArgumentException>()
               .Where(e => e.ParamName == "username" && e.Message.StartsWith("Username must not be null or empty."));
        }

        [Fact]
        public void Constructor2ThrowsExceptionIfSessionIsNull()
        {
            // Arrange
            Action sut = () => new Character(0, ValidUsername, string.Empty, 0, 0, null);

            // Act / Assert
            sut.ShouldThrow<ArgumentNullException>()
               .Where(e => e.ParamName == "session");
        }

        [Fact]
        public void Constructor2DoesNotThrowExceptionIfParametersAreValid()
        {
            // Arrange
            var sessionMock = new Mock<ISession>();
            Action sut = () => new Character(0, ValidUsername, string.Empty, 0, 0, sessionMock.Object);

            // Act / Assert
            sut.ShouldNotThrow<Exception>();
        }

        [Fact]
        public async Task RefreshRequestsNoDataIfCharacterIsLoadedAndForceIsFalse()
        {
            // Arrange
            var sessionMock = new Mock<ISession>();

            var savegameMock = new Mock<ISavegame>();
            savegameMock.Setup(sg => sg.GetValue<int>(It.IsAny<SF>())).Returns(1);
            savegameMock.Setup(sg => sg.GetValue<int>(It.IsAny<int>())).Returns(1);
            savegameMock.Setup(sg => sg.GetValue(It.IsAny<SF>())).Returns(1);
            savegameMock.Setup(sg => sg.GetValue(It.IsAny<int>())).Returns(1);

            var characterResponseMock = new Mock<ICharacterResponse>();
            characterResponseMock.Setup(c => c.Savegame).Returns(savegameMock.Object);

            var sut = new Character(characterResponseMock.Object, ValidUsername, sessionMock.Object, TestConstants.ValidServerUri);

            // Act
            await sut.Refresh();

            // Assert
            sessionMock.Verify(s => s.RequestCharacterAsync(It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public async Task RefreshRequestsDataIfCharacterIsLoadedAndForceIsTrue()
        {
            // Arrange
            var characterMock = new Mock<ICharacter>();

            var sessionMock = new Mock<ISession>();
            sessionMock.Setup(s => s.RequestCharacterAsync(It.IsAny<string>()))
                       .ReturnsAsync(characterMock.Object)
                       .Verifiable();

            var savegameMock = new Mock<ISavegame>();
            savegameMock.Setup(sg => sg.GetValue<int>(It.IsAny<SF>())).Returns(1);
            savegameMock.Setup(sg => sg.GetValue<int>(It.IsAny<int>())).Returns(1);
            savegameMock.Setup(sg => sg.GetValue(It.IsAny<SF>())).Returns(1);
            savegameMock.Setup(sg => sg.GetValue(It.IsAny<int>())).Returns(1);

            var characterResponseMock = new Mock<ICharacterResponse>();
            characterResponseMock.Setup(c => c.Savegame).Returns(savegameMock.Object);

            var sut = new Character(characterResponseMock.Object, ValidUsername, sessionMock.Object, TestConstants.ValidServerUri);

            // Act
            await sut.Refresh(force: true);

            // Assert
            sessionMock.Verify(s => s.RequestCharacterAsync(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task RefreshRequestsDataIfCharacterIsNotLoaded()
        {
            // Arrange
            var characterMock = new Mock<ICharacter>();
            var sessionMock = new Mock<ISession>();
            sessionMock.Setup(s => s.RequestCharacterAsync(It.IsAny<string>()))
                       .ReturnsAsync(characterMock.Object)
                       .Verifiable();
            var sut = new Character(0, ValidUsername, string.Empty, 0, 0, sessionMock.Object);

            // Act
            await sut.Refresh();

            // Assert
            // Data should only be requested once, as the request was not forced
            sessionMock.Verify(s => s.RequestCharacterAsync(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task RefreshRequestsNoDataIfCharacterIsLoadedAndForceIsFalse2()
        {
            // Arrange
            var characterMock = new Mock<ICharacter>();
            var sessionMock = new Mock<ISession>();
            sessionMock.Setup(s => s.RequestCharacterAsync(It.IsAny<string>()))
                       .ReturnsAsync(characterMock.Object)
                       .Verifiable();
            var sut = new Character(0, ValidUsername, string.Empty, 0, 0, sessionMock.Object);

            // Act
            await sut.Refresh();
            await sut.Refresh();

            // Assert
            // Data should only be requested once, as the request was not forced
            sessionMock.Verify(s => s.RequestCharacterAsync(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task RefreshRequestsDataIfCharacterIsLoadedAndForceIsTrue2()
        {

            // Arrange
            var characterMock = new Mock<ICharacter>();
            var sessionMock = new Mock<ISession>();
            sessionMock.Setup(s => s.RequestCharacterAsync(It.IsAny<string>()))
                       .ReturnsAsync(characterMock.Object)
                       .Verifiable();
            var sut = new Character(0, ValidUsername, string.Empty, 0, 0, sessionMock.Object);

            // Act
            await sut.Refresh();
            await sut.Refresh(force: true);

            // Assert
            // Data should be requested two times, due to the second request was forced
            sessionMock.Verify(s => s.RequestCharacterAsync(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task RefreshSetsPropertiesToBeUpdatedIfNewDataIsReceived()
        {
            // Arrange
            var characterMock = new Mock<ICharacter>();
            // Setup difference
            characterMock.Setup(c => c.Rank).Returns(1);
            
            var sessionMock = new Mock<ISession>();
            sessionMock.Setup(s => s.RequestCharacterAsync(It.IsAny<string>()))
                       .ReturnsAsync(characterMock.Object);

            var sut = new Character(0, ValidUsername, string.Empty, 0, 0, sessionMock.Object);
            sut.MonitorEvents();

            // Act
            await sut.Refresh();

            // Assert
            sut.ShouldRaise("PropertyChanged");
        }
    }
}