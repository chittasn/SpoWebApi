using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Net;
using System.Configuration;

namespace SpoWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            string corsUrl = ConfigurationManager.AppSettings["CorsUrl"];
   
            config.EnableCors(
                    new EnableCorsAttribute(corsUrl,
                "*", "*"));

            config.MapHttpAttributeRoutes();
        //config.Routes.MapHttpRoute(
        //name: "swagger_root",
        //      routeTemplate: "",
        //      defaults: null,
        //      constraints: null,
        //      handler: new RedirectHandler((message => message.RequestUri.ToString()), "swagger"));

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
