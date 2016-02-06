using ReusableLibrary.Memcached.Tests.Infrastructure;
using Xunit;

namespace ReusableLibrary.Memcached.Tests
{
    public sealed class TextCacheClientTest : AbstractCacheClientTest, IUseFixture<CacheClientContext>
    {
        #region IUseFixture<CacheClientContext> Members

        public void SetFixture(CacheClientContext context)
        {
            Cache = context.CacheClientText();
        }

        #endregion
    }
}