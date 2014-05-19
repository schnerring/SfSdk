using System;
using FluentAssertions;
using SfSdk.Response;
using Xunit;

namespace SfSdk.Tests.Response
{
    public class ScrapbookResponseTests
    {
        private const string ValidResponseString = "wOBBQAKgSYIELIQEBACJdE8AAAAAAAAAAAAAAAAAAAAAAAAAAAACAEhAIwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgRAGlAAAAAAAAAAAAAAAAAAAAAAAAIoAAAAAAAAAAAAAAAAAAgABAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAyAAAAAAAAAAAAAAAACEgAAAAAAAAAAAAAAAAAAAEQAAAAAAAAAAAAAAABYAAAAAAAAAAAAAAAAAAAYAAAAAAAAAAAAAAAAIEIAAAAAAAAAAAAAAAAAQsAAAAAAAAAAAAAAAAAAIIIAAAAAAAAAAAAAAAAIMAIAAAAAAAAAAAAAAAoAAAAAAAAAAAAAAAAAAEjAEAAAAAAAAAAAAAAAAKAAAAAAAAAAAAAAAAAAAEAIAAAAAAAAAAAAAAABAAAAAAAAAAAAAAAAAAAAEAIAAAAAAAAAAAAAABAAACAAAAAAAAAAAAAAAAAABAAAAAAAAAAAAAAAEBAAAAAAAAAAAAAAAAAAA==";

        [Fact]
        public void ConstructorThrowsExceptionIfArgumentsHaveInvalidLength()
        {
            // Arrange
            var invalidArgs = new string[0];
            Action sut = () => new ScrapbookResponse(invalidArgs, TestConstants.ValidServerUri);

            // Act / Assert
            sut.ShouldThrow<ArgumentException>()
               .Where(e => e.Message.StartsWith("The arguments must have a minimum length of 1.") &&
                           e.ParamName == "args");
        }

        [Fact]
        public void ConstructorThrowsExceptionIfServerUriIsNull()
        {
            // Arrange
            var validArgs = new string[1];
            validArgs[0] = ValidResponseString;
            Action sut = () => new ScrapbookResponse(validArgs, null);

            // Act / Assert
            sut.ShouldThrow<ArgumentException>()
               .Where(e => e.ParamName == "serverUri");
        }

        [Fact]
        public void ConstructorThrowsNoExceptionIfWithValidArguments()
        {
            // Arrange
            var validArgs = new string[1];
            validArgs[0] = ValidResponseString;
            Action sut = () => new ScrapbookResponse(validArgs, TestConstants.ValidServerUri);

            // Act / Assert
            sut.ShouldNotThrow<Exception>();
        }
    }
}