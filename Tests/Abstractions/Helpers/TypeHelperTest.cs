using System;
using System.Collections.Generic;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Models;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Helpers
{
    public sealed class TypeHelperTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Helpers, "TypeHelper")]
        public static void GetName()
        {
            // Arrange

            // Act
            var result = TypeHelper.GetName<Version>();

            // Assert
            Assert.Equal("Version", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "TypeHelper")]
        public static void GetName_Generic()
        {
            // Arrange

            // Act
            var result = TypeHelper.GetName<IEnumerable<ValueObject<int>>>();

            // Assert
            Assert.Equal("IEnumerable`1[ValueObject`1[Int32]]", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "TypeHelper")]
        public static void GetName_Static_Delegate()
        {
            // Arrange

            // Act
            var result = TypeHelper.GetName(new Action(GetName_Static_Delegate));

            // Assert
            Assert.Equal("TypeHelperTest.GetName_Static_Delegate", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "TypeHelper")]
        public void GetName_Instance_Delegate()
        {
            // Arrange

            // Act
            var result = TypeHelper.GetName(new Action(GetName_Instance_Delegate));

            // Assert
            Assert.Equal("TypeHelperTest.GetName_Instance_Delegate", result);
        }
    }
}
