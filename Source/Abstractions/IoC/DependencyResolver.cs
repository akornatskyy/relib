using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ReusableLibrary.Abstractions.IoC
{
    public static class DependencyResolver
    {
        private static IDependencyResolver g_resolver;

        [DebuggerStepThrough]
        public static void InitializeWith(IDependencyResolver resolver)
        {
            if (resolver == null)
            {
                throw new ArgumentNullException("resolver");
            }

            if (g_resolver != null)
            {
                throw new InvalidOperationException(Properties.Resources.ErrorDependencyResolverInitializedAlready);
            }

            g_resolver = resolver;
        }

        [DebuggerStepThrough]
        public static T Resolve<T>()
        {
            return g_resolver.Resolve<T>();
        }

        [DebuggerStepThrough]
        public static T Resolve<T>(string name)
        {
            return g_resolver.Resolve<T>(name);
        }

        [DebuggerStepThrough]
        public static IEnumerable<T> ResolveAll<T>()
        {
            return g_resolver.ResolveAll<T>();
        }

        [DebuggerStepThrough]
        public static T Resolve<T>(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return g_resolver.Resolve<T>(type);
        }

        [DebuggerStepThrough]
        public static void Reset()
        {
            if (g_resolver != null)
            {
                g_resolver.Dispose();
                g_resolver = null;
            }
        }
    }
}
