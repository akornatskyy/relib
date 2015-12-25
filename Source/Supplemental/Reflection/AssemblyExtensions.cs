using System.Reflection;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Supplemental.Reflection
{
    public static class AssemblyExtensions
    {
        public static string ToShortVersionString(this Assembly assembly)
        {
            return AssemblyHelper.ToShortVersionString(assembly);
        }

        public static string ToLongVersionString(this Assembly assembly)
        {
            return AssemblyHelper.ToLongVersionString(assembly);
        }
    }
}
