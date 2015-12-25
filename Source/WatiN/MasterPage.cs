using System;
using WatiN.Core;

namespace ReusableLibrary.WatiN
{
    public class MasterPage : Page, IElementContainerProvider
    {
        protected override void InitializeContents()
        {
            base.InitializeContents();

            if (Container == null || !((Element)Container).Exists)
            {
                throw new InvalidOperationException("Container does not exist");
            }
        }

        #region IElementContainerProvider Members

        public IElementContainer Container { get; protected set; }

        #endregion
    }
}
