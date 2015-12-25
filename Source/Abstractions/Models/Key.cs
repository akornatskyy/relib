using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Abstractions.Models
{
    public static class Key
    {
        public static string From<TModel>(TModel model)
        {
            return FromModel(model, typeof(TModel));
        }

        public static string From<TModel>(params object[] models)
        {
            if (models.Length == 0)
            {
                return TypeHelper.GetName<TModel>();
            }

            return new StringBuilder(0x40)
                .Append(TypeHelper.GetName<TModel>())
                .Append(From(models))
                .ToString();
        }

        public static string From(Delegate @delegate, params object[] models)
        {
            if (@delegate == null)
            {
                throw new ArgumentNullException("delegate");
            }

            if (models.Length == 0)
            {
                return TypeHelper.GetName(@delegate);
            }

            return new StringBuilder(0x50)
                .Append(TypeHelper.GetName(@delegate))
                .Append(From(models))
                .ToString();
        }

        private static string From(params object[] models)
        {
            if (models.Length == 0)
            {
                return string.Empty;
            }

            var cacheKey = new StringBuilder(0x30);
            foreach (var model in models)
            {
                if (model == null)
                {
                    cacheKey.Append(@" null");
                    continue;
                }

                cacheKey.Append(" ");
                var p = model as IKeyProvider;
                if (p != null)
                {
                    cacheKey.Append(p.ToKeyString());
                    continue;
                }

                cacheKey.Append(FromModel(model, model.GetType()));
            }

            return cacheKey.ToString();
        }

        private static string FromModel(object model, Type modelType)
        {
            if (modelType == null)
            {
                throw new ArgumentNullException("modelType");
            }

            if (model == null)
            {
                return string.Concat(modelType.Name, "=null");
            }

            if (modelType == typeof(string))
            {
                return Format("String", (string)model);
            }

            var p = model as IKeyProvider;
            if (p != null)
            {
                return p.ToKeyString();
            }

            var f = model as IFormattable;
            if (f != null)
            {
                return Format(modelType.Name, f.ToString());
            }

            var cacheKey = new StringBuilder(0x40);
            cacheKey.Append(TypeHelper.GetName(modelType));
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(model))
            {
                var value = descriptor.GetValue(model);

                string str;
                p = value as IKeyProvider;
                if (p != null)
                {
                    str = p.ToKeyString();
                }
                else
                {
                    str = Convert.ToString(value, CultureInfo.InvariantCulture);
                }

                cacheKey.Append(" ");
                cacheKey.Append(Format(descriptor.Name, str));
            }

            return cacheKey.ToString();
        }

        private static string Format(string name, string value)
        {
            return string.Concat(name, "='", value.ToUpper(CultureInfo.InvariantCulture), "'");
        }
    }
}
