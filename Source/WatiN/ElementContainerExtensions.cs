using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using ReusableLibrary.Supplemental.Collections;
using ReusableLibrary.Supplemental.System;
using WatiN.Core;
using WatiN.Core.Constraints;

namespace ReusableLibrary.WatiN
{
    public static class ElementContainerExtensions
    {
        public static IElementContainer UpdateModel<T>(this IElementContainer container, T model, Regex regex)
        {
            var element = container as Element;
            var properties = TypeDescriptor.GetProperties(model);
            foreach (Match match in regex.Matches(element.InnerHtml))
            {
                var name = match.Groups["name"].Value;
                foreach (PropertyDescriptor descriptor in properties)
                {
                    if (String.Equals(descriptor.Name, name, StringComparison.OrdinalIgnoreCase))
                    {
                        var value = match.Groups["value"].Value;
                        if (!descriptor.IsReadOnly)
                        {
                            descriptor.SetValue(model, descriptor.Converter.ConvertFromInvariantString(value));
                        }

                        break;
                    }
                }
            }

            return container;
        }

        public static IElementContainer Fill(this IElementContainer container, NameValueCollection properties)
        {
            return Fill(container, properties, null);
        }

        public static IElementContainer Fill(this IElementContainer container, NameValueCollection properties, string keyPrefix)
        {
            keyPrefix = keyPrefix ?? string.Empty;
            container.TextFields.Where(textField => 
                textField.Id != null 
                && textField.Id.StartsWith(keyPrefix, StringComparison.OrdinalIgnoreCase)
                && !"hidden".Equals(textField.GetAttributeValue("type"))).ForEach(textField =>
            {
                var key = textField.Id.Substring(keyPrefix.Length);
                if (properties.HasKey(key))
                {
                    var value = properties[key];
                    if (value != null)
                    {
                        textField.Value = value;
                    }
                }
            });
            container.SelectLists.Where(selectList => selectList.Id != null && selectList.Id.StartsWith(keyPrefix, StringComparison.OrdinalIgnoreCase)).ForEach(selectList =>
            {
                var key = selectList.Id.Substring(keyPrefix.Length);
                if (properties.HasKey(key))
                {
                    var value = properties[key];
                    if (value != null && selectList.Option(o => String.Equals(o.Value, value, StringComparison.OrdinalIgnoreCase)).Exists)
                    {
                        selectList.SelectByValue(value);
                    }
                }
            });
            container.CheckBoxes.Where(checkBox => checkBox.Id != null && checkBox.Id.StartsWith(keyPrefix, StringComparison.OrdinalIgnoreCase)).ForEach(checkBox =>
            {
                var key = checkBox.Id.Substring(keyPrefix.Length);
                if (properties.HasKey(key))
                {
                    var value = properties[key];
                    if (checkBox.Checked.ToString() != value)
                    {
                        checkBox.Click();
                    }
                }
            });
            return container;
        }

        public static IElementContainer Fill(this IElementContainer container, object model)
        {
            return Fill(container, model, null);
        }

        public static IElementContainer Fill(this IElementContainer container, object model, string keyPrefix)
        {
            return Fill(container, model.PropertiesToNameValueCollection(), keyPrefix);
        }

        public static IElementContainer FillTextField(this IElementContainer container, Constraint findBy, string value)
        {
            var element = container.TextField(findBy);
            if (element.Exists)
            {
                element.Value = value ?? string.Empty;
            }

            return container;
        }

        public static IElementContainer FillCheckbox(this IElementContainer container, Constraint findBy, bool value)
        {
            var element = container.CheckBox(findBy);
            if (element.Exists && element.Checked != value)
            {
                element.Click();
            }

            return container;
        }

        public static IElementContainer FillSelectList(this IElementContainer container, Constraint findBy, string value)
        {
            var element = container.SelectList(findBy);
            if (element.Exists)
            {
                if (value != null && element.Option(o => String.Equals(o.Value, value, StringComparison.OrdinalIgnoreCase)).Exists)
                {
                    element.SelectByValue(value);
                }
            }

            return container;
        }

        public static IElementContainer Submit(this IElementContainer container)
        {
            return Submit(container, Find.Any);
        }

        public static IElementContainer Submit(this IElementContainer container, Constraint findBy)
        {
            var element = container.Button(findBy);
            if (element.EnsureClick())
            {
                element.TryWaitUntilEnabled();
            }

            return container;
        }

        public static IElementContainer SubmitForm(this IElementContainer container)
        {
            return SubmitForm(container, Find.Any);
        }

        public static IElementContainer SubmitForm(this IElementContainer container, Constraint findBy)
        {
            var element = container.Form(findBy);
            if (element.Exists)
            {
                element.Submit();
            }

            return container;
        }

        public static IElementContainer OnInfoMessage(this IElementContainer container, Action<string> action)
        {
            return OnMessage(container, Find.ByClass(Constants.CssClassNames.InfoMessage), action);
        }

        public static IElementContainer OnWarningMessage(this IElementContainer container, Action<string> action)
        {
            return OnMessage(container, Find.ByClass(Constants.CssClassNames.WarningMessage), action);
        }

        public static IElementContainer OnErrorMessage(this IElementContainer container, Action<string> action)
        {
            return OnMessage(container, Find.ByClass(Constants.CssClassNames.ErrorMessage), action);
        }

        public static IElementContainer OnMessage(this IElementContainer container, Constraint findBy, Action<string> action)
        {
            var element = container.Element(findBy);
            if (element.Exists)
            {
                action(element.Text);
            }

            return container;
        }

        public static NameValueCollection ValidationErrors(this IElementContainer container)
        {
            return ValidationErrors(container, DefaultValidationErrorStrategy.Composite);
        }

        public static NameValueCollection ValidationErrors(this IElementContainer container, Func<Span, string> relatedFieldSearchStrategy)
        {
            return container.Spans.Filter(Find.By("className", 
                className => className.Contains(Constants.CssClassNames.FieldValidationError, StringComparison.OrdinalIgnoreCase))).ToNameValueCollection(
                span => new KeyValuePair<string, string>(relatedFieldSearchStrategy(span), span.Text));
        }
    }
}
