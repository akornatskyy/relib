using System;
using System.Collections.Generic;
using System.Diagnostics;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Supplemental.System
{
    public static class RandomExtensions
    {
        [DebuggerStepThrough]
        public static char NextChar(this Random random, string characterGroup)
        {
            return RandomHelper.NextChar(random, characterGroup);
        }

        [DebuggerStepThrough]
        public static string NextString(this Random random, int length, string characterGroup)
        {
            return RandomHelper.NextString(random, length, characterGroup);
        }

        [DebuggerStepThrough]
        public static string NextWord(this Random random)
        {
            return RandomHelper.NextWord(random);
        }

        [DebuggerStepThrough]
        public static string NextWord(this Random random, params string[] words)
        {
            return RandomHelper.NextWord(random, words);
        }

        [DebuggerStepThrough]
        public static T Next<T>(this Random random, params T[] sequence)
        {
            return RandomHelper.Next<T>(random, sequence);
        }

        [DebuggerStepThrough]
        public static string NextSentence(this Random random, int wordNumber)
        {
            return RandomHelper.NextSentence(random, wordNumber);
        }

        [DebuggerStepThrough]
        public static string NextSentences(this Random random, params int[] wordNumbers)
        {
            return RandomHelper.NextSentences(random, wordNumbers);
        }

        [DebuggerStepThrough]
        public static DateTime NextDate(this Random random, int daysOffset)
        {
            return RandomHelper.NextDate(random, daysOffset);
        }

        [DebuggerStepThrough]
        public static DateTime NextDate(this Random random, DateTime date, int daysOffset)
        {
            return RandomHelper.NextDate(random, date, daysOffset);
        }

        [DebuggerStepThrough]
        public static int NextInt(this Random random, int min, int max)
        {
            return RandomHelper.NextInt(random, min, max);
        }

        [DebuggerStepThrough]
        public static string NextSubstring(this Random random, string text, int minLength, int maxLength)
        {
            return RandomHelper.NextSubstring(random, text, minLength, maxLength);
        }

        [DebuggerStepThrough]
        public static string NextStartsWith(this Random random, string text, int minLength, int maxLength)
        {
            return RandomHelper.NextStartsWith(random, text, minLength, maxLength);
        }

        [DebuggerStepThrough]
        public static bool NextBoolean(this Random random)
        {
            return RandomHelper.NextBoolean(random);
        }

        [DebuggerStepThrough]
        public static T FirstRandom<T>(this Random random, IEnumerable<T> enumerable)
        {
            return RandomHelper.FirstRandom<T>(random, enumerable);
        }

        [DebuggerStepThrough]
        public static void TimesRandom<T>(this Random random, IEnumerable<T> enumerable, int iterations, Action<T> action)
        {
            RandomHelper.TimesRandom<T>(random, enumerable, iterations, action);
        }

        [DebuggerStepThrough]
        public static IEnumerable<T> Shuffle<T>(this Random random, IEnumerable<T> enumerable)
        {
            return RandomHelper.Shuffle<T>(random, enumerable);
        }

        [DebuggerStepThrough]
        public static IEnumerable<T> NextSequence<T>(this Random random, Func2<int, T> func)
        {
            return RandomHelper.NextSequence<T>(random, func);
        }

        [DebuggerStepThrough]
        public static IEnumerable<T> NextSequence<T>(this Random random, int max, Func2<int, T> func)
        {
            return RandomHelper.NextSequence<T>(random, max, func);
        }

        [DebuggerStepThrough]
        public static IEnumerable<T> NextSequence<T>(this Random random, int min, int max, Func2<int, T> func)
        {
            return RandomHelper.NextSequence<T>(random, min, max, func);
        }
    }
}
