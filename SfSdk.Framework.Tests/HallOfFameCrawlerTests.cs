using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SfSdk.Contracts;
using Xunit;

namespace SfSdk.Framework.Tests
{
    public class HallOfFameCrawlerTests
    {
        [Fact]
        public void ConstructorThrowsNoExceptionWithValidArguments()
        {
            // Arrange
            var sessionMock = new Mock<ISession>();
            Action sut = () => new HallOfFameCrawler(sessionMock.Object);

            // Act / Assert
            sut.ShouldNotThrow<Exception>();
        }

        [Fact]
        public void SearchThrowsExceptionIfSearchPredicateIsNull()
        {
            // Arrange
            var sessionMock = new Mock<ISession>();
            var crawler = new HallOfFameCrawler(sessionMock.Object);
            Func<Task> sut = async () => await crawler.Search(null);

            // Act / Assert
            sut.ShouldThrow<ArgumentNullException>()
               .Where(e => e.ParamName == "searchPredicate");
        }

        [Fact]
        public void SearchThrowsExceptionIfSearchPredicateMinRankIsLessThanOne()
        {
            // Arrange
            var sessionMock = new Mock<ISession>();
            var crawler = new HallOfFameCrawler(sessionMock.Object);
            Func<Task> sut = async () => await crawler.Search(new HallOfFameSearchPredicate { MinRank = 0 });

            // Act / Assert
            sut.ShouldThrow<ArgumentException>()
               .Where(
                   e => e.ParamName == "searchPredicate" && e.Message.StartsWith("MinRank must be greater than zero."));
        }

        [Fact]
        public void SearchThrowsExceptionIfSearchPredicateMinHonorIsLessThanOne()
        {
            // Arrange
            var sessionMock = new Mock<ISession>();
            var crawler = new HallOfFameCrawler(sessionMock.Object);
            Func<Task> sut = async () => await crawler.Search(new HallOfFameSearchPredicate { MinHonor = 0 });

            // Act / Assert
            sut.ShouldThrow<ArgumentException>()
               .Where(
                   e => e.ParamName == "searchPredicate" && e.Message.StartsWith("MinHonor must be greater than zero."));
        }

        [Fact]
        public void SearchThrowsExceptionIfSearchPredicateMinLevelIsLessThanOne()
        {
            // Arrange
            var sessionMock = new Mock<ISession>();
            var crawler = new HallOfFameCrawler(sessionMock.Object);
            Func<Task> sut = async () => await crawler.Search(new HallOfFameSearchPredicate { MinLevel = 0 });

            // Act / Assert
            sut.ShouldThrow<ArgumentException>()
               .Where(
                   e => e.ParamName == "searchPredicate" && e.Message.StartsWith("MinLevel must be greater than zero."));
        }

        [Fact]
        public void SearchThrowsExceptionIfSearchPredicateMaxRankIsLessMinRank()
        {
            // Arrange
            var sessionMock = new Mock<ISession>();
            var crawler = new HallOfFameCrawler(sessionMock.Object);
            Func<Task> sut = async () => await crawler.Search(new HallOfFameSearchPredicate { MaxRank = 0 });

            // Act / Assert
            sut.ShouldThrow<ArgumentException>()
               .Where(
                   e =>
                   e.ParamName == "searchPredicate" && e.Message.StartsWith("MaxRank must be greater than MinRank."));
        }

        [Fact]
        public void SearchThrowsExceptionIfSearchPredicateMaxHonorIsLessTahnMinHonor()
        {
            // Arrange
            var sessionMock = new Mock<ISession>();
            var crawler = new HallOfFameCrawler(sessionMock.Object);
            Func<Task> sut = async () => await crawler.Search(new HallOfFameSearchPredicate { MaxHonor = 0 });

            // Act / Assert
            sut.ShouldThrow<ArgumentException>()
               .Where(
                   e =>
                   e.ParamName == "searchPredicate" && e.Message.StartsWith("MaxHonor must be greater than MinHonor."));
        }

        [Fact]
        public void SearchThrowsExceptionIfSearchPredicateMaxLevelIsLessThanMinLevel()
        {
            // Arrange
            var sessionMock = new Mock<ISession>();
            var crawler = new HallOfFameCrawler(sessionMock.Object);
            Func<Task> sut = async () => await crawler.Search(new HallOfFameSearchPredicate { MaxLevel = 0 });

            // Act / Assert
            sut.ShouldThrow<ArgumentException>()
               .Where(
                   e =>
                   e.ParamName == "searchPredicate" && e.Message.StartsWith("MaxLevel must be greater than MinLevel."));
        }

        [Fact]
        public async Task SearchShouldReturnListOfCharactersWithValidArguments()
        {
            // Arrange
            var characters = new List<ICharacter>();
            for (int i = 1; i < 5; i++)
            {
                var characterMock = new Mock<ICharacter>();
                characterMock.Setup(c => c.Rank).Returns(i);
                characterMock.Setup(c => c.Honor).Returns(i);
                characterMock.Setup(c => c.Level).Returns(i);
                characterMock.Setup(c => c.Username).Returns("User No. " + i);
                characters.Add(characterMock.Object);
            }
            var sessionMock = new Mock<ISession>();
            sessionMock.Setup(s => s.HallOfFameAsync(It.IsAny<string>(), It.IsAny<bool>()))
                       .ReturnsAsync(characters);
            var crawler = new HallOfFameCrawler(sessionMock.Object);
            
            // Act
            var results = await crawler.Search(new HallOfFameSearchPredicate());

            // Assert
            results.Should().HaveCount(c => c > 0);
        }
    }
}