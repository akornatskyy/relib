using System;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.WorkItem
{
    public interface IWorkItem
    {
        bool DoWork();

        bool DoWork(Func2<bool> shutdownCallback);
    }
}
