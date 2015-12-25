using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Collections.Generic;

namespace ReusableLibrary.Abstractions.Helpers
{
    public static class NameValueCollectionHelper
    {
        public static bool HasKey(NameValueCollection collection, string key)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            foreach (string k in collection.AllKeys)
            {
                if (String.Equals(k, key, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public static int ConvertToInt32(NameValueCollection collection, string paramName)
        {
            var value = collection[paramName];
            if (String.IsNullOrEmpty(value))
            {
                throw RequiredValue(paramName);
            }

            return ConvertToInt32(paramName, value, -1);
        }

        public static int ConvertToInt32(NameValueCollection collection, string paramName, int defaultValue)
        {
            return ConvertToInt32(paramName, collection[paramName], defaultValue);
        }

        public static bool ConvertToBoolean(NameValueCollection collection, string paramName)
        {
            var value = collection[paramName];
            if (String.IsNullOrEmpty(value))
            {
                throw RequiredValue(paramName);
            }

            return ConvertToBoolean(paramName, value, false);
        }

        public static bool ConvertToBoolean(NameValueCollection collection, string paramName, bool defaultValue)
        {
            return ConvertToBoolean(paramName, collection[paramName], defaultValue);
        }

        public static Type ConvertToType(NameValueCollection collection, string paramName)
        {
            var value = collection[paramName];
            if (String.IsNullOrEmpty(value))
            {
                throw RequiredValue(paramName);
            }

            return ConvertToType(paramName, value, null);
        }

        public static Type ConvertToType(NameValueCollection collection, string paramName, Type defaultValue)
        {
            return ConvertToType(paramName, collection[paramName], defaultValue);
        }

        public static IEnumerable<KeyValuePair<string, string>> AllPairs(NameValueCollection collection)
        {
            foreach (var key in collection.AllKeys)
            {
                var values = collection.GetValues(key);
                if (values == null)
                {
                    yield return new KeyValuePair<string, string>(key, null);
                    continue;
                }

                foreach (var value in values)
                {
                    yield return new KeyValuePair<string, string>(key, value);
                }
            }
        }

        private static int ConvertToInt32(string paramName, string value, int defaultValue)
        {
            if (String.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            int result;
            try
            {
                result = Int32.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture);
            }
            catch (FormatException fex)
            {
                throw InvalidValue(paramName, fex);
            }
            catch (OverflowException oex)
            {
                throw InvalidValue(paramName, oex);
            }

            return result;
        }

        private static bool ConvertToBoolean(string paramName, string value, bool defaultValue)
        {
            if (String.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            bool result;
            try
            {
                result = Boolean.Parse(value);
            }
            catch (FormatException fex)
            {
                throw InvalidValue(paramName, fex);
            }

            return result;
        }

        private static Type ConvertToType(string paramName, string value, Type defaultValue)
        {
            if (String.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            Type result;
            try
            {
                result = Type.GetType(value);
            }
            catch (SystemException sex)
            {
                throw InvalidValue(paramName, sex);
            }

            return result;
        }

        private static ArgumentException RequiredValue(string paramName)
        {
            return new ArgumentException(string.Format(CultureInfo.InvariantCulture,
                    Properties.Resources.NameValueCollectionHelperRequiredValue, paramName), paramName);
        }

        private static ArgumentException InvalidValue(string paramName, Exception inner)
        {
            return new ArgumentException(string.Format(CultureInfo.InvariantCulture,
                    Properties.Resources.NameValueCollectionHelperInvalidValue, paramName), paramName, inner);
        }
    }
}
