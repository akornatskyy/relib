using System;
using System.Threading;
using Moq;
using ReusableLibrary.Abstractions.Models;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Models
{
    public sealed class WaitPoolTest : DecoratedPoolTest
    {
        private readonly WaitPool<string> m_waitPool;

        public WaitPoolTest()
        {
            MockInnerPool.Setup(inner => inner.Count).Returns(4);
            MockInnerPool.Setup(inner => inner.Size).Returns(4);
            m_waitPool = new WaitPool<string>(MockInnerPool.Object, 0);
            DecoratedPool = m_waitPool;
        }

        public static void Main()
        {
            using (var test = new WaitPoolTest())
            {
                test.Take_Inner_Take_Returns_Null();
            }
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "WaitPool")]
        public override void Take()
        {
            // Arrange
            MockInnerPool.Setup(inner => inner.Return("x")).Returns(true);
            MockInnerPool.Setup(inner => inner.Take(null)).Returns("x");
            m_waitPool.Return("x");

            // Act
            var result = m_waitPool.Take(null);

            // Assert
            Assert.Equal("x", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "WaitPool")]
        public void Take_Inner_Take_Returns_Null()
        {
            // Arrange
            int attempt = 0;
            MockInnerPool.Setup(inner => inner.Return("x")).Returns(true);
            MockInnerPool.Setup(inner => inner.Take(null)).Returns(() => attempt++ == 0 ? (string)null : "x");
            while (m_waitPool.Return("x")) 
            { 
            }

            // Act
            var result = m_waitPool.Take(null);

            // Assert
            Assert.Null(result);
            for (int i = 0; i < m_waitPool.Size; i++)
            {
                Assert.Equal("x", m_waitPool.Take(null));
            }

            Assert.Null(m_waitPool.Take(null));
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "WaitPool")]
        public void Return_More_Than_Taken()
        {
            // Arrange
            for (int i = 0; i < m_waitPool.Size; i++)
            {
                Return();
            }

            // Act
            var result = DecoratedPool.Return("data");

            // Assert
            Assert.False(result);
        }
    }
}
