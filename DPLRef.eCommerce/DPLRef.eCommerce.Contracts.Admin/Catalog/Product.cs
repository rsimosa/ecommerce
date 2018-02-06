using System.Runtime.Serialization;

namespace DPLRef.eCommerce.Contracts.Admin.Catalog
{
    [DataContract]
    public class Product
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int CatalogId { get; set; }

        [DataMember]
        public string SellerProductId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Summary { get; set; }

        [DataMember]
        public string Detail { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public bool IsDownloadable { get; set; }
        [DataMember]

        public bool IsAvailable { get; set; }

        [DataMember]
        public string SupplierName { get; set; }

        [DataMember]
        public decimal ShippingWeight { get; set; }
    }
}