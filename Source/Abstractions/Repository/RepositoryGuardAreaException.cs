using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace ReusableLibrary.Abstractions.Repository
{
    [Serializable]
    public class RepositoryGuardAreaException : RepositoryGuardException
    {
        public RepositoryGuardAreaException(string area, int number)
            : this(area, number, (Exception)null)
        {
        }

        public RepositoryGuardAreaException(string area, int number, string message)
            : this(area, number, message, null)
        {
        }

        public RepositoryGuardAreaException(string area, int number, Exception inner)
            : this(area, number, 
            string.Format(CultureInfo.CurrentCulture, Properties.Resources.ErrorRepositoryGuardAreaFailed, area, number), 
            inner)
        {
        }

        public RepositoryGuardAreaException(string area, int number, string message, Exception inner)
            : base(number, message, inner)
        {
            Area = area;
        }

        public RepositoryGuardAreaException()
        {
        }

        public RepositoryGuardAreaException(string message)
            : base(message)
        {
        }

        public RepositoryGuardAreaException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected RepositoryGuardAreaException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public string Area { get; private set; }
    }
}
