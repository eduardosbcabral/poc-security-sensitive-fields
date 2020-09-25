using PocSecurityDotNetFramework.Attributes;
using PocSecurityDotNetFramework.Http;

namespace PocSecurityDotNetFramework.Models
{
    [SensitiveClass]
    public class UserQueryModel
    {
        [SensitiveField]
        public Sensitive Id { get; set; }
        public string Username { get; set; }
        public string Cpf { get; set; }
    }
}
