using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TomatoGame.Service.Services;
using TomatoGame.Storage;

namespace TomatoGame.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Configure Autofac
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<AuthService>().As<IAuthService>().InstancePerRequest();
            builder.RegisterType<GameService>().As<IGameService>().InstancePerRequest();
            builder.RegisterType<GameDbContext>().AsSelf().InstancePerRequest(); // Register GameDbContext
                                                                                 // Register other services and dependencies here

            var container = builder.Build();

            // Set Autofac as the dependency resolver for MVC
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }

}
