using System;
using System.Collections.Generic;
using System.Globalization;
using ReusableLibrary.Abstractions.Helpers;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Helpers
{
    public static class IpNumberHelperTest
    {
        private static readonly Random g_random = new Random();

        public static IEnumerable<object[]> RandomIpSequence
        {
            get
            {
                return EnumerableHelper.Translate(
                    RandomHelper.NextSequence(g_random,
                    i => string.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}.{3}",
                        RandomHelper.NextInt(g_random, 0, 255),
                        RandomHelper.NextInt(g_random, 0, 255),
                        RandomHelper.NextInt(g_random, 0, 255),
                        RandomHelper.NextInt(g_random, 0, 255))),
                    ip => new object[] { ip });
            }
        }

        [Theory]
        [Trait(Constants.TraitNames.Helpers, "IpNumberHelper")]
        [InlineData("261.88.0.0")]
        [InlineData("61.288.0.0")]
        [InlineData("61.88.300.0")]
        [InlineData("61.88.0.330")]
        [InlineData("61.88.0")]
        [InlineData("61.88.0.0.0")]
        public static void ToIpNumber_ArgumentException(string ip)
        {
            // Arrange

            // Act
            Assert.Throws<ArgumentException>(() => IpNumberHelper.ToIpNumber(ip));

            // Assert            
        }

        [Theory]
        [Trait(Constants.TraitNames.Helpers, "IpNumberHelper")]
        [InlineData(1029177344, "61.88.0.0")]
        [InlineData(1029439487, "61.91.255.255")]
        [InlineData(-631405769, "218.93.131.55")]
        [InlineData(-563992514, "222.98.40.62")]
        public static void ToIpNumber(int expected, string ip)
        {
            // Arrange

            // Act
            var result = IpNumberHelper.ToIpNumber(ip);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [Trait(Constants.TraitNames.Helpers, "IpNumberHelper")]
        [InlineData("61.88.0.0", 1029177344)]
        [InlineData("61.91.255.255", 1029439487)]
        [InlineData("218.93.131.55", -631405769)]
        [InlineData("222.98.40.62", -563992514)]
        public static void ToIpString(string expected, int ipnum)
        {
            // Arrange

            // Act
            var result = IpNumberHelper.ToIpString(ipnum);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [Trait(Constants.TraitNames.Helpers, "IpNumberHelper")]
        [InlineData(1029177344, "61.88.0.0")]
        [InlineData(1029439487, "61.91.255.255")]
        [InlineData(3663561527, "218.93.131.55")]
        [InlineData(3730974782, "222.98.40.62")]
        public static void ToIpLongFromString(long expected, string ip)
        {
            // Arrange

            // Act
            var result = IpNumberHelper.ToIpLong(IpNumberHelper.ToIpNumber(ip));

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [Trait(Constants.TraitNames.Helpers, "IpNumberHelper")]
        [InlineData(1029177344, 1029177344)]
        [InlineData(1029439487, 1029439487)]
        [InlineData(3663561527, -631405769)]
        [InlineData(3730974782, -563992514)]
        public static void ToIpLongFromInt(long expected, int ip)
        {
            // Arrange

            // Act
            var result = IpNumberHelper.ToIpLong(ip);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [Trait(Constants.TraitNames.Helpers, "IpNumberHelper")]
        [PropertyData("RandomIpSequence")]
        public static void Random(string ip)
        {
            // Arrange            

            // Act
            var ipnum = IpNumberHelper.ToIpNumber(ip);
            var result = IpNumberHelper.ToIpString(ipnum);

            // Assert
            Assert.Equal(ip, result);
        }

        [Theory]
        [Trait(Constants.TraitNames.Helpers, "IpNumberHelper")]
        [InlineData("255.255.255.255", 32)]
        [InlineData("255.255.255.254", 31)]
        [InlineData("255.255.255.252", 30)]
        [InlineData("255.255.255.0", 24)]
        [InlineData("255.255.0.0", 16)]
        [InlineData("252.0.0.0", 6)]
        [InlineData("128.0.0.0", 1)]
        [InlineData("0.0.0.0", 0)]
        public static void Netmask(string expected, int cidr)
        {
            // Arrange

            // Act
            var result = IpNumberHelper.Netmask(cidr);

            // Assert
            Assert.Equal(expected, IpNumberHelper.ToIpString(result));
        }

        [Theory]
        [Trait(Constants.TraitNames.Helpers, "IpNumberHelper")]
        [InlineData(33)]
        [InlineData(-1)]
        public static void Netmask_CIDR_OutOfRange(int cidr)
        {
            // Arrange

            // Act
            Assert.Throws<ArgumentOutOfRangeException>(() => IpNumberHelper.Netmask(cidr));

            // Assert
        }

        [Theory]
        [Trait(Constants.TraitNames.Helpers, "IpNumberHelper")]
        [InlineData(32, "255.255.255.255")]
        [InlineData(31, "255.255.255.254")]
        [InlineData(30, "255.255.255.252")]
        [InlineData(24, "255.255.255.0")]
        [InlineData(16, "255.255.0.0")]
        [InlineData(6, "252.0.0.0")]
        [InlineData(1, "128.0.0.0")]
        [InlineData(0, "0.0.0.0")]
        public static void Cidr(int expected, string netmask)
        {
            // Arrange

            // Act
            var result = IpNumberHelper.Cidr(IpNumberHelper.ToIpNumber(netmask));

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [Trait(Constants.TraitNames.Helpers, "IpNumberHelper")]
        [InlineData("255.2.255.255")]
        [InlineData("255.255.2.0")]
        [InlineData("2.255.255.252")]
        public static void Cidr_Invalid(string netmask)
        {
            // Arrange

            // Act
            Assert.Throws<ArgumentException>(() => IpNumberHelper.Cidr(IpNumberHelper.ToIpNumber(netmask)));

            // Assert
        }

        [Theory]
        [Trait(Constants.TraitNames.Helpers, "IpNumberHelper")]
        [InlineData("192.168.20.15", "192.168.20.15", 32)]
        [InlineData("192.168.20.0", "192.168.20.15", 24)]
        [InlineData("219.201.128.0", "219.201.128.2", 19)]
        [InlineData("30.6.57.64", "30.6.57.66", 29)]
        public static void Network(string expected, string ip, int cidr)
        {
            // Arrange
            var ipnum = IpNumberHelper.ToIpNumber(ip);
            var mask = IpNumberHelper.Netmask(cidr);

            // Act
            var result = IpNumberHelper.Network(ipnum, mask);

            // Assert
            Assert.Equal(expected, IpNumberHelper.ToIpString(result));
        }

        [Theory]
        [Trait(Constants.TraitNames.Helpers, "IpNumberHelper")]
        [InlineData("192.168.20.15", "192.168.20.15", 32)]
        [InlineData("192.168.20.255", "192.168.20.15", 24)]
        [InlineData("219.201.159.255", "219.201.128.2", 19)]
        [InlineData("30.6.57.71", "30.6.57.66", 29)]
        public static void Broadcast(string expected, string ip, int cidr)
        {
            // Arrange
            var mask = IpNumberHelper.Netmask(cidr);
            var ipnum = IpNumberHelper.Network(IpNumberHelper.ToIpNumber(ip), mask);

            // Act
            var result = IpNumberHelper.Broadcast(ipnum, mask);

            // Assert
            Assert.Equal(expected, IpNumberHelper.ToIpString(result));
        }

        [Theory]
        [Trait(Constants.TraitNames.Helpers, "IpNumberHelper")]
        [InlineData(true, "192.168.20.0", 24, "192.168.20.15")]
        [InlineData(false, "192.168.20.0", 24, "192.168.19.255")]
        [InlineData(false, "192.168.20.0", 24, "192.168.21.1")]
        [InlineData(true, "219.201.128.0", 19, "219.201.158.122")]
        [InlineData(false, "219.201.128.0", 19, "219.201.127.254")]
        [InlineData(false, "219.201.128.0", 19, "219.201.160.1")]
        [InlineData(true, "30.6.57.64", 29, "30.6.57.70")]
        [InlineData(false, "30.6.57.64", 29, "30.6.57.63")]
        [InlineData(false, "30.6.57.64", 29, "30.6.57.72")]
        public static void Contains(bool expected, string net, int cidr, string ip)
        {
            // Arrange
            var network = IpNumberHelper.ToIpNumber(net);
            var mask = IpNumberHelper.Netmask(cidr);
            var ipnum = IpNumberHelper.ToIpNumber(ip);

            // Act
            var result = IpNumberHelper.Contains(network, mask, ipnum);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
