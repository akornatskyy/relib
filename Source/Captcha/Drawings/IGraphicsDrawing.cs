using System;
using System.Drawing;

namespace ReusableLibrary.Captcha.Drawing
{
    public interface IGraphicsDrawing
    {
        void Draw(Graphics graphics, Random random, string text);
    }
}
