using System;
using System.Runtime.Serialization;
using DPLRef.eCommerce.Common.Contracts;

namespace DPLRef.eCommerce.Accessors.DataTransferObjects
{
    [DataContract]
    public class Cart
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public Address BillingAddress { get; set; }

        [DataMember]
        public Address ShippingAddress { get; set; }

        [DataMember]
        public CartItem[] CartItems { get; set; }
    }

    [DataContract]
    public class CartItem
    {
        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public int Quantity { get; set; }
    }
}
