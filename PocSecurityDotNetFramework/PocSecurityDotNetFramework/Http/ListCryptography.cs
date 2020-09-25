using PocSecurityDotNetFramework.Services;
using System;
using System.Collections;

namespace PocSecurityDotNetFramework.Http
{
    public class ListCryptography : IHttpCryptography
    {
        private readonly ICipherService _cipherService;

        public ListCryptography(ICipherService cipherService)
        {
            _cipherService = cipherService;
        }

        public T Decrypt<T>(T model)
        {
            var classCryptography = new ClassCryptography(_cipherService);
            var list = (IList)model;
            for (var i = 0; i < list.Count; i++)
            {
                var encryptedItem = classCryptography.Decrypt(list[i]);
                list[i] = encryptedItem;
            }

            return (T)list;
        }

        public T Encrypt<T>(T model)
        {
            var classCryptography = new ClassCryptography(_cipherService);
            var list = (IList)model;
            for (var i = 0; i < list.Count; i++)
            {
                var encryptedItem = classCryptography.Encrypt(list[i]);
                list[i] = encryptedItem;
            }

            return (T)list;
        }
    }
}