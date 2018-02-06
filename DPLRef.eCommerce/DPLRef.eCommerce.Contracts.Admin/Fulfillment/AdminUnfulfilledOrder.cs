using System.Runtime.Serialization;
using DPLRef.eCommerce.Common.Contracts;

namespace DPLRef.eCommerce.Contracts.Admin.Fulfillment
{
    [DataContract]
    public class AdminUnfulfilledOrder
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public Address ShippingAddress { get; set; }

        [DataMember]
        public OrderStatuses Status { get; set; }

        [DataMember]
        public AdminUnfulfilledOrderLine[] OrderLines { get; set; }
    }

    [DataContract]
    public class AdminUnfulfilledOrderLine
    {
        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public int Quantity { get; set; }
    }
}
