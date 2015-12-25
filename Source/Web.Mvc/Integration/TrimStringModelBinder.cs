using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Mvc;

namespace ReusableLibrary.Web.Mvc.Integration
{
    public sealed class TrimStringModelBinder : DefaultModelBinder
    {
        [DebuggerStepThrough]
        protected override void SetProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value)
        {
            if (propertyDescriptor.PropertyType != typeof(string))
            {
                base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
                return;
            }

            var str = value as string;
            if (String.IsNullOrEmpty(str))
            {
                base.SetProperty(controllerContext, bindingContext, propertyDescriptor, string.Empty);
            }
            else
            {
                base.SetProperty(controllerContext, bindingContext, propertyDescriptor, str.Trim());
            }
        }
    }
}
