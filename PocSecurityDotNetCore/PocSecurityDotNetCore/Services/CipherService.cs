using Microsoft.AspNetCore.DataProtection;
using System.Security.Cryptography;

namespace PocSecurity.Services
{
    public class CipherService : ICipherService
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private const string Key = "my-very-long-key-of-no-exact-size";

        public CipherService(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtectionProvider = dataProtectionProvider;
        }

        public string Encrypt(string input)
        {
            var protector = _dataProtectionProvider.CreateProtector(Key);
            try
            {
                return protector.Protect(input);
            }
            catch (CryptographicException)
            {
                return "";
            }
        }

        public string Decrypt(string cipherText)
        {
            var protector = _dataProtectionProvider.CreateProtector(Key);

            try
            {
                return protector.Unprotect(cipherText);
            }
            catch (CryptographicException)
            {
                return "";
            }
        }
    }
}
