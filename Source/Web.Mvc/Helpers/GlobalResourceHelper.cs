using System;
using System.Globalization;
using System.Web;
using ReusableLibrary.Web.Mvc.Integration;

namespace ReusableLibrary.Web.Mvc.Helpers
{
    internal static class GlobalResourceHelper
    {
        public static string ErrorUnspecified()
        {
            return GetString("ErrorUnspecified");
        }

        public static string ErrorHttpBadRequest()
        {
            return GetString("ErrorHttpBadRequest");
        }

        public static string ErrorHttpNotFound()
        {
            return GetString("ErrorHttpNotFound");
        }

        public static string ErrorHttpAccessDenied()
        {
            return GetString("ErrorHttpAccessDenied");
        }

        public static string ErrorNoRecordsFound()
        {
            return GetString("ErrorNoRecordsFound");
        }

        public static string NextPage()
        {
            return GetString("NextPage");
        }

        public static string NextPageTitle()
        {
            return GetString("NextPageTitle");
        }

        public static string NextNPages()
        {
            return GetString("NextNPages");
        }

        public static string NextNPagesTitle()
        {
            return GetString("NextNPagesTitle");
        }

        public static string PreviousNPages()
        {
            return GetString("PreviousNPages");
        }

        public static string PreviousNPagesTitle()
        {
            return GetString("PreviousNPagesTitle");
        }

        public static string PreviousPage()
        {
            return GetString("PreviousPage");
        }

        public static string PreviousPageTitle()
        {
            return GetString("PreviousPageTitle");
        }

        private static string GetString(string resourceKey)
        {
            string resourceValue = null;
            if (!String.IsNullOrEmpty(GlobalResource.ResourceClassKey))
            {
                resourceValue = HttpContext.GetGlobalResourceObject(GlobalResource.ResourceClassKey, resourceKey, CultureInfo.CurrentUICulture) as string;
            }

            return resourceValue ?? Properties.Resources.ResourceManager.GetString(resourceKey, CultureInfo.CurrentUICulture);
        }
    }
}
