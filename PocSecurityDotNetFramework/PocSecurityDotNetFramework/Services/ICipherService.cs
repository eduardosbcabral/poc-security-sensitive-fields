namespace PocSecurityDotNetFramework.Services
{
    public interface ICipherService
    {
        string Encrypt(string text, string secret);
        string Decrypt(string text, string secret);
    }
}