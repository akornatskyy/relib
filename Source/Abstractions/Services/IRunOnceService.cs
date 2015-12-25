using System;

namespace ReusableLibrary.Abstractions.Services
{
    public interface IRunOnceService
    {
        bool Begin(string key);

        void Error(string message);

        void Result<T>(T value);

        RunOnceResult<T> End<T>();

        RunOnceResult<T> BeginOrEnd<T>(string key);
    }
}
