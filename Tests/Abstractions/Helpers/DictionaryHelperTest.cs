using System;
using System.Collections;
using System.Collections.Generic;
using ReusableLibrary.Abstractions.Helpers;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Helpers
{
    public sealed class DictionaryHelperTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Helpers, "DictionaryHelper")]
        public static void TryGetValue_Dictionary_IsNull()
        {
            // Arrange
            IDictionary dictionary = null;
            int result = -1;

            // Act
            Assert.Throws<ArgumentNullException>(() => DictionaryHelper.TryGetValue(dictionary, "test", out result));

            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "DictionaryHelper")]
        public static void TryGetValue_DoesNot_Contain_Key()
        {
            // Arrange
            IDictionary dictionary = new Dictionary<string, int>();
            int value = -1;

            // Act
            var result = DictionaryHelper.TryGetValue(dictionary, "test", out value);

            // Assert
            Assert.False(result);
            Assert.Equal(0, value);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "DictionaryHelper")]
        public static void TryGetValue_ValueType_IsNot_Expected()
        {
            // Arrange
            IDictionary dictionary = new Dictionary<string, int>();
            dictionary.Add("test", 100);
            string value = string.Empty;

            // Act
            var result = DictionaryHelper.TryGetValue(dictionary, "test", out value);

            // Assert
            Assert.False(result);
            Assert.Equal(null, value);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "DictionaryHelper")]
        public static void TryGetValue()
        {
            // Arrange
            IDictionary dictionary = new Dictionary<string, int>();
            dictionary.Add("test", 100);
            int value = -1;

            // Act
            var result = DictionaryHelper.TryGetValue(dictionary, "test", out value);

            // Assert
            Assert.True(result);
            Assert.Equal(100, value);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "DictionaryHelper")]
        public static void GetValue_DoesNot_Contain_Key()
        {
            // Arrange
            IDictionary dictionary = new Dictionary<string, int>();

            // Act
            var result = DictionaryHelper.GetValue(dictionary, "test", -1);

            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "DictionaryHelper")]
        public static void GetValue()
        {
            // Arrange
            IDictionary dictionary = new Dictionary<string, int>();
            dictionary.Add("test", 100);

            // Act
            var result = DictionaryHelper.GetValue(dictionary, "test", -1);

            // Assert
            Assert.Equal(100, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "DictionaryHelper")]
        public static void Generic_GetValue_DoesNot_Contain_Key()
        {
            // Arrange
            IDictionary<string, int> dictionary = new Dictionary<string, int>();

            // Act
            var result = DictionaryHelper.GetValue(dictionary, "test", -1);

            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "DictionaryHelper")]
        public static void Generic_GetValue()
        {
            // Arrange
            IDictionary<string, int> dictionary = new Dictionary<string, int>();
            dictionary.Add("test", 100);

            // Act
            var result = DictionaryHelper.GetValue(dictionary, "test", -1);

            // Assert
            Assert.Equal(100, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "DictionaryHelper")]
        public static void UniqueueValues()
        {
            // Arrange
            IDictionary<string, int> dictionary = new Dictionary<string, int>();
            dictionary.Add("1", 100);
            dictionary.Add("2", 200);
            dictionary.Add("3", 100);
            dictionary.Add("4", 200);
            dictionary.Add("5", 100);
            dictionary.Add("6", 300);

            // Act
            var result = new List<int>(DictionaryHelper.UniqueueValues(dictionary));

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains(100, result);
            Assert.Contains(200, result);
            Assert.Contains(300, result);
        }
    }
}
