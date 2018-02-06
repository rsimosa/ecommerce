using System.Runtime.Serialization;
using DPLRef.eCommerce.Common.Shared;

namespace DPLRef.eCommerce.Contracts.Admin.Catalog
{
    [DataContract]
    public class AdminCatalogResponse : ResponseBase
    {
        [DataMember]
        public WebStoreCatalog Catalog { get; set; }
    }
}
