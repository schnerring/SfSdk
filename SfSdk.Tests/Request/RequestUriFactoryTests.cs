using System;
using FluentAssertions;
using SfSdk.Constants;
using SfSdk.Request;
using Xunit;

namespace SfSdk.Tests.Request
{
    public class RequestUriFactoryTests
    {
        private const string ValidSessionId = "00000000000000000000000000000000";
        private const string ValidServerUrl = "http://s25.sfgame.de/";

        [Fact]
        public void ConstructorThrowsExceptionIfSessionIdIsNull()
        {
            // Arrange
            Action a = () => new SnFUriFactory(null, new Uri(ValidServerUrl), SF.ActAccountCreate);

            // Act / Assert
            a.ShouldThrow<ArgumentNullException>().Where(e => e.ParamName == "sessionId");
        }

        [Fact]
        public void ConstructorThrowsExceptionIfServerUriIsNull()
        {
            // Arrange
            Action a = () => new SnFUriFactory(ValidSessionId, null, SF.ActAccountCreate);

            // Act / Assert
            a.ShouldThrow<ArgumentNullException>().Where(e => e.ParamName == "serverUri");
        }

        [Fact]
        public void ConstructorThrowsExceptionIfSessionIdHasInvalidLength()
        {
            // Arrange
            const string invalidSessionid = "000";
            Action a = () => new SnFUriFactory(invalidSessionid, new Uri(ValidServerUrl), SF.ActAccountCreate);

            // Act / Assert
            a.ShouldThrow<ArgumentException>()
             .Where(e => e.ParamName == "sessionId" && e.Message.StartsWith("SessionId must have a length of 32."));
        }
    }
}