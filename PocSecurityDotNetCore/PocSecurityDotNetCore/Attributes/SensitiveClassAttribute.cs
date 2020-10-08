using System;

namespace PocSecurityDotNetCore.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SensitiveClassAttribute : Attribute
    {
    }
}
