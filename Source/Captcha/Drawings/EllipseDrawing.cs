using System;
using System.Drawing;
using ReusableLibrary.Captcha.Internals;

namespace ReusableLibrary.Captcha.Drawing
{
    public sealed class EllipseDrawing : IGraphicsDrawing
    {
        private readonly int m_width;
        private readonly int m_height;
        private readonly Color m_color;
        private readonly int m_maxSize;
        private readonly int m_count;

        public EllipseDrawing(int width, int height, NoiseLevel level, Color color)
        {
            m_width = width;
            m_height = height;
            m_color = color;
            int m1 = 1;
            int m2 = 1;
            switch (level)
            {
                case NoiseLevel.None:
                    return;

                case NoiseLevel.Low:
                    m1 = 100; // 15 points
                    m2 = 25; // 8 size
                    break;

                case NoiseLevel.Medium:
                    m1 = 60; // 25 points
                    m2 = 30; // 6 size
                    break;

                case NoiseLevel.High:
                    m1 = 40; // 35 points
                    m2 = 40; // 5 size
                    break;

                case NoiseLevel.Extreme:
                    m1 = 30; // 50 points
                    m2 = 50; // 4 size
                    break;
            }

            m_maxSize = Convert.ToInt32(Math.Max(width, height) / m2);
            m_count = Convert.ToInt32(width * height / m1);
        }

        public EllipseDrawing(CaptchaOptions options)
            : this(options.Width, options.Height,
            (NoiseLevel)Enum.Parse(typeof(NoiseLevel), options.Items[CaptchaOptionNames.EllipseNoiseLevel]
                ?? options.Items[CaptchaOptionNames.NoiseLevel] ?? CaptchaOptionDefaults.EllipseNoiseLevel, true),
            ColorHelper.Parse(options.Items[CaptchaOptionNames.EllipseNoiseColor]
                ?? options.Items[CaptchaOptionNames.NoiseColor] ?? CaptchaOptionDefaults.EllipseNoiseColor, Color.Beige))
        {
        }

        #region ICaptchaDrawing Members

        public void Draw(Graphics graphics, Random random, string text)
        {
            if (m_count == 0)
            {
                return;
            }

            using (var brush = new SolidBrush(m_color))
            {
                for (int i = 0; i < m_count; i++)
                {
                    graphics.FillEllipse(brush,
                        random.Next(m_width),
                        random.Next(m_height),
                        random.Next(m_maxSize),
                        random.Next(m_maxSize));
                }
            }
        }

        #endregion
    }
}
