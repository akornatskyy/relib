using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace ReusableLibrary.EntLib
{
    public interface IValidatorFactory
    {
        Validator ProvideValidator(Type type, string ruleset);
    }
}
