using System;
using System.Text.RegularExpressions;
using ReusableLibrary.Abstractions.Bootstrapper;

namespace ReusableLibrary.Web.Integration
{
    public sealed class IpPolicyModuleSettings : IStartupTask
    {
        public bool SecureOnly { get; set; }

        public string NoExceptionPathRegex { get; set; }

        public string Allowed { get; set; }

        public string Denied { get; set; }

        #region IStartupTask Members

        public void Execute()
        {
            IpPolicyModule.SecureOnly = SecureOnly;

            if (!string.IsNullOrEmpty(NoExceptionPathRegex))
            {
                IpPolicyModule.NoExceptionPathRegex = new Regex(NoExceptionPathRegex,
                    RegexOptions.IgnoreCase | RegexOptions.Compiled);
            }

            if (Allowed != null && Allowed.Length > 0)
            {
                IpPolicyModule.Allowed(Allowed.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries));
            }

            if (Denied != null && Denied.Length > 0)
            {
                IpPolicyModule.Denied(Denied.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries));
            }
        }

        #endregion
    }
}
