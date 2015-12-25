using System.Web.Mvc;

namespace ReusableLibrary.Web.Mvc
{
    public static class ViewDataDictionaryExtensions
    {
        public static string AlternatePartialViewName(this ViewDataDictionary viewData)
        {
            return (string)viewData[AbstractController.AlternatePartialViewName];
        }
    }
}
