using System;
using System.Linq;
using FluentAssertions;
using SfSdk.Data;
using SfSdk.Providers;
using SfSdk.Response;
using Xunit;

namespace SfSdk.Tests.Providers
{
    public class ScrapbookItemProviderTests
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
        public void CreateMonsterItemsShouldReturn252MonsterItems()
        {
            // Arrange
            var sut = new ItemProvider(TestConstants.ValidServerUri);

            // Act
            var monsterItems = sut.CreateMonsterItems(TestConstants.ValidAlbumContent.ToList());

            // Assert
            monsterItems.Count().Should().Be(252);
        }

        [Fact]
        public void CreateMonsterItemsShouldReturnNoItemWithTheSameText()
        {
            // Arrange
            var sut = new ItemProvider(TestConstants.ValidServerUri);

            // Act
            var monsterItems = sut.CreateMonsterItems(TestConstants.ValidAlbumContent.ToList());

            // Assert
            var monsterItemTexts = monsterItems.Select(i => i.Text).ToList();
            monsterItemTexts.Count.Should().Be(monsterItemTexts.Distinct().Count());
        }

        [Fact]
        public void CreateValuableItemsShouldReturn246ValuableItems()
        {
            // Arrange
            var sut = new ItemProvider(TestConstants.ValidServerUri);

            // Act
            var valuableItems = sut.CreateValuableItems(TestConstants.ValidAlbumContent.ToList());

            // Assert
            valuableItems.Count().Should().Be(246);
        }

        [Fact]
        public void CreateWarriorItemsShouldReturn506WarriorItems()
        {
            // Arrange
            var sut = new ItemProvider(TestConstants.ValidServerUri);

            // Act
            var warriorItems = sut.CreateWarriorItems(TestConstants.ValidAlbumContent.ToList());

            // Assert
            warriorItems.Count().Should().Be(506);
        }

        [Fact]
        public void CreateMageOrScoutItemsThrowsExceptionWithInvalidTypeParameter()
        {
            // Arrange
            var sut = new ItemProvider(TestConstants.ValidServerUri);

            // Act
            Action createInvalidItems = () => sut.CreateMageOrScoutItems<MonsterItem>(TestConstants.ValidAlbumContent.ToList());

            // Assert
            createInvalidItems.ShouldThrow<ArgumentException>()
                .Where(e => e.Message == "TScrapbookItem be of type MageItem or ScoutItem.");
        }

        [Fact]
        public void CreateMageOrScoutItemsShouldReturn348MageItems()
        {
            // Arrange
            var sut = new ItemProvider(TestConstants.ValidServerUri);

            // Act
            var mageItems = sut.CreateMageOrScoutItems<MageItem>(TestConstants.ValidAlbumContent.ToList());

            // Assert
            mageItems.Count().Should().Be(348);
        }

        [Fact]
        public void CreateMageOrScoutItemsShouldReturn348ScoutItems()
        {
            // Arrange
            var sut = new ItemProvider(TestConstants.ValidServerUri);

            // Act
            var scoutItems = sut.CreateMageOrScoutItems<ScoutItem>(TestConstants.ValidAlbumContent.ToList());

            // Assert
            scoutItems.Count().Should().Be(348);
        }

        [Fact]
        public void CreateInventoryShouldReturnInventoryWith15Items()
        {
            // Arrange
            var sut = new ItemProvider(TestConstants.ValidServerUri);
            var sg = new Savegame(TestConstants.ValidSavegameString);

            // Act
            var inventory = sut.CreateInventory(sg);

            // Assert
            inventory.AllItems.Count().Should().Be(15);
        }
    }
}