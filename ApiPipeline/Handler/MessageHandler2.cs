using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using log4net;

namespace ApiPipeline.Handler
{
    public class MessageHandler2 : DelegatingHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MessageHandler2));
        protected override Task<HttpResponseMessage> SendAsync( HttpRequestMessage request, CancellationToken cancellationToken)
        {



            //Log.Info("Process request");
            //return base.SendAsync(request, cancellationToken);

            // Create the response.
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Hello!")
            };

            response.Headers.Add("ZJ",new []{"a","b"});

            // Note: TaskCompletionSource creates a task that does not contain a delegate.
            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);   // Also sets the task state to "RanToCompletion"

            Log.Info("Process response");

            return tsc.Task;
        }
    }


    public class MethodOverrideHandler : DelegatingHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MethodOverrideHandler));
        readonly string[] _methods = { "DELETE", "HEAD", "PUT" };
        const string _header = "X-HTTP-Method-Override";

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Log.Info(" ");
            Log.Info("-------------New request-----------------");
            // Check for HTTP POST with the X-HTTP-Method-Override header.
            if (request.Method == HttpMethod.Post && request.Headers.Contains(_header))
            {
                // Check if the header value is in our methods list.
                var method = request.Headers.GetValues(_header).FirstOrDefault();
                if (_methods.Contains(method, StringComparer.InvariantCultureIgnoreCase))
                {
                    // Change the request method.
                    request.Method = new HttpMethod(method);
                }
            }
            return base.SendAsync(request, cancellationToken);
        }
    }



    public class CustomHeaderHandler : DelegatingHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CustomHeaderHandler));
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Log.Info("Process request");
            return base.SendAsync(request, cancellationToken).ContinueWith(
                (task) =>
                {
                    Log.Info("Process response");
                    HttpResponseMessage response = task.Result;
                    response.Headers.Add("X-Custom-Header", "This is my custom header.");
                    return response;
                }
            );
        }
    }



    public class ApiKeyHandler : DelegatingHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ApiKeyHandler));
        public string Key { get; set; }

        public ApiKeyHandler(string key)
        {
            this.Key = key;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!ValidateKey(request))
            {
                var response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                var tsc = new TaskCompletionSource<HttpResponseMessage>();
                tsc.SetResult(response);
                return tsc.Task;
            }
            return base.SendAsync(request, cancellationToken);
        }

        private bool ValidateKey(HttpRequestMessage message)
        {
            var query = message.RequestUri.ParseQueryString();
            string key = query["key"];
            return (key == Key);
        }
    }
}