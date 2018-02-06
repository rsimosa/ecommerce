using System.Runtime.Serialization;
using DPLRef.eCommerce.Common.Shared;

namespace DPLRef.eCommerce.Contracts.Admin.Catalog
{
    [DataContract]
    public class AdminCatalogsResponse : ResponseBase
    {
        [DataMember]
        public WebStoreCatalog[] Catalogs { get; set; }
    }
}
