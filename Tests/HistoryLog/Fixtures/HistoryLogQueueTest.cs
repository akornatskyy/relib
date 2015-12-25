using System;
using System.Collections.Generic;
using ReusableLibrary.HistoryLog.Models;
using Xunit;

namespace ReusableLibrary.HistoryLog.Tests.Fixtures
{
    public sealed class HistoryLogQueueTest
    {
        private readonly HistoryLogQueue m_queue;

        public HistoryLogQueueTest()
        {
            m_queue = new HistoryLogQueue();
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "HistoryLogQueue")]
        public void Enqueue_Null()
        {
            // Arrange

            // Act
            Assert.Throws<ArgumentNullException>(() => m_queue.Enqueue(null));

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "HistoryLogQueue")]
        public void Enqueue()
        {
            // Arrange
            Assert.True(m_queue.IsEmpty());
            var item = new HistoryLogItem();

            // Act
            m_queue.Enqueue(item);

            // Assert
            Assert.False(m_queue.IsEmpty());
            m_queue.Process(x => Assert.Equal(item, x));
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "HistoryLogQueue")]
        public void IsEmpty()
        {
            // Arrange
            Assert.True(m_queue.IsEmpty());

            // Act
            m_queue.Enqueue(new HistoryLogItem());

            // Assert
            Assert.False(m_queue.IsEmpty());
            m_queue.Process(x => { });
            Assert.True(m_queue.IsEmpty());
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "HistoryLogQueue")]
        public void Process()
        {
            // Arrange
            Assert.True(m_queue.IsEmpty());
            var item1 = new HistoryLogItem();
            var item2 = new HistoryLogItem();
            m_queue.Enqueue(item1);
            m_queue.Enqueue(item2);
            var items = new List<HistoryLogItem>();

            // Act
            m_queue.Process(x => items.Add(x));

            // Assert
            Assert.Equal(item1, items[0]);
            Assert.Equal(item2, items[1]);            
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "HistoryLogQueue")]
        public void Process_Queue_IsEmpty()
        {
            // Arrange
            Assert.True(m_queue.IsEmpty());
            var called = false;

            // Act
            m_queue.Process(x => called = true);

            // Assert
            Assert.False(called);
        }
    }
}
