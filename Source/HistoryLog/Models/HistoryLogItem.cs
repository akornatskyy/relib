using System;
using System.Globalization;
using System.Linq;
using System.Resources;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Supplemental.Collections;

namespace ReusableLibrary.HistoryLog.Models
{
    [Serializable]
    public sealed class HistoryLogItem
    {
        public const char Separator = '\t';
        private static readonly char[] Separators = new[] { Separator };

        public string Username { get; set; }

        public short EventId { get; set; }

        public HistoryLogEvent Event { get; set; }

        public DateTime Timestamp { get; set; }

        public int[] Hosts { get; set; }

        public string RelatedTo { get; set; }

        public string Arguments { get; set; }

        public static string Join(string[] arguments)
        {
            if (arguments == null || arguments.Length == 0)
            {
                return null;
            }

            var separator = Separators[0];
            var args = arguments.Translate(a => a.Replace(separator, ' ')).ToArray();
            return string.Join(new string(Separators), args, 0, args.Length);
        }

        public static string[] Split(string arguments)
        {
            if (string.IsNullOrEmpty(arguments))
            {
                return new string[] { };
            }

            return arguments.Split(Separators, StringSplitOptions.None);
        }

        public string Host()
        {
            return IpNumberHelper.ToIpString(Hosts[0]);
        }

        public string Message()
        {
            return Message(null, CultureInfo.CurrentCulture);
        }

        public string Message(ResourceManager resourceManager)
        {
            return Message(resourceManager, CultureInfo.CurrentCulture);
        }

        public string Message(ResourceManager resourceManager, CultureInfo cultureInfo)
        {
            cultureInfo = cultureInfo ?? CultureInfo.InvariantCulture;
            var format = resourceManager != null
                ? resourceManager.GetString(Event.Format, cultureInfo)
                : Event.Format;
            if (string.IsNullOrEmpty(format))
            {
                throw new InvalidOperationException("Unable locate format string for message");
            }

            return string.Format(cultureInfo, format, Split(Arguments));
        }
    }
}
