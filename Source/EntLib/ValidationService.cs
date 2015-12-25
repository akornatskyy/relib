using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Services;

namespace ReusableLibrary.EntLib
{
    public sealed class ValidationService : IValidationService
    {
        private readonly IValidationState m_validationState;
        private readonly IValidatorFactory m_validatorFactory;

        public ValidationService(IValidationState validationState)
            : this(validationState, null)
        {
        }

        public ValidationService(IValidationState validationState, IValidatorFactory validatorFactory)
        {
            m_validationState = validationState;
            m_validatorFactory = validatorFactory ?? ValidatorFactoryCache.Instance;
        }

        #region IValidationService Members

        public bool Validate<T>(T target)
             where T : class
        {
            return Validate<T>(target, null);
        }

        public bool Validate<T>(T target, string ruleset)
             where T : class
        {
            return Validate(typeof(T), target, ruleset);
        }

        public bool Validate(object target)
        {
            return Validate(target, null);
        }

        public bool Validate(object target, string ruleset)
        {
            if (target == null)
            {
                return false;
            }

            return Validate(target.GetType(), target, ruleset);
        }

        #endregion

        private bool Validate(Type type, object target, string ruleset)
        {
            bool result = true;
            foreach (var itype in type.GetInterfaces())
            {
                result &= ValidateType(itype, target, ruleset);
            }

            return result & ValidateType(type, target, ruleset);
        }

        private bool ValidateType(Type type, object target, string ruleset)
        {
            if (target == null)
            {
                return false;
            }

            var validator = m_validatorFactory.ProvideValidator(type, ruleset ?? "Rule Set");
            var validationResults = validator.Validate(target);
            if (!validationResults.IsValid)
            {
                if (m_validationState != null)
                {
                    foreach (var result in validationResults)
                    {
                        AddError(result);
                    }
                }

                return false;
            }

            return true;
        }

        private void AddError(ValidationResult result)
        {
            if (EnumerableHelper.Count(result.NestedValidationResults) == 0)
            {
                m_validationState.AddError(result.Key, result.Message);
                return;
            }

            EnumerableHelper.ForEach(result.NestedValidationResults, r => AddError(r));
        }
    }
}
