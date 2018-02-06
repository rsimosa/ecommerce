using System.Runtime.Serialization;
using DPLRef.eCommerce.Common.Shared;

namespace DPLRef.eCommerce.Contracts.WebStore.Catalog
{
    [DataContract]
    public class WebStoreProductResponse : ResponseBase
    {
        [DataMember]
        public ProductDetail Product { get; set; }
    }
}
