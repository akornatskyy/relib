using System;
using System.Collections.Generic;
using System.Text;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Abstractions.Cryptography
{
    public class SymmetricAlgorithmOptions
    {
        public SymmetricAlgorithmOptions(string options)
        {
            var items = StringHelper.ParseOptions(options);
            Name = items["name"] ?? "Default";
            MaxPoolSize = NameValueCollectionHelper.ConvertToInt32(items, "max pool size", 16);
            PoolAccessTimeout = NameValueCollectionHelper.ConvertToInt32(items, "pool access timeout", 100);
            PoolWaitTimeout = NameValueCollectionHelper.ConvertToInt32(items, "pool wait timeout", 250);
        }

        public string Name { get; private set; }

        public int MaxPoolSize { get; private set; }

        public int PoolAccessTimeout { get; private set; }

        public int PoolWaitTimeout { get; private set; }
    }
}
