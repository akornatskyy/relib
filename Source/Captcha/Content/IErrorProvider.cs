using System.IO;

namespace ReusableLibrary.Captcha.Content
{
    public interface IErrorProvider
    {
        string ContentType();

        void WriteTo(Stream stream);
    }
}
