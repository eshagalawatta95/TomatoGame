using Serilog;
using System;
using System.Web.Mvc;

namespace TomatoGame.Web.Handlers
{
    public class ExceptionHandleFilter : ActionFilterAttribute, IExceptionFilter
    {
        private readonly Serilog.ILogger _logger;

        public ExceptionHandleFilter()
        {
            _logger = new LoggerConfiguration()
                .MinimumLevel.Error()
                .CreateLogger();
        }

        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                Exception ex = filterContext.Exception;
                filterContext.ExceptionHandled = true;
                _logger.Error(ex.ToString());
                filterContext.Result = new ViewResult()
                {
                    ViewName = ex.GetType().ToString(),
                    ViewData = new ViewDataDictionary(filterContext.Controller.ViewData)
                    {
                        Model = new HandleErrorInfo(ex, filterContext.RouteData.Values["controller"].ToString(), filterContext.RouteData.Values["action"].ToString())
                    }
                };
            }
        }
    }
}