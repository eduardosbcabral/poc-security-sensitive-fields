using PocSecurityDotNetCore.Attributes;

namespace PocSecurityDotNetCore.Models
{
    [SensitiveClass]
    public class CommandID<T>
    {
        [SensitiveField]
        public T Id { get; set; }
    }
}