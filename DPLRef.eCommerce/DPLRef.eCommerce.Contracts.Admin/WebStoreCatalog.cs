using System.Runtime.Serialization;

namespace DPLRef.eCommerce.Contracts.Admin
{
    public class WebStoreCatalog
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}