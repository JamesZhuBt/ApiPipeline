using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Http.ExceptionHandling;
using ApiPipeline.Handler;
using log4net;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http;

namespace ApiPipeline.Services
{
    public class Log4NetExceptionLogger : ExceptionLogger
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ExecutionTimeLoggerMessageHandler));
        public override void Log(ExceptionLoggerContext context)
        {

            //ILog log = LogManager.GetLogger(context.ExceptionContext.ControllerContext.Controller.GetType());
            log.ErrorFormat("Unhandled exception processing {0} for {1}: {2}",
                context.Request.Method,
                context.Request.RequestUri,
                context.Exception);

            //base.Log(context);

        }
    }
}