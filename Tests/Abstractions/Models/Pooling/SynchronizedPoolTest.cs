using System;
using System.Threading;
using ReusableLibrary.Abstractions.Models;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Models
{
    public class SynchronizedPoolTest : DecoratedPoolTest
    {        
        private readonly AutoResetEvent m_sync;

        public SynchronizedPoolTest()
        {
            SynchronizedPool = new SynchronizedPool<string>(this, MockInnerPool.Object, TimeSpan.Zero);
            DecoratedPool = SynchronizedPool;
            m_sync = new AutoResetEvent(false);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "SynchronizedPool")]
        public void Clear_TimedOut()
        {
            // Arrange

            // Act
            bool result = true;
            WaitThread(() => result = SynchronizedPool.Clear());

            // Assert   
            Assert.False(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "SynchronizedPool")]
        public void Take_TimedOut()
        {
            // Arrange

            // Act
            string result = "x";
            WaitThread(() => result = SynchronizedPool.Take(null));

            // Assert   
            Assert.Null(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "SynchronizedPool")]
        public virtual void Return_TimedOut()
        {
            // Arrange

            // Act
            bool result = true;
            WaitThread(() => result = SynchronizedPool.Return(null));

            // Assert   
            Assert.False(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "SynchronizedPool")]
        public void Count_TimedOut()
        {
            // Arrange

            // Act
            int result = 0;
            WaitThread(() => result = SynchronizedPool.Count);

            // Assert   
            Assert.Equal(-1, result);
        }

        protected SynchronizedPool<string> SynchronizedPool { get; set; }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            m_sync.Close();
        }

        protected void WaitThread(Action action)
        {
            lock (this)
            {
                ThreadPool.QueueUserWorkItem(state =>
                {
                    action();
                    m_sync.Set();
                });

                m_sync.WaitOne();
            }
        }
    }
}
