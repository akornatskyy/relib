using System;
using System.Runtime.Serialization;

namespace ReusableLibrary.Web.Integration
{
    [Serializable]
    public class IpPolicyException : Exception
    {
        public IpPolicyException()
            : base()
        {
        }

        public IpPolicyException(string message)
            : base(message)
        {
        }

        public IpPolicyException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected IpPolicyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
