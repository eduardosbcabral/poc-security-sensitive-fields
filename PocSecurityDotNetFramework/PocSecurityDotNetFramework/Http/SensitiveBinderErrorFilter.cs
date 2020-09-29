using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace PocSecurityDotNetFramework.Http
{
    public class SensitiveBinderErrorFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ModelState.Any(x => x.Key == "SensitiveBinderError"))
            {
                var objMessage = new
                {
                    Message = "Ocorreu um erro no parametro enviado.",
                    Error = actionContext.ModelState
                        .Where(x => x.Key == "SensitiveBinderError")
                        .Select(x => x.Value)
                        .SelectMany(x => x.Errors)
                        .Select(x => x.Exception)
                        .FirstOrDefault()
                };

                actionContext.Response = actionContext.Request.CreateResponse(
                    HttpStatusCode.BadRequest,
                    objMessage);
            }
        }
    }
}