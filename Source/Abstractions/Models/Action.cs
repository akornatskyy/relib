using System;

namespace ReusableLibrary.Abstractions.Models
{
    public delegate void Action2();

    public delegate void Action2<TA, TB>(TA a, TB b);

    public delegate TResult Func2<TA, TB, TResult>(TA a, TB b);

    public delegate TResult Func2<T, TResult>(T arg);

    public delegate TResult Func2<TResult>();
}
