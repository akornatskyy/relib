using System;

namespace ReusableLibrary.Abstractions.Services
{
    public interface IMementoService
    {
        bool Save<T>(T value) where T : new();

        T Load<T>() where T : new();
    }
}
