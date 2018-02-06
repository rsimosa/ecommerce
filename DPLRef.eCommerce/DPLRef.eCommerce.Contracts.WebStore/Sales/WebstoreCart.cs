using System;
using System.Runtime.Serialization;
using DPLRef.eCommerce.Common.Contracts;

namespace DPLRef.eCommerce.Contracts.WebStore.Sales
{
    [DataContract]
    public class WebStoreCart
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public Address BillingAddress { get; set; }

        [DataMember]
        public Address ShippingAddress { get; set; }

        [DataMember]
        public WebStoreCartItem[] CartItems { get; set; }

        [DataMember]
        public decimal SubTotal { get; set; }

        [DataMember]
        public decimal TaxAmount { get; set; }

        [DataMember]
        public decimal Total { get; set; }
    }

    [DataContract]
    public class WebStoreCartItem
    {
        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public decimal UnitPrice { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        [DataMember]
        public decimal ExtendedPrice { get; set; }
    }
}
