using System.Net.Mail;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Abstractions.Services
{
    public interface IMailService
    {
        void Send(MailMessage message);

        string MergeTemplate(string template, params string[] pairs);
    }
}
