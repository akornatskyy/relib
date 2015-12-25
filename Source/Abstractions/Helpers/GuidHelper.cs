using System;
using System.Diagnostics;

namespace ReusableLibrary.Abstractions.Helpers
{
    public static class GuidHelper
    {
        [DebuggerStepThrough]
        public static bool IsEmpty(Guid target)
        {
            return target == Guid.Empty;
        }

        [DebuggerStepThrough]
        public static string Shrink(Guid target)
        {
            if (IsEmpty(target))
            {
                throw new ArgumentNullException("target");
            }

            return Convert.ToBase64String(target.ToByteArray())
                .Replace("/", "_").Replace("+", "-")
                .Substring(0, 22);
        }
    }
}
