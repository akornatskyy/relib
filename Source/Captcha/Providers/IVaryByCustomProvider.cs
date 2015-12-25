using System;

namespace ReusableLibrary.Captcha.Providers
{
    public interface IVaryByCustomProvider
    {
        string NextVaryByCustomString();
    }
}
