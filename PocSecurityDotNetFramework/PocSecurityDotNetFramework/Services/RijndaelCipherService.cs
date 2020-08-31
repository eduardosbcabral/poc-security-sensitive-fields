using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PocSecurityDotNetFramework.Services
{
    public class RijndaelCipherService
    {
        public static string Encrypt(string cipherText, string secret)
        {
            var salt = Encoding.ASCII.GetBytes(secret.Length.ToString());

            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            byte[] plainText = Encoding.Unicode.GetBytes(cipherText);
            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(secret, salt);

            using (ICryptoTransform encryptor = rijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16)))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainText, 0, plainText.Length);
                        cryptoStream.FlushFinalBlock();
                        cipherText = Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }

            return cipherText;
        }

        public static string Decrypt(string input, string secret)
        {
            var salt = Encoding.ASCII.GetBytes(secret.Length.ToString());

            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            byte[] encryptedData = Convert.FromBase64String(input);
            PasswordDeriveBytes secretKey = new PasswordDeriveBytes(secret, salt);

            using (ICryptoTransform decryptor = rijndaelCipher.CreateDecryptor(secretKey.GetBytes(32), secretKey.GetBytes(16)))
            {
                using (MemoryStream memoryStream = new MemoryStream(encryptedData))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        byte[] plainText = new byte[encryptedData.Length];
                        int decryptedCount = cryptoStream.Read(plainText, 0, plainText.Length);
                        input = Encoding.Unicode.GetString(plainText, 0, decryptedCount);
                    }
                }
            }

            return input;
        }
    }
}
