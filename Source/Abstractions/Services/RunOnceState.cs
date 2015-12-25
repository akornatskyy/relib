using System;

namespace ReusableLibrary.Abstractions.Services
{
    public enum RunOnceState
    {
        None = 0,

        Wait,

        Error,

        Done
    }
}
