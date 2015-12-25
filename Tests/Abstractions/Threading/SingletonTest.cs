using ReusableLibrary.Abstractions.Threading;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Threading
{
    public sealed class SingletonTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Threading, "Singleton")]
        public static void Instance()
        {
            // Arrange
            var expected = 100;
            var singleton = new Singleton<int>(name => expected++);

            // Act
            var result = singleton.Instance("x");

            // Assert
            Assert.Equal(100, result);
            Assert.Equal(100, singleton.Instance("x"));
            Assert.Equal(101, expected);
        }
    }
}
