using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Abstractions.Tracing
{
    internal static class ErrorFormatter
    {
        public static StringBuilder Append(StringBuilder buffer, Exception ex)
        {
            foreach (DictionaryEntry entry in ex.Data)
            {
                var key = entry.Key.ToString();
                var collection = entry.Value as NameValueCollection;
                if (collection != null)
                {
                    buffer.AppendLine(key);
                    buffer.AppendLine(StringHelper.Repeat("=", key.Length));
                    Append(buffer, collection);
                    buffer.AppendLine();
                }
                else
                {
                    AppendKeyValue(buffer, key, entry.Value);
                }
            }

            buffer.AppendLine();
            buffer.Append(ex.ToString());
            return buffer;
        }

        public static void Append(StringBuilder buffer, NameValueCollection collection)
        {
            foreach (var key in collection.AllKeys)
            {
                AppendKeyValue(buffer, key, collection[key]);
            }
        }

        private static void AppendKeyValue(StringBuilder buffer, string key, object value)
        {
            buffer.AppendLine(String.Format(CultureInfo.InvariantCulture,
                    "{0}: {1}", key, value));
        }
    }
}
