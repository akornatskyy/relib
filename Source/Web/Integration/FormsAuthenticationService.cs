using System;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Security;

namespace ReusableLibrary.Web.Integration
{
    public sealed class FormsAuthenticationService : IFormsAuthentication
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            SignIn(userName, createPersistentCookie, null);
        }

        [SecurityPermission(SecurityAction.Demand, ControlPrincipal = true)]
        public void SignIn(string userName, bool createPersistentCookie, string userData)
        {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);

            var context = HttpContext.Current;
            IPrincipal principal;
            if (FormsAuthentication.CookieMode != HttpCookieMode.UseCookies)
            {
                if (!string.IsNullOrEmpty(userData))
                {
                    throw new NotSupportedException("User data available only in UseCookies authentication mode");
                }

                principal = new GenericPrincipal(new GenericIdentity(userName), new string[0]);
            }
            else
            {
                var cookie = context.Response.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie == null)
                {
                    throw new InvalidOperationException(Properties.Resources.FormsAuthenticationServiceFormsCookie);
                }

                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                if (!string.IsNullOrEmpty(userData))
                {
                    ticket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate,
                        ticket.Expiration, ticket.IsPersistent, userData, ticket.CookiePath);
                    cookie.Value = FormsAuthentication.Encrypt(ticket);
                }

                principal = new GenericPrincipal(new FormsIdentity(ticket), new string[0]);
            }

            Thread.CurrentPrincipal = principal;
            context.User = principal;
        }

        public string UserData()
        {
            var identity = Thread.CurrentPrincipal.Identity as FormsIdentity;
            if (identity == null)
            {
                return string.Empty;
            }

            return identity.Ticket.UserData;
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
            Thread.CurrentPrincipal = null;
            HttpContext.Current.User = null;
        }
    }
}
