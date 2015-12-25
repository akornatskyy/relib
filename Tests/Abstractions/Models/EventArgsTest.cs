using ReusableLibrary.Abstractions.Models;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Models
{
    public sealed class EventArgsTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Models, "EventArgs")]
        public static void NullPayload_IsNull()
        {
            // Arrange
            var e = new EventArgs<string>(null);

            // Act
            var payload = e.Payload;

            // Assert
            Assert.Null(payload);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "EventArgs")]
        public static void Payload_IsNotNull()
        {
            // Arrange
            var e = new EventArgs<int>(1);

            // Act
            var payload = e.Payload;

            // Assert
            Assert.Equal(1, payload);
        }
    }
}
