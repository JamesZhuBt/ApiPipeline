using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Tracing;
using ApiPipeline.Controllers;
using log4net;
 

namespace ApiPipeline.Handler
{
    public class ExecutionTimeLoggerMessageHandler : DelegatingHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ExecutionTimeLoggerMessageHandler));
        protected async override Task<HttpResponseMessage> SendAsync( HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Log.Info("Process request");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            // Call the inner handler.
            var response = await base.SendAsync(request, cancellationToken);
            stopwatch.Stop();

            response.Headers.Add("X-ExecutionTime", stopwatch.ElapsedMilliseconds.ToString());
            //request.GetConfiguration().Services.GetTraceWriter().Info(
            //    request, "Timing",
            //    "Ellapsed milliseseconds for request {0} : {1}",
            //    request.RequestUri, stopwatch.ElapsedMilliseconds);
            Log.Info("Process response. Time Elapsed:" + stopwatch.ElapsedMilliseconds.ToString());
            return response;
        }
    }



}