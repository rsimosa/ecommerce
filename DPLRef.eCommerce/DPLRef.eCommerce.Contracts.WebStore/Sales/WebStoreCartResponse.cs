using System.Runtime.Serialization;
using DPLRef.eCommerce.Common.Shared;

namespace DPLRef.eCommerce.Contracts.WebStore.Sales
{
    [DataContract]
    public class WebStoreCartResponse : ResponseBase
    {
        [DataMember]
        public WebStoreCart Cart { get; set; }
    }
}
