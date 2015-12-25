using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.EntLib.Validation
{
    [ConfigurationElementType(typeof(ObjectValidatorData))]
    public class ObjectValidator : Microsoft.Practices.EnterpriseLibrary.Validation.Validators.ObjectValidator
    {
        private delegate void ValidationStrategy(object objectToValidate, object currentTarget, string key, ValidationResults validationResults);

        private readonly ValidationStrategy m_strategy;
        private readonly Validator[] m_validators;

        public ObjectValidator(Type targetType, ValidatorFactory validatorFactory, string targetRuleset, string keyFormat)
            : base(targetType, targetRuleset)
        {
            var itypes = targetType.GetInterfaces();
            m_validators = new Validator[itypes.Length];
            if (m_validators.Length > 0)
            {
                EnumerableHelper.ForEach(itypes,
                    (i, type) => m_validators[i] = validatorFactory.CreateValidator(type, targetRuleset));
                m_strategy = ValidateInterfacesStrategy;
            }
            else
            {
                m_strategy = base.DoValidate;
            }

            if (!String.IsNullOrEmpty(keyFormat))
            {
                KeyFormat = keyFormat;
                m_strategy = FormatKeyStrategy;
            }
        }

        public override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            if (objectToValidate == null)
            {
                return;
            }

            m_strategy(objectToValidate, currentTarget, key, validationResults);
        }

        protected string KeyFormat { get; set; }

        private void ValidateInterfacesStrategy(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {            
            foreach (var validator in m_validators)
            {
                validator.Validate(objectToValidate, validationResults);
            }

            base.DoValidate(objectToValidate, currentTarget, key, validationResults);
        }

        private void FormatKeyStrategy(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            var nestedValidationResults = new ValidationResults();
            ValidateInterfacesStrategy(objectToValidate, currentTarget, key, nestedValidationResults);
            FormatValidationResults(nestedValidationResults, validationResults);
        }

        private IEnumerable<ValidationResult> FormatValidationResults(IEnumerable<ValidationResult> source, ValidationResults destination)
        {
            var hasResults = false;
            foreach (var result in source)
            {
                hasResults = true;
                destination.AddResult(new ValidationResult(
                    result.Message,
                    result.Target,
                    String.Format(CultureInfo.InvariantCulture, KeyFormat, result.Key),
                    result.Tag,
                    result.Validator,
                    FormatValidationResults(result.NestedValidationResults, new ValidationResults())));
            }

            return hasResults ? destination : source;
        }
    }
}
