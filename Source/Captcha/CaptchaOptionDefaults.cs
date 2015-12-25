using System;

namespace ReusableLibrary.Captcha
{
    public class CaptchaOptionDefaults
    {
        public const string Path = "/captcha.jpg";

        public const int MinTimeout = 2;

        public const int MaxTimeout = 5 * 60;

        public const string Chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

        public const int MaxChars = 4;

        public const int Width = 200;

        public const int Height = 75;

        public const int EncoderQuality = 85;

        public const int MaxVaryCacheSize = 600;

        public const int VaryByCacheLifetime = 300;

        public const string ChallengeQueryKey = "c";

        public const string ValidationStateKey = "TuringNumber";

        public const string BackColor = "FFEEEECC";

        public const string TextColor = "FF5C87B2";

        public const string Fonts = "Tahoma;Courier New";

        public const string FontWarp = "Medium";

        public const string NoiseColor = "FFEEEECC";

        public const string NoiseLevel = "Medium";

        public const string CurveNoiseColor = "FFEEEECC";

        public const string CurveNoiseLevel = "Medium";

        public const string EllipseNoiseColor = "FFEEEECC";

        public const string EllipseNoiseLevel = "Medium";

        public const string InstrumentationCategory = "Captcha";

        public const bool InstrumentationEnabled = true;
    }
}
