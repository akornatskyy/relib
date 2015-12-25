using System;
using System.Diagnostics;

namespace ReusableLibrary.Abstractions.Models
{
    /// <summary>
    /// see http://www.martinfowler.com/bliki/ValueObject.html
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class ValueObject<T>
    {
        private readonly T m_key;
        private readonly string m_displayName;

        public ValueObject(T key, string displayName)
        {
            m_key = key;
            m_displayName = displayName;
        }

        public T Key
        {
            [DebuggerStepThrough]
            get { return m_key; }
        }

        public string DisplayName
        {
            [DebuggerStepThrough]
            get { return m_displayName; }
        }
    }
}
