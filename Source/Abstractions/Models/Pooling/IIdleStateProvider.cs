using System;

namespace ReusableLibrary.Abstractions.Models
{
    public interface IIdleStateProvider
    {
        IdleState IdleState { get; }
    }
}
