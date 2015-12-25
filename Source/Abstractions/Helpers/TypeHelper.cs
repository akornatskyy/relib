using System;
using System.Globalization;

namespace ReusableLibrary.Abstractions.Helpers
{
    public static class TypeHelper
    {
        public static string GetName<T>()
        {
            return GetName(typeof(T));
        }

        public static string GetName(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (!type.IsGenericType)
            {
                return type.Name;
            }

            return String.Format(CultureInfo.InvariantCulture, "{0}[{1}]", type.Name,
                StringHelper.Join(", ", EnumerableHelper.Translate(type.GetGenericArguments(), t => GetName(t))));
        }
        
        public static string GetName(Delegate @delegate)
        {
            return String.Format(CultureInfo.InvariantCulture, "{0}.{1}",
                GetName(@delegate.Method.DeclaringType), @delegate.Method.Name);
        }
    }
}
