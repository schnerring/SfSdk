using System;
using FluentAssertions;
using SfSdk.Constants;
using SfSdk.Request;
using Xunit;

namespace SfSdk.Tests.Request
{
    public class SnFUriWrapperTests
    {
        [Fact]
        public void ConstructorThrowsExceptionIfSessionIdIsNull()
        {
            // Arrange
            Action sut = () => new SnFUriWrapper(null, TestConstants.ValidServerUri, SF.ActAccountCreate);

            // Act / Assert
            sut.ShouldThrow<ArgumentNullException>().Where(e => e.ParamName == "sessionId");
        }

        [Fact]
        public void ConstructorThrowsExceptionIfServerUriIsNull()
        {
            // Arrange
            Action sut = () => new SnFUriWrapper(TestConstants.ValidSessionId, null, SF.ActAccountCreate);

            // Act / Assert
            sut.ShouldThrow<ArgumentNullException>().Where(e => e.ParamName == "serverUri");
        }

        [Fact]
        public void ConstructorThrowsExceptionIfSessionIdHasInvalidLength()
        {
            // Arrange
            Action sut =
                () =>
                new SnFUriWrapper(TestConstants.InvalidSessionId, TestConstants.ValidServerUri, SF.ActAccountCreate);

            // Act / Assert
            sut.ShouldThrow<ArgumentException>()
               .Where(e => e.ParamName == "sessionId" && e.Message.StartsWith("sessionId must have a length of 32."));
        }

        [Fact]
        public void ConstructorThrowsNoExceptionWithValidParameters()
        {
            // Arrange
            Action sut =
                () => new SnFUriWrapper(TestConstants.ValidSessionId, TestConstants.ValidServerUri, SF.ActAccountCreate);

            // Act / Assert
            sut.ShouldNotThrow<Exception>();
        }
    }
}