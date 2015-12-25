using System.Web.Mvc;

namespace ReusableLibrary.Web.Mvc.Integration
{
    public sealed class ViewDataAccessor : IViewDataAccessor
    {
        public ViewDataAccessor()
        {
            ViewData = new ViewDataDictionary();
        }

        #region IModelStateAccessor Members

        public ViewDataDictionary ViewData
        {
            get;
            private set;
        }

        #endregion

        public void Setup(ControllerBase value)
        {
            ViewData = value.ViewData;
        }
    }
}
