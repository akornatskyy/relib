using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace ReusableLibrary.EntLib.Validation
{
    public class ObjectValidatorData : Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.ObjectValidatorData
    {
        public ObjectValidatorData()
        {
        }

        public ObjectValidatorData(string name)
            : base(name)
        {
        }

        [ConfigurationProperty("keyFormat")]
        public string KeyFormat
        {
            get { return (string)base["keyFormat"]; }
            set { base["keyFormat"] = value; }
        }

        protected override Validator DoCreateValidator(Type targetType, Type ownerType, MemberValueAccessBuilder memberValueAccessBuilder, ValidatorFactory validatorFactory)
        {
            return new ObjectValidator(targetType, validatorFactory, TargetRuleset, KeyFormat);
        }
    }
}
