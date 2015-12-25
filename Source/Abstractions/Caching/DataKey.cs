using System;
using ReusableLibrary.Abstractions.Serialization;

namespace ReusableLibrary.Abstractions.Caching
{
    public abstract class DataKey : AbstractObjectState
    {
        protected DataKey(string key)
        {
            Key = key;
        }

        public string Key { get; private set; }

        public long Version { get; set; }

        public override string ToString()
        {
            return String.Concat("DataKey:", Key);
        }
    }
}
