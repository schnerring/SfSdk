using System;
using System.Collections.Generic;
using FluentAssertions;
using SfSdk.Constants;
using SfSdk.Providers;
using Xunit;

namespace SfSdk.Tests.Providers
{
    public class LanguageResourceProviderTests
    {
        [Fact]
        public void GetResourcesThrowsExceptionIfServerUriIsNull()
        {
            // Arrange  / Act
            Action sut = () => new LanguageResourceProvider().GetResources(null);

            // Assert
            sut.ShouldThrow<ArgumentNullException>().Where(e => e.ParamName == "serverUri");
        }

        [Fact] // Needs internet
        public void GetResourcesReturnsLanguageInformation()
        {
            // Arrange  / Act
            Dictionary<SF, string> languageResources =
                new LanguageResourceProvider().GetResources(TestConstants.ValidServerUri);

            // Assert
            languageResources.Count.Should().BeGreaterOrEqualTo(100);
        }


//        [Fact] // Needs internet
        [Fact(Skip = "Interrupts Network connection, run manually if needed.")]
        public void GetResourcesThrowsExceptionWithoutNetworkConnection()
        {
            TestHelpers.Disconnect();

            // Arrange
            var sut = new LanguageResourceProvider();
            Action getResources = () => sut.GetResources(TestConstants.ValidServerUri);

            // Act / Assert
            getResources.ShouldThrow<NotImplementedException>().Where(e => e.Message == "Network connection lost.");

            TestHelpers.Connect();
        }
    }
}