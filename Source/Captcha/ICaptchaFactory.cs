using ReusableLibrary.Abstractions.Caching;
using ReusableLibrary.Captcha.Content;
using ReusableLibrary.Captcha.Drawing;
using ReusableLibrary.Captcha.Providers;

namespace ReusableLibrary.Captcha
{
    public interface ICaptchaFactory
    {
        CaptchaOptions Options { get; set; }

        IChallengeCodeProvider ChallengeCodeProvider();

        IVaryByCustomProvider VaryByCustomProvider();

        ITuringNumberProvider TuringNumberProvider();

        ICache ChallengeCache();

        IContentProvider ContentProvider();

        IErrorProvider ErrorProvider();

        ICaptchaInstrumentationProvider InstrumentationProvider();
    }
}
