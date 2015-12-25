using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
using System.Text;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Services;

namespace ReusableLibrary.Abstractions.Tracing
{
    public sealed class MailExceptionHandler : IExceptionHandler
    {
        private readonly IMailService m_mailService;

        public MailExceptionHandler(IMailService mailService)
        {
            m_mailService = mailService;
        }

        public string Application { get; set; }

        #region IExceptionHandler Members

        public bool HandleException(Exception ex)
        {
            var mail = new MailMessage()
            {
                IsBodyHtml = true,
                SubjectEncoding = Encoding.UTF8,
                Subject = String.Format(CultureInfo.InvariantCulture, Properties.Resources.ErrorReportMailSubject, Application, ex.GetType().Name),
                BodyEncoding = Encoding.UTF8,
                Body = MergeErrorReportMessage(ex)
            };
            m_mailService.Send(mail);
            return false;
        }

        #endregion

        private static string FormatError(Exception ex)
        {
            var buffer = new StringBuilder(0x4000);
            ErrorFormatter.Append(buffer, ex);
            return HtmlEncode(buffer);
        }

        private static string HtmlEncode(string text)
        {
            return HtmlEncode(new StringBuilder(text));
        }

        private static string HtmlEncode(StringBuilder buffer)
        {
            // Html Encoding
            buffer
                .Replace("&", "&amp;")
                .Replace("\"", "&quot;")                
                .Replace("<", " &lt;")
                .Replace(">", "&gt;")
                .Replace("\r\n", "<br />");
            return buffer.ToString();
        }

        private string MergeErrorReportMessage(Exception ex)
        {
            var names = new List<string>();
            var messages = new List<string>();
            foreach (var error in ExceptionHelper.Errors(ex))
            {
                names.Add(error.GetType().Name);
                messages.Add(error.Message);
            }

            return m_mailService.MergeTemplate(Properties.Resources.ErrorReportMailBody,
                "ExceptionName", HtmlEncode(String.Join(", ", names.ToArray())),
                "Application", HtmlEncode(Application),
                /* 2010/9/3 8:28:30 PM UTC */
                "DateSubmitted", DateTime.UtcNow.ToString("yyyy/M/d h:mm:ss tt UTC", CultureInfo.InvariantCulture),
                "HostEnvironment", Environment.MachineName,
                "ExceptionMessage", HtmlEncode(String.Join(Environment.NewLine, messages.ToArray())),
                "ExceptionFormatted", FormatError(ex));
        }
    }
}
