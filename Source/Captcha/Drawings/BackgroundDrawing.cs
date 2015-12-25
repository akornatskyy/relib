using System;
using System.Drawing;
using ReusableLibrary.Captcha.Internals;

namespace ReusableLibrary.Captcha.Drawing
{
    public sealed class BackgroundDrawing : IGraphicsDrawing
    {
        private readonly Color m_color;

        public BackgroundDrawing(Color color)
        {
            m_color = color;
        }

        public BackgroundDrawing(CaptchaOptions options)
        {
            m_color = ColorHelper.Parse(options.Items[CaptchaOptionNames.BackColor] ?? CaptchaOptionDefaults.BackColor, Color.Beige);
        }

        #region ICaptchaDrawing Members

        public void Draw(Graphics graphics, Random random, string text)
        {
            using (var brush = new SolidBrush(m_color))
            {
                graphics.FillRectangle(brush, graphics.ClipBounds);
            }
        }

        #endregion
    }
}
