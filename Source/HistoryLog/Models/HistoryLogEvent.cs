using System;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.HistoryLog.Models
{
    [Serializable]
    public sealed class HistoryLogEvent : ValueObject<short>
    {
        public HistoryLogEvent(short key, string displayName, string format)
            : base(key, displayName)
        {
            Format = format;
        }

        public string Format { get; private set; }

        public static readonly HistoryLogEvent None = new HistoryLogEvent(0, "None", "None");
    }
}
