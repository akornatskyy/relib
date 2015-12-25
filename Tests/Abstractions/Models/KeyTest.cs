using System;
using System.Collections.Generic;
using ReusableLibrary.Abstractions.Models;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Models
{
    public sealed class KeyTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Models, "Key")]
        public static void From_Object()
        {
            // Arrange

            // Act
            var result = Key.From<Version>("536543");

            // Assert
            Assert.Equal("Version String='536543'", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "Key")]
        public static void From_Generic_Object()
        {
            // Arrange

            // Act
            var result = Key.From<IEnumerable<ValueObject<int>>>("536543", 15);

            // Assert
            Assert.Equal("IEnumerable`1[ValueObject`1[Int32]] String='536543' Int32='15'", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "Key")]
        public static void From_Generic()
        {
            // Arrange

            // Act
            var result = Key.From<IEnumerable<ValueObject<int>>>();

            // Assert
            Assert.Equal("IEnumerable`1[ValueObject`1[Int32]]", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "Key")]
        public static void From_Object_Null()
        {
            // Arrange

            // Act
            var result = Key.From<Version>((string)null);

            // Assert
            Assert.Equal("Version null", result);
        }

        [Theory]
        [InlineData("Model TestA='412343' TestB='HELLO'", 412343, "Hello")]
        [InlineData("Model TestA='1' TestB='SOMETHING'", 1, "something")]
        [InlineData("Model TestA='250' TestB=''", 250, null)]
        [InlineData("Model TestA='' TestB=''", null, null)]
        [Trait(Constants.TraitNames.Models, "Key")]
        public static void From_Model(string expected, int? a, string b)
        {
            // Arrange
            var model = new Model { TestA = a, TestB = b };

            // Act
            var result = Key.From(model);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "Key")]
        public static void From_Model_Property_Implements_IKeyProvider()
        {
            // Arrange
            var model = new Model2 { Data = new KeyProvider() };

            // Act
            var result = Key.From(model);

            // Assert
            Assert.Equal("Model2 Data='X'", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "Key")]
        public static void From_IKeyProvider()
        {
            // Arrange
            var model = new KeyProvider();

            // Act
            var result = Key.From(model);

            // Assert
            Assert.Equal("x", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "Key")]
        public static void From_Model_Null()
        {
            // Arrange

            // Act
            var result = Key.From<string>((string)null);

            // Assert
            // Assert
            Assert.Equal("String=null", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "Key")]
        public static void From_Delegate()
        {
            // Arrange

            // Act
            var result = Key.From(new Action(From_Delegate), "536543");

            // Assert
            Assert.Equal("KeyTest.From_Delegate String='536543'", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "Key")]
        public static void From_Delegate_Null_Throws_ArgumentNullException()
        {
            // Arrange
            Action action = null;

            // Act
            Assert.Throws<ArgumentNullException>(() => Key.From((Delegate)action));

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "Key")]
        public static void From_Delegate_Model()
        {
            // Arrange
            var model = new Model { TestA = 100, TestB = "b" };

            // Act
            var result = Key.From(new Action(From_Delegate), model, Guid.Empty);

            // Assert
            Assert.Equal("KeyTest.From_Delegate Model TestA='100' TestB='B' Guid='00000000-0000-0000-0000-000000000000'", 
                result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "Key")]
        public static void From_Delegate_IKeyProvider()
        {
            // Arrange
            var model = new Model2 { Data = new KeyProvider() };

            // Act
            var result = Key.From(new Action(From_Delegate), model);

            // Assert
            Assert.Equal("KeyTest.From_Delegate Model2 Data='X'", result);
        }

        private sealed class KeyProvider : IKeyProvider
        {
            #region IKeyProvider Members

            public string ToKeyString()
            {
                return "x";
            }

            #endregion
        }

        private sealed class Model
        {
            public int? TestA { get; set; }

            public string TestB { get; set; }
        }

        private sealed class Model2
        {
            public KeyProvider Data { get; set; }
        }
    }
}
