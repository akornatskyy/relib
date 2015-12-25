using System;
using ReusableLibrary.Abstractions.Caching;
using ReusableLibrary.Captcha.Content;
using ReusableLibrary.Captcha.Drawing;
using ReusableLibrary.Captcha.Internals;

namespace ReusableLibrary.Captcha
{
    public sealed class BitmapCaptchaFactory : AbstractCaptchaFactory
    {
        private readonly IContentProvider m_contentProvider;
        private readonly IErrorProvider m_errorProvider;

        public BitmapCaptchaFactory(CaptchaOptions options)
            : this(options, () => new InnerWebCache())
        {
        }

        public BitmapCaptchaFactory(CaptchaOptions options, Func<ICache> challengeCacheFactory)
            : base(options, challengeCacheFactory)
        {
            m_contentProvider = new BitmapContent(options,
                new JpegImageCodecInfo(options),
                new IGraphicsDrawing[] 
                { 
                    new BackgroundDrawing(options),
                    new WarpTextDrawing(options),
                    new EllipseDrawing(options),
                    new CurveDrawing(options)
                });
            m_errorProvider = new EmptyErrorProvider();
        }

        #region ICaptchaFactory Members

        public override IContentProvider ContentProvider()
        {
            return m_contentProvider;
        }

        public override IErrorProvider ErrorProvider()
        {
            return m_errorProvider;
        }

        #endregion
    }
}
