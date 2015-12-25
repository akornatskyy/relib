using System;
using System.Collections.Generic;

namespace ReusableLibrary.Abstractions.Models
{
    public interface IPagedList<T> : IEnumerable<T>
    {
        IPagedListState State { get; }
    }
}