using System;

namespace PocSecurityDotNetFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SensitiveFieldAttribute : Attribute
    {
        public SensitiveFieldAttribute()
        {
            
        }
    }
}
