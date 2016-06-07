using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using ApiPipeline.Handler;
using ApiPipeline.Services;

namespace ApiPipeline
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.MessageHandlers.Add(new ExecutionTimeLoggerMessageHandler());
            config.MessageHandlers.Add(new MethodOverrideHandler());

            //config.MessageHandlers.Add(new MessageHandler2());
            config.MessageHandlers.Add(new CustomHeaderHandler());
            //config.MessageHandlers.Add(new ApiKeyHandler("12345"));


            config.Services.Replace(typeof(IExceptionHandler), new GenericExpectionHandler());
            config.Services.Add(typeof(IExceptionLogger), new Log4NetExceptionLogger());
            config.Services.Add(typeof(IExceptionLogger), new Log4NetExceptionLogger());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            DelegatingHandler[] handlers = new DelegatingHandler[]
            {
                new ApiKeyHandler("12345")
            };

            var routeHandlers = HttpClientFactory.CreatePipeline(
                new HttpControllerDispatcher(config), handlers);

            config.Routes.MapHttpRoute(
                name: "Route2",
                routeTemplate: "api2/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: null,
                handler: routeHandlers
            );

        }
    }
}
