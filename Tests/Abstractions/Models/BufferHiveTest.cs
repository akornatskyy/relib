using System;
using ReusableLibrary.Abstractions.Models;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Models
{
    public sealed class BufferHiveTest : IDisposable
    {
        private readonly BufferHive<byte> m_bufferPool;

        public BufferHiveTest()
        {
            m_bufferPool = new BufferHive<byte>(4);
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_bufferPool.Dispose();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Models, "BufferHive")]
        public void Allocate()
        {
            // Arrange

            // Act
            using (var result = m_bufferPool.Allocate(10))
            {
                // Assert
                Assert.NotNull(result);
                var buffer = result.Item;
                Assert.NotNull(buffer);
                Assert.True(buffer.Capacity >= 10);
            }
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "BufferHive")]
        public void Allocate_Over_PoolSize()
        {
            // Arrange

            // Act
            for (int i = 0; i < 10; i++)
            {
                var result = m_bufferPool.Allocate(10);

                // Assert
                Assert.NotNull(result);
            }
        }
    }
}
