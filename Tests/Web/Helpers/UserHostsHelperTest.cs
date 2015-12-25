using System.Collections.Specialized;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Supplemental.Collections;
using ReusableLibrary.Web.Helpers;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Web.Tests.Helpers
{
    public sealed class UserHostsHelperTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Helpers, "UserHostsHelper")]
        public static void UserHosts_Empty()
        {
            // Arrange
            var variables = new NameValueCollection();

            // Act
            var result = UserHostsHelper.UserHosts(variables);

            // Assert
            Assert.Equal(0, result.Length);
        }

        [Theory]
        [InlineData("HTTP_X_FORWARDED_FOR")]
        [InlineData("HTTP_X_CLUSTER_CLIENT_IP")]
        [InlineData("REMOTE_ADDR")]
        [Trait(Constants.TraitNames.Helpers, "UserHostsHelper")]
        public static void UserHosts(string header)
        {
            // Arrange
            var variables = new NameValueCollection();
            variables.Add(header, IpNumberHelper.Localhost);

            // Act
            var result = UserHostsHelper.UserHosts(variables);

            // Assert
            Assert.Equal(1, result.Length);
            Assert.Equal(IpNumberHelper.Localhost, result[0]);
        }

        [Theory]
        [InlineData("127.0.0.1, 192.168.20.101")]
        [InlineData("127.0.0.1 192.168.20.101")]
        [Trait(Constants.TraitNames.Helpers, "UserHostsHelper")]
        public static void UserHosts_Multiple(string hosts)
        {
            // Arrange
            var variables = new NameValueCollection();
            variables.Add("HTTP_X_FORWARDED_FOR", hosts);

            // Act
            var result = UserHostsHelper.UserHosts(variables);

            // Assert
            Assert.Equal(2, result.Length);
            result.ForEach(h => Assert.True(hosts.Contains(h)));
        }

        [Theory]
        [InlineData("127.0.0.1 127.0.0.1")]
        [Trait(Constants.TraitNames.Helpers, "UserHostsHelper")]
        public static void UserHosts_Unique(string hosts)
        {
            // Arrange
            var variables = new NameValueCollection();
            variables.Add("HTTP_X_FORWARDED_FOR", hosts);

            // Act
            var result = UserHostsHelper.UserHosts(variables);

            // Assert
            Assert.Equal(1, result.Length);
            Assert.Equal("127.0.0.1", result[0]);
        }
    }
}
