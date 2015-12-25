using System;
using ReusableLibrary.Abstractions.Models;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Models
{
    public sealed class TopicTest : IDisposable
    {
        private readonly Topic m_topic;

        public TopicTest()
        {
            m_topic = new Topic("topic://relib.abstractions/test");
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_topic.Dispose();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Models, "Topic")]
        public void Subscribe()
        {
            // Arrange

            // Act
            m_topic.Subscribe(() => { }, DelegateInvokeStrategy.Publisher);
            Assert.Equal(1, m_topic.Length);

            m_topic.Subscribe(() => { }, DelegateInvokeStrategy.Publisher);            

            // Assert            
            Assert.Equal(2, m_topic.Length);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "Topic")]
        public void Publish()
        {
            // Arrange
            int count = 0;
            m_topic.Subscribe(() => count++, DelegateInvokeStrategy.Publisher);

            // Act
            m_topic.Publish();

            // Assert
            Assert.Equal(1, m_topic.Length);
            Assert.Equal(1, count);
        }

        [Theory]
        [InlineData(10)]
        [Trait(Constants.TraitNames.Models, "Topic")]
        public void Publish_Many_Subscribers(int number)
        {
            // Arrange
            int count = 0;
            for (int i = 0; i < number; i++)
            {
                m_topic.Subscribe(() => count++, DelegateInvokeStrategy.Publisher);
            }

            // Act
            m_topic.Publish();

            // Assert
            Assert.Equal(number, m_topic.Length);
            Assert.Equal(number, count);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "Topic")]
        public void Unsubscribe()
        {
            // Arrange
            int count = 0;
            Action2 action1 = () => count++;
            Action2 action2 = () => count++;
            m_topic.Subscribe(action1, DelegateInvokeStrategy.Publisher);
            m_topic.Subscribe(action1, DelegateInvokeStrategy.Publisher);
            m_topic.Subscribe(action2, DelegateInvokeStrategy.Publisher);
            Assert.Equal(3, m_topic.Length);

            // Act
            m_topic.Unsubscribe(action1);

            // Assert
            Assert.Equal(1, m_topic.Length);
            m_topic.Publish();            
            Assert.Equal(1, count);
        }
    }
}
