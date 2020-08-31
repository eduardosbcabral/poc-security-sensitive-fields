namespace PocSecurity.Services
{
    public interface ICipherService
    {
        string Encrypt(string input, string secret = "");
        string Decrypt(string cipherText, string secret = "");
    }
}