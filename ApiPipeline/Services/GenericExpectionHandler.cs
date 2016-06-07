using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace ApiPipeline.Services
{
    public class GenericExpectionHandler: ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            context.Result = new InternalServerErrorCustomResult(context.Exception, context.Request);
            //base.Handle(context);
        }
    }

    public class InternalServerErrorCustomResult : IHttpActionResult
    {
        private Exception _exception;
        private HttpRequestMessage _request;
        public InternalServerErrorCustomResult(Exception exception, HttpRequestMessage request)
        {
            _exception = exception;
            _request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            response.RequestMessage = _request;
            response.Content = new StringContent("An error has occurred:" + _exception.Message);
            response.ReasonPhrase = "Oh man !!!";

            return response;
        }

 
    }
}