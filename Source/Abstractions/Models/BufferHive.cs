using System;
using System.Threading;

namespace ReusableLibrary.Abstractions.Models
{
    public sealed class BufferHive<T> : Disposable
    {
        private readonly object m_sync = new object();
        private readonly IPool<Buffer<T>> m_pool;

        public BufferHive(int size)
            : this(size, Timeout.Infinite)
        {
        }

        public BufferHive(int size, int accessTimeout)
        {
            m_pool = new LazyPool<Buffer<T>>(
                new SynchronizedPool<Buffer<T>>(m_sync,
                    new StackPool<Buffer<T>>("BufferHive", size), 
                    accessTimeout),
                CreateFactory, ReleaseFactory);
        }

        public Pooled<Buffer<T>> Allocate(int bufferSize)
        {
            if (bufferSize <= 0)
            {
                throw new ArgumentOutOfRangeException("bufferSize");
            }

            var pooled = new Pooled<Buffer<T>>(m_pool, bufferSize);
            if (pooled.Item == null)
            {
                throw new InvalidOperationException("Unable obtain a buffer from pool");
            }

            pooled.Item.EnsureCapacity(bufferSize);
            return pooled;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_pool.Dispose();
            }
        }

        private static Buffer<T> CreateFactory(object state)
        {
            return new Buffer<T>((int)state);
        }

        private static void ReleaseFactory(Buffer<T> buffer)
        {
        }
    }
}
