using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using ReusableLibrary.Abstractions.IoC;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Unity
{
    public class UnityDependencyResolver : Disposable, IDependencyResolver
    {
        private readonly IUnityContainer m_container;

        [DebuggerStepThrough]
        public UnityDependencyResolver(UnityConfigurationSection[] sections)
            : this(new UnityContainer())
        {
            Load(sections);
        }

        [DebuggerStepThrough]
        public UnityDependencyResolver(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            m_container = container;
        }

        #region IDependencyResolver Members

        [DebuggerStepThrough]
        public T Resolve<T>()
        {
            return m_container.Resolve<T>();
        }

        [DebuggerStepThrough]
        public T Resolve<T>(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            return m_container.Resolve<T>(name);
        }

        [DebuggerStepThrough]
        public T Resolve<T>(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return (T)m_container.Resolve(type);
        }

        [DebuggerStepThrough]
        public T Resolve<T>(Type type, string name)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            return (T)m_container.Resolve(type, name);
        }

        [DebuggerStepThrough]
        public IEnumerable<T> ResolveAll<T>()
        {
            IEnumerable<T> namedInstances = m_container.ResolveAll<T>();
            T unnamedInstance = default(T);

            try
            {
                unnamedInstance = m_container.Resolve<T>();
            }
            catch (ResolutionFailedException)
            {
                // When default instance is missing
            }

            if (Equals(unnamedInstance, default(T)))
            {
                return namedInstances;
            }

            return new ReadOnlyCollection<T>(new List<T>(namedInstances) { unnamedInstance });
        }

        #endregion

        [DebuggerStepThrough]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_container.Dispose();
            }
        }

        private void Load(UnityConfigurationSection[] sections)
        {
            foreach (var section in sections)
            {
                Load(section);
            }
        }

        private void Load(UnityConfigurationSection configuration)
        {
            if (configuration == null)
            {
                throw new ConfigurationErrorsException("Unity configuration is missing");
            }

            configuration.Configure(m_container);
        }
    }
}
