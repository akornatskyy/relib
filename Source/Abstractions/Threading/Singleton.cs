using System;
using System.Collections.Generic;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Threading
{
    public sealed class Singleton<T>
    {
        private readonly Func2<string, T> m_factory;
        private readonly IDictionary<string, T> m_instances = new Dictionary<string, T>();

        public Singleton(Func2<string, T> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            m_factory = factory;
        }

        public T Instance(string name)
        {
            T value;
            if (!m_instances.TryGetValue(name, out value))
            {
                lock (m_instances)
                {
                    if (!m_instances.TryGetValue(name, out value))
                    {
                        value = m_factory(name);
                        m_instances.Add(name, value);
                    }
                }
            }

            return value;
        }
    }
}
