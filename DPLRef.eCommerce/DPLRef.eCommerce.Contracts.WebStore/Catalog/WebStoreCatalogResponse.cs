using DPLRef.eCommerce.Common.Shared;
using System.Runtime.Serialization;

namespace DPLRef.eCommerce.Contracts.WebStore.Catalog
{
    [DataContract]
    public class WebStoreCatalogResponse : ResponseBase
    {
        [DataMember]
        public WebStoreCatalog Catalog { get; set; }
    }
}
