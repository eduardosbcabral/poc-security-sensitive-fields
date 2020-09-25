using PocSecurityDotNetFramework.Http;
using System.Web.Http;

namespace PocSecurityDotNetFramework
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            SecurityConfiguration.SensitiveSecret = "AAAAAAAAAAAAAAAAAAAAA";

            //config.Filters.Add(new SensitiveFilter());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
