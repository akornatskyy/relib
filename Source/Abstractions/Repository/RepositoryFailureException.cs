using System;
using System.Runtime.Serialization;

namespace ReusableLibrary.Abstractions.Repository
{
    [Serializable]
    public class RepositoryFailureException : RepositoryException
    {
        public RepositoryFailureException()
        {
        }

        public RepositoryFailureException(string message)
            : base(message)
        {
        }

        public RepositoryFailureException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected RepositoryFailureException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
