using System;

namespace PocSecurityDotNetFramework.Http
{
    public class SensitiveClassNullException : Exception
    {
        public SensitiveClassNullException()
        {

        }

        public SensitiveClassNullException(string message)
            : base(message)
        {

        }
    }
}