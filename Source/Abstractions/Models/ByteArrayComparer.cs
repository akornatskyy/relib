using System.Collections.Generic;

namespace ReusableLibrary.Abstractions.Models
{
    public class ByteArrayComparer : IComparer<byte[]>
    {
        public static readonly ByteArrayComparer Default = new ByteArrayComparer();

        #region IComparer<byte[]> Members

        public int Compare(byte[] x, byte[] y)
        {
            if (x == y)
            {
                return 0;
            }

            if (x == null)
            {
                return -1;
            }

            if (y == null)
            {
                return 1;
            }

            int result = x.Length.CompareTo(y.Length);
            for (int i = 0; i < x.Length && i < y.Length && result == 0; i++)
            {
                result = x[i].CompareTo(y[i]);
            }

            return result;
        }

        #endregion
    }
}
