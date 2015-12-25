using System;

namespace ReusableLibrary.Abstractions.Serialization.Formatters
{
    public abstract class DecoratedObjectFormatter : IObjectFormatter
    {
        public const int TypeCodeMask = 0x1F;

        protected DecoratedObjectFormatter(IObjectFormatter inner)
        {
            Inner = inner;
        }

        protected IObjectFormatter Inner { get; private set; }

        #region IObjectFormatter Members

        public virtual ArraySegment<byte> Serialize<T>(T value, out int flags)
        {
            return Inner.Serialize<T>(value, out flags);
        }

        public virtual T Deserialize<T>(ArraySegment<byte> data, int flags)
        {
            return Inner.Deserialize<T>(data, flags);
        }

        #endregion
    }
}
