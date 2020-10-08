using Microsoft.Extensions.DependencyInjection;

namespace PocSecurityDotNetCore.Http
{
    public static class IMvcBuilderExtensions
    {
        public static void UseCgdfSecurity(this IMvcBuilder mvcBuilder, string securitySecret)
        {
            SecurityConfiguration.SensitiveSecret = securitySecret;

            mvcBuilder.AddMvcOptions(options =>
            {
                options.Filters.Add(typeof(SensitiveBinderErrorFilter));
            });
        }
    }
}