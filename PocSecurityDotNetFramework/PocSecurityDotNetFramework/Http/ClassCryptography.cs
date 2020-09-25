using PocSecurityDotNetFramework.Services;
using System.Web;

namespace PocSecurityDotNetFramework.Http
{
    public class ClassCryptography : IHttpCryptography
    {
        private readonly ICipherService _cipherService;

        public ClassCryptography(ICipherService cipherService)
        {
            _cipherService = cipherService;
        }

        public T Decrypt<T>(T model)
        {
            var sensitiveHelper = new SensitiveAnnotationHelper();
            var classProperties = sensitiveHelper.GetSensitiveProperties(model);

            foreach (var property in classProperties)
            {
                var propertyValue = property.GetValue(model);
                if (propertyValue != null)
                {
                    var decoded = HttpUtility.UrlDecode(propertyValue.ToString());
                    var decrypted = _cipherService.Decrypt(decoded, SecurityConfiguration.SensitiveSecret);
                    property.SetValue(model, (Sensitive.Sensitive)decrypted, null);
                }
            }

            return model;
        }

        public T Encrypt<T>(T model)
        {
            var sensitiveHelper = new SensitiveAnnotationHelper();
            var classProperties = sensitiveHelper.GetSensitiveProperties(model);

            foreach (var property in classProperties)
            {
                var propertyValue = property.GetValue(model);
                if(propertyValue != null)
                {
                    var encrypted = _cipherService.Encrypt(propertyValue.ToString(), SecurityConfiguration.SensitiveSecret);
                    var encoded = HttpUtility.UrlEncode(encrypted);
                    property.SetValue(model, (Sensitive.Sensitive)encoded, null);
                }
            }

            return model;
        }
    }
}