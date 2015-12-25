using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Security;
using System.Threading;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Abstractions.Tracing
{
    public sealed class ProcessExceptionHandler : IExceptionHandler
    {
        #region IExceptionHandler Members

        public bool HandleException(Exception ex)
        {
            var data = new NameValueCollection();
            data.Add("Runtime", System.Environment.Version.ToString());
            data.Add("AppDomainName", AppDomain.CurrentDomain.FriendlyName);
            try
            {
                var currentProcess = Process.GetCurrentProcess();                

                data.Add("ProcessId", currentProcess.Id.ToString(CultureInfo.InvariantCulture));
                data.Add("ProcessName", currentProcess.ProcessName);

                data.Add("UpTime", TimeSpanHelper.ToLongTimeString(DateTime.Now.Subtract(currentProcess.StartTime)));

                data.Add("TotalProcessorTime", TimeSpanHelper.ToLongTimeString(currentProcess.TotalProcessorTime));
                data.Add("UserProcessorTime", TimeSpanHelper.ToLongTimeString(currentProcess.UserProcessorTime));
                data.Add("PrivilegedProcessorTime", TimeSpanHelper.ToLongTimeString(currentProcess.PrivilegedProcessorTime));

                data.Add("WorkingSet", FormatMb(currentProcess.WorkingSet64));
                data.Add("PagedMemory", FormatMb(currentProcess.PagedMemorySize64));
                data.Add("HandleCount", currentProcess.HandleCount.ToString(CultureInfo.InvariantCulture));

                data.Add("ThreadCount", currentProcess.Threads.Count.ToString(CultureInfo.InvariantCulture));
            }
            catch (SecurityException sex)
            {
                data.Add("CurrentProcess", sex.Message);
            }

            var currentThread = Thread.CurrentThread;
            data.Add("ThreadName", String.IsNullOrEmpty(currentThread.Name) ? "N/A" : currentThread.Name);

            var principal = Thread.CurrentPrincipal;
            if (principal != null && principal.Identity != null)
            {
                data.Add("ThreadIdentity", 
                    String.IsNullOrEmpty(principal.Identity.Name) ? "N/A" : principal.Identity.Name);
            }

            data.Add("CurrentCulture", currentThread.CurrentCulture.Name);
            data.Add("CurrentUICulture", currentThread.CurrentUICulture.Name);

            ex.Data.Add("Hosting Process", data);
            return false;
        }

        #endregion

        private static string FormatMb(long n)
        {
            return String.Format(CultureInfo.InvariantCulture, "{0:F1} Mb", Convert.ToDouble(n) / 1048576);
        }
    }
}
