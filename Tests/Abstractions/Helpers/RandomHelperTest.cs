using System;
using System.Collections.Generic;
using ReusableLibrary.Abstractions.Helpers;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Helpers
{
    public sealed class RandomHelperTest
    {
        private static readonly Random g_random = new Random();

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "RandomHelper")]
        public static void NextChar()
        {
            // Arrange
            var characterGroup = RandomHelper.NextSentence(g_random, 4);

            // Act
            var c = RandomHelper.NextChar(g_random, characterGroup);

            // Assert
            Assert.Contains(c, characterGroup);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [Trait(Constants.TraitNames.Helpers, "RandomHelper")]
        public static void NextChar_NullOrEmpty(string characterGroup)
        {
            // Arrange

            // Act
            var c = RandomHelper.NextChar(g_random, characterGroup);

            // Assert
            Assert.Equal(Char.MinValue, c);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "RandomHelper")]
        public static void NextString()
        {
            // Arrange
            var characterGroup = "Hello World";

            // Act
            var str = RandomHelper.NextString(g_random, 20, characterGroup);

            // Assert
            Assert.Equal(20, str.Length);
            foreach (var c in str)
            {
                Assert.Contains(c, characterGroup);
            }
        }

        [Theory]
        [InlineData(new int[] { 1 })]
        [InlineData(new int[] { 2, 5 })]
        [Trait(Constants.TraitNames.Helpers, "RandomHelper")]
        public static void NextSentences(int[] wordsNumber)
        {
            // Arrange

            // Act
            var result = RandomHelper.NextSentences(g_random, wordsNumber);

            // Assert
            var sentences = result.Split(new[] { ". ", "." }, StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(wordsNumber.Length, sentences.Length);
            EnumerableHelper.ForEach(sentences, (index, sentence) => 
            {
                Assert.Equal(wordsNumber[index], sentence.Split(' ').Length);
            });
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "RandomHelper")]
        public static void TimesRandom()
        {
            // Arrange
            var list = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var seq = new List<int>();

            // Act
            RandomHelper.TimesRandom(g_random, list, list.Length * 2, (item) => seq.Add(item));

            // Assert
            Assert.Equal(list.Length * 2, seq.Count);
            foreach (var item in list)
            {
                Assert.Contains(item, seq);
            }
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "RandomHelper")]
        public static void FirstRandom()
        {
            // Arrange
            var list = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            // Act
            var first = RandomHelper.FirstRandom(g_random, list);

            // Assert
            Assert.Contains(first, list);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "RandomHelper")]
        public static void NextBoolean()
        {
            // Arrange
            var list = new bool[100];

            // Act
            for (int i = 0; i < 100; i++)
            {
                list[i] = RandomHelper.NextBoolean(g_random);
            }

            // Assert
            Assert.Contains(true, list);
            Assert.Contains(false, list);
        }

        [Theory]
        [InlineData("hello world", 1, 5)]
        [InlineData("hello world", 3, 7)]
        [Trait(Constants.TraitNames.Helpers, "RandomHelper")]
        public static void NextStartsWith(string text, int minLength, int maxLength)
        {
            // Arrange

            // Act
            var next = RandomHelper.NextStartsWith(g_random, text, minLength, maxLength);

            // Assert
            Assert.True(text.StartsWith(next, StringComparison.Ordinal));
        }

        [Theory]
        [InlineData("hello world", 1, 5)]
        [InlineData("hello world", 3, 7)]
        [Trait(Constants.TraitNames.Helpers, "RandomHelper")]
        public static void NextSubstring(string text, int minLength, int maxLength)
        {
            // Arrange

            // Act
            var next = RandomHelper.NextSubstring(g_random, text, minLength, maxLength);

            // Assert
            Assert.Contains(next, text, StringComparison.Ordinal);
        }

        [Theory]
        [InlineData(1, 100)]
        [InlineData(-100, 1)]
        [Trait(Constants.TraitNames.Helpers, "RandomHelper")]
        public static void NextInt(int min, int max)
        {
            // Arrange

            // Act
            var next = RandomHelper.NextInt(g_random, min, max);

            // Assert
            Assert.InRange(next, min, max);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(-100)]
        [Trait(Constants.TraitNames.Helpers, "RandomHelper")]
        public static void NextDate(int daysOffset)
        {
            // Arrange
            var source = DateTime.Now;

            // Act
            var next = RandomHelper.NextDate(g_random, source, daysOffset);

            // Assert
            Assert.NotEqual(next.Millisecond, source.Millisecond);
            Assert.NotEqual(next.Second, source.Second);
            Assert.NotEqual(next.Minute, source.Minute);
            Assert.NotEqual(next.Hour, source.Hour);

            if (daysOffset < 0)
            {
                Assert.True(next <= source);
            }
            else
            {
                Assert.True(next >= source);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        [Trait(Constants.TraitNames.Helpers, "RandomHelper")]
        public static void NextSentence_WordsNumber_NegativeOrZero(int wordsNumber)
        {
            // Arrange

            // Act
            var sentence = RandomHelper.NextSentence(g_random, wordsNumber);

            // Assert
            Assert.Equal(string.Empty, sentence);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        [Trait(Constants.TraitNames.Helpers, "RandomHelper")]
        public static void NextSentence(int wordsNumber)
        {
            // Arrange

            // Act
            var sentence = RandomHelper.NextSentence(g_random, wordsNumber);

            // Assert
            Assert.Equal(wordsNumber, sentence.Split(' ').Length);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "RandomHelper")]
        public static void NextWord()
        {
            // Arrange
            var words = new[] { "hello", "world" };

            // Act
            var word = RandomHelper.NextWord(g_random, words);

            // Assert
            Assert.Contains(word, words);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "RandomHelper")]
        public static void NextWord_But_Empty()
        {
            // Arrange
            var words = new string[] { };

            // Act
            var word = RandomHelper.NextWord(g_random, words);

            // Assert
            Assert.Null(word);
        }
    }
}
