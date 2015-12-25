using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace ReusableLibrary.Abstractions.Models
{
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct Pair<T>
    {
        private readonly T m_first;
        private readonly T m_second;

        public Pair(T first, T second)
        {
            m_first = first;
            m_second = second;
        }

        public T First 
        {
            get { return m_first; } 
        }

        public T Second 
        {
            get { return m_second; } 
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "Pair<'{0}','{1}'>", First, Second);
        }
    }
}
