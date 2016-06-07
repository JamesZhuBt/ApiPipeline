using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http.Filters;

namespace ApiPipeline.Filter
{
    public class CachingFilterAttribute:ActionFilterAttribute
    {
        public int Maxage { get; set; }

        public CachingFilterAttribute(int maxAge)
        {
            Maxage = maxAge;
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            HttpStatusCode statusCode = actionExecutedContext.Response.StatusCode;
            if (statusCode == HttpStatusCode.OK || statusCode == HttpStatusCode.NotModified)
            {
                var reponse = actionExecutedContext.Response;
                var cacheControl = new CacheControlHeaderValue();
                cacheControl.MaxAge = TimeSpan.FromSeconds(Maxage);
                reponse.Headers.CacheControl = cacheControl;
            }

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}