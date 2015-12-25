using System.ComponentModel;
using System.Diagnostics;
using System.Web.Mvc;

namespace ReusableLibrary.Web.Mvc.Integration
{
    public sealed class NullToEmptyStringModelBinder : DefaultModelBinder
    {
        [DebuggerStepThrough]
        protected override void SetProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value)
        {
            if (value == null && propertyDescriptor.PropertyType == typeof(string))
            {
                base.SetProperty(controllerContext, bindingContext, propertyDescriptor, string.Empty);
            }
            else
            {
                base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
            }
        }
    }
}
