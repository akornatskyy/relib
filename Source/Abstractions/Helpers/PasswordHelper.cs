using System;
using System.Collections.Generic;
using System.Text;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Helpers
{
    public static class PasswordHelper
    {
        public static string NextPassword(Random random, Pair<int, string>[] passwordChars)
        {
            var password = new StringBuilder();
            foreach (var pair in passwordChars)
            {
                for (var i = 0; i < pair.First; i++)
                {
                    password.Append(RandomHelper.NextChar(random, pair.Second));
                }
            }

            return new string(new List<char>(
                RandomHelper.Shuffle(random, password.ToString())).ToArray());
        }
    }
}
