using System;
using FluentAssertions;
using SfSdk.Response;
using Xunit;

namespace SfSdk.Tests.Response
{
    public class HallOfFameResponseTests
    {
        [Fact]
        public void ConstructorThrowsExceptionIfArgumentsHaveInvalidLength()
        {
            // Arrange
            var invalidArgs = new string[0];
            Action sut = () => new HallOfFameResponse(invalidArgs);

            // Act / Assert
            sut.ShouldThrow<ArgumentException>()
               .Where(e => e.Message.StartsWith("The arguments must have a minimum length of 1.") &&
                           e.ParamName == "args");
        }

        [Fact]
        public void ConstructorThrowsNoExceptionWithValidArguments()
        {
            // Arrange
            var validArgs = new string[1];
            validArgs[0] = string.Empty;
            Action sut = () => new HallOfFameResponse(validArgs);

            // Act / Assert
            sut.ShouldNotThrow<Exception>();
        } 
    }
}