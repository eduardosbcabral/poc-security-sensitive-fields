using Newtonsoft.Json;
using PocSecurityDotNetFramework.Services;
using System;
using System.Net;

namespace PocSecurityDotNetFramework.Http
{
    public class JsonCryptoConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value.GetType() == typeof(int) || value.GetType() == typeof(string))
            {
                writer.WriteValue(Encrypt(value));
                return;
            }

            throw new InvalidOperationException("Não é possível criptograr tipos complexos usando o conversor JsonCrypto.");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            else if (reader.TokenType == JsonToken.Integer)
            {
                return serializer.Deserialize(reader, objectType);
            }
            if (reader.TokenType == JsonToken.String)
            {
                var stringCipher = serializer.Deserialize(reader, typeof(string));

                if (objectType == typeof(int))
                {
                    var decrypted = DecryptInt(stringCipher);
                    return decrypted;
                }
                else if (objectType == typeof(string))
                {
                    var decrypted = DecryptString(stringCipher);
                    return decrypted;
                }
                else
                {
                    return serializer.Deserialize(reader, objectType);
                }
            }
            else
            {
                return serializer.Deserialize(reader, objectType);
            }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override bool CanConvert(Type objectType)
        {
            return false;
        }

        public int DecryptInt(object model)
        {
            ICipherService cipherService = new RijndaelCipherService();
            var _sensitiveHelper = new SensitiveAnnotationHelper();
            string decoded;
            try
            {
                var base64Decoded = _sensitiveHelper.DecodeFrom64(model.ToString());
                decoded = WebUtility.UrlDecode(base64Decoded);
            }
            catch
            {
                return int.Parse(model.ToString());
            }

            return int.Parse(cipherService.Decrypt(decoded, SecurityConfiguration.SensitiveSecret));
        }

        public string DecryptString(object model)
        {
            ICipherService cipherService = new RijndaelCipherService();
            var _sensitiveHelper = new SensitiveAnnotationHelper();
            string decoded;
            try
            {
                var base64Decoded = _sensitiveHelper.DecodeFrom64(model.ToString());
                decoded = WebUtility.UrlDecode(base64Decoded);
            }
            catch
            {
                return model.ToString();
            }

            return cipherService.Decrypt(decoded, SecurityConfiguration.SensitiveSecret);
        }

        public string Encrypt(object model)
        {
            ICipherService cipherService = new RijndaelCipherService();
            var _sensitiveHelper = new SensitiveAnnotationHelper();
            var encrypted = cipherService.Encrypt(model.ToString(), SecurityConfiguration.SensitiveSecret);
            var encoded = WebUtility.UrlEncode(encrypted);
            return _sensitiveHelper.EncodeTo64(encoded);
        }
    }
}