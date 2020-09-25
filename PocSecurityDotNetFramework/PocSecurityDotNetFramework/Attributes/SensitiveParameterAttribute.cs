using System;

namespace PocSecurityDotNetFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class SensitiveParameterAttribute : Attribute
    {
        public SensitiveParameterAttribute()
        {

        }
    }
}
