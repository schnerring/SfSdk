using System;
using SfSdk.Response;
using Xunit;
using FluentAssertions;

namespace SfSdk.Tests.Response
{
    public class LoginResponseTests
    {
        [Fact]
        public void ConstructorThrowsExceptionIfArgumentsHaveInvalidLength()
        {
            // Arrange
            var validArgs = new string[2];
            validArgs[0] = string.Empty.ToValidSavegameString();
            Action sut = () => new LoginResponse(validArgs);

            // Act / Assert
            sut.ShouldThrow<ArgumentException>()
               .Where(e => e.Message.StartsWith("The arguments must have a minimum length of 3.") &&
                           e.ParamName == "args");
        }

        [Fact]
        public void ConstructorThrowsNoExceptionIfWithValidArguments()
        {
            // Arrange
            var validArgs = new string[3];
            validArgs[0] = string.Empty.ToValidSavegameString("0");
            Action sut = () => new LoginResponse(validArgs);

            // Act / Assert
            sut.ShouldNotThrow<Exception>();
        }
    }
}