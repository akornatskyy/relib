using System;
using System.Diagnostics;
using ReusableLibrary.Abstractions.IoC;

namespace ReusableLibrary.Abstractions.Services
{
    public static class UnitOfWork
    {
        [DebuggerStepThrough]
        public static IUnitOfWork Begin(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            return DependencyResolver.Resolve<IUnitOfWork>(name);
        }
    }
}
