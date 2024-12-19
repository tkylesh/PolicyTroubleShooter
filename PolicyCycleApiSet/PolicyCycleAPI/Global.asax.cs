using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using log4net;
using log4net.Repository.Hierarchy;
using log4net.Appender;

namespace BTS.PolicyCycleAPI
{
  public class WebApiApplication : System.Web.HttpApplication
  {
    protected void Application_Start()
    {
      GlobalConfiguration.Configure(WebApiConfig.Register);

      // Get the Hierarchy object that organizes the loggers
      Hierarchy hier = LogManager.GetLoggerRepository() as Hierarchy;

      if (hier != null)
      {
        var appender = hier.GetAppenders().OfType<AdoNetAppender>().FirstOrDefault();
        appender.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["LoggingConnectionString"].ConnectionString;

        //refresh settings of appender
        appender.ActivateOptions();
      }
      
    }
  }
}
