using ReusableLibrary.Memcached.Tests.Infrastructure;

namespace ReusableLibrary.Memcached.Tests
{
    public sealed class BinaryCacheClientProfilingTest : AbstractCacheClientProfilingTest
    {
        public BinaryCacheClientProfilingTest()
            : base((CacheClientContext context) =>
            {
                var test = new BinaryCacheClientTest();
                test.SetFixture(context);
                return test;
            })
        {
        }
    }
}