using System;
using System.Text;

namespace ReusableLibrary.Abstractions.Models
{
    public class TextEncoder : IEncoder
    {
        private readonly Encoding m_encoding;

        public TextEncoder(Encoding encoding)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            m_encoding = encoding;
        }

        #region IEncoder Members

        public virtual byte[] GetBytes(string s)
        {
            return m_encoding.GetBytes(s);
        }

        #endregion
    }
}
