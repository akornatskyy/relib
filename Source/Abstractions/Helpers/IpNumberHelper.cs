using System;
using System.Globalization;

namespace ReusableLibrary.Abstractions.Helpers
{
    public static class IpNumberHelper
    {
        public const int LocalhostIpNumber = 2130706433;

        public const long LocalhostIpLong = 2130706433L;

        public const string Localhost = "127.0.0.1";

        public static int ToIpNumber(string ip)
        {
            if (String.IsNullOrEmpty(ip))
            {
                throw new ArgumentNullException("ip");
            }

            var parts = ip.Split('.');
            if (parts.Length != 4)
            {
                throw new ArgumentException(Properties.Resources.ErrorInvalidIpAddress);
            }

            byte w, x, y, z;
            if (!Byte.TryParse(parts[0], out w)
                || !Byte.TryParse(parts[1], out x)
                || !Byte.TryParse(parts[2], out y)
                || !Byte.TryParse(parts[3], out z))
            {
                throw new ArgumentException(Properties.Resources.ErrorInvalidIpAddress);
            }

            return (w << 24) | (x << 16) | (y << 8) | z;
        }

        public static string ToIpString(int ipnum)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}.{3}",
                (ipnum >> 24) & 255,
                (ipnum >> 16) & 255,
                (ipnum >> 8) & 255,
                ipnum & 255);
        }

        public static long ToIpLong(int ipnum)
        {
            return ipnum >= 0 ? ipnum : 4294967295L & ipnum;
        }

        public static long ToIpLong(string ip)
        {
            return ToIpLong(ToIpNumber(ip));
        }

        public static int Netmask(int cidr)
        {
            if (cidr < 0 || cidr > 32)
            {
                throw new ArgumentOutOfRangeException("cidr");
            }

            return cidr == 0 ? 0 : -1 << (32 - cidr);
        }

        public static int Cidr(int netmask)
        {
            var cidr = 0;
            var mask = (uint)netmask;
            while ((mask & 0x80000000) == 0x80000000)
            {
                mask = mask << 1;
                cidr++;
            }

            if (mask != 0 || cidr > 32)
            {
                throw new ArgumentException("mask");
            }

            return cidr;
        }

        public static int Network(int ipnum, int mask)
        {
            return ipnum & mask;
        }

        public static int Broadcast(int network, int mask)
        {
            return network + ~mask;
        }

        public static bool Contains(int network, int mask, int ipnum)
        {
            return ipnum >= network && ipnum <= Broadcast(network, mask);
        }
    }
}
