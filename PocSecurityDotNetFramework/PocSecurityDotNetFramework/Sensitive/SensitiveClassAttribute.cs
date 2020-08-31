using System;

namespace PocSecurityDotNetFramework.Sensitive
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SensitiveClassAttribute : Attribute
    {
    }
}
