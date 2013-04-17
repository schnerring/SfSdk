using System;
using System.Threading.Tasks;
using Moq;
using SfSdk.Constants;
using SfSdk.Data;
using SfSdk.Response;
using Xunit;
using FluentAssertions;

namespace SfSdk.Tests.Data
{
    public class CharacterTests
    {
        [Fact]
        public void ConstructorThrowsExceptionIfResponseIsNull()
        {
            // Arrange
            Action sut = () => new Character(null);

            // Act / Assert
            sut.ShouldThrow<ArgumentNullException>().Where(e => e.ParamName == "response");
        }

        [Fact]
        public void ConstructorThrowsExceptionIfResponseSavegameIsNull()
        {
            // Arrange
            var characterResponseMock = new Mock<ICharacterResponse>();
            characterResponseMock.Setup(c => c.Savegame).Returns(null as Savegame);

            Action sut = () => new Character(characterResponseMock.Object);

            // Act / Assert
            sut.ShouldThrow<ArgumentException>()
               .Where(e => e.ParamName == "response" && e.Message.StartsWith("Character response must contain a savegame."));
        }

        [Fact]
        public void ConstructorThrowsExceptionIfCharacterClassIsLessThanOne()
        {
            // Arrange
            var savegameMock = new Mock<ISavegame>();
            savegameMock.Setup(sg => sg.GetValue<int>(It.IsAny<SF>())).Returns(0);
            savegameMock.Setup(sg => sg.GetValue<int>(It.IsAny<int>())).Returns(0);
            savegameMock.Setup(sg => sg.GetValue(It.IsAny<SF>())).Returns(0);
            savegameMock.Setup(sg => sg.GetValue(It.IsAny<int>())).Returns(0);

            var characterResponseMock = new Mock<ICharacterResponse>();
            characterResponseMock.Setup(c => c.Savegame).Returns(savegameMock.Object);

            Action sut = () => new Character(characterResponseMock.Object);

            // Act / Assert
            sut.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void ConstructorThrowsExceptionIfCharacterClassIsGreaterThanThree()
        {
            // Arrange
            var savegameMock = new Mock<ISavegame>();
            savegameMock.Setup(sg => sg.GetValue<int>(It.IsAny<SF>())).Returns(4);
            savegameMock.Setup(sg => sg.GetValue<int>(It.IsAny<int>())).Returns(4);
            savegameMock.Setup(sg => sg.GetValue(It.IsAny<SF>())).Returns(4);
            savegameMock.Setup(sg => sg.GetValue(It.IsAny<int>())).Returns(4);

            var characterResponseMock = new Mock<ICharacterResponse>();
            characterResponseMock.Setup(c => c.Savegame).Returns(savegameMock.Object);

            Action sut = () => new Character(characterResponseMock.Object);

            // Act / Assert
            sut.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void ConstructorDoesNotThrowExceptionIfResponseSavegamesValuesAreValid()
        {
            // TODO how to determine a "valid" savegame?
            // Arrange
            var savegameMock = new Mock<ISavegame>();
            savegameMock.Setup(sg => sg.GetValue<int>(It.IsAny<SF>())).Returns(1);
            savegameMock.Setup(sg => sg.GetValue<int>(It.IsAny<int>())).Returns(1);
            savegameMock.Setup(sg => sg.GetValue(It.IsAny<SF>())).Returns(1);
            savegameMock.Setup(sg => sg.GetValue(It.IsAny<int>())).Returns(1);

            var characterResponseMock = new Mock<ICharacterResponse>();
            characterResponseMock.Setup(c => c.Savegame).Returns(savegameMock.Object);

            Action sut = () => new Character(characterResponseMock.Object);

            // Act / Assert
            sut.ShouldNotThrow<Exception>();
        }

        [Fact]
        public void RefreshThrowsNotImplementedException()
        {
            // Arrange
            var savegameMock = new Mock<ISavegame>();
            savegameMock.Setup(sg => sg.GetValue<int>(It.IsAny<SF>())).Returns(1);
            savegameMock.Setup(sg => sg.GetValue<int>(It.IsAny<int>())).Returns(1);
            savegameMock.Setup(sg => sg.GetValue(It.IsAny<SF>())).Returns(1);
            savegameMock.Setup(sg => sg.GetValue(It.IsAny<int>())).Returns(1);

            var characterResponseMock = new Mock<ICharacterResponse>();
            characterResponseMock.Setup(c => c.Savegame).Returns(savegameMock.Object);

            var sut = new Character(characterResponseMock.Object);
            Func<Task> refresh = async () => await sut.Refresh();

            // Act / Assert
            refresh.ShouldThrow<NotImplementedException>();
        }
    }
}