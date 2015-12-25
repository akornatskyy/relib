using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Supplemental.Collections
{
    public static class NameValueCollectionExtensions
    {
        public static bool HasKey(this NameValueCollection collection, string key)
        {
            return NameValueCollectionHelper.HasKey(collection, key);
        }

        public static int ConvertToInt32(this NameValueCollection collection, string paramName)
        {
            return NameValueCollectionHelper.ConvertToInt32(collection, paramName);
        }

        public static int ConvertToInt32(this NameValueCollection collection, string paramName, int defaultValue)
        {
            return NameValueCollectionHelper.ConvertToInt32(collection, paramName, defaultValue);
        }

        public static bool ConvertToBoolean(this NameValueCollection collection, string paramName)
        {
            return NameValueCollectionHelper.ConvertToBoolean(collection, paramName);
        }

        public static bool ConvertToBoolean(this NameValueCollection collection, string paramName, bool defaultValue)
        {
            return NameValueCollectionHelper.ConvertToBoolean(collection, paramName, defaultValue);
        }

        public static Type ConvertToType(this NameValueCollection collection, string paramName)
        {
            return NameValueCollectionHelper.ConvertToType(collection, paramName);
        }

        public static Type ConvertToType(this NameValueCollection collection, string paramName, Type defaultValue)
        {
            return NameValueCollectionHelper.ConvertToType(collection, paramName, defaultValue);
        }

        public static IEnumerable<KeyValuePair<string, string>> AllPairs(this NameValueCollection collection)
        {
            return NameValueCollectionHelper.AllPairs(collection);
        }
    }
}
