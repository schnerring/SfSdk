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
        public async Task GetLanguageResourcesAsyncReturnsLanguageInformation()
        {
            // Arrange  / Act
            var languageResources = await new LanguageResourceProvider().GetLanguageResourcesAsnyc();

            // Assert
            languageResources.Count.Should().BeGreaterOrEqualTo(100);
        }

//        [Fact] // Needs internet
        [Fact(Skip = "Interrupts Network connection, run manually if needed.")]
        public void ExecuteAsyncThrowsExceptionWithoutNetworkConnection()
        {
            TestHelpers.Disconnect();

            // Arrange
            var sut = new LanguageResourceProvider();
            Func<Task> getLanguageResources = async () => await sut.GetLanguageResourcesAsnyc();

            // Act / Assert
            getLanguageResources.ShouldThrow<NotImplementedException>().Where(e => e.Message == "Network connection lost.");

            TestHelpers.Connect();
        }
    }
}
