using System;

namespace PocSecurityDotNetCore.Http
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