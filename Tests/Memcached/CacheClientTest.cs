using System;
using System.Collections.Generic;
using System.Linq;
using ReusableLibrary.Abstractions.Caching;
using ReusableLibrary.Memcached.Tests.Infrastructure;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Memcached.Tests
{
    public abstract class CacheClientTest
    {
        protected CacheClientTest()
        {
        }

        #region PropertyData: StringSamples, PersonSamples

        public static IEnumerable<object[]> StringSamples
        {
            get
            {
                return DomainModelFactory.StringSamples
                    .Select(s => new object[] { s });
            }
        }

        public static IEnumerable<object[]> PersonSamples
        {
            get
            {
                return DomainModelFactory.PersonSamples
                    .Select(s => new object[] { s });
            }
        }

        public static IEnumerable<object[]> IncrementKeys
        {
            get
            {
                return DomainModelFactory.IncrementKeys
                    .Select(s => new object[] { s });
            }
        }

        public static IEnumerable<object[]> DecrementKeys
        {
            get
            {
                return DomainModelFactory.DecrementKeys
                    .Select(s => new object[] { s });
            }
        }

        public static IEnumerable<object[]> RandomKeys
        {
            get
            {
                return DomainModelFactory.RandomKeys(DomainModelFactory.SamplesCount)
                    .Select(s => new object[] { s }).ToArray();
            }
        }

        #endregion

        [Theory]
        [PropertyData("StringSamples")]
        [Trait(Constants.TraitNames.Integration, "Get")]
        public void GetString(KeyValuePair<string, string> sample)
        {
            TestGet(sample);
        }

        [Theory]
        [PropertyData("PersonSamples")]
        [Trait(Constants.TraitNames.Integration, "Get")]
        public void GetPerson(KeyValuePair<string, Person> sample)
        {
            TestGet(sample);
        }

        [Theory]
        [PropertyData("StringSamples")]
        [Trait(Constants.TraitNames.Integration, "Store")]
        public void StoreString(KeyValuePair<string, string> sample)
        {
            TestStore(sample);
        }

        [Theory]
        [PropertyData("PersonSamples")]
        [Trait(Constants.TraitNames.Integration, "Store")]
        public void StorePerson(KeyValuePair<string, Person> sample)
        {
            TestStore(sample);
        }

        [Fact]
        [Trait(Constants.TraitNames.Integration, "Increment")]
        public void Increment_NoInitial()
        {
            // Arrange
            var initial = 100L;
            var delta = 10L;
            var key = DomainModelFactory.RandomKey();

            // Act
            var result = Cache.Increment(key, delta, initial, TimeSpan.FromSeconds(2));

            // Assert
            Assert.Equal(initial, result);

            // Act
            result = Cache.Increment(key, delta);
            Assert.Equal(initial + delta, result);
        }

        [Theory]
        [PropertyData("IncrementKeys")]
        [Trait(Constants.TraitNames.Integration, "Increment")]
        public void Increment(string key)
        {
            // Arrange

            // Act
            var result = Cache.Increment(key);

            // Assert
            Assert.True(result > 0L);
        }

        [Theory]
        [PropertyData("DecrementKeys")]
        [Trait(Constants.TraitNames.Integration, "Decrement")]
        public void Decrement(string key)
        {
            // Arrange

            // Act
            var result = Cache.Increment(key, -1L);

            // Assert
            Assert.True(result < Int64.MaxValue);
        }

        [Theory]
        [PropertyData("RandomKeys")]
        [Trait(Constants.TraitNames.Integration, "Decrement")]
        public void Remove(string key)
        {
            // Arrange
            Assert.True(Cache.Store(key, 100, TimeSpan.FromSeconds(2)));
            Assert.Equal(100, Cache.Get<int>(key));

            // Act
            var result = Cache.Remove(key);

            // Assert
            Assert.True(result);
            Assert.Equal(0, Cache.Get<int>(key));
        }

        [Theory]
        [PropertyData("RandomKeys")]
        [Trait(Constants.TraitNames.Integration, "Decrement")]
        public void Remove_KeyNotFound(string key)
        {
            // Arrange

            // Act
            var result = Cache.Remove(key);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [PropertyData("RandomKeys")]
        [Trait(Constants.TraitNames.Integration, "Increment_Decrement")]
        public void Increment_Decrement(string key)
        {
            // Arrange
            var results = new long[10];
            Assert.Equal(1000L, Cache.Increment(key, 0L, 1000L, TimeSpan.FromSeconds(5)));

            // Act
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = Cache.Increment(key);
            }

            // Assert
            for (int i = 1; i < results.Length; i++)
            {
                Assert.Equal(1, results[i] - results[i - 1]);
            }

            // Act
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = Cache.Increment(key, -1L);
            }

            // Assert
            for (int i = results.Length - 1; i > 0; i--)
            {
                Assert.Equal(-1, results[i] - results[i - 1]);
            }

            Assert.Equal(1000L, Cache.Increment(key, 0L, 0L));
        }

        public ICache Cache { get; set; }

        private void TestGet<T>(KeyValuePair<string, T> sample)
        {
            // Arrange

            // Act
            var result = Cache.Get<T>(sample.Key);

            // Assert
            Assert.Equal(sample.Value, result);
        }

        private void TestStore<T>(KeyValuePair<string, T> sample)
        {
            // Arrange

            // Act
            var result = Cache.Store<T>(new DataKey<T>(sample.Key) 
            { 
                Value = sample.Value
            });

            // Assert
            Assert.True(result);
        }

        public sealed class Text : CacheClientTest, IUseFixture<CacheClientContext>
        {
            #region IUseFixture<CacheClientContext> Members

            public void SetFixture(CacheClientContext context)
            {
                Cache = context.CacheClientText();
            }

            #endregion
        }

        public sealed class Binary : CacheClientTest, IUseFixture<CacheClientContext>
        {
            #region IUseFixture<CacheClientContext> Members

            public void SetFixture(CacheClientContext context)
            {
                Cache = context.CacheClientBinary();
            }

            #endregion
        }
    }
}
