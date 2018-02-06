using System.Runtime.Serialization;
using DPLRef.eCommerce.Common.Shared;

namespace DPLRef.eCommerce.Contracts.Admin.Catalog
{
    [DataContract]
    public class AdminProductResponse : ResponseBase
    {
        [DataMember]
        public Product Product { get; set; }
    }
}
