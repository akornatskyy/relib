using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Xunit;

namespace ReusableLibrary.Web.Mvc.Tests
{
    internal sealed class MockAbstractController : AbstractController
    {
        public static RedirectToRouteResult Ajax2(RedirectToRouteResult result)
        {
            return Ajax(result);
        }

        public RedirectToRouteResult RedirectToAction2(string actionName, string controllerName)
        {
            return RedirectToAction(actionName, controllerName);
        }

        public RedirectToRouteResult RedirectToRoute2(string actionName, string controllerName)
        {
            return RedirectToRoute(actionName, controllerName);
        }

        public ViewResultBase AlternatePartialView2(IPartialViewNameProvider viewNameProvider)
        {
            return AlternatePartialView(viewNameProvider);
        }

        public ViewResultBase AlternatePartialView2(string partialViewName, object viewData)
        {
            return AlternatePartialView(partialViewName, viewData);
        }

        public void List()
        {
            Assert.False(this == null);
        }

        public void HandleUnknownAction2(string actionName)
        {
            HandleUnknownAction(actionName);
        }

        public ActionResult HandleAjaxError2()
        {
            return HandleAjaxError();
        }
    }
}
