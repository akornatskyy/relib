using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace ReusableLibrary.Abstractions.Helpers
{
    public static class AssemblyHelper
    {
        public static string ToShortVersionString(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            var v = assembly.GetName().Version;
            return String.Format(CultureInfo.CurrentCulture,
                "{0}.{1}", v.Major, v.Minor);
        }

        public static string ToLongVersionString(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            FileInfo file = new FileInfo(assembly.Location);
            return String.Format(CultureInfo.CurrentCulture,
                "{0} [{1} {2}]",
                assembly.GetName().Version,
                file.LastWriteTime.ToShortDateString(),
                file.LastWriteTime.ToShortTimeString());
        }
    }
}
