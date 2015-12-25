using System;

namespace ReusableLibrary.Captcha.Providers
{
    public interface ICaptchaInstrumentationProvider
    {
        void FireIssued(bool succeed);

        void FireRendered(bool succeed);

        void FireVerified(bool succeed);
    }
}
