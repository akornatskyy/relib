using System.Collections.Generic;
using ReusableLibrary.Abstractions.Models;
using Xunit;

namespace ReusableLibrary.Web.Mvc.Tests
{
    public sealed class EnumerableExtensionsTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Mvc, "EnumerableExtensions")]
        public static void ToSelectList()
        {
            // Arrange
            var selectedValue = 1;
            var list = new List<ValueObject<int>>();

            // Act
            var selectList = list.ToSelectList(selectedValue);

            // Assert
            Assert.Equal("Key", selectList.DataValueField);
            Assert.Equal("DisplayName", selectList.DataTextField);
            Assert.Equal(list, selectList.Items);
            Assert.Equal(selectedValue, selectList.SelectedValue);
        }
    }
}
