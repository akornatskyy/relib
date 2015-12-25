using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ReusableLibrary.Abstractions.Helpers
{
    public static class StringHelper
    {
        public const string AlphabetLowerCase = "abcdefghijklmnopqrstuvwxyz";
        public const string AlphabetUpperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string Numeric = "0123456789";
        public const string Space = " ";
        public const string Special = @"!""#$%&'()*+,-./:;<=>?@[\]^_`{|}~";
        public const string Alphabet = AlphabetUpperCase + AlphabetLowerCase;
        public const string AlphaNumeric = Alphabet + Numeric;
        public const string PrintableASCII = AlphaNumeric + Space + Special;

        private static readonly char[] EncodedBase64Symbols = (AlphabetUpperCase + AlphabetLowerCase + Numeric + "-_").ToCharArray();
        private static readonly Regex RegexStripHtml = new Regex(@"<\S[^><]*>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private static readonly Regex RegexStripWhitespace = new Regex(@"\s+", RegexOptions.Compiled);
        private static readonly char[] OptionsSplitter = new[] { ';' };
        private static readonly char[] KeyValueSeparator = new[] { '=' };

        [DebuggerStepThrough]
        public static string NullSafe(string target)
        {
            return NullSafe(target, string.Empty);
        }

        [DebuggerStepThrough]
        public static string NullSafe(string target, string defaultValue)
        {
            if (target == null)
            {
                return defaultValue;
            }

            return target.Trim();
        }

        [DebuggerStepThrough]
        public static string NullEmpty(string target)
        {
            return NullEmpty(target, string.Empty);
        }

        [DebuggerStepThrough]
        public static string NullEmpty(string target, string empty)
        {
            if (target == null)
            {
                return null;
            }

            var result = target.Trim();
            if (result == empty)
            {
                return null;
            }

            return result;
        }

        [DebuggerStepThrough]
        public static string WrapAt(string target, int length)
        {
            return WrapAt(target, length, 3);
        }

        [DebuggerStepThrough]
        public static string WrapAt(string target, int length, int dotCount)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException("length");
            }

            if (dotCount <= 0)
            {
                throw new ArgumentOutOfRangeException("dotCount");
            }

            if (target.Length <= length)
            {
                return target;
            }

            return string.Concat(target.Substring(0, length - dotCount), new string('.', dotCount));
        }

        [DebuggerStepThrough]
        public static bool AllOf(string target, char[] chars)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            return AllOf(target, chars, 0, target.Length);
        }

        [DebuggerStepThrough]
        public static bool AllOf(string target, char[] chars, int startIndex)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            return AllOf(target, chars, startIndex, target.Length - startIndex);
        }

        [DebuggerStepThrough]
        public static bool AllOf(string target, char[] chars, int startIndex, int count)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            if (chars == null)
            {
                throw new ArgumentNullException("chars");
            }

            if (count <= 0 || startIndex + count > target.Length)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            if (startIndex >= target.Length)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }

            if (target.Length == 0)
            {
                return false;
            }

            var allOfString = new string(chars);
            for (int i = startIndex; i < startIndex + count; i++)
            {
                if (allOfString.IndexOf(target[i]) < 0)
                {
                    return false;
                }
            }

            return true;
        }

        [DebuggerStepThrough]
        public static Guid ToGuid(string target)
        {
            if (target == null || target.Length != 22 || !AllOf(target, EncodedBase64Symbols))
            {
                return Guid.Empty;
            }

            var base64String = string.Concat(target.Replace("-", "+").Replace("_", "/"), "==");
            var raw = Convert.FromBase64String(base64String);
            if (raw.Length != 0x10)
            {
                return Guid.Empty;
            }

            return new Guid(raw);
        }

        [DebuggerStepThrough]
        public static string StripHtml(string target)
        {
            return RegexStripHtml.Replace(target, string.Empty);
        }

        [DebuggerStepThrough]
        public static string StripNewLine(string target)
        {
            return target.Replace("\r", string.Empty).Replace("\n", string.Empty);
        }

        [DebuggerStepThrough]
        public static string StripWhitespace(string target)
        {
            return RegexStripWhitespace.Replace(target, Space).Trim();
        }

        [DebuggerStepThrough]
        public static string NewLineToBr(string target)
        {
            return String.Join("<br />", target.Split(new[] { "\r\n", "\n\r", "\n", "\r" }, StringSplitOptions.None));
        }

        [DebuggerStepThrough]
        public static string Capitalize(string target)
        {
            if (String.IsNullOrEmpty(target))
            {
                return string.Empty;
            }

            if (target.Length == 1)
            {
                return target.ToUpper(CultureInfo.InvariantCulture);
            }

            return target.Substring(0, 1).ToUpper(CultureInfo.InvariantCulture) + target.Substring(1);
        }

        [DebuggerStepThrough]
        public static bool Contains(string target, string value, StringComparison comparisonType)
        {
            return target != null && value != null && target.IndexOf(value, comparisonType) >= 0;
        }

        [DebuggerStepThrough]
        public static string Left(string target, int length)
        {
            if (String.IsNullOrEmpty(target))
            {
                return target;
            }

            return target.Substring(0, length > target.Length ? target.Length : length);
        }

        [DebuggerStepThrough]
        public static string Right(string target, int length)
        {
            if (String.IsNullOrEmpty(target))
            {
                return target;
            }

            return target.Substring(length > target.Length ? 0 : target.Length - length);
        }

        [DebuggerStepThrough]
        public static string Repeat(string target, int count)
        {
            if (String.IsNullOrEmpty(target) || count <= 1)
            {
                return target;
            }

            var buffer = new StringBuilder(target.Length * count);
            for (int i = 0; i < count; i++)
            {
                buffer.Append(target);
            }

            return buffer.ToString();
        }

        [DebuggerStepThrough]
        public static string Reverse(string target)
        {
            if (String.IsNullOrEmpty(target))
            {
                return target;
            }

            char[] arr = target.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        [DebuggerStepThrough]
        public static string Join(string separator, IEnumerable<string> value)
        {
            var list = value as List<string>;
            if (list == null)
            {
                list = new List<string>(value);
            }

            return String.Join(separator, list.ToArray());
        }

        [DebuggerStepThrough]
        public static NameValueCollection ParseOptions(string options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            var items = new NameValueCollection();
            foreach (var pair in options.Split(OptionsSplitter, StringSplitOptions.RemoveEmptyEntries))
            {
                var kv = pair.Split(KeyValueSeparator, 3, StringSplitOptions.RemoveEmptyEntries);
                if (kv.Length != 2)
                {
                    throw new ArgumentException(Properties.Resources.StringOptionsInvalidOption);
                }

                items[kv[0].Trim().ToLowerInvariant()] = kv[1].Trim();
            }

            return items;
        }

        [DebuggerStepThrough]
        public static byte[] Hash(string target, string hashName)
        {
            return Hash(target, hashName, null);
        }

        [DebuggerStepThrough]
        public static byte[] Hash(string target, string hashName, Encoding encoding)
        {
            var algorithm = HashAlgorithm.Create(hashName);
            if (algorithm == null)
            {
                throw new ArgumentOutOfRangeException("hashName");
            }

            using (algorithm)
            {
                return Hash(target, algorithm, encoding ?? Encoding.Unicode);
            }
        }

        private static byte[] Hash(string target, HashAlgorithm algorithm, Encoding encoding)
        {
            if (String.IsNullOrEmpty(target))
            {
                throw new ArgumentNullException("target");
            }

            if (algorithm == null)
            {
                throw new ArgumentNullException("algorithm");
            }

            var data = encoding.GetBytes(target);
            var hash = algorithm.ComputeHash(data);
            return hash;
        }
    }
}
