using System.Runtime.Serialization;
using DPLRef.eCommerce.Common.Shared;

namespace DPLRef.eCommerce.Contracts.WebStore.Sales
{
    [DataContract]
    public class WebStoreOrderResponse : ResponseBase
    {
        [DataMember]
        public WebStoreOrder Order { get; set; }
    }
}
