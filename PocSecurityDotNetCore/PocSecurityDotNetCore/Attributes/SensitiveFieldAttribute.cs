using System;

namespace PocSecurityDotNetCore.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SensitiveFieldAttribute : Attribute
    {
        public SensitiveFieldAttribute()
        {
            
        }
    }
}
