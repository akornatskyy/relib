using System;
using System.Globalization;
using System.Net.Mail;
using System.Text;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Threading;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Abstractions.Services
{
    public class MailService : IMailService
    {
        private static object g_syncRoot = new object();

        public MailService()
        {
            Enabled = true;
        }

        public IExceptionHandler ExceptionHandler { get; set; }

        public bool Enabled { get; set; }

        public bool EnableSsl { get; set; }

        public string[] Recipients { get; set; }

        public string[] CarbonCopies { get; set; }

        public string[] BlindCarbonCopies { get; set; }

        #region IMailService Members

        public void Send(MailMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            if (!CollectionHelper.IsNullOrEmpty(Recipients))
            {
                EnumerableHelper.ForEach(Recipients, to =>
                {
                    if (!string.IsNullOrEmpty(to))
                    {
                        message.To.Add(to);
                    }
                });
            }
            
            if (CollectionHelper.IsNullOrEmpty(message.To))
            {
                throw new ArgumentException("No recipient specified");
            }

            if (!CollectionHelper.IsNullOrEmpty(CarbonCopies))
            {
                EnumerableHelper.ForEach(CarbonCopies, cc =>
                {
                    if (!string.IsNullOrEmpty(cc))
                    {
                        message.CC.Add(cc);
                    }
                });
            }

            if (!CollectionHelper.IsNullOrEmpty(BlindCarbonCopies))
            {
                EnumerableHelper.ForEach(BlindCarbonCopies, bcc =>
                {
                    if (!string.IsNullOrEmpty(bcc))
                    {
                        message.Bcc.Add(bcc);
                    }
                });
            }

            AsyncHelper.FireAndForget(SendAsync, message);
        }

        public string MergeTemplate(string template, params string[] pairs)
        {
            if (String.IsNullOrEmpty(template))
            {
                throw new ArgumentNullException("template");
            }

            var body = new StringBuilder(template);
            for (var i = 0; i < pairs.Length; i += 2)
            {
                var key = pairs[i];
                string value = i + 1 < pairs.Length ? pairs[i + 1] : null;
                if (String.IsNullOrEmpty(value))
                {
                    value = "N/A";
                }

                body = body.Replace(String.Format(CultureInfo.InvariantCulture, "<%= {0}%>", key), value);
            }

            return body.ToString();
        }

        #endregion

        protected virtual void DispatchMessage(MailMessage message)
        {
            if (Enabled)
            {
                var client = new SmtpClient();
                client.EnableSsl = EnableSsl;
                client.Send(message);
            }
        }

        protected virtual void HandleException(Exception ex)
        {
            if (ExceptionHandler != null)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        private void SendAsync(MailMessage message)
        {
            lock (g_syncRoot)
            {
                try
                {
                    try
                    {
                        DispatchMessage(message);
                    }
                    finally
                    {
                        message.Dispose();
                    }
                }
                catch (SystemException ex)
                {
                    HandleException(ex);
                }
                catch (SmtpException ex)
                {
                    HandleException(ex);
                }
            }
        }
    }
}
