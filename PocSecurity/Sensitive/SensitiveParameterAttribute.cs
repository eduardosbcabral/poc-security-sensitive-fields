using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace PocSecurity.Sensitive
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class SensitiveParameterAttribute : Attribute
    {
        public SensitiveParameterAttribute()
        {

        }
    }
}
