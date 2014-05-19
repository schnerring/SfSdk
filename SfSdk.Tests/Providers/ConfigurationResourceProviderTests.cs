using System;
using System.Collections.Generic;
using FluentAssertions;
using SfSdk.Constants;
using SfSdk.Providers;
using Xunit;

namespace SfSdk.Tests.Providers
{
    public class ConfigurationResourceProviderTests
    {
        [Fact]
        public void GetResourcesThrowsExceptionIfServerUriIsNull()
        {
            // Arrange  / Act
            Action sut = () => new ConfigurationResourceProvider().GetResources(null);

            // Assert
            sut.ShouldThrow<ArgumentNullException>().Where(e => e.ParamName == "serverUri");
        }

        [Fact] // Needs internet
        public void GetResourcesReturnsConfigurationInformation()
        {
            // Arrange  / Act
            Dictionary<SF, string> configurationResources =
                new ConfigurationResourceProvider().GetResources(TestConstants.ValidServerUri);

            // Assert
            configurationResources.Count.Should().BeGreaterOrEqualTo(25);
        }

        //        [Fact] // Needs internet
        [Fact(Skip = "Interrupts Network connection, run manually if needed.")]
        public void GetResourcesThrowsExceptionWithoutNetworkConnection()
        {
            TestHelpers.Disconnect();

            // Arrange
            var sut = new ConfigurationResourceProvider();
            Action getResources = () => sut.GetResources(TestConstants.ValidServerUri);

            // Act / Assert
            getResources.ShouldThrow<NotImplementedException>().Where(e => e.Message == "Network connection lost.");

            TestHelpers.Connect();
        }
    }
}