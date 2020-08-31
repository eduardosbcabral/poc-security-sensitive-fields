using System;

namespace PocSecurity.Sensitive
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SensitiveClassAttribute : Attribute
    {
    }
}
