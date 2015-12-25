using System;

namespace ReusableLibrary.Abstractions.Models
{
    public interface IEncoder
    {
        byte[] GetBytes(string s);
    }
}
