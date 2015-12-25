using System.Globalization;
using System.Text;
using ReusableLibrary.Supplemental.Collections;
using ReusableLibrary.Supplemental.System;

namespace ReusableLibrary.HistoryLog.Models
{
    public static class HistoryLogKeyHelper
    {
        public const string Prefix = "relib:hl:";

        public static string ListEvents(string languageCode)
        {
            return string.Concat(Prefix + "l:", languageCode);
        }

        public static string MapEvents(string languageCode)
        {
            return string.Concat(Prefix + "m:", languageCode);
        }

        public static string Dependency(string username)
        {
            return string.IsNullOrEmpty(username)
                ? Prefix + "d"
                : string.Concat(Prefix + "d:", username);
        }

        public static string Specification(HistoryLogSpecification spec)
        {
            var buffer = new StringBuilder(Prefix + "s:");            
            buffer.Append(spec.Username.NullSafe());
            buffer.Append(':');
            buffer.Append(spec.DateRange.From.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture));
            buffer.Append(':');
            buffer.Append(spec.DateRange.To.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture));
            buffer.Append(':');
            
            if (spec.EventId != 0)
            {
                buffer.Append(spec.EventId);
            }

            buffer.Append(':');
            buffer.Append(",".Join(spec.Events.Translate(e => e.ToString(CultureInfo.InvariantCulture))));
            buffer.Append(':');
            buffer.Append(spec.RelatedTo.NullSafe());
            return buffer.ToString();
        }
    }
}
