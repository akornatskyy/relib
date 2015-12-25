using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ReusableLibrary.Abstractions.Caching;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Memcached.Protocol;
using ReusableLibrary.Supplemental.Collections;
using ReusableLibrary.Supplemental.System;
using Xunit;

namespace ReusableLibrary.Memcached.Tests.Infrastructure
{
    public static class DomainModelFactory
    {
        public const int SamplesCount = 100;

        private static readonly TimeSpan g_validFor = TimeSpan.FromMinutes(15);
        private static readonly Random g_random = new Random(RandomHelper.Seed());

        public static KeyValuePair<string, string>[] StringSamples { get; private set; }

        public static KeyValuePair<string, Person>[] PersonSamples { get; private set; }

        public static string[] IncrementKeys { get; private set; }

        public static string[] DecrementKeys { get; private set; }

        static DomainModelFactory()
        {
            StringSamples = RandomSamples<string>(SamplesCount, RandomString).ToArray();
            PersonSamples = RandomSamples<Person>(SamplesCount, RandomPerson).ToArray();
            IncrementKeys = RandomKeys(SamplesCount).ToArray();
            DecrementKeys = RandomKeys(SamplesCount).ToArray();

            using (var context = new CacheClientContext())
            {
                var client = context.CacheClientText("No Reply = true");
                IncrementKeys.ForEach(key => Assert.True(client.Store(key, "0")));
                DecrementKeys.ForEach(key => Assert.True(client.Store(key, "1000000")));
                EnsureSamples(client, StringSamples);
                EnsureSamples(client, PersonSamples);
            }

            using (var context = new CacheClientContext())
            {
                var client = context.CacheClientBinary("No Reply = true");
                IncrementKeys.ForEach(key => Assert.Equal(0L, client.Increment(key, 0L, 0L)));
                DecrementKeys.ForEach(key => Assert.Equal(1000000L, client.Increment(key, 0L, 1000000L)));
                EnsureSamples(client, StringSamples);
                EnsureSamples(client, PersonSamples);
            }
        }

        public static void EnsureSamples<T>(ICache client, IEnumerable<KeyValuePair<string, T>> samples)
        {
            foreach (var sample in samples)
            {
                var succeed = client.Store(new DataKey<T>(sample.Key)
                {
                    Value = sample.Value
                }, g_validFor);

                Assert.True(succeed);
            }
        }

        public static IEnumerable<KeyValuePair<string, T>> RandomSamples<T>(int count, Func<T> factory)
        {
            return g_random.NextSequence(count, count,
                i => new KeyValuePair<string, T>(RandomKey(), factory()));
        }

        public static IEnumerable<string> RandomKeys(int count)
        {
            return g_random.NextSequence(count, count, i => RandomKey());
        }

        public static string RandomKey()
        {
            return string.Format(CultureInfo.InvariantCulture,
                "{0}={1}:{2}={3}",
                g_random.NextWord(), g_random.NextInt(1, 999),
                g_random.NextWord(), g_random.NextInt(1, 999));
        }

        public static Person RandomPerson()
        {
            return new Person()
            {
                Age = g_random.NextInt(10, 80),
                FirstName = g_random.NextSentence(g_random.NextInt(1, 4)),
                Id = g_random.NextInt(1, 1000),
                LastName = g_random.NextSentence(g_random.NextInt(1, 4)),
                Name = g_random.NextSentence(g_random.NextInt(1, 2)),
                PostalCode = g_random.NextInt(10000, 99999)
            };
        }

        public static string RandomString()
        {
            return g_random.NextSentence(g_random.NextInt(10, 20));
        }

        public static IEnumerable<object[]> StoreInputSequence()
        {
            foreach (var operation in StoreOperations())
            {
                foreach (var noreply in BooleanStates())
                {
                    foreach (var connectSucceed in BooleanStates())
                    {
                        foreach (var storeSucceed in BooleanStates())
                        {
                            yield return new object[] 
                            { 
                                operation, 
                                "No Reply = {0}".FormatWith(noreply), 
                                connectSucceed, 
                                storeSucceed 
                            };
                        }
                    }
                }
            }
        }

        public static IEnumerable<object[]> DeleteInputSequence()
        {
            foreach (var noreply in DomainModelFactory.BooleanStates())
            {
                foreach (var connectSucceed in DomainModelFactory.BooleanStates())
                {
                    foreach (var deleteSucceed in DomainModelFactory.BooleanStates())
                    {
                        yield return new object[] 
                        { 
                            "No Reply = {0}".FormatWith(noreply), 
                            connectSucceed, 
                            deleteSucceed 
                        };
                    }
                }
            }
        }

        private static StoreOperation[] StoreOperations()
        {
            return new[] { StoreOperation.Add, StoreOperation.CheckAndSet, StoreOperation.Replace, StoreOperation.Set };
        }

        private static bool[] BooleanStates()
        {
            return new[] { true, false };
        }
    }
}
