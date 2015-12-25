using System;
using System.ComponentModel;
using System.Globalization;

namespace ReusableLibrary.Abstractions.Models
{
    public class DecimalConverter : System.ComponentModel.DecimalConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context,
           Type sourceType)
        {
            return sourceType == typeof(int) || base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(int) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
           CultureInfo culture, object value)
        {
            if (value is int)
            {
                return Convert.ToDecimal((int)value);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
           CultureInfo culture, object value, Type destinationType)
        {
            if (value is decimal && destinationType == typeof(int))
            {
                return Convert.ToInt32((decimal)value);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
