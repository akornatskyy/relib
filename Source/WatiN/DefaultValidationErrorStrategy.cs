using System;
using WatiN.Core;

namespace ReusableLibrary.WatiN
{
    public static class DefaultValidationErrorStrategy
    {
        private static Func<Span, string>[] g_all = new Func<Span, string>[]
        {
            ClassName,
            PreviousSpanWrap,
            PreviousSiblingId
        };

        public static Func<Span, string>[] All
        {
            get
            {
                Func<Span, string>[] copy = new Func<Span, string>[g_all.Length];
                Array.Copy(g_all, copy, g_all.Length);
                return copy;
            }
        }

        public static string Composite(Span span)
        {
            return Composite(span, null);
        }

        public static string Composite(Span span, Func<Span, string>[] strategies)
        {
            strategies = strategies ?? g_all;
            foreach (var strategy in strategies)
            {
                var id = strategy(span);
                if (id != null)
                {
                    return id;
                }
            }

            return null;
        }

        public static string ClassName(Span span)
        {
            var className = span.ClassName;
            var classes = className.Split(' ');
            if (classes.Length > 1)
            {
                return classes[1];
            }

            return null;
        }

        public static string PreviousSpanWrap(Span span)
        {
            var previousSpan = span.PreviousSibling as Span;
            if (previousSpan == null)
            {
                return null;
            }

            var element = previousSpan.Element(Find.By("id", id => !String.IsNullOrEmpty(id)));
            if (!element.Exists)
            {
                return null;
            }

            return element.Id;
        }

        public static string PreviousSiblingId(Span span)
        {
            var element = span.PreviousSibling;
            if (!element.Exists)
            {
                return null;
            }

            return element.Id;
        }
    }
}
