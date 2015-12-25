using System.Drawing.Imaging;
using System.Linq;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Captcha.Content
{
    public sealed class JpegImageCodecInfo : IImageCodecInfo
    {
        private readonly ImageCodecInfo m_codec = ImageCodecInfo.GetImageEncoders()
            .First(enc => enc.FormatID == ImageFormat.Jpeg.Guid);

        private readonly EncoderParameters m_params;

        public JpegImageCodecInfo(int quality)
        {
            m_params = new EncoderParameters()
            {
                Param = new EncoderParameter[] 
                { 
                    new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality) 
                }
            };
        }

        public JpegImageCodecInfo(CaptchaOptions options)
            : this(NameValueCollectionHelper.ConvertToInt32(options.Items, CaptchaOptionNames.EncoderQuality, CaptchaOptionDefaults.EncoderQuality))
        {
        }

        #region IImageCodecInfo Members

        public string ContentType()
        {
            return "image/jpeg";
        }

        public ImageCodecInfo Codec()
        {
            return m_codec;
        }

        public EncoderParameters Parameters()
        {
            return m_params;
        }

        #endregion
    }
}
