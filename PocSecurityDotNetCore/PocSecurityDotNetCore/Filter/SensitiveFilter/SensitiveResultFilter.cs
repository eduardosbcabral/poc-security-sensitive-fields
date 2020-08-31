using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PocSecurity.Filter.SensitiveFilter;
using PocSecurity.Sensitive;
using PocSecurity.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PocSecurity.Filter
{
    public class SensitiveResultFilter : IResultFilter
    {
        private readonly ICipherService _cipherService;

        private const string SECRET = "aee6f218d6ab4cd595846fa172b360a0";
        
        public SensitiveResultFilter(ICipherService cipherService)
        {
            _cipherService = cipherService;
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            OkObjectResult okResult = (OkObjectResult)context.Result;

            if (okResult == null) return;

            var resultValue = okResult.Value;

            SensitiveFilterHelper.EncryptProperties(resultValue, _cipherService, SECRET);
        }
    }
}
