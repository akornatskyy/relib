using System.IO;

namespace ReusableLibrary.Abstractions.IO
{
    public abstract class DecoratedStream : Stream
    {
        private readonly Stream m_inner;

        protected DecoratedStream(Stream inner)
        {
            m_inner = inner;
        }

        #region Stream Members

        public override bool CanRead
        {
            get { return m_inner.CanRead; }
        }

        public override bool CanSeek
        {
            get { return m_inner.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return m_inner.CanWrite; }
        }

        public override void Flush()
        {
            m_inner.Flush();
        }

        public override long Length
        {
            get { return m_inner.Length; }
        }

        public override long Position
        {
            get
            {
                return m_inner.Position;
            }

            set
            {
                m_inner.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return m_inner.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return m_inner.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            m_inner.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            m_inner.Write(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    m_inner.Close();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        #endregion
    }
}
