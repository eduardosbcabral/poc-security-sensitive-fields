using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using PocSecurity.Filter.SensitiveFilter;
using PocSecurity.Sensitive;
using PocSecurity.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PocSecurity.Filter
{
    public class SensitiveRequestFilter : IActionFilter
    {
        private readonly ICipherService _cipherService;

        public SensitiveRequestFilter(ICipherService cipherService)
        {
            _cipherService = cipherService;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var parameters = context.ActionDescriptor.Parameters;

            foreach (ControllerParameterDescriptor p in parameters)
            {
                var attributes = p.ParameterInfo.CustomAttributes;
                var sensitiveParameter = attributes.Where(a => a.AttributeType == typeof(SensitiveParameterAttribute)).FirstOrDefault();
                if (sensitiveParameter != null)
                {
                    var actionArguments = context.ActionArguments;
                    foreach (var arg in actionArguments.ToList())
                    {
                        SensitiveFilterHelper.DecryptProperties(arg.Value, p, context, _cipherService);
                    }
                }
            }
        }
    }
}
