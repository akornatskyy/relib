using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using ReusableLibrary.Web.Mvc.Helpers;

namespace ReusableLibrary.Web.Mvc
{
    public abstract class AbstractController : Controller
    {
        internal const string AlternatePartialViewName = "__AlternatePartialViewName__";

        protected AbstractController()
        {
        }

        protected static RedirectToRouteResult Ajax(RedirectToRouteResult result)
        {
            return new AjaxRedirectToRouteResult(result);
        }

        protected static RedirectResult Ajax(RedirectResult result)
        {
            return new AjaxRedirectResult(result);
        }

        protected ViewResultBase AlternatePartialView(IPartialViewNameProvider viewNameProvider)
        {
            return AlternatePartialView(null, viewNameProvider);
        }

        protected ViewResultBase AlternatePartialView(string viewName, IPartialViewNameProvider viewNameProvider)
        {
            if (viewNameProvider == null)
            {
                throw new ArgumentNullException("viewNameProvider");
            }

            return AlternatePartialView(viewName, viewNameProvider.PartialViewName, viewNameProvider);
        }

        protected ViewResultBase AlternatePartialView(string partialViewName)
        {
            return AlternatePartialView(null, partialViewName, null);
        }

        protected ViewResultBase AlternatePartialView(string viewName, string partialViewName)
        {
            return AlternatePartialView(viewName, partialViewName, null);
        }

        protected ViewResultBase AlternatePartialView(string partialViewName, object viewData)
        {
            return AlternatePartialView(null, partialViewName, viewData);
        }

        protected ViewResultBase AlternatePartialView(string viewName, string partialViewName, object viewData)
        {
            if (String.IsNullOrEmpty(partialViewName))
            {
                throw new ArgumentNullException("partialViewName");
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView(partialViewName, viewData);
            }

            ViewData[AlternatePartialViewName] = partialViewName;
            return viewName == null ? View(viewData) : View(viewName, viewData);
        }

        protected override void HandleUnknownAction(string actionName)
        {
            throw new HttpException(404, "Action not found");
        }

        protected ActionResult HandleAjaxError()
        {
            foreach (var ms in ModelState.Values)
            {
                if (ms.Errors.Count > 0)
                {
                    var error = ms.Errors[0];
                    return Content(!string.IsNullOrEmpty(error.ErrorMessage)
                        ? error.ErrorMessage
                        : error.Exception.Message);
                }
            }

            return Content(GlobalResourceHelper.ErrorUnspecified());
        }

        protected bool TryValidateAntiForgeryToken(params string[] salts)
        {
            try
            {
                ValidateAntiForgeryToken(salts);
                return true;
            }
            catch (HttpAntiForgeryException ex)
            {
                ViewData.ModelState.AddModelError("__ERROR__", ex.Message);
                return false;
            }
        }

        protected void ValidateAntiForgeryToken(params string[] salts)
        {
            var validator = new ValidateAntiForgeryTokenAttribute()
            {
                Salt = string.Join(AntiForgeryTokenHelper.Separator, salts)
            };
            validator.OnAuthorization(new AuthorizationContext(ControllerContext, new DummyActionDescriptor()));
        }

        private class DummyActionDescriptor : ActionDescriptor
        {
            public override string ActionName
            {
                get { throw new NotImplementedException(); }
            }

            public override ControllerDescriptor ControllerDescriptor
            {
                get { throw new NotImplementedException(); }
            }

            public override object Execute(ControllerContext controllerContext, IDictionary<string, object> parameters)
            {
                throw new NotImplementedException();
            }

            public override ParameterDescriptor[] GetParameters()
            {
                throw new NotImplementedException();
            }
        }
    }
}
