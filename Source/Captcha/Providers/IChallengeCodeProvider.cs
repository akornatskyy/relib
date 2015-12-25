using System.Collections.Specialized;

namespace ReusableLibrary.Captcha.Providers
{
    public interface IChallengeCodeProvider
    {
        bool HasChallengeCode(NameValueCollection @params);

        string ReadChallengeCode(NameValueCollection @params);
    }
}
