using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace ReusableLibrary.Abstractions.Models
{
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct Range<T>
    {
        private readonly T m_from;
        private readonly T m_to;

        public Range(T from, T to)
        {
            m_from = from;
            m_to = to;
        }

        public T From 
        { 
            get { return m_from; } 
        }

        public T To 
        { 
            get { return m_to; } 
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "Range<'{0}'-'{1}'>", From, To);
        }
    }
}
