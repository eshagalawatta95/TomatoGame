using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Owin;

[assembly: OwinStartup(typeof(TomatoGame.Web.Startup))]

namespace TomatoGame.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure cookies for authentication
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/Account/Login") // Customize the login path
            });
            app.UseFacebookAuthentication(
                appId: System.Configuration.ConfigurationManager.AppSettings["owin:AppId"],
                appSecret: System.Configuration.ConfigurationManager.AppSettings["owin:AppSecret"]);
        }
    }
}