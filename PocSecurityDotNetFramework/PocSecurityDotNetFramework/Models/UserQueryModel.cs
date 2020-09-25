using PocSecurityDotNetFramework.Sensitive;

namespace PocSecurityDotNetFramework.Models
{
    [SensitiveClass]
    public class UserQueryModel
    {
        [SensitiveField]
        public Sensitive.Sensitive Id { get; set; }
        public string Username { get; set; }
        public string Cpf { get; set; }
    }
}
