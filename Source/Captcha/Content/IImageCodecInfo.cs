using System.Drawing.Imaging;

namespace ReusableLibrary.Captcha.Content
{
    public interface IImageCodecInfo
    {
        string ContentType();

        ImageCodecInfo Codec();

        EncoderParameters Parameters();
    }
}
