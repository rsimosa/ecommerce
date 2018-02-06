using System;
using System.Runtime.Serialization;
using DPLRef.eCommerce.Common.Contracts;

namespace DPLRef.eCommerce.Accessors.DataTransferObjects
{
    [DataContract]
    public class Order
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public Address BillingAddress { get; set; }

        [DataMember]
        public Address ShippingAddress { get; set; }

        [DataMember]
        public OrderLine[] OrderLines { get; set; }

        [DataMember]
        public decimal SubTotal { get; set; }

        [DataMember]
        public decimal TaxAmount { get; set; }

        [DataMember]
        public decimal Total { get; set; }

        [DataMember]
        public OrderStatuses Status { get; set; }

        [DataMember]
        public Guid FromCartId { get; set; }

        [DataMember]
        public string AuthorizationCode { get; set;}

        [DataMember]
        public string ShippingProvider { get; set; }

        [DataMember]
        public string TrackingCode { get; set; }

        [DataMember]
        public string Notes { get; set; }

        [DataMember]
        public int SellerId { get; set; }
    }

    [DataContract]
    public class OrderLine
    {
        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        [DataMember]
        public decimal UnitPrice { get; set; }

        [DataMember]
        public decimal ExtendedPrice { get; set; }
    }
}
