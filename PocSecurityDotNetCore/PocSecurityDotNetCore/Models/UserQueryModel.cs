using PocSecurityDotNetCore.Attributes;
using PocSecurityDotNetCore.Http;
using System;

namespace PocSecurityDotNetCore.Models
{
    [SensitiveClass]
    public class UserQueryModel
    {
        [SensitiveField]
        public Sensitive Id { get; set; }

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
