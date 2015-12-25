using System;
using System.Collections.Generic;
using System.Text;

namespace ReusableLibrary.Abstractions.Repository
{
    public interface IMementoRepository
    {
        T Retrieve<T>(string id) where T : new();

        bool Store<T>(string id, T value) where T : new();
    }
}
