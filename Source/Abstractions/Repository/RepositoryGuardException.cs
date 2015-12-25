using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace ReusableLibrary.Abstractions.Repository
{
    [Serializable]
    public class RepositoryGuardException : RepositoryException
    {
        public RepositoryGuardException(int number)
            : this(number, null, (Exception)null)
        {
        }

        public RepositoryGuardException(int number, string message)
            : this(number, message, null)
        {
        }

        public RepositoryGuardException(int number, Exception inner)
            : this(number, string.Format(CultureInfo.CurrentCulture, Properties.Resources.ErrorRepositoryGuardFailed, number), inner)
        {
        }

        public RepositoryGuardException(int number, string message, Exception inner)
            : base(message, inner)
        {
            Number = number;
        }

        public RepositoryGuardException()
        {
        }

        public RepositoryGuardException(string message)
            : base(message)
        {
        }

        public RepositoryGuardException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected RepositoryGuardException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public int Number { get; private set; }
    }
}
