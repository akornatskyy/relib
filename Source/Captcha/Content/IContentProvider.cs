using System.IO;

namespace ReusableLibrary.Captcha.Content
{
    public interface IContentProvider
    {
        string ContentType();

        void WriteTo(Stream stream, string turingNumber);
    }
}
