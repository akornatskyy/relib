using System;
using System.Collections.Generic;
using System.IO;
using ReusableLibrary.Memcached.Tests.Infrastructure;
using ReusableLibrary.QualityAssurance.Profiling;
using ReusableLibrary.Supplemental.Collections;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Memcached.Tests
{
    public abstract class CacheClientProfilingTest : AbstractProfilingTest<CacheClientContext>
    {
        public const int SamplesCount = 5000;

        private static readonly IEnumerable<object[]> g_threads = (new[] { 2 }).ToPropertyData();

        private readonly TextWriter m_logger = System.Console.Out;
        private readonly Func<CacheClientContext, CacheClientTest> m_testFactory;

        protected CacheClientProfilingTest(Func<CacheClientContext, CacheClientTest> testFactory)
        {
            m_testFactory = testFactory;
        }

        public static IEnumerable<object[]> Threads
        {
            get
            {
                return g_threads;
            }
        }

        [Theory]
        [PropertyData("Threads")]
        [Trait(Constants.TraitNames.Profiling, "Get")]
        public void GetString(int threads)
        {
            var report = Profile<KeyValuePair<string, string>>(threads,
                TestGetString,
                () => DomainModelFactory.StringSamples,
                SamplesCount);
            
            m_logger.WriteLine(report.ToShortString());
            
            Assert.True(report.Succeed);
        }

        [Theory]
        [PropertyData("Threads")]
        [Trait(Constants.TraitNames.Profiling, "Get")]
        public void GetPerson(int threads)
        {
            var report = Profile<KeyValuePair<string, Person>>(threads,
                TestGetPerson,
                () => DomainModelFactory.PersonSamples,
                SamplesCount);

            m_logger.WriteLine(report.ToShortString());

            Assert.True(report.Succeed);
        }

        [Theory]
        [PropertyData("Threads")]
        [Trait(Constants.TraitNames.Profiling, "Store")]
        public void StoreString(int threads)
        {
            var report = Profile<KeyValuePair<string, string>>(threads,
                TestStoreString,
                () => DomainModelFactory.StringSamples,
                SamplesCount);

            m_logger.WriteLine(report.ToShortString());

            Assert.True(report.Succeed);
        }

        [Theory]
        [PropertyData("Threads")]
        [Trait(Constants.TraitNames.Profiling, "Store")]
        public void StorePerson(int threads)
        {
            var report = Profile<KeyValuePair<string, Person>>(threads,
                TestStorePerson,
                () => DomainModelFactory.PersonSamples,
                SamplesCount);

            m_logger.WriteLine(report.ToShortString());

            Assert.True(report.Succeed);
        }

        [Theory]
        [PropertyData("Threads")]
        [Trait(Constants.TraitNames.Profiling, "Increment")]
        public void Increment(int threads)
        {
            var report = Profile<string>(threads,
                TestIncrement,
                () => DomainModelFactory.IncrementKeys,
                SamplesCount);

            m_logger.WriteLine(report.ToShortString());

            Assert.True(report.Succeed);
        }

        [Theory]
        [PropertyData("Threads")]
        [Trait(Constants.TraitNames.Profiling, "Decrement")]
        public void Decrement(int threads)
        {
            var report = Profile<string>(threads,
                TestDecrement,
                () => DomainModelFactory.DecrementKeys,
                SamplesCount);

            m_logger.WriteLine(report.ToShortString());

            Assert.True(report.Succeed);
        }

        [Theory]
        [PropertyData("Threads")]
        [Trait(Constants.TraitNames.Profiling, "Remove")]
        public void Remove_KeyNotFound(int threads)
        {
            var report = Profile<string>(threads,
                TestDecrement,
                () => DomainModelFactory.RandomKeys(DomainModelFactory.SamplesCount),
                SamplesCount);

            m_logger.WriteLine(report.ToShortString());

            Assert.True(report.Succeed);
        }

        public Action<KeyValuePair<string, string>> TestGetString(CacheClientContext context)
        {
            var test = m_testFactory(context);
            return test.GetString;
        }

        public Action<KeyValuePair<string, Person>> TestGetPerson(CacheClientContext context)
        {
            var test = m_testFactory(context);
            return test.GetPerson;
        }

        public Action<KeyValuePair<string, string>> TestStoreString(CacheClientContext context)
        {
            var test = m_testFactory(context);
            return test.StoreString;
        }

        public Action<KeyValuePair<string, Person>> TestStorePerson(CacheClientContext context)
        {
            var test = m_testFactory(context);
            return test.StorePerson;
        }

        public Action<string> TestIncrement(CacheClientContext context)
        {
            var test = m_testFactory(context);
            return test.Increment;
        }

        public Action<string> TestDecrement(CacheClientContext context)
        {
            var test = m_testFactory(context);
            return test.Decrement;
        }

        public Action<string> TestRemove_KeyNotFound(CacheClientContext context)
        {
            var test = m_testFactory(context);
            return test.Remove_KeyNotFound;
        }

        public sealed class Text : CacheClientProfilingTest
        {
            public Text()
                : base((CacheClientContext context) => 
                {
                    var test = new CacheClientTest.Text();
                    test.SetFixture(context);
                    return test;
                })
            {
            }
        }

        public sealed class Binary : CacheClientProfilingTest
        {
            public Binary()
                : base((CacheClientContext context) =>
                {
                    var test = new CacheClientTest.Binary();
                    test.SetFixture(context);
                    return test;
                })
            {
            }
        }
    }
}
