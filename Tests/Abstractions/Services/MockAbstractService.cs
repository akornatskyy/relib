using System.Security.Principal;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Services;

namespace ReusableLibrary.Abstractions.Tests.Services
{
    internal sealed class MockAbstractService : AbstractService
    {
        public TResult WithValid2<TResult>(object validatable, Func2<TResult> func)
        {
            return WithValid<TResult>(validatable, func);
        }

        public TResult WithValid2<TResult>(bool isValid, Func2<TResult> func)
        {
            return WithValid<TResult>(isValid, func);
        }

        public bool WithValid2(object validatable, Action2 action)
        {
            return WithValid(validatable, action);
        }

        public bool WithValid2(bool isValid, Action2 action)
        {
            return WithValid(isValid, action);
        }
    }
}
