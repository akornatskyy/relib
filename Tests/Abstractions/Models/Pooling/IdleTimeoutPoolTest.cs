using System;
using System.Collections.Generic;
using Moq;
using ReusableLibrary.Abstractions.Models;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Models
{
    public sealed class IdleTimeoutPoolTest : DecoratedPoolTest
    {
        private int m_releaseFactoryCalls;

        public IdleTimeoutPoolTest()
        {
            MockInnerPool.Setup(inner => inner.Size).Returns(2);
            IdleTimeoutPool = new IdleTimeoutPool2(MockInnerPool.Object,
                new Action<PoolItem>(item => m_releaseFactoryCalls++));
            DecoratedPool = IdleTimeoutPool;
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "IdleTimeoutPool")]
        public void Take_Inner_Returned_Null()
        {
            // Arrange            
            MockInnerPool.Setup(inner => inner.Take(null)).Returns((string)null);

            // Act
            var result = DecoratedPool.Take(null);

            // Assert
            Assert.Null(result);
        }

        public override void Take()
        {
            // The default behavior is overriden
        }

        public override void Return()
        {
            // The default behavior is overriden
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "IdleTimeoutPool")]
        public void RecycleIdled()
        {
            // Arrange
            var current = DateTime.UtcNow;
            Assert.False(IdleTimeoutPool.TryIdleOut(new PoolItem("x"), current.AddSeconds(-10)));
            Assert.False(IdleTimeoutPool.TryIdleOut("y", current.AddSeconds(-9)));
            Assert.False(IdleTimeoutPool.TryIdleOut("z", current.AddSeconds(-9)));
            Assert.False(IdleTimeoutPool.TryIdleOut("_", current.AddSeconds(-10)));

            var taken = new Queue<string>(new string[] { "x", "y", "z", "_", null });
            MockInnerPool.Setup(inner => inner.Count).Returns(4);
            MockInnerPool.Setup(inner => inner.Take(null)).Returns(() => taken.Dequeue());
            
            MockInnerPool.Setup(inner => inner.Return(It.IsAny<string>()))
                .Callback((string item) => taken.Enqueue(item))
                .Returns(() => true);

            // Act
            lock (MockInnerPool.Object)
            {
                Assert.Equal(2, IdleTimeoutPool.RecycleIdled(current));
            }

            // Assert
            Assert.Equal(2, m_releaseFactoryCalls);
            Assert.Equal("z", IdleTimeoutPool.TakeCurrent(null, current));
            Assert.Equal("y", IdleTimeoutPool.TakeCurrent(null, current));            
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "IdleTimeoutPool")]
        public void TakeCurrent_Not_IdledOut()
        {
            // Arrange
            var current = DateTime.UtcNow;
            Assert.False(IdleTimeoutPool.TryIdleOut("x", current.AddSeconds(-1)));
            MockInnerPool.Setup(inner => inner.Take(null)).Returns("x");

            // Act
            var result = IdleTimeoutPool.TakeCurrent(null, current);

            // Assert
            Assert.Equal("x", result);
            Assert.Equal(0, m_releaseFactoryCalls);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "IdleTimeoutPool")]
        public void TakeCurrent_IdledOut()
        {
            // Arrange
            var current = DateTime.UtcNow;
            Assert.False(IdleTimeoutPool.TryIdleOut("x", current.AddSeconds(-10)));
            Assert.False(IdleTimeoutPool.TryIdleOut("y", current.AddSeconds(-10)));
            var items = new Queue<string>(new string[] { "x", "y", null });
            MockInnerPool.Setup(inner => inner.Take(null)).Returns(() => items.Dequeue());

            // Act
            var result = IdleTimeoutPool.TakeCurrent(null, current);

            // Assert
            Assert.Null(result);
            Assert.Equal(2, m_releaseFactoryCalls);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "IdleTimeoutPool")]
        public void TakeCurrent_First_Not_IdledOut()
        {
            // Arrange
            var current = DateTime.UtcNow;
            Assert.False(IdleTimeoutPool.TryIdleOut("x", current.AddSeconds(-10)));
            Assert.False(IdleTimeoutPool.TryIdleOut("y", current.AddSeconds(-9)));
            Assert.False(IdleTimeoutPool.TryIdleOut("z", current.AddSeconds(-9)));
            Assert.False(IdleTimeoutPool.TryIdleOut("_", current.AddSeconds(-10)));
            var items = new Queue<string>(new string[] { "x", "y", "z", "_", null });
            MockInnerPool.Setup(inner => inner.Take(null)).Returns(() => items.Dequeue());

            // Act
            var result = IdleTimeoutPool.TakeCurrent(null, current);

            // Assert
            Assert.Equal("y", result);
            Assert.Equal(1, m_releaseFactoryCalls);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "IdleTimeoutPool")]
        public void TryIdleOut_New()
        {
            // Arrange
            var current = DateTime.UtcNow;

            // Act
            var result = IdleTimeoutPool.TryIdleOut("x", current);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "IdleTimeoutPool")]
        public void TryIdleOut_Existing_NotExpired()
        {
            // Arrange
            var current = DateTime.UtcNow;
            Assert.False(IdleTimeoutPool.TryIdleOut("x", current.AddSeconds(-1)));

            // Act
            var result = IdleTimeoutPool.TryIdleOut("x", current);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "IdleTimeoutPool")]
        public void TryIdleOut_Existing_Expired()
        {
            // Arrange
            var current = DateTime.UtcNow;
            Assert.False(IdleTimeoutPool.TryIdleOut("x", current.AddSeconds(-10)));

            // Act
            var result = IdleTimeoutPool.TryIdleOut("x", current);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "IdleTimeoutPool")]
        public void CheckExpired_Idle_NoExpiration()
        {
            // Arrange
            var current = DateTime.UtcNow;
            var state = new IdleState(current.AddSeconds(-9));

            // Act
            var result = IdleTimeoutPool.CheckExpired(state, current);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "IdleTimeoutPool")]
        public void CheckExpired_Idle()
        {
            // Arrange
            var current = DateTime.UtcNow;
            var state = new IdleState(current.AddSeconds(-10));

            // Act
            var result = IdleTimeoutPool.CheckExpired(state, current);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "IdleTimeoutPool")]
        public void CheckExpired_Lease_NoExpiration()
        {
            // Arrange
            var current = DateTime.UtcNow;
            var state = new IdleState(current.AddSeconds(-19));
            state.UsedOn = current;

            // Act
            var result = IdleTimeoutPool.CheckExpired(state, current);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "IdleTimeoutPool")]
        public void CheckExpired_Lease()
        {
            // Arrange
            var current = DateTime.UtcNow;
            var state = new IdleState(current.AddSeconds(-20));
            state.UsedOn = current;

            // Act
            var result = IdleTimeoutPool.CheckExpired(state, current);

            // Assert
            Assert.True(result);
        }

        private IdleTimeoutPool2 IdleTimeoutPool { get; set; }

        private class IdleTimeoutPool2 : IdleTimeoutPool<PoolItem>
        {
            public IdleTimeoutPool2(IPool<PoolItem> inner, Action<PoolItem> releasefactory)
                : base(inner, inner, releasefactory,
                10000,
                20000)
            {
            }

            public new virtual int RecycleIdled(DateTime current)
            {
                return base.RecycleIdled(current);
            }

            public new virtual PoolItem TakeCurrent(object state, DateTime current)
            {
                return base.TakeCurrent(state, current);
            }

            public new virtual bool CheckExpired(IdleState state, DateTime current)
            {
                return base.CheckExpired(state, current);
            }
        }

        private class PoolItem : IIdleStateProvider
        {
            public PoolItem(string name)
            {
                Name = name;
            }

            public string Name { get; set; }

            #region IIdleStateProvider Members

            public IdleState IdleState { get; set; }

            #endregion
        }
    }
}
