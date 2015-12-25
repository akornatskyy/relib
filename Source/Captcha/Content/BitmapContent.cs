using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Captcha.Drawing;

namespace ReusableLibrary.Captcha.Content
{
    public sealed class BitmapContent : IContentProvider
    {
        private readonly int m_width;
        private readonly int m_height;
        private readonly IImageCodecInfo m_codecInfo;
        private readonly IGraphicsDrawing[] m_drawings;

        public BitmapContent(int width, int height, 
            IImageCodecInfo codecInfo, IGraphicsDrawing[] drawings)
        {
            m_width = width;
            m_height = height;
            m_codecInfo = codecInfo;
            m_drawings = drawings;
        }

        public BitmapContent(CaptchaOptions options,
            IImageCodecInfo codecInfo, IGraphicsDrawing[] drawings)
            : this(options.Width, options.Height, codecInfo, drawings)
        {
        }

        #region ICaptchaContent Members

        public string ContentType()
        {
            return m_codecInfo.ContentType();
        }

        public void WriteTo(Stream stream, string turingNumber)
        {
            using (var image = new Bitmap(m_width, m_height, PixelFormat.Format32bppArgb))
            {
                using (var graphics = Graphics.FromImage(image))
                {
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    var random = new Random(RandomHelper.Seed());
                    foreach (var drawing in m_drawings)
                    {
                        drawing.Draw(graphics, random, turingNumber);
                    }
                }

                image.Save(stream, m_codecInfo.Codec(), m_codecInfo.Parameters());
            }
        }

        #endregion
    }
}
