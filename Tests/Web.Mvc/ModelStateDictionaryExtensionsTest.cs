using System.Web.Mvc;
using Xunit;

namespace ReusableLibrary.Web.Mvc.Tests
{
    public sealed class ModelStateDictionaryExtensionsTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Mvc, "ModelStateDictionaryExtensions")]
        public static void ResetModelValue()
        {
            // Arrange
            var modelStateDictionary = new ModelStateDictionary();
            modelStateDictionary.Add("test", new ModelState() 
            {  
                Value = new ValueProviderResult("x", "x", null) 
            });

            // Act
            var result = modelStateDictionary["test"].Value;
            Assert.Equal("x", result.RawValue);
            Assert.Equal("x", result.AttemptedValue);
            modelStateDictionary.ResetModelValue("test");

            // Assert
            result = modelStateDictionary["test"].Value;
            Assert.Null(result.RawValue);
            Assert.Null(result.AttemptedValue);
        }

        [Fact]
        [Trait(Constants.TraitNames.Mvc, "ModelStateDictionaryExtensions")]
        public static void ResetModelValue_Key_NotFound()
        {
            // Arrange
            var modelStateDictionary = new ModelStateDictionary();

            // Act
            modelStateDictionary.ResetModelValue("test");

            // Assert
            var value = modelStateDictionary["test"];
            Assert.Null(value);
        }
    }
}
