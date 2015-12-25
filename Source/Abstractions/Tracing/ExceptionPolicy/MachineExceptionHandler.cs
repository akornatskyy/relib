using System;
using System.Collections.Specialized;
using System.Globalization;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Abstractions.Tracing
{
    public sealed class MachineExceptionHandler : IExceptionHandler
    {
        #region IExceptionHandler Members

        public bool HandleException(Exception ex)
        {
            var data = new NameValueCollection();
            data.Add("MachineName", Environment.MachineName);
            data.Add("OSVersion", Environment.OSVersion.ToString());
            data.Add("MachineUpTime", TimeSpanHelper.ToLongTimeString(TimeSpan.FromMilliseconds(Environment.TickCount & Int32.MaxValue)));
            data.Add("TimeStamp", DateTime.Now.ToString(CultureInfo.InvariantCulture));
            data.Add("TimeStamp-UTC", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
            data.Add("UserName", Environment.UserName);
            
            ex.Data.Add("Machine Environment", data);
            return false;
        }

        #endregion
    }
}
