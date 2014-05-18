namespace SfSdk.Tests.Providers
{
    using System;
    using System.Threading.Tasks;

    using FluentAssertions;

    using SfSdk.Providers;

    using Xunit;

    public class ConfigurationResourceProviderTests
    {
        [Fact]
        public void GetConfigurationResourcesAsyncThrowsExceptionIfServerUriIsNull()
        {
            // Arrange  / Act
            Func<Task> sut = async () => await new ConfigurationResourceProvider().GetConfigurationResourcesAsnyc(null);

            // Assert
            sut.ShouldThrow<ArgumentNullException>().Where(e => e.ParamName == "serverUri");
        }

        [Fact] // Needs internet
        public async Task GetConfigurationResourcesAsyncReturnsConfigurationInformation()
        {
            // Arrange  / Act
            var configurationResources = await new ConfigurationResourceProvider().GetConfigurationResourcesAsnyc(TestConstants.ValidServerUri);

            // Assert
            configurationResources.Count.Should().BeGreaterOrEqualTo(25);
        }

        //         [Fact] // Needs internet
        [Fact(Skip = "Interrupts Network connection, run manually if needed.")]
        public void GetConfigurationResourcesAsyncThrowsExceptionWithoutNetworkConnection()
        {
            TestHelpers.Disconnect();

            // Arrange
            var sut = new ConfigurationResourceProvider();
            Func<Task> getConfigurationResources = async () => await sut.GetConfigurationResourcesAsnyc(TestConstants.ValidServerUri);

            // Act / Assert
            getConfigurationResources.ShouldThrow<NotImplementedException>().Where(e => e.Message == "Network connection lost.");

            TestHelpers.Connect();
        }

        [Fact]
        public void GetConfigurationResourcesThrowsExceptionIfServerUriIsNull()
        {
            // Arrange  / Act
            Action sut = () => new ConfigurationResourceProvider().GetConfigurationResources(null);

            // Assert
            sut.ShouldThrow<ArgumentNullException>().Where(e => e.ParamName == "serverUri");
        }

        [Fact] // Needs internet
        public void GetConfigurationResourcesReturnsConfigurationInformation()
        {
            // Arrange  / Act
            var configurationResources = new ConfigurationResourceProvider().GetConfigurationResources(TestConstants.ValidServerUri);

            // Assert
            configurationResources.Count.Should().BeGreaterOrEqualTo(25);
        }

        //        [Fact] // Needs internet
        [Fact(Skip = "Interrupts Network connection, run manually if needed.")]
        public void GetConfigurationResourcesThrowsExceptionWithoutNetworkConnection()
        {
            TestHelpers.Disconnect();

            // Arrange
            var sut = new ConfigurationResourceProvider();
            Action getConfigurationResources = () => sut.GetConfigurationResources(TestConstants.ValidServerUri);

            // Act / Assert
            getConfigurationResources.ShouldThrow<NotImplementedException>().Where(e => e.Message == "Network connection lost.");

            TestHelpers.Connect();
        }
    }
}
