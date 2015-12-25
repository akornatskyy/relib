using System;
using System.Drawing;
using ReusableLibrary.Captcha.Internals;

namespace ReusableLibrary.Captcha.Drawing
{
    public sealed class CurveDrawing : IGraphicsDrawing
    {
        private readonly int m_width;
        private readonly int m_height;
        private readonly Color m_color;
        private readonly float m_penWidth;

        private readonly int m_linesCount;
        private readonly int m_pointsCount;

        public CurveDrawing(int width, int height, NoiseLevel level, Color color)
        {
            m_width = width;
            m_height = height;
            m_color = color;

            switch (level)
            {
                case NoiseLevel.None:
                    return;

                case NoiseLevel.Low:
                    m_pointsCount = 4;
                    m_penWidth = height / 38f; // width 2
                    m_linesCount = 1;
                    break;

                case NoiseLevel.Medium:
                    m_pointsCount = 3;
                    m_penWidth = height / 30f; // width 2.5
                    m_linesCount = 2;
                    break;

                case NoiseLevel.High:
                    m_pointsCount = 3;
                    m_penWidth = height / 25f; // width 3
                    m_linesCount = 3;
                    break;

                case NoiseLevel.Extreme:
                    m_pointsCount = 4;
                    m_penWidth = height / 23f; // width 3
                    m_linesCount = 3;
                    break;
            }
        }

        public CurveDrawing(CaptchaOptions options)
            : this(options.Width, options.Height,
            (NoiseLevel)Enum.Parse(typeof(NoiseLevel), options.Items[CaptchaOptionNames.CurveNoiseLevel]
                ?? options.Items[CaptchaOptionNames.NoiseLevel] ?? CaptchaOptionDefaults.CurveNoiseLevel, true),
            ColorHelper.Parse(options.Items[CaptchaOptionNames.CurveNoiseColor]
                ?? options.Items[CaptchaOptionNames.NoiseColor] ?? CaptchaOptionDefaults.CurveNoiseColor, Color.Beige))
        {
        }

        #region ICaptchaDrawing Members

        public void Draw(Graphics graphics, Random random, string text)
        {
            using (var pen = new Pen(m_color, m_penWidth))
            {
                var points = new PointF[m_pointsCount];
                for (var i = 1; i <= m_linesCount; i++)
                {
                    for (var j = 0; j < m_pointsCount; j++)
                    {
                        points[j] = new PointF(random.Next(m_width), random.Next(m_height));
                    }

                    graphics.DrawCurve(pen, points, 1.75f);
                }
            }
        }

        #endregion
    }
}
