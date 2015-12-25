using System.Web.Mvc;

namespace ReusableLibrary.Web.Mvc
{
    public static class ModelStateDictionaryExtensions
    {
        public static void ResetModelValue(this ModelStateDictionary modelStateDictionary, string key)
        {
            if (modelStateDictionary.ContainsKey(key))
            {
                modelStateDictionary.SetModelValue(key, new ValueProviderResult(null, null, null));
            }
        }

        public static string ModelErrorMessage(this ModelStateDictionary modelStateDictionary)
        {
            if (!modelStateDictionary.ContainsKey("__ERROR__"))
            {
                return null;
            }

            ModelState modelState = modelStateDictionary["__ERROR__"];
            ModelErrorCollection errors = (modelState == null) ? null : modelState.Errors;
            ModelError error = ((errors == null) || (errors.Count == 0)) ? null : errors[0];
            if (error == null)
            {
                return null;
            }

            return error.ErrorMessage;
        }
    }
}
