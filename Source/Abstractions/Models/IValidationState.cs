using System;

namespace ReusableLibrary.Abstractions.Models
{
    public interface IValidationState
    {
        void AddError(string message);

        void AddError(string key, string message);

        bool IsValid { get; }
    }
}
