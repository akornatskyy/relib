using System;

namespace ReusableLibrary.Abstractions.Models
{
    public interface IDelegateInvokeStrategy
    {
        object Invoke(Delegate @delegate, params object[] args);
    }
}
