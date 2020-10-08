using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace PocSecurityDotNetCore.Http
{
    public class SensitiveBinderErrorFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.Any(x => x.Key == "SensitiveBinderError"))
            {
                var objMessage = new
                {
                    Message = "Ocorreu um erro no parametro enviado.",
                    Error = context.ModelState
                        .Where(x => x.Key == "SensitiveBinderError")
                        .Select(x => x.Value)
                        .SelectMany(x => x.Errors)
                        .Select(x => x.ErrorMessage)
                        .FirstOrDefault()
                };

                context.Result = new ObjectResult(objMessage) { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }
    }
}