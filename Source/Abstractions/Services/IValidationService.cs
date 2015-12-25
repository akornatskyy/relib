using System;

namespace ReusableLibrary.Abstractions.Services
{
    public interface IValidationService
    {
        bool Validate<T>(T target) where T : class;

        bool Validate<T>(T target, string ruleset) where T : class;

        bool Validate(object target);

        bool Validate(object target, string ruleset);
    }
}
