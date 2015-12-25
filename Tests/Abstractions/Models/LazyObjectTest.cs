using ReusableLibrary.Abstractions.Models;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Models
{
    public sealed class LazyObjectTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Models, "LazyObject")]
        public static void Object()
        {
            // Arrange
            var count = 0;
            var lazyString = new LazyObject<string>(() => { count++; return "test"; });
            Assert.Equal(0, count);
            Assert.False(lazyString.Loaded);

            // Act            
            var result = lazyString.Object;

            // Assert
            Assert.Equal("test", result);
            Assert.Equal(1, count);
            Assert.True(lazyString.Loaded);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "LazyObject")]
        public static void Object_Twice()
        {
            // Arrange
            var count = 0;
            var lazyString = new LazyObject<string>(() => { count++; return "test"; });
            var result = lazyString.Object;
            Assert.NotNull(result);
            Assert.True(lazyString.Loaded);

            // Act
            result = lazyString.Object;
            Assert.NotNull(result);
            Assert.True(lazyString.Loaded);

            // Assert
            Assert.Equal("test", result);
            Assert.Equal(1, count);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "LazyObject")]
        public static void Reset()
        {
            // Arrange
            var count = 0;
            var lazyString = new LazyObject<string>(() => { count++; return "test"; });
            var result = lazyString.Object;
            Assert.True(lazyString.Loaded);

            // Act            
            lazyString.Reset();
            Assert.False(lazyString.Loaded);
            result = lazyString.Object;
            Assert.True(lazyString.Loaded);

            // Assert
            Assert.Equal("test", result);
            Assert.Equal(2, count);
        }
    }
}
