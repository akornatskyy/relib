using System;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Supplemental.System
{
    public static class TypeExtensions
    {
        public static string GetName(this Type type)
        {
            return TypeHelper.GetName(type);
        }
    }
}
