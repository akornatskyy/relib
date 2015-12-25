using System.Web.Mvc;
using ReusableLibrary.Abstractions.Bootstrapper;

namespace ReusableLibrary.Web.Mvc.Integration
{
    public sealed class RegisterNullToEmptyStringModelBinder : IStartupTask
    {
        public void Execute()
        {
            ModelBinders.Binders.DefaultBinder = new NullToEmptyStringModelBinder();
        }
    }
}
