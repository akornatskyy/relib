using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace ReusableLibrary.WatiN
{
    internal static class ProcessHelper
    {
        public static void Shutdown(Process process)
        {
            try
            {
                process.Refresh();
                if (!process.HasExited && !process.WaitForExit(2000))
                {
                    process.Kill();
                    process.WaitForExit();
                }

                process.Close();
                process.Dispose();
            }
            catch (InvalidOperationException)
            {
                // Cannot process request because the process (XXXX) has exited.
            }
            catch (Win32Exception)
            {
                // Can fail with: Access is denied
                // It is safe to ignore
                Thread.Sleep(500);
            }
            catch (SystemException)
            {
                // There is no process associated with this Process object.
            }
        }
    }
}
