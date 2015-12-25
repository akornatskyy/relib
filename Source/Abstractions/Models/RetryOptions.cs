using System;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Abstractions.Models
{
    [Serializable]
    public class RetryOptions
    {
        public RetryOptions()
            : this(string.Empty, string.Empty)
        {
        }

        public RetryOptions(string options)
            : this(options, string.Empty)
        {
        }

        public RetryOptions(string options, string prefix)
        {
            var items = StringHelper.ParseOptions(options);
            MaxRetryCount = NameValueCollectionHelper.ConvertToInt32(items, String.Concat(prefix, " max retry count").TrimStart(), 0);
            RetryTimeout = NameValueCollectionHelper.ConvertToInt32(items, String.Concat(prefix, " retry timeout").TrimStart(), 0);
            RetryDelay = NameValueCollectionHelper.ConvertToInt32(items, String.Concat(prefix, " retry delay").TrimStart(), 0);
            RetryFails = NameValueCollectionHelper.ConvertToBoolean(items, String.Concat(prefix, " retry fails").TrimStart(), false);
        }

        public int MaxRetryCount { get; set; }

        public int RetryTimeout { get; set; }

        public int RetryDelay { get; set; }

        public bool RetryFails { get; set; }
    }
}
