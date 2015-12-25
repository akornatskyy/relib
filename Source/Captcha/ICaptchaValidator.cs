using System;

namespace ReusableLibrary.Captcha
{
    public interface ICaptchaValidator
    {
        bool Enabled { get; set; }

        bool Validate();

        bool Validate(string path);
    }
}
