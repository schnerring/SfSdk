namespace SfSdk.Tests.Providers
{
    using System;
    using System.Threading.Tasks;

    using FluentAssertions;

    using SfSdk.Providers;

    using Xunit;

    public class LanguageResourceProviderTests
    {
        [Fact]
        public void GetLanguageResourcesAsyncThrowsExceptionIfServerUriIsNull()
        {
            // Arrange  / Act
            Func<Task> sut = async () => await new LanguageResourceProvider().GetLanguageResourcesAsnyc(null);

            // Assert
            sut.ShouldThrow<ArgumentNullException>().Where(e => e.ParamName == "serverUri");
        }

        [Fact] // Needs internet
        public async Task GetLanguageResourcesAsyncReturnsLanguageInformation()
        {
            // Arrange  / Act
            var languageResources = await new LanguageResourceProvider().GetLanguageResourcesAsnyc(TestConstants.ValidServerUri);

            // Assert
            languageResources.Count.Should().BeGreaterOrEqualTo(100);
        }

//         [Fact] // Needs internet
        [Fact(Skip = "Interrupts Network connection, run manually if needed.")]
        public void GetLanguageResourcesAsyncThrowsExceptionWithoutNetworkConnection()
        {
            TestHelpers.Disconnect();

            // Arrange
            var sut = new LanguageResourceProvider();
            Func<Task> getLanguageResources = async () => await sut.GetLanguageResourcesAsnyc(TestConstants.ValidServerUri);

            // Act / Assert
            getLanguageResources.ShouldThrow<NotImplementedException>().Where(e => e.Message == "Network connection lost.");

            TestHelpers.Connect();
        }

        [Fact]
        public void GetLanguageResourcesThrowsExceptionIfServerUriIsNull()
        {
            // Arrange  / Act
            Action sut = () =>  new LanguageResourceProvider().GetLanguageResources(null);

            // Assert
            sut.ShouldThrow<ArgumentNullException>().Where(e => e.ParamName == "serverUri");
        }

        [Fact] // Needs internet
        public void GetLanguageResourcesReturnsLanguageInformation()
        {
            // Arrange  / Act
            var languageResources = new LanguageResourceProvider().GetLanguageResources(TestConstants.ValidServerUri);

            // Assert
            languageResources.Count.Should().BeGreaterOrEqualTo(100);
        }

//        [Fact] // Needs internet
        [Fact(Skip = "Interrupts Network connection, run manually if needed.")]
        public void GetLanguageResourcesThrowsExceptionWithoutNetworkConnection()
        {
            TestHelpers.Disconnect();

            // Arrange
            var sut = new LanguageResourceProvider();
            Action getLanguageResources = () => sut.GetLanguageResources(TestConstants.ValidServerUri);

            // Act / Assert
            getLanguageResources.ShouldThrow<NotImplementedException>().Where(e => e.Message == "Network connection lost.");

            TestHelpers.Connect();
        }
    }
}
