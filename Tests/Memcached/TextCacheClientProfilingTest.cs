using ReusableLibrary.Memcached.Tests.Infrastructure;

namespace ReusableLibrary.Memcached.Tests
{
    public sealed class TextCacheClientProfilingTest : AbstractCacheClientProfilingTest
    {
        public TextCacheClientProfilingTest()
            : base((CacheClientContext context) =>
            {
                var test = new TextCacheClientTest();
                test.SetFixture(context);
                return test;
            })
        {
        }
    }
}