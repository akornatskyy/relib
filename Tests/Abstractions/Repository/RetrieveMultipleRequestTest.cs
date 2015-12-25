using System;
using ReusableLibrary.Abstractions.Repository;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Repository
{
    public sealed class RetrieveMultipleRequestTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Repository, "RetrieveMultipleRequest")]
        public static void Constructor_Specification_IsNotNull()
        {
            // Arrange
            var specification = "test";

            // Act
            var req = new RetrieveMultipleRequest<string>(specification);

            // Assert
            Assert.NotNull(req);
            Assert.Equal(0, req.PageIndex);
            Assert.Equal(10, req.PageSize);
            Assert.Equal(specification, req.Specification);
        }

        [Fact]
        [Trait(Constants.TraitNames.Repository, "RetrieveMultipleRequest")]
        public static void Constructor_Specification_IsNull()
        {
            // Arrange
            string specification = null;

            // Act
            Assert.Throws<ArgumentNullException>(() =>
            {
                var req = new RetrieveMultipleRequest<string>(specification);
                Assert.Null(req);
            });

            // Assert
        }
    }
}
