using System.Threading;
using WatiN.Core;

namespace ReusableLibrary.WatiN
{
    public static class ElementExtensions
    {
        public static bool EnsureClick(this Element element)
        {
            element.RefreshNativeElement();
            if (!element.Exists || !element.Enabled)
            {
                return false;
            }

            element.Click();
            return true;
        }

        public static bool TryWaitUntilEnabled(this Element element)
        {
            element.RefreshNativeElement();
            while (element.Exists && !element.Enabled)
            {
                Thread.Sleep(Settings.SleepTime);
            }

            return element.Exists;
        }

        public static bool TryWaitUntilExists(this Element element)
        {
            return TryWaitUntilExists(element, Settings.WaitUntilExistsTimeOut);
        }

        public static bool TryWaitUntilExists(this Element element, int timeout)
        {
            var result = false;
            try
            {
                element.RefreshNativeElement();
                element.WaitUntilExists(timeout);
                result = true;
            }
            catch (global::WatiN.Core.Exceptions.TimeoutException)
            {
            }

            return result;
        }
    }
}
