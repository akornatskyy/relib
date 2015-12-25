using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Captcha.Internals;

namespace ReusableLibrary.Captcha.Drawing
{
    public sealed class WarpTextDrawing : IGraphicsDrawing
    {
        private delegate void WarpTextStrategyAction(GraphicsPath path, Rectangle r, Random random);

        private readonly int m_width;
        private readonly int m_height;
        private readonly string[] m_fonts;
        private readonly Color m_textColor;
        private readonly float m_fontSize;
        private readonly float m_warpMultiplier1;
        private readonly float m_warpMultiplier2;
        private readonly WarpTextStrategyAction m_strategy = NoWarpTextStrategy;

        public WarpTextDrawing(int width, int height, string fonts, Color textColor, FontWarpFactor fontWarpFactor)
        {
            m_width = width;
            m_height = height;
            m_fonts = ParseFonts(fonts);
            m_textColor = textColor;            
            m_fontSize = FontSize(fontWarpFactor, height);
            if (fontWarpFactor != FontWarpFactor.None)
            {
                var m = WarpMutipliers(fontWarpFactor);
                m_warpMultiplier1 = m.First;
                m_warpMultiplier2 = m.Second;
                m_strategy = WarpText;
            }
        }

        public WarpTextDrawing(CaptchaOptions options)
            : this(options.Width, options.Height,
            options.Items[CaptchaOptionNames.Fonts] ?? CaptchaOptionDefaults.Fonts,
            ColorHelper.Parse(options.Items[CaptchaOptionNames.TextColor] ?? CaptchaOptionDefaults.TextColor, Color.BlueViolet),
            (FontWarpFactor)Enum.Parse(typeof(FontWarpFactor), options.Items[CaptchaOptionNames.FontWarp] ?? CaptchaOptionDefaults.FontWarp, true))
        {
        }

        #region ICaptchaDrawing Members

        public void Draw(Graphics graphics, Random random, string text)
        {
            var ageraveCharWidth = m_width / text.Length * 0.9f;
            var ageraveCharWidthI = Convert.ToInt32(ageraveCharWidth);
            using (var brush = new SolidBrush(m_textColor))
            {
                for (var i = 0; i < text.Length; i++)
                {
                    using (var font = GetFont(random))
                    {
                        var c = text[i];
                        var r = new Rectangle(Convert.ToInt32(i * ageraveCharWidth), 0, ageraveCharWidthI, m_height);
                        using (var path = TextPath(c.ToString(), font, r))
                        {
                            m_strategy(path, r, random);                            
                            graphics.FillPath(brush, path);
                        }
                    }
                }
            }
        }

        #endregion

        private static void NoWarpTextStrategy(GraphicsPath path, Rectangle r, Random random)
        {
        }

        private static string[] ParseFonts(string fonts)
        {
            return fonts.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static float FontSize(FontWarpFactor fontWarpFactor, int height)
        {
            float size = 0f;
            switch (fontWarpFactor)
            {
                case FontWarpFactor.None:
                    size = height * 0.7f;
                    break;

                case FontWarpFactor.Low:
                    size = height * 0.8f;
                    break;

                case FontWarpFactor.Medium:
                    size = height * 0.85f;
                    break;

                case FontWarpFactor.High:
                    size = height * 0.9f;
                    break;
            }

            return size;
        }

        private static Pair<float> WarpMutipliers(FontWarpFactor fontWarpFactor)
        {
            float m1 = 1f;
            float m2 = 1f;
            switch (fontWarpFactor)
            {
                case FontWarpFactor.None:
                    break;

                case FontWarpFactor.Low:
                    m1 = 6f;
                    m2 = 1f;
                    break;

                case FontWarpFactor.Medium:
                    m1 = 5f;
                    m2 = 1.3f;
                    break;

                case FontWarpFactor.High:
                    m1 = 4.5f;
                    m2 = 1.4f;
                    break;
            }

            return new Pair<float>(m1, m2);
        }

        private static PointF RandomPoint(Random random, int xmin, int xmax, int ymin, int ymax)
        {
            return new PointF(
                (float)RandomHelper.NextInt(random, xmin, xmax),
                (float)RandomHelper.NextInt(random, ymin, ymax));
        }

        private static GraphicsPath TextPath(string s, Font f, Rectangle r)
        {
            var path = new GraphicsPath();
            path.AddString(s, f.FontFamily, (int)f.Style, f.Size, r, new StringFormat()
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Near
            });
            return path;
        }

        private Font GetFont(Random random)
        {
            string fontName = m_fonts[random.Next(m_fonts.Length)];
            return new Font(fontName, m_fontSize, FontStyle.Bold);
        }

        private void WarpText(GraphicsPath textPath, Rectangle rect, Random random)
        {
            var src = new RectangleF(Convert.ToSingle(rect.Left), 0f, Convert.ToSingle(rect.Width), (float)rect.Height);
            int dx = Convert.ToInt32(rect.Width / m_warpMultiplier1);
            int dy = Convert.ToInt32(rect.Height / m_warpMultiplier1);            
            int xmin = rect.Left - Convert.ToInt32(dx * m_warpMultiplier2);
            int xmax = rect.Left + rect.Width + Convert.ToInt32(dx * m_warpMultiplier2);
            int ymax = rect.Height;

            var dest = new PointF[] 
            { 
                RandomPoint(random, xmin, xmin + dx, 0, dy), 
                RandomPoint(random, xmax - dx, xmax, 0, dy), 
                RandomPoint(random, xmin, xmin + dx, ymax - dy, ymax), 
                RandomPoint(random, xmax - dx, xmax, ymax - dy, ymax) 
            };
            using (var matrix = new Matrix())
            {
                matrix.Translate(0f, 0f);
                textPath.Warp(dest, src, matrix, WarpMode.Perspective, 0f);
            }
        }
    }
}
