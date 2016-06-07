using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using ApiPipeline.Handler;
using log4net;

namespace ApiPipeline
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ExecutionTimeLoggerMessageHandler));
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();
            Log.Info("#####################  Start   #####################");
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
