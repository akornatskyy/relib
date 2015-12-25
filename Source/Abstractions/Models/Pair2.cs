using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace ReusableLibrary.Abstractions.Models
{
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct Pair<TFirst, TSecond>
    {
        private readonly TFirst m_first;
        private readonly TSecond m_second;

        public Pair(TFirst first, TSecond second)
        {
            m_first = first;
            m_second = second;
        }

        public TFirst First 
        {
            get { return m_first; } 
        }

        public TSecond Second 
        {
            get { return m_second; } 
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "Pair<'{0}','{1}'>", First, Second);
        }
    }
}
