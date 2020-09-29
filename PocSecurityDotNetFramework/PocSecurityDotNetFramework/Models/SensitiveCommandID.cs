using PocSecurityDotNetFramework.Attributes;
using PocSecurityDotNetFramework.Http;

namespace PocSecurityDotNetFramework.Models
{
    [SensitiveClass]
    public class SensitiveCommandID
    {
        [SensitiveField]
        public Sensitive Id { get; set; }

        public SensitiveCommandID()
        {

        }

        public SensitiveCommandID(Sensitive id)
        {
            Id = id;
        }

        public SensitiveCommandID(int id)
        {
            Id = new Sensitive(id);
        }

        public SensitiveCommandID(string id)
        {
            Id = new Sensitive(id);
        }
    }
}