using ReusableLibrary.Memcached.Tests.Infrastructure;
using Xunit;

namespace ReusableLibrary.Memcached.Tests
{
    public sealed class BinaryCacheClientTest : AbstractCacheClientTest, IUseFixture<CacheClientContext>
    {
        #region IUseFixture<CacheClientContext> Members

        public void SetFixture(CacheClientContext context)
        {
            Cache = context.CacheClientBinary();
        }

        #endregion
    }
}