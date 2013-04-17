using System;
using FluentAssertions;
using SfSdk.Constants;
using SfSdk.Request;
using Xunit;

namespace SfSdk.Tests.Request
{
    public class SnFUriWrapperTests
    {
        private const string ValidSessionId = "00000000000000000000000000000000";
        private const string ValidServerUrl = "http://s25.sfgame.de/";

        [Fact]
        public void ConstructorThrowsExceptionIfSessionIdIsNull()
        {
            // Arrange
            Action sut = () => new SnFUriWrapper(null, new Uri(ValidServerUrl), SF.ActAccountCreate);

            // Act / Assert
            sut.ShouldThrow<ArgumentNullException>().Where(e => e.ParamName == "sessionId");
        }

        [Fact]
        public void ConstructorThrowsExceptionIfServerUriIsNull()
        {
            // Arrange
            Action sut = () => new SnFUriWrapper(ValidSessionId, null, SF.ActAccountCreate);

            // Act / Assert
            sut.ShouldThrow<ArgumentNullException>().Where(e => e.ParamName == "serverUri");
        }

        [Fact]
        public void ConstructorThrowsExceptionIfSessionIdHasInvalidLength()
        {
            // Arrange
            const string invalidSessionid = "000";
            Action sut = () => new SnFUriWrapper(invalidSessionid, new Uri(ValidServerUrl), SF.ActAccountCreate);

            // Act / Assert
            sut.ShouldThrow<ArgumentException>()
               .Where(e => e.ParamName == "sessionId" && e.Message.StartsWith("SessionId must have a length of 32."));
        }

        [Fact]
        public void ConstructorThrowsNoExceptionWithValidParameters()
        {
            // Arrange
            Action sut = () => new SnFUriWrapper(ValidSessionId, new Uri(ValidServerUrl), SF.ActAccountCreate);

            // Act / Assert
            sut.ShouldNotThrow<Exception>();
        }
    }
}