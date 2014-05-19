using System;
using FluentAssertions;
using SfSdk.Providers;
using Xunit;

namespace SfSdk.Tests.Providers
{
    public class ScrapbookItemProviderTests
    {
        [Fact]
        public void ConstructorThrowsExceptionIfServerUriIsNull()
        {
            // Arrange / Act
            Action sut = () => new ScrapbookItemProvider(null);

            // Assert
            sut.ShouldThrow<ArgumentNullException>().Where(e => e.ParamName == "serverUri");
        }

        [Fact]
        public void ConstructorThrowsNoExceptionWithValidArguments()
        {
            // Arrange / Act
            Action sut = () => new ScrapbookItemProvider(TestConstants.ValidServerUri);

            // Assert
            sut.ShouldNotThrow<Exception>();
        }
    }
}