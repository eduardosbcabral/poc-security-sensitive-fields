using System;

namespace PocSecurity.Sensitive
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SensitiveFieldAttribute : Attribute
    {
        public SensitiveFieldAttribute()
        {
            
        }
    }
}
