using System;
using System.Globalization;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Repository;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Abstractions.Services
{
    public abstract class AbstractService
    {
        protected AbstractService()
        {
        }

        public IValidationService ValidationService { get; set; }

        public IValidationState ValidationState { get; set; }

        public IExceptionHandler ExceptionHandler { get; set; }

        protected static IPrincipal User
        {
            get { return Thread.CurrentPrincipal; }
            [SecurityPermission(SecurityAction.Demand, ControlPrincipal = true)]
            set { Thread.CurrentPrincipal = value; }
        }

        protected static CultureInfo CurrentCulture
        {
            get { return Thread.CurrentThread.CurrentCulture; }
        }

        protected TResult WithValid<TResult>(object validatable, Func2<TResult> func)
        {
            return WithValid(ValidationService.Validate(validatable), func);
        }

        protected bool WithValid(object validatable, Action2 action)
        {
            return WithValid(ValidationService.Validate(validatable), action);
        }

        protected TResult WithValid<TResult>(bool isValid, Func2<TResult> func)
        {
            TResult result = default(TResult);
            if (!isValid)
            {
                return result;
            }

            try
            {
                result = func();
            }
            catch (InvalidOperationException ioex)
            {
                HandleException(null, ioex);
            }
            catch (SecurityException sex)
            {
                HandleException(null, sex);
            }
            catch (UnauthorizedAccessException uaex)
            {
                HandleException(null, uaex);
            }
            catch (RepositoryGuardAreaException rgex)
            {
                HandleException(rgex.Area, rgex);
            }
            catch (RepositoryException rex)
            {
                HandleException(null, rex);
            }

            return result;
        }

        protected bool WithValid(bool isValid, Action2 action)
        {
            return WithValid(isValid, () => { action(); return true; });
        }

        protected virtual void HandleException(string area, Exception ex)
        {
            if (ExceptionHandler != null)
            {
                ExceptionHandler.HandleException(ex);
            }

            ValidationState.AddError(area, ex.Message);
        }
    }
}
