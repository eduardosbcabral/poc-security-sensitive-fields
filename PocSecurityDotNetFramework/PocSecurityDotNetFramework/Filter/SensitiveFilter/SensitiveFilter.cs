using PocSecurity.Filter.SensitiveFilter;
using PocSecurityDotNetFramework.Sensitive;
using PocSecurityDotNetFramework.Services;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace PocSecurity.Filter
{
    public class SensitiveResultFilter : ActionFilterAttribute
    {
        private const string SECRET = "793f97ea960e45eb8ab72f91f770bcae";

        public override void OnActionExecuted(HttpActionExecutedContext filterContext)
        {
            var obj = (filterContext.Response.Content as ObjectContent);
            if (obj == null)
                return;
            var apiResult = obj.Value;
            if (apiResult == null)
                return;

            SensitiveFilterHelper.EncryptProperties(apiResult, SECRET);
        }

        public override void OnActionExecuting(HttpActionContext context)
        {
            var parameters = context.ActionDescriptor.GetParameters();

            foreach (HttpParameterDescriptor p in parameters)
            {
                var attributes = p.GetCustomAttributes<SensitiveParameterAttribute>();
                if (attributes == null) return;
                if (attributes.Count == 0) return;

                var actionArguments = context.ActionArguments;
                foreach (var arg in actionArguments.ToList())
                {
                    SensitiveFilterHelper.DecryptProperties(arg.Value, p.ParameterName, context, SECRET);
                }
            }
        }
    }
}
