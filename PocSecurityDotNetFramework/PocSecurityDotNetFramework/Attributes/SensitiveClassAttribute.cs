using System;

namespace PocSecurityDotNetFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SensitiveClassAttribute : Attribute
    {
    }
}
