using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ReusableLibrary.Abstractions.Tracing
{
    public class PerformanceCounterFactory
    {
        private static readonly Regex g_friendlyNameRegex = new Regex(@"[^\w\d]*", RegexOptions.Compiled);
        
        private readonly string m_category;
        private readonly string m_instance;
        private readonly bool m_enabled;

        public PerformanceCounterFactory(string category)
            : this(category, null)
        {
        }

        public PerformanceCounterFactory(string category, string instanceNameSuffix)
            : this(category, instanceNameSuffix, true)
        {
        }

        public PerformanceCounterFactory(string category, string instanceNameSuffix, bool enabled)
        {
            try
            {
                m_enabled = enabled && PerformanceCounterCategory.Exists(category);
                if (!m_enabled)
                {
                    return;
                }

                m_category = category;
                var friendlyName = AppDomain.CurrentDomain.FriendlyName;
                friendlyName = g_friendlyNameRegex.Replace(friendlyName, string.Empty);
                friendlyName = friendlyName.Length > 8 ? friendlyName.Substring(0, 8) : friendlyName;
                if (String.IsNullOrEmpty(instanceNameSuffix))
                {
                    m_instance = friendlyName;
                }
                else
                {
                    m_instance = String.Format(CultureInfo.InvariantCulture, "{0} - {1}", friendlyName, instanceNameSuffix);
                }
            }
            catch (UnauthorizedAccessException)
            {
                //// To read performance counters in Windows Vista and later, 
                //// Windows XP Professional x64 Edition, or Windows Server 2003, 
                //// you must either be a member of the Performance Monitor Users group 
                //// or have administrative privileges.
            }
        }

        public IPerformanceCounter Create(params string[] names)
        {
            return Create(names, new[] { "__Total__", m_instance });
        }

        public IPerformanceCounter Create(string name, string[] instances)
        {
            return Create(new[] { name }, instances);
        }

        public IPerformanceCounter Create(string[] names, string[] instances)
        {
            if (!m_enabled)
            {
                return new NullPerformanceCounter();
            }

            var counters = new PerformanceCounter[names.Length * instances.Length];
            for (var i = 0; i < names.Length; i++)
            {
                for (var j = 0; j < instances.Length; j++)
                {
                    counters[(i * instances.Length) + j] = new PerformanceCounter(m_category, names[i], instances[j], false);
                }
            }

            return new CompositePerformanceCounter(counters);
        }
    }
}
