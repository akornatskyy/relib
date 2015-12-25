using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace ReusableLibrary.Abstractions.Helpers
{
    public static class ObjectHelper
    {
        [DebuggerStepThrough]
        public static NameValueCollection PropertiesToNameValueCollection(object model)
        {
            var result = new NameValueCollection();
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(model))
            {
                var value = Convert.ToString(descriptor.GetValue(model), CultureInfo.InvariantCulture);
                result.Add(descriptor.Name, value);
            }

            return result;
        }
    }
}
