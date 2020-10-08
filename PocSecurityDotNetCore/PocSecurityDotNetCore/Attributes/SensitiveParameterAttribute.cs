using System;

namespace PocSecurityDotNetCore.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class SensitiveParameterAttribute : Attribute
    {
        public SensitiveParameterAttribute()
        {

        }
    }
}
