using System;
using System.Runtime.Serialization;

namespace ReusableLibrary.Abstractions.Repository
{
    [Serializable]
    public abstract class RepositoryException : Exception
    {
        protected RepositoryException()
            : base()
        {
        }

        protected RepositoryException(string message)
            : base(message)
        {
        }

        protected RepositoryException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected RepositoryException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
