namespace PocSecurityDotNetCore.Models
{
    public class User
    {
        public int Id { get; private set; }
        public string Username { get; private set; }
        public string Cpf { get; private set; }

        public User(int id, string username, string cpf)
        {
            Id = id;
            Username = username;
            Cpf = cpf;
        }
    }
}
