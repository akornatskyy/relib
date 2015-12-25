using System;
using ReusableLibrary.Abstractions.Serialization.Formatters;

namespace ReusableLibrary.Abstractions.Serialization
{
    public abstract class AbstractObjectState : IObjectState
    {
        protected AbstractObjectState()
        {
        }

        #region IObjectState Members

        public bool HasValue { get; protected set; }

        public void Initialize(IObjectFormatter formatter)
        {
            Formatter = formatter;
        }

        public abstract void Load(ArraySegment<byte> segment, int flags);

        public abstract ArraySegment<byte> Save(out int flags);

        #endregion

        protected IObjectFormatter Formatter { get; private set; }
    }
}
