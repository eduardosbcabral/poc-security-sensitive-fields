using PocSecurityDotNetFramework.Attributes;

namespace PocSecurityDotNetFramework.Models
{
    [SensitiveClass]
    public class CommandID<T>
    {
        [SensitiveField]
        public T Id { get; set; }
    }
}