using System.Runtime.Serialization;

namespace DPLRef.eCommerce.Contracts.WebStore.Catalog
{
    [DataContract]
    public class ProductSummary
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public decimal Price { get; set; }
    }
}
