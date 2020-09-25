using System.Web.Http;

namespace PocSecurityDotNetFramework.Http
{
    public static class HttpConfigurationExtensions
    {
        public static void UseCgdfSecurity(this HttpConfiguration config, string securitySecret)
        {
            SecurityConfiguration.SensitiveSecret = securitySecret;
            config.Filters.Add(new SensitiveBinderErrorFilter());
        }
    }
}