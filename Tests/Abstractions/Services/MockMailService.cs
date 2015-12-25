using System;
using System.Net.Mail;
using System.Threading;
using ReusableLibrary.Abstractions.Services;

namespace ReusableLibrary.Abstractions.Tests.Services
{
    public class MockMailService : MailService
    {
        public MockMailService()
        {
            WaitHandle = new ManualResetEvent(false);
        }

        public EventWaitHandle WaitHandle { get; set; }

        public Exception DispatchMessageThrows { get; set; }

        public bool ExceptionHandled { get; set; }

        protected override void DispatchMessage(MailMessage message)
        {
            if (DispatchMessageThrows != null)
            {
                var ex = DispatchMessageThrows;
                throw ex;
            }

            WaitHandle.Set();
        }

        protected override void HandleException(Exception ex)
        {
            ExceptionHandled = true;
        }
    }
}
