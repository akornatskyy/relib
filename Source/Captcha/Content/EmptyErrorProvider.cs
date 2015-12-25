using System;
using System.IO;

namespace ReusableLibrary.Captcha.Content
{
    public sealed class EmptyErrorProvider : IErrorProvider
    {
        private static readonly byte[] g_gif1x1 = Convert.FromBase64String("R0lGODlhAQABAPAAAAAAAAAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==");

        #region IErrorProvider Members

        public string ContentType()
        {
            return "image/gif";
        }

        public void WriteTo(Stream stream)
        {
            stream.Write(g_gif1x1, 0, g_gif1x1.Length);
        }

        #endregion
    }
}
