using Newtonsoft.Json;
using PocSecurityDotNetFramework.Attributes;
using PocSecurityDotNetFramework.Http;
using System;

namespace PocSecurityDotNetFramework.Models
{
    public class UserQueryModel
    {
        [JsonConverter(typeof(JsonCryptoConverter))]
        public int Id { get; set; }

        [JsonConverter(typeof(JsonCryptoConverter))]
        public string Id2 { get; set; }

        public int Numero { get; set; }

        [SensitiveField]
        public SensitiveCommandID CommandId { get; set; }

        public string Username { get; set; }
        public string Cpf { get; set; }

        public DateTime Data { get; set; }

        public UserQueryModel()
        {
            CommandId = new SensitiveCommandID();
        }
    }
}
