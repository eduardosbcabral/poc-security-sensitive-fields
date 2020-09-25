namespace PocSecurityDotNetFramework.Http
{
    public interface IHttpCryptography
    {
        T Encrypt<T>(T model);
        T Decrypt<T>(T model);
    }
}