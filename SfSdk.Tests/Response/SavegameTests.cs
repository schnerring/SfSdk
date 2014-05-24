using System;
using SfSdk.Constants;
using SfSdk.Response;
using Xunit;
using FluentAssertions;

namespace SfSdk.Tests.Response
{
    public class SavegameTests
    {
        [Fact]
        public void ConstructorThrowsExceptionIfSavegameStringIsNotLongEnough()
        {
            // Arrange
            Action sut = () => new Savegame(string.Empty.ToInvalidSavegameString());

            // Act / Assert
            sut.ShouldThrow<ArgumentException>()
               .Where(
                   e =>
                   e.Message.StartsWith("The savegame string is not valid.") &&
                   e.ParamName == "savegameString");
        }

        [Fact]
        public void ConstructorDoesNotThrowAnExceptionWithValidArguments()
        {
            // Arrange
            
            Action sut = () => new Savegame(string.Empty.ToValidSavegameString());

            // Act / Assert
            sut.ShouldNotThrow<Exception>();

        }

        [Fact]
        public void GetValueReturnsDefaultTIfKeyDoesNotExist()
        {
            // Arrange
            var savegame = new Savegame("123/456".ToValidSavegameString());
            const int invalidKey = 1000;

            // Act
            var result = savegame.GetValue<int>(invalidKey);

            // Assert
            result.Should().Be(default(int));
        }

        [Fact]
        public void GetValueSfReturnsDefaultTIfKeyDoesNotExist()
        {
            // Arrange
            var savegame = new Savegame("123/456".ToValidSavegameString());
            const SF invalidKey = (SF) 1000;

            // Act
            var result = savegame.GetValue<int>(invalidKey);

            // Assert
            result.Should().Be(default(int));
        }

        [Fact]
        public void GetValueReturnsDefaultIntIfKeyDoesNotExist()
        {
            // Arrange
            var savegame = new Savegame("123/456".ToValidSavegameString());
            const int invalidKey = 1000;

            // Act
            var result = savegame.GetValue(invalidKey);

            // Assert
            result.Should().Be(default(int));
        }

        [Fact]
        public void GetValueSfReturnsDefaultIntIfKeyDoesNotExist()
        {
            // Arrange
            var savegame = new Savegame("123/456".ToValidSavegameString());
            const SF invalidKey = (SF) 1000;

            // Act
            var result = savegame.GetValue(invalidKey);

            // Assert
            result.Should().Be(default(int));
        }

        [Fact]
        public void GetValueReturnsValueIfKeyDoesExist()
        {
            // Arrange
            var savegame = new Savegame("123/456".ToValidSavegameString());
            const int validKey = 2;

            // Act
            var result = savegame.GetValue<int>(validKey);

            // Assert
            result.Should().Be(456);
        }

        [Fact]
        public void GetValueSfReturnsValueIfKeyDoesExist()
        {
            // Arrange
            var savegame = new Savegame("123/456".ToValidSavegameString());
            const SF validKey = (SF) 2;

            // Act
            var result = savegame.GetValue<int>(validKey);

            // Assert
            result.Should().Be(456);
        }

        [Fact]
        public void GetValueReturnsValueIntIfKeyDoesExist()
        {
            // Arrange
            var savegame = new Savegame("123/456".ToValidSavegameString());
            const int validKey = 2;

            // Act
            var result = savegame.GetValue(validKey);

            // Assert
            result.Should().Be(456);
        }

        [Fact]
        public void GetValueSfReturnsValueIntIfKeyDoesExist()
        {
            // Arrange
            var savegame = new Savegame("123/456".ToValidSavegameString());
            const SF validKey = (SF) 2;

            // Act
            var result = savegame.GetValue(validKey);

            // Assert
            result.Should().Be(456);
        }

        [Fact]
        public void SetValueThrowsNoExceptionIfKeyDoesNotExist()
        {
            // Arrange
            var savegame = new Savegame("123/456".ToValidSavegameString());
            const int invalidKey = 1000;

            // Act
            Action a = () => savegame.SetValue(invalidKey, 0);

            // Assert
            a.ShouldNotThrow<Exception>();
        }

        [Fact]
        public void SetValueSfThrowsNoExceptionIfKeyDoesNotExist()
        {
            // Arrange
            var savegame = new Savegame("123/456".ToValidSavegameString());
            const SF invalidKey = (SF) 1000;

            // Act
            Action a = () => savegame.SetValue(invalidKey, 0);

            // Assert
            a.ShouldNotThrow<Exception>();
        }

        [Fact]
        public void SetValueChangesValue()
        {
            // Arrange
            var savegame = new Savegame("123/456".ToValidSavegameString());
            const int validKey = 2;
            var oldValue = savegame.GetValue(validKey); // 456

            // Act
            savegame.SetValue(validKey, 0);

            // Assert
            var newValue = savegame.GetValue(validKey);
            newValue.Should().NotBe(oldValue);
        }

        [Fact]
        public void SetValueSfChangesValue()
        {
            // Arrange
            var savegame = new Savegame("123/456".ToValidSavegameString());
            const SF validKey = (SF) 2;
            var oldValue = savegame.GetValue(validKey); // 456

            // Act
            savegame.SetValue(validKey, 0);

            // Assert
            var newValue = savegame.GetValue(validKey);
            newValue.Should().NotBe(oldValue);
        }
    }
}