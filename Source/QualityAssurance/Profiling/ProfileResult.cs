using System;

namespace ReusableLibrary.QualityAssurance.Profiling
{
    public sealed class ProfileResult<TPayload>
    {
        public TPayload Payload { get; set; }

        public bool Succeed 
        { 
            get { return Error == null; } 
        }

        public Exception Error { get; set; }

        public TimeSpan Elapsed { get; set; }
    }
}
