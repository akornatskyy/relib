using System.Collections.Generic;
using System;

namespace ReusableLibrary.QualityAssurance.Profiling
{
    public interface IProfiler
    {
        string Name { get; }

        TimeSpan Elapsed { get; }

        IEnumerable<ProfileResult<TPayload>> Go<TPayload>(ProfileRequest<TPayload> request);
    }
}
