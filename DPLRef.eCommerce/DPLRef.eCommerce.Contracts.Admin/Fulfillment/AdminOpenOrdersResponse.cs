using System.Runtime.Serialization;
using DPLRef.eCommerce.Common.Shared;

namespace DPLRef.eCommerce.Contracts.Admin.Fulfillment
{
    [DataContract]
    public class AdminOpenOrdersResponse : ResponseBase
    {
        [DataMember]
        public AdminUnfulfilledOrder[] Orders;
    }
}
