using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Supplemental.System
{
    public static class StringExtensions
    {
        public static string NullSafe(this string target)
        {
            return StringHelper.NullSafe(target);
        }

        public static string NullSafe(this string target, string empty)
        {
            return StringHelper.NullSafe(target, empty);
        }

        public static string NullEmpty(this string target)
        {
            return StringHelper.NullEmpty(target);
        }

        public static string NullEmpty(this string target, string empty)
        {
            return StringHelper.NullEmpty(target, empty);
        }

        public static string WrapAt(this string target, int length)
        {
            return StringHelper.WrapAt(target, length);
        }

        public static string WrapAt(this string target, int length, int dotCount)
        {
            return StringHelper.WrapAt(target, length, dotCount);
        }

        public static bool AllOf(this string target, char[] chars)
        {
            return StringHelper.AllOf(target, chars);
        }

        public static bool AllOf(this string target, char[] chars, int startIndex)
        {
            return StringHelper.AllOf(target, chars, startIndex);
        }

        public static bool AllOf(this string target, char[] chars, int startIndex, int count)
        {
            return StringHelper.AllOf(target, chars, startIndex, count);
        }

        public static Guid ToGuid(this string target)
        {
            return StringHelper.ToGuid(target);
        }

        public static string StripHtml(this string target)
        {
            return StringHelper.StripHtml(target);
        }

        public static string StripNewLine(this string target)
        {
            return StringHelper.StripNewLine(target);
        }

        public static string StripWhitespace(this string target)
        {
            return StringHelper.StripWhitespace(target);
        }

        public static string NewLineToBr(this string target)
        {
            return StringHelper.NewLineToBr(target);
        }

        public static bool Is(this string str, string compareTo, StringComparison comparisonType)
        {
            return String.Compare(str, compareTo, comparisonType) == 0;
        }

        public static string Capitalize(this string value)
        {
            return StringHelper.Capitalize(value);
        }

        public static string FormatWith(this string str, params object[] args)
        {
            return String.Format(CultureInfo.CurrentCulture, str, args);
        }

        public static bool Contains(this string str, string value, StringComparison comparisonType)
        {
            return StringHelper.Contains(str, value, comparisonType);
        }

        public static string Left(this string target, int length)
        {
            return StringHelper.Left(target, length);
        }

        public static string Right(this string target, int length)
        {
            return StringHelper.Right(target, length);
        }

        public static string Repeat(this string target, int count)
        {
            return StringHelper.Repeat(target, count);
        }

        public static string Reverse(this string target)
        {
            return StringHelper.Reverse(target);
        }

        public static string Join(this string separator, IEnumerable<string> value)
        {
            return StringHelper.Join(separator, value);
        }

        public static NameValueCollection ParseOptions(this string options)
        {
            return StringHelper.ParseOptions(options);
        }

        public static byte[] Hash(this string str, string hashName)
        {
            return StringHelper.Hash(str, hashName);
        }

        public static byte[] Hash(this string str, string hashName, Encoding encoding)
        {
            return StringHelper.Hash(str, hashName, encoding);
        }
    }
}
