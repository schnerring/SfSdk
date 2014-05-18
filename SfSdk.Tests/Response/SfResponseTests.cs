using System;
using FluentAssertions;
using SfSdk.Response;
using Xunit;

namespace SfSdk.Tests.Response
{
    public class SfResponseTests
    {
        [Fact]
        public void ConstructorThrowsExceptionIfResponseStringIsNull()
        {
            // Arrange / Act
            Action sut = () => new SfResponse(null, null);

            // Assert
            sut.ShouldThrow<ArgumentNullException>().Where(e => e.ParamName == "responseString");
        }

        [Fact]
        public void ConstructorThrowsExceptionIfResponseStringIsEmpty()
        {
            // Arrange / Act
            Action sut = () => new SfResponse(string.Empty, null);

            // Assert
            sut.ShouldThrow<ArgumentException>()
               .Where(e => e.Message.StartsWith("Response string must not be empty.") && e.ParamName == "responseString");
        }

        [Fact]
        public void ConstructorThrowsExceptionIfServerUriIsNull()
        {
            // Arrange / Act
            Action sut = () => new SfResponse("a", null);

            // Assert
            sut.ShouldThrow<ArgumentNullException>().Where(e => e.ParamName == "serverUri");
        }

        [Fact]
        public void ConstructorThrowsExceptionIfErrorResponseStringHasInvalidLength()
        {
            // Arrange / Act
            Action sut = () => new SfResponse("E01", TestConstants.ValidServerUri);

            // Assert
            sut.ShouldThrow<ArgumentException>()
               .Where(
                   e =>
                   e.Message.StartsWith("Error response string (starting with \"E\") must have a minimum length of 4.") &&
                   e.ParamName == "responseString");
        }

        [Fact]
        public void ConstructorThrowsExceptionIfErrorResponseStringHasInvalidCode()
        {
            // Arrange / Act
            Action sut = () => new SfResponse("E01A", TestConstants.ValidServerUri);

            // Assert
            sut.ShouldThrow<ArgumentException>()
               .Where(e => e.Message.StartsWith("Error code must be of type int.") && e.ParamName == "responseString");
        }

        [Fact]
        public void ConstructorThrowsExceptionIfSuccessResponseStringHasInvalidLength()
        {
            // Arrange / Act
            Action sut = () => new SfResponse("01", TestConstants.ValidServerUri);

            // Assert
            sut.ShouldThrow<ArgumentException>()
               .Where(e => e.Message.StartsWith("Success response string must have a minimum length of 3."));
        }

        [Fact]
        public void ConstructorThrowsExceptionIfSuccessResponseStringHasInvalidCode()
        {
            // Arrange / Act
            Action sut = () => new SfResponse("abc", TestConstants.ValidServerUri);

            // Assert
            sut.ShouldThrow<ArgumentException>()
               .Where(e => e.Message.StartsWith("Success code must be of type int.") && e.ParamName == "responseString");
        }

        [Fact]
        public void ErrorsListIsInitializedAfterCreatingInstance()
        {
            // Arrange / Act
            var sut = new SfResponse(TestConstants.ExistingSuccess, TestConstants.ValidServerUri);

            // Assert
            sut.Errors.Should().NotBeNull();
        }

        [Fact]
        public void ErrorsListCountIsZeroAfterCreatingSuccessInstance()
        {
            // Arrange / Act
            var sut = new SfResponse(TestConstants.ExistingSuccess, TestConstants.ValidServerUri);

            // Assert
            sut.Errors.Should().HaveCount(0);
        }

        [Fact]
        public void ResultIsNotNullAfterCreatingInstanceWithImplementedSuccessCode()
        {
            // Arrange / Act
            var sut = new SfResponse(TestConstants.ExistingSuccess, TestConstants.ValidServerUri);

            // Assert
            sut.Response.Should().NotBeNull();
        }

        [Fact]
        public void ProcessSuccessThrowsExceptionForNotImplementedSuccessCode()
        {
            // Arrange / Act
            Action sut = () => new SfResponse(TestConstants.NonExistingSuccess, TestConstants.ValidServerUri);

            // Assert
            sut.ShouldThrow<ArgumentOutOfRangeException>().Where(e => e.ParamName == "success");
        }

        [Fact]
        public void ErrorsListCountIsGreaterZeroAfterCreatingErrorInstance()
        {
            // Arrange / Act
            var sut = new SfResponse(TestConstants.ExistingError, TestConstants.ValidServerUri);

            // Assert
            sut.Errors.Should().HaveCount(c => c > 0);
        }

        [Fact]
        public void ResultIsNullAfterCreatingInstanceWithImplementedErrorCode()
        {
            // Arrange / Act
            var sut = new SfResponse(TestConstants.ExistingError, TestConstants.ValidServerUri);

            // Assert
            sut.Response.Should().BeNull();
        }

        [Fact]
        public void ProcessErrorThrowsExceptionForNotImplementedErrorCode()
        {
            // Arrange / Act
            Action sut = () => new SfResponse(TestConstants.NonExistingError, TestConstants.ValidServerUri);

            // Assert
            sut.ShouldThrow<ArgumentOutOfRangeException>().Where(e => e.ParamName == "error");
        }
    }
}