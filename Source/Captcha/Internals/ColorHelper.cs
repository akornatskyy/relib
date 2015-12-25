using System;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace ReusableLibrary.Captcha.Internals
{
    public static class ColorHelper
    {
        private static readonly string[] g_knownColorNames = Enum.GetNames(typeof(KnownColor));

        public static Color Parse(string value, Color defaultColor)
        {
            if (g_knownColorNames.Any(name => name.Equals(value, StringComparison.OrdinalIgnoreCase)))
            {
                return Color.FromName(value);
            }

            int argb = 0;
            if (Int32.TryParse(value, NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture, out argb))
            {
                return Color.FromArgb(argb);
            }

            return defaultColor;
        }
    }
}
