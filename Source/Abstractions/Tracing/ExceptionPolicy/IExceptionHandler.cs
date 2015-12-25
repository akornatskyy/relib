using System;

namespace ReusableLibrary.Abstractions.Tracing
{
    public interface IExceptionHandler
    {
        bool HandleException(Exception ex);
    }
}
