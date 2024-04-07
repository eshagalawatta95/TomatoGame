using System.Web.Mvc;

namespace TomatoGame.Web.Attributes
{
    public class SessionAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string _sessionVariableName;

        public SessionAuthorizeAttribute(string sessionVariableName)
        {
            _sessionVariableName = sessionVariableName;
        }

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            // Check if the session variable exists and has a valid value
            var sessionValue = httpContext.Session[_sessionVariableName] as string;
            return !string.IsNullOrEmpty(sessionValue);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // Redirect to login page or any other action if authorization fails
            filterContext.Result = new RedirectResult("/Home/Index");
        }
    }
}