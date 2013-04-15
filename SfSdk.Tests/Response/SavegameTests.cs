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
        public void ConstructorThrowsExceptionIfSavegameLengthIsEqualToMinimumKey()
        {
            // Arrange
            Action sut = () => new Savegame(new string[10], (SF) 10);

            // Act / Assert
            sut.ShouldThrow<ArgumentException>()
               .Where(
                   e =>
                   e.Message.StartsWith("The savegame parts must contain a value for the minimum savegame key.") &&
                   e.ParamName == "savegameParts");
        }

        [Fact]
        public void ConstructorThrowsExceptionIfSavegameLengthIsLessThanMinimumKey()
        {
            // Arrange
            Action sut = () => new Savegame(new string[9], (SF) 10);

            // Act / Assert
            sut.ShouldThrow<ArgumentException>()
               .Where(
                   e =>
                   e.Message.StartsWith("The savegame parts must contain a value for the minimum savegame key.") &&
                   e.ParamName == "savegameParts");
        }

        [Fact]
        public void ConstructorDoesNotThrowAnExceptionWithValidArguments()
        {
            // Arrange
            Action sut = () => new Savegame(new string[9], (SF) 8);

            // Act / Assert
            sut.ShouldNotThrow<ArgumentException>();

        }

        [Fact]
        public void GetValueReturnsDefaultTIfKeyDoesNotExist()
        {
            // Arrange
            var savegame = new Savegame(new[] {"123", "456"}, (SF) 1);
            const int invalidKey = 2;

            // Act
            var result = savegame.GetValue<int>(invalidKey);

            // Assert
            result.Should().Be(default(int));
        }

        [Fact]
        public void GetValueSfReturnsDefaultTIfKeyDoesNotExist()
        {
            // Arrange
            var savegame = new Savegame(new[] {"123", "456"}, (SF) 1);
            const SF invalidKey = (SF) 2;

            // Act
            var result = savegame.GetValue<int>(invalidKey);

            // Assert
            result.Should().Be(default(int));
        }

        [Fact]
        public void GetValueReturnsDefaultIntIfKeyDoesNotExist()
        {
            // Arrange
            var savegame = new Savegame(new[] {"123", "456"}, (SF) 1);
            const int invalidKey = 2;

            // Act
            var result = savegame.GetValue(invalidKey);

            // Assert
            result.Should().Be(default(int));
        }

        [Fact]
        public void GetValueSfReturnsDefaultIntIfKeyDoesNotExist()
        {
            // Arrange
            var savegame = new Savegame(new[] {"123", "456"}, (SF) 1);
            const SF invalidKey = (SF) 2;

            // Act
            var result = savegame.GetValue(invalidKey);

            // Assert
            result.Should().Be(default(int));
        }

        [Fact]
        public void GetValueReturnsValueIfKeyDoesExist()
        {
            // Arrange
            var savegame = new Savegame(new[] {"123", "456"}, (SF) 1);
            const int validKey = 1;

            // Act
            var result = savegame.GetValue<int>(validKey);

            // Assert
            result.Should().Be(456);
        }

        [Fact]
        public void GetValueSfReturnsValueIfKeyDoesExist()
        {
            // Arrange
            var savegame = new Savegame(new[] {"123", "456"}, (SF) 1);
            const SF validKey = (SF) 1;

            // Act
            var result = savegame.GetValue<int>(validKey);

            // Assert
            result.Should().Be(456);
        }

        [Fact]
        public void GetValueReturnsValueIntIfKeyDoesExist()
        {
            // Arrange
            var savegame = new Savegame(new[] {"123", "456"}, (SF) 1);
            const int validKey = 1;

            // Act
            var result = savegame.GetValue(validKey);

            // Assert
            result.Should().Be(456);
        }

        [Fact]
        public void GetValueSfReturnsValueIntIfKeyDoesExist()
        {
            // Arrange
            var savegame = new Savegame(new[] {"123", "456"}, (SF) 1);
            const SF validKey = (SF) 1;

            // Act
            var result = savegame.GetValue(validKey);

            // Assert
            result.Should().Be(456);
        }
    }
}