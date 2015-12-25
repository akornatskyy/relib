using System;
using System.Collections.Generic;
using System.IO;
using ReusableLibrary.QualityAssurance.Profiling;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Net
{
    public sealed class ClientProfilingTest : AbstractProfilingTest<ClientProfilingContext>
    {
        private readonly TextWriter m_logger = System.Console.Out;

        [Theory]
        [InlineData(2)]
        [Trait(Constants.TraitNames.Net, "Client")]
        public void ProfileTest(int threads)
        {
            var report = Profile<string>(threads, Test, Payload, 5000);

            m_logger.WriteLine(report.ToShortString());

            Assert.Equal(0, report.FailedRequests);            
        }

        private static IEnumerable<string> Payload()
        {
            return new string[1];
        }

        private Action<string> Test(ClientProfilingContext context)
        {
            var client = context.Client;
            return payload => client.Context(payload)((connection, state) =>
            {
            });
        }
    }
}
