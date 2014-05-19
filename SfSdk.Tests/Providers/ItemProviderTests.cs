using System;
using FluentAssertions;
using SfSdk.Providers;
using Xunit;

namespace SfSdk.Tests.Providers
{
    public class ItemProviderTests
    {
        [Fact]
        public void ConstructorThrowsExceptionIfServerUriIsNull()
        {
            // Arrange / Act
            Action sut = () => new ItemProvider(null);

            // Assert
            sut.ShouldThrow<ArgumentNullException>().Where(e => e.ParamName == "serverUri");
        }

        [Fact]
        public void ConstructorThrowsNoExceptionWithValidArguments()
        {
            // Arrange / Act
            Action sut = () => new ItemProvider(TestConstants.ValidServerUri);

            // Assert
            sut.ShouldNotThrow<Exception>();
        }

        [Fact]
        public void GetImageUriReturnsValidUri()
        {
            // Arrange
            var sut = new ItemProvider(TestConstants.ValidServerUri);

            // Act
            var serverUri = sut.GetImageUri(24855, 24855);

            // Assert
            serverUri.Should().NotBeNull();
        }
    }
}