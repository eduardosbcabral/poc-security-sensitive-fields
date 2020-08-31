using System;

namespace PocSecurityDotNetFramework.Sensitive
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class SensitiveParameterAttribute : Attribute
    {
        public SensitiveParameterAttribute()
        {

        }
    }
}
