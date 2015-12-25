using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Reflection;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Abstractions.Tracing
{
    public sealed class AssembliesExceptionHandler : IExceptionHandler
    {
        public string[] Ignore { get; set; }

        #region IExceptionHandler Members

        public bool HandleException(Exception ex)
        {
            var assemblies = Array.ConvertAll<Assembly, string>(AppDomain.CurrentDomain.GetAssemblies(), 
                a => a.FullName.Substring(0, a.FullName.IndexOf(", Culture=", StringComparison.OrdinalIgnoreCase)));
            var total = assemblies.Length;
            
            assemblies = Filter(assemblies, Ignore);
            Array.Sort(assemblies);
            
            var data = new NameValueCollection();
            EnumerableHelper.ForEach(assemblies, (number, name) =>
                data.Add(number.ToString(CultureInfo.InvariantCulture), name));

            ex.Data.Add(String.Format(CultureInfo.InvariantCulture, 
                "Loaded Assemblies (listing {0} of {1})", assemblies.Length, total), data);
            return false;
        }

        #endregion

        private static string[] Filter(string[] assemblies, string[] ignore)
        {
            if (ignore == null || ignore.Length == 0)
            {
                return assemblies;
            }

            return Array.FindAll(assemblies, name =>
            {
                return Array.FindIndex(ignore, 0, ignore.Length,
                    i => name.StartsWith(i, StringComparison.OrdinalIgnoreCase)) == -1;
            });
        }
    }
}
