using System;

namespace PocSecurityDotNetFramework.Http
{
    public class SecurityConfiguration
    {
        private static string _sensitiveSecret;
        public static string SensitiveSecret
        {
            get
            {
                if (string.IsNullOrEmpty(_sensitiveSecret))
                {
                    throw new ArgumentNullException(nameof(SensitiveSecret), $"Defina o valor do atributo ({nameof(SensitiveSecret)}) no objeto ({nameof(SecurityConfiguration)}).");
                }

                return _sensitiveSecret;
            }
            set => _sensitiveSecret = value;
        }
    }
}