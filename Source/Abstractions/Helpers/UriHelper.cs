using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ReusableLibrary.Abstractions.Helpers
{
    public static class UriHelper
    {
        private const char QuerySplitter = '&';
        private const char KeyValueSeparator = '=';
        private static readonly char[] QuerySplitters = new char[] { QuerySplitter };        

        public static string ToQuery(NameValueCollection items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            var pairs = new List<string>(items.Count);
            foreach (var pair in NameValueCollectionHelper.AllPairs(items))
            {
                pairs.Add(String.Concat(pair.Key, KeyValueSeparator, pair.Value));
            }

            return String.Join(new string(QuerySplitter, 1), pairs.ToArray());
        }

        public static NameValueCollection ParseQuery(string query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            if (query.StartsWith("?", StringComparison.Ordinal))
            {
                query = query.Substring(1);
            }

            var items = new NameValueCollection();
            foreach (var pair in query.Split(QuerySplitters, StringSplitOptions.RemoveEmptyEntries))
            {
                var kv = pair.Split(KeyValueSeparator);
                items.Add(kv[0], kv[1]);
            }

            return items;
        }
    }
}
