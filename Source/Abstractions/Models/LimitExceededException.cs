using System;
using System.Runtime.Serialization;

namespace ReusableLibrary.Abstractions.Models
{
    [Serializable]
    public class LimitExceededException : Exception
    {
        public LimitExceededException()
            : base()
        {
        }

        public LimitExceededException(string message)
            : base(message)
        {
        }

        public LimitExceededException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected LimitExceededException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
