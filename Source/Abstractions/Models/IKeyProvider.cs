using System;

namespace ReusableLibrary.Abstractions.Models
{
    public interface IKeyProvider
    {
        string ToKeyString();
    }
}
