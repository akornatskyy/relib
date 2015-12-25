using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Globalization;

namespace ReusableLibrary.Abstractions.Tracing
{
    public sealed class PerformanceCounterExceptionHandler : IExceptionHandler
    {
        private static readonly Regex RegexCounter = new Regex(@"\\(?<category>.+)\\(?<name>.+)", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private static readonly Regex RegexInstanceCounter = new Regex(@"\\(?<category>.+)\((?<instance>.+)\)\\(?<name>.+)", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        public string[] Counters { get; set; }

        public PerformanceCounterExceptionHandler()
        {
            WatchWindow = 100;
            Counters = new string[] { };
        }

        public int WatchWindow { get; set; }

        #region IExceptionHandler Members

        public bool HandleException(Exception ex)
        {
            var data = new NameValueCollection();
            var performanceCounters = CreateCounters();
            Thread.Sleep(WatchWindow);
            foreach (var counter in performanceCounters)
            {
                if (counter.Value == null)
                {
                    data.Add(counter.Key, "N/A");
                }
                else
                {
                    data.Add(counter.Key, counter.Value.NextValue().ToString(CultureInfo.InvariantCulture));
                }
            }

            if (data.Count > 0)
            {
                ex.Data.Add("Performance Counters", data);
            }

            return false;
        }

        #endregion

        public IDictionary<string, PerformanceCounter> CreateCounters()
        {
            var performanceCounters = new Dictionary<string, PerformanceCounter>(Counters.Length);
            for (int i = 0; i < Counters.Length; i++)
            {
                var name = Counters[i];
                var counter = CreateCounter(name);

                try
                {                    
                    if (counter != null)
                    {
                        counter.NextValue();
                    }

                    performanceCounters.Add(name, counter);
                }
                catch (InvalidOperationException)
                {
                }
                catch (UnauthorizedAccessException)
                {
                }                
            }

            return performanceCounters;
        }

        private static PerformanceCounter CreateCounter(string longname)
        {
            PerformanceCounter c = null;
            var match = RegexInstanceCounter.Match(longname);
            try
            {
                var category = string.Empty;
                var name = string.Empty;
                var instance = string.Empty;
                if (match.Success)
                {
                    instance = match.Groups["instance"].Value;
                }
                else
                {
                    match = RegexCounter.Match(longname);
                    if (!match.Success)
                    {
                        return null;
                    }
                }

                category = match.Groups["category"].Value;
                name = match.Groups["name"].Value;
                if (!PerformanceCounterCategory.CounterExists(name, category))
                {
                    return null;
                }

                c = new PerformanceCounter(category, name, instance);
            }
            catch (InvalidOperationException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }

            return c;
        }
    }
}
