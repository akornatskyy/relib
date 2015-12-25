using System;
using System.Text;

namespace ReusableLibrary.Abstractions.Models
{
    public sealed class Base64Encoder : TextEncoder
    {
        private readonly IEncoder m_inner;

        public Base64Encoder(IEncoder inner)
            : base(Encoding.ASCII)
        {
            m_inner = inner;
        }

        #region IEncoder Members

        public override byte[] GetBytes(string s)
        {
            return base.GetBytes(Convert.ToBase64String(m_inner.GetBytes(s)));
        }

        #endregion
    }
}
