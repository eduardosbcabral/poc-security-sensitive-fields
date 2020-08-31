using Newtonsoft.Json;
using PocSecurity.Filter.SensitiveFilter;
using PocSecurityDotNetFramework.Sensitive;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace PocSecurityDotNetFramework.Filter
{
    public class SensitiveFilter : ActionFilterAttribute
    {
        private const string SECRET = "793f97ea960e45eb8ab72f91f770bcae";

        public override void OnActionExecuted(HttpActionExecutedContext filterContext)
        {
            try
            {
                var obj = (filterContext.Response.Content as ObjectContent);
                if (obj == null)
                    return;
                var apiResult = obj.Value;
                if (apiResult == null)
                    return;

                SensitiveFilterHelper.EncryptProperties(apiResult, SECRET);
            }
            catch (Exception)
            {
                var objMessage = new
                {
                    Message = "Ocorreu um erro no atributo da resposta."
                };

                filterContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(objMessage), Encoding.UTF8, "application/json")
                };
            }
        }

        public override void OnActionExecuting(HttpActionContext context)
        {
            try
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
            catch (Exception)
            {
                var objMessage = new
                {
                    Message = "Ocorreu um erro no parametro enviado."
                };

                context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(objMessage), Encoding.UTF8, "application/json")
                };
            }
        }
    }
}
