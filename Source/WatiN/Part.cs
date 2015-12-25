using WatiN.Core;

namespace ReusableLibrary.WatiN
{
    public abstract class Part : IElementContainerProvider
    {
        protected Part(IElementContainer container)
        {
            Container = container;
        }

        #region IElementContainerProvider Members

        public virtual IElementContainer Container { get; private set; }

        #endregion
    }
}
