using PocSecurityDotNetFramework.Services;
using System;
using System.Net;

namespace PocSecurityDotNetFramework.Http
{
    public class FieldCryptography
    {
        private readonly ICipherService _cipherService;
        private readonly SensitiveAnnotationHelper _sensitiveHelper;

        public FieldCryptography(ICipherService cipherService)
        {
            _cipherService = cipherService;
            _sensitiveHelper = new SensitiveAnnotationHelper();
        }

        public Sensitive Decrypt(Sensitive model)
        {
            var base64Decoded = _sensitiveHelper.DecodeFrom64(model.ToString());
            var decoded = WebUtility.UrlDecode(base64Decoded);
            return _cipherService.Decrypt(decoded, SecurityConfiguration.SensitiveSecret);
        }

        public Sensitive Encrypt(Sensitive model)
        {
            var encrypted = _cipherService.Encrypt(model.ToString(), SecurityConfiguration.SensitiveSecret);
            var encoded = WebUtility.UrlEncode(encrypted);
            return _sensitiveHelper.EncodeTo64(encoded);
        }
    }
}