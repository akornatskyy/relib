using System;
using System.Collections.Generic;
using System.Linq;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.HistoryLog.Models;
using ReusableLibrary.Supplemental.Collections;
using ReusableLibrary.Supplemental.System;

namespace ReusableLibrary.HistoryLog.Tests
{
    public static class DomainModelFactory
    {
        private static readonly Random g_random = new Random(RandomHelper.Seed());
        private static readonly string[] g_users = Properties.Resources.ListUsers.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList().ToArray();
        private static readonly int[] g_hosts = g_random.Shuffle(new[] { "192.168.1.1", "192.168.1.2", "192.168.1.3" }).Translate(s => IpNumberHelper.ToIpNumber(s)).ToArray();

        public static IEnumerable<string> RandomUsers()
        {
            return g_random.Shuffle(g_users);
        }

        public static HistoryLogItem RandomHistoryLogItem()
        {
            return new HistoryLogItem()
            {
                Username = g_random.Next(g_users),
                EventId = g_random.Next(new short[] { 1001, 1002, 1003 }),
                Timestamp = DateTime.UtcNow.AddSeconds(-g_random.Next(100)),
                Hosts = g_random.Next(g_hosts.Take(g_random.Next(1, 3))).ToArray(),
                RelatedTo = g_random.Next(null, string.Empty, "x", "y"),
                Arguments = g_random.Next(null, string.Empty, "a", "b"),
            };
        }
    }
}
