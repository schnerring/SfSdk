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
            Action sut = () => new LoginResponse(new string[2]);

            // Act / Assert
            sut.ShouldThrow<ArgumentException>()
               .Where(e => e.Message.StartsWith("The arguments must have a minimum length of 3.") &&
                           e.ParamName == "args");
        }
    }
}