using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Converters;
using System.Web.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Web.Http.Cors;
using System.Web.Http.Dispatcher;
//using BTS.ApiVersioning;

namespace BTS.PolicyCycleAPI
{
  public static class WebApiConfig
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public static void Register(HttpConfiguration config)
    {
      log.Debug("Registering Controller versioning");

      // Web API configuration and services
      config.EnableCors();

      // Web API routes
      config.MapHttpAttributeRoutes();

      ////Uncomment if not using route attributes
      //config.Routes.MapHttpRoute(
      //    name: "DefaultApi",
      //    routeTemplate: "api/{controller}/{id}",
      //    defaults: new { id = RouteParameter.Optional });

      //Use custom ControllerSelector
      //config.Services.Replace(typeof(IHttpControllerSelector), new ApiVersioningSelector(config));
      log.Debug("Done registering");

    }
  }
}
