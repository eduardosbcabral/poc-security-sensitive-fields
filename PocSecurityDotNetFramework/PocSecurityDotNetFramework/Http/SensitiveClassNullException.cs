using System;

namespace PocSecurityDotNetFramework.Http
{
    public class SensitiveClassNullException : Exception
    {
        public SensitiveClassNullException(string message)
            : base(message)
        {

        }
    }
}