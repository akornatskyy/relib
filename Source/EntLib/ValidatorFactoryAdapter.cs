using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace ReusableLibrary.EntLib
{
    public sealed class ValidatorFactoryAdapter : IValidatorFactory
    {
        #region IValidatorFactory Members

        public Validator ProvideValidator(Type type, string ruleset)
        {
            return ValidationFactory.CreateValidator(type, ruleset);
        }

        #endregion
    }
}
