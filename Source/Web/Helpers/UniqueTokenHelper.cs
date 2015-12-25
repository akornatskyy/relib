using System;
using System.Collections.Specialized;
using System.Globalization;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Web.Helpers
{
    public static class UniqueTokenHelper
    {
        static UniqueTokenHelper()
        {
            UniqueTokenName = "__u";
        }

        public static string UniqueTokenName { get; set; }

        public static string UniqueToken()
        {
            return GuidHelper.Shrink(Guid.NewGuid());
        }

        public static bool TryUniqueToken(NameValueCollection form, out string uniqueToken)
        {
            var token = form[UniqueTokenHelper.UniqueTokenName];
            if (string.IsNullOrEmpty(token))
            {
                uniqueToken = null;
                return false;
            }

            uniqueToken = token;
            return true;
        }

        public static string UniqueToken(NameValueCollection form)
        {
            string uniqueToken;
            if (!TryUniqueToken(form, out uniqueToken))
            {
                throw new InvalidOperationException(string.Format(
                    CultureInfo.InvariantCulture, 
                    Properties.Resources.UniqueTokenHelperNotFound, 
                    UniqueTokenHelper.UniqueTokenName));
            }

            return uniqueToken;
        }
    }
}
