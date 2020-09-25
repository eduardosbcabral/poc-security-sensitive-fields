using PocSecurityDotNetFramework.Http;
using System.Configuration;
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

            config.UseCgdfSecurity(ConfigurationManager.AppSettings.Get("security_secret"));

            //config.Filters.Add(new SensitiveFilter());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
