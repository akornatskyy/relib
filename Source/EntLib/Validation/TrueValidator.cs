using System.Collections.Specialized;
using System.Diagnostics;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;

namespace ReusableLibrary.EntLib.Validation
{
    [ConfigurationElementType(typeof(CustomValidatorData))]
    public sealed class TrueValidator : Validator
    {
        public TrueValidator(NameValueCollection attributes)
            : base(null, null)
        {
            Trace.Assert(attributes != null);
        }

        public override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            if (objectToValidate is bool && !(bool)objectToValidate)
            {
                LogValidationResult(validationResults, MessageTemplate, currentTarget, key);
            }
        }

        protected override string DefaultMessageTemplate
        {
            get { return Properties.Resources.NotTrue; }
        }
    }
}
