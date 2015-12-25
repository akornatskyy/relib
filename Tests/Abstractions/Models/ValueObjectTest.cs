using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Models;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Models
{
    public sealed class ValueObjectTest
    {
        private ValueObject<int> m_instance = new ValueObject<int>(100, "Test");

        [Fact]
        [Trait(Constants.TraitNames.Models, "ValueObject")]
        public void ValueObject_Properties()
        {
            // Arrange

            // Act

            // Assert
            Assert.Equal(100, m_instance.Key);
            Assert.Equal("Test", m_instance.DisplayName);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "ValueObject")]
        public void ValueObject_Is_Immutable()
        {
            // Arrange

            // Act
            var properties = m_instance.GetType().GetProperties();

            // Assert
            Assert.True(properties.Length > 0);
            EnumerableHelper.ForEach(properties, (property) => Assert.False(property.CanWrite));
        }
    }
}
