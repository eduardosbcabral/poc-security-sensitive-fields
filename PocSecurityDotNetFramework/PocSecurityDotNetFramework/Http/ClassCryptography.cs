using PocSecurityDotNetFramework.Services;
using System.Net;

namespace PocSecurityDotNetFramework.Http
{
    public class ClassCryptography : IHttpCryptography
    {
        private readonly ICipherService _cipherService;
        private readonly SensitiveAnnotationHelper _sensitiveHelper;
        private readonly FieldCryptography _fieldCryptography;

        public ClassCryptography(ICipherService cipherService)
        {
            _cipherService = cipherService;
            _sensitiveHelper = new SensitiveAnnotationHelper();
            _fieldCryptography = new FieldCryptography(_cipherService);
        }

        public T Decrypt<T>(T model)
        {
            var classProperties = _sensitiveHelper.GetSensitiveProperties(model);

            foreach (var property in classProperties)
            {
                var isClass = property.PropertyType.IsClass;
                if(isClass)
                {
                    Decrypt(property.GetValue(model));
                } 
                else
                {
                    var propertyValue = (Sensitive)property.GetValue(model);
                    if (propertyValue.HasValue())
                    {
                        var decrypted = _fieldCryptography.Decrypt(propertyValue);
                        property.SetValue(model, (Sensitive)decrypted, null);
                    }
                    else
                    {
                        throw new SensitiveClassNullException();
                    }
                }
            }

            return model;
        }

        public T Encrypt<T>(T model)
        {
            var classProperties = _sensitiveHelper.GetSensitiveProperties(model);

            foreach (var property in classProperties)
            {
                var isClass = property.PropertyType.IsClass;
                if (isClass)
                {
                    var propertyValue = property.GetValue(model);
                    if(propertyValue == null)
                    {
                        throw new SensitiveClassNullException($"A propriedade {property.Name} está nula, então não é possível definir um valor sensitivo para ela.");
                    }

                    Encrypt(property.GetValue(model));
                }
                else
                {
                    var propertyValue = (Sensitive)property.GetValue(model);
                    if(propertyValue.HasValue())
                    {
                        var encrypted = _fieldCryptography.Encrypt(propertyValue);
                        property.SetValue(model, encrypted, null);
                    }
                }
            }

            return model;
        }
    }
}