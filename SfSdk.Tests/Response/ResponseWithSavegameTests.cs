using System;
using FluentAssertions;
using SfSdk.Response;
using Xunit;

namespace SfSdk.Tests.Response
{
    public class ResponseWithSavegameTests
    {
        private class TestResponse : ResponseWithSavegame
        {
            public TestResponse(string[] args)
                : base(args)
            {
            }
        }

        [Fact]
        public void ConstructorThrowsExceptionIfWithInvalidArguments()
        {
            // Arrange
            Action sut = () => new TestResponse(new string[0]);

            // Act / Assert
            sut.ShouldThrow<ArgumentException>()
               .Where(e => e.ParamName == "args" && e.Message.StartsWith("The arguments must have a minimum length of 1."));
        }

        [Fact]
        public void ConstructorThrowsNoExceptionWithValidArguments()
        {
            // Arrange
            var validArgs = new string[1];
            validArgs[0] = string.Empty.ToValidSavegameString();
            Action sut = () => new TestResponse(validArgs);

            // Act / Assert
            sut.ShouldNotThrow<Exception>();
        }
    }
}