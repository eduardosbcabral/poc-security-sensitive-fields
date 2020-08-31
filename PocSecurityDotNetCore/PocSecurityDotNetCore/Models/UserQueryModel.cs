using PocSecurity.Sensitive;

namespace PocSecurity.Models
{
    [SensitiveClass]
    public class UserQueryModel
    {
        [SensitiveField]
        public string Id { get; set; }
        public string Username { get; set; }
        public string Cpf { get; set; }

        public int Crypto(int id)
        {
            return id + 5000;
        }
    }
}
