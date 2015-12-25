using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using ReusableLibrary.Abstractions.Caching;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.QualityAssurance.Profiling;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Net
{
    public sealed class DistributedProfilingTest : AbstractProfilingTest<DistributedProfilingContext>
    {
        private static readonly Random g_random = new Random(RandomHelper.Seed());
        private static readonly Encoding g_encoding = Encoding.UTF8;

        private readonly TextWriter m_logger = System.Console.Out;

        [Theory]
        [InlineData(2)]
        [Trait(Constants.TraitNames.Net, "Distributed")]
        public void ProfileTest(int threads)
        {
            var report = Profile<DataKey<string>>(threads, Test, Payload, 5000);

            m_logger.WriteLine(report.ToShortString());

            Assert.Equal(0, report.FailedRequests);
        }

        private static IEnumerable<DataKey<string>> Payload()
        {
            return RandomHelper.NextSequence(g_random, 100, 100,
                i => new DataKey<string>(i.ToString(CultureInfo.InvariantCulture)));
        }

        private Action<DataKey<string>> Test(DistributedProfilingContext context)
        {
            var distributed = context.Distributed;
            return datakey => distributed.Context(g_encoding.GetBytes(datakey.Key))((connection, state) =>
            {
            });
        }
    }
}
