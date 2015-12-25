using System;

namespace ReusableLibrary.Supplemental.System
{
    public static class Int32Extensions
    {
        public static void Times(this int count, Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            for (int i = 0; i < count; i++)
            {
                action();
            }
        }

        public static void Times(this int count, Action<int> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            for (int i = 0; i < count; i++)
            {
                action(i);
            }
        }
    }
}
