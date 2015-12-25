namespace ReusableLibrary.Supplemental.System
{
    public static class StructExtensions
    {
        public static T? AsNullable<T>(this T value)
            where T : struct
        {
            /// if (Convert.ToInt64(value, CultureInfo.InvariantCulture) == 0L)
            /// This is the case for decimal round to 0 in case of anything beween > 0.0 and < 0.5
            if (default(T).Equals(value))
            {
                return new T?();
            }

            return new T?(value);
        }
    }
}
