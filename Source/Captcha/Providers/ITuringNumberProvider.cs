using System;

namespace ReusableLibrary.Captcha.Providers
{
    public interface ITuringNumberProvider
    {
        string NextTuringNumber();
    }
}
