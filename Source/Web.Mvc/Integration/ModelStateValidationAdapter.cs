using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Web.Mvc.Integration
{
    public sealed class ModelStateValidationAdapter : IValidationState
    {
        private readonly IViewDataAccessor m_viewDataAccessor;

        public ModelStateValidationAdapter(IViewDataAccessor viewDataAccessor)
        {
            m_viewDataAccessor = viewDataAccessor;
        }

        #region IValidationDictionary Members

        public void AddError(string message)
        {
            AddError(null, message);
        }

        public void AddError(string key, string message)
        {
            m_viewDataAccessor.ViewData.ModelState.AddModelError(key ?? "__ERROR__", message);
        }

        public bool IsValid
        {
            get { return m_viewDataAccessor.ViewData.ModelState.IsValid; }
        }

        #endregion
    }
}
