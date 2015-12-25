using System;

namespace ReusableLibrary.HistoryLog.Models
{
    [Serializable]
    public sealed class HistoryLogReport
    {
        public DateTime Timestamp { get; set; }

        public int Count { get; set; }
    }
}
