using System;
using System.Globalization;
using ReusableLibrary.HistoryLog.Models;
using Xunit;

namespace ReusableLibrary.HistoryLog.Tests.Fixtures
{
    public static class HistoryLogItemTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Models, "HistoryLogItem")]
        public static void Join_NullOrEmpty()
        {
            // Arrange
            var args = new string[] { };

            // Act
            var result1 = HistoryLogItem.Join(args);
            var result2 = HistoryLogItem.Join(null);

            // Assert
            Assert.Equal(null, result1);
            Assert.Equal(null, result2);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "HistoryLogItem")]
        public static void Join_Replace_Tabs_With_Spaces()
        {
            // Arrange
            var args = new string[] { "0\t\t1" };

            // Act
            var result = HistoryLogItem.Join(args);

            // Assert
            Assert.Equal("0  1", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "HistoryLogItem")]
        public static void Join()
        {
            // Arrange
            var args = new string[] { "01", "02" };

            // Act
            var result = HistoryLogItem.Join(args);

            // Assert
            Assert.Equal("01\t02", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "HistoryLogItem")]
        public static void Split_NullOrEmpty()
        {
            // Arrange
            var args = string.Empty;

            // Act
            var result1 = HistoryLogItem.Split(args);
            var result2 = HistoryLogItem.Split(null);

            // Assert
            Assert.Equal(0, result1.Length);
            Assert.Equal(0, result2.Length);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "HistoryLogItem")]
        public static void Split()
        {
            // Arrange
            var args = "a1\tb2";

            // Act
            var result = HistoryLogItem.Split(args);

            // Assert
            Assert.Equal(2, result.Length);
            Assert.Equal("a1", result[0]);
            Assert.Equal("b2", result[1]);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "HistoryLogItem")]
        public static void Host()
        {
            // Arrange
            var model = new HistoryLogItem()
            {
                Hosts = new[] { 2130706433, 2130706434 }
            };

            // Act
            var result = model.Host();

            // Assert
            Assert.Equal("127.0.0.1", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "HistoryLogItem")]
        public static void Message_ResourceManager_No_Args()
        {
            // Arrange
            var model = new HistoryLogItem()
            {
                Event = new HistoryLogEvent(0, "x", "FormatNoArgs")
            };

            // Act
            var result = model.Message(Properties.Resources.ResourceManager, CultureInfo.InvariantCulture);

            // Assert
            Assert.Equal("Format: no args", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "HistoryLogItem")]
        public static void Message_ResourceManager_With_Args()
        {
            // Arrange
            var model = new HistoryLogItem()
            {
                Event = new HistoryLogEvent(0, "x", "FormatWithArgs"),
                Arguments = "a1\tb2"
            };

            // Act
            var result = model.Message(Properties.Resources.ResourceManager, CultureInfo.InvariantCulture);

            // Assert
            Assert.Equal("Format: a1 and b2.", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "HistoryLogItem")]
        public static void Message_No_Args()
        {
            // Arrange
            var model = new HistoryLogItem()
            {
                Event = new HistoryLogEvent(0, "x", "Format: no args")
            };

            // Act
            var result = model.Message();

            // Assert
            Assert.Equal("Format: no args", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "HistoryLogItem")]
        public static void Message_With_Args()
        {
            // Arrange
            var model = new HistoryLogItem()
            {
                Event = new HistoryLogEvent(0, "x", "Format: {0} and {1}."),
                Arguments = "a1\tb2"
            };

            // Act
            var result = model.Message();

            // Assert
            Assert.Equal("Format: a1 and b2.", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "HistoryLogItem")]
        public static void Message_ResourceManager_Throws_InvalidOperationException()
        {
            // Arrange
            var model = new HistoryLogItem()
            {
                Event = new HistoryLogEvent(0, "x", "FormatX")
            };

            // Act
            Assert.Throws<InvalidOperationException>(() => 
                model.Message(Properties.Resources.ResourceManager, CultureInfo.InvariantCulture));

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "HistoryLogItem")]
        public static void Message_Throws_InvalidOperationException()
        {
            // Arrange
            var model = new HistoryLogItem()
            {
                Event = new HistoryLogEvent(0, "x", string.Empty)
            };

            // Act
            Assert.Throws<InvalidOperationException>(() =>
                model.Message());

            // Assert
        }
    }
}
