using System.Collections;
using Xunit;
using System.Linq;
using FluentAssertions;

namespace SfSdk.Framework.Tests
{
    public class HallOfFameSearchPredicateTests
    {
        [Fact]
        public void SearchPredicateScalarPropertiesAreProperlyInitialized()
        {
            // Arrange / Act
            var sut = new HallOfFameSearchPredicate();

            // Assert
            var t = sut.GetType();
            var scalarProperties = t.GetProperties().Where(p => !p.PropertyType.IsGenericType);
            foreach (var scalarProperty in scalarProperties)
            {
                scalarProperty.GetValue(sut).Should().Be(1);
            }
        }

        [Fact]
        public void SearchPredicateListPropertiesAreProperlyInitialized()
        {
            // Arrange / Act
            var sut = new HallOfFameSearchPredicate();

            // Assert
            var t = sut.GetType();
            var f = t.GetProperties().ToList();
            var listProperties = t.GetProperties().Where(p => p.PropertyType.IsGenericType).ToList();
            foreach (var listProperty in listProperties)
            {
                ((IList) listProperty.GetValue(sut)).Should().NotBeNull();
            }
        }
    }
}