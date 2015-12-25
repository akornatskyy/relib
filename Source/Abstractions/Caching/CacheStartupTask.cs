using ReusableLibrary.Abstractions.Bootstrapper;
using ReusableLibrary.Abstractions.IoC;

namespace ReusableLibrary.Abstractions.Caching
{
    public sealed class CacheStartupTask : IStartupTask
    {
        #region IStartupTask Members

        public void Execute()
        {
            DefaultCache.InitializeWith(DependencyResolver.Resolve<ICache>());
        }

        #endregion
    }
}
