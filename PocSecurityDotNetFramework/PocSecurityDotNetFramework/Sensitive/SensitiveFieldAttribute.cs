using System;

namespace PocSecurityDotNetFramework.Sensitive
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SensitiveFieldAttribute : Attribute
    {
        public SensitiveFieldAttribute()
        {
            
        }
    }
}
