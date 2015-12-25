using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Helpers
{
    public static class RandomHelper
    {
        private static readonly RNGCryptoServiceProvider g_seedProvider = new RNGCryptoServiceProvider();
        private static readonly string[] g_words = new string[]
        {
            "lorem", "ipsum", "dolor", "sit", "amet", "consetetur", "sadipscing", "elitr", "sed", "diam", 
            "nonumy", "eirmod", "tempor", "invidunt", "ut", "labore", "et",
            "dolore", "magna", "aliquyam", "erat", "sed", "diam", "voluptua",
            "at", "vero", "eos", "accusam", "et", "justo", "duo", "dolores",
            "et", "ea", "rebum", "stet", "clita", "kasd", "gubergren", "no",
            "sea", "takimata", "sanctus", "est", "minim", "exercitation",
            "ullamco", "laboris", "nisi", "aliquip", "ex", "ea", "commodo",
            "consequat", "officia", "deserunt", "mollit" 
        };

        [DebuggerStepThrough]
        public static int Seed()
        {
            byte[] buffer = new byte[4];
            g_seedProvider.GetBytes(buffer);
            return BitConverter.ToInt32(buffer, 0);
        }

        [DebuggerStepThrough]
        public static char NextChar(Random random, string characterGroup)
        {
            if (String.IsNullOrEmpty(characterGroup))
            {
                return char.MinValue;
            }

            return characterGroup[NextInt(random, 0, characterGroup.Length - 1)];
        }

        [DebuggerStepThrough]
        public static string NextString(Random random, int length, string characterGroup)
        {
            var buffer = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                buffer.Append(NextChar(random, characterGroup));
            }

            return buffer.ToString();
        }

        [DebuggerStepThrough]
        public static string NextWord(Random random)
        {
            return Next<string>(random, g_words);
        }

        [DebuggerStepThrough]
        public static string NextWord(Random random, params string[] words)
        {
            return Next<string>(random, words);
        }

        [DebuggerStepThrough]
        public static T Next<T>(Random random, params T[] sequence)
        {
            if (sequence == null || sequence.Length == 0)
            {
                return default(T);
            }

            return sequence[random.Next(sequence.Length)];
        }

        [DebuggerStepThrough]
        public static string NextSentence(Random random, int wordsNumber)
        {
            if (wordsNumber <= 0)
            {
                return string.Empty;
            }

            StringBuilder buffer = new StringBuilder(wordsNumber * 8);
            buffer.Append(NextWord(random));
            int i = 1;
            while (i < wordsNumber)
            {
                buffer.Append(" ");
                buffer.Append(NextWord(random));
                i++;
            }

            buffer.Insert(0, buffer.ToString(0, 1).ToUpper(CultureInfo.InvariantCulture));
            buffer.Remove(1, 1);
            return buffer.ToString();
        }

        [DebuggerStepThrough]
        public static string NextSentences(Random random, params int[] wordsNumber)
        {
            if (wordsNumber.Length == 0)
            {
                return String.Empty;
            }

            StringBuilder buffer = new StringBuilder(wordsNumber.Length * 25);
            buffer.Append(NextSentence(random, wordsNumber[0]));
            int i = 1;
            while (i < wordsNumber.Length)
            {
                buffer.Append(". ");
                buffer.Append(NextSentence(random, wordsNumber[i]));
                i++;
            }

            buffer.Append(".");

            return buffer.ToString();
        }

        [DebuggerStepThrough]
        public static DateTime NextDate(Random random, int daysOffset)
        {
            return NextDate(random, DateTime.Now, daysOffset);
        }

        [DebuggerStepThrough]
        public static DateTime NextDate(Random random, DateTime date, int daysOffset)
        {
            var sign = Math.Sign(daysOffset);
            return date.AddDays(sign * random.Next(sign * daysOffset))
                .AddHours(sign * NextInt(random, 1, 22))
                .AddMinutes(sign * NextInt(random, 1, 58))
                .AddSeconds(sign * NextInt(random, 1, 58))
                .AddMilliseconds(sign * NextInt(random, 1, 999));
        }

        [DebuggerStepThrough]
        public static int NextInt(Random random, int min, int max)
        {
            if (min == max)
            {
                return min;
            }

            if (max < min)
            {
                return NextInt(random, max, min);
            }

            return random.Next(min, max + 1);
        }

        [DebuggerStepThrough]
        public static string NextSubstring(Random random, string text, int minLength, int maxLength)
        {
            if (String.IsNullOrEmpty(text) || minLength == 0 || text.Length <= minLength)
            {
                return text;
            }

            int len = Math.Min(NextInt(random, minLength, maxLength), text.Length - 1);
            int startIndex = NextInt(random, 0, text.Length - len);
            return text.Substring(startIndex, len);
        }

        [DebuggerStepThrough]
        public static string NextStartsWith(Random random, string text, int minLength, int maxLength)
        {
            if (String.IsNullOrEmpty(text) || minLength == 0 || text.Length <= minLength)
            {
                return text;
            }

            int len = Math.Min(NextInt(random, minLength, maxLength), text.Length - 1);
            return text.Substring(0, len);
        }

        [DebuggerStepThrough]
        public static bool NextBoolean(Random random)
        {
            return random.Next(2) == 1;
        }

        [DebuggerStepThrough]
        public static T FirstRandom<T>(Random random, IEnumerable<T> enumerable)
        {
            var list = new List<T>(enumerable);
            return list[NextInt(random, 0, list.Count - 1)];
        }

        [DebuggerStepThrough]
        public static void TimesRandom<T>(Random random, IEnumerable<T> enumerable, int iterations, Action<T> action)
        {
            EnumerableHelper.ForEach(new LoopIterator<T>(Shuffle(random, enumerable), iterations), action);
        }

        [DebuggerStepThrough]
        public static IEnumerable<T> Shuffle<T>(Random random, IEnumerable<T> enumerable)
        {
            // http://en.wikipedia.org/wiki/Fisher-Yates_shuffle#The_modern_algorithm
            var list = new List<T>();
            list.AddRange(enumerable);
            for (int i = list.Count - 1; i > 0; i--)
            {
                int position = random.Next(i + 1);
                var temp = list[i];
                list[i] = list[position];
                list[position] = temp;
            }

            return list.AsReadOnly();
        }

        [DebuggerStepThrough]
        public static IEnumerable<T> NextSequence<T>(Random random, Func2<int, T> func)
        {
            return NextSequence<T>(random, 1, 100, func);
        }

        [DebuggerStepThrough]
        public static IEnumerable<T> NextSequence<T>(Random random, int max, Func2<int, T> func)
        {
            return NextSequence<T>(random, 1, max, func);
        }

        [DebuggerStepThrough]
        public static IEnumerable<T> NextSequence<T>(Random random, int min, int max, Func2<int, T> func)
        {
            var length = NextInt(random, min, max);
            for (int i = 0; i < length; i++)
            {
                yield return func(i);
            }
        }        
    }
}
