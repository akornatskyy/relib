using System.Web.Mvc;
using ReusableLibrary.Abstractions.Bootstrapper;

namespace ReusableLibrary.Web.Mvc.Integration
{
    public sealed class RegisterTrimStringModelBinder : IStartupTask
    {
        public void Execute()
        {
            ModelBinders.Binders.DefaultBinder = new TrimStringModelBinder();
        }
    }
}
