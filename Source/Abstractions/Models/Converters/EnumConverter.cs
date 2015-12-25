using System;
using System.ComponentModel;
using System.Globalization;

namespace ReusableLibrary.Abstractions.Models
{
    public class EnumConverter : System.ComponentModel.EnumConverter
    {
        private readonly Type m_type;

        public EnumConverter(Type type)
            : base(type)
        {
            m_type = type;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
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
                return Enum.ToObject(m_type, (int)value);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
           CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(int))
            {
                return (int)value;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            return Enum.IsDefined(m_type, value);
        }
    }
}
