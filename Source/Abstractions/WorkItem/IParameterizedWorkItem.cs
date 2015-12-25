using System;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.WorkItem
{
    public interface IParameterizedWorkItem<TStateBag>
    {
        TStateBag StateBag { get; }

        bool DoWork(TStateBag state);

        bool DoWork(TStateBag state, Func2<bool> shutdownCallback);
    }
}
