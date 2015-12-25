using System;

namespace ReusableLibrary.Web.Helpers
{
    public sealed class ShrinkState
    {
        public byte Current { get; set; }

        public bool InsideTag { get; set; }

        public int IgnoreCount { get; set; }
    }
}
