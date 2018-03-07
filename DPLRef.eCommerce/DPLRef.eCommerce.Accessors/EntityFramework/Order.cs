using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DPLRef.eCommerce.Common.Contracts;

namespace DPLRef.eCommerce.Accessors.EntityFramework
{
    internal class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
        }

        public int Id { get; set; }

        public Guid FromCartId { get; set; }

        [StringLength(50)]
        public string BillingFirst { get; set; }

        [StringLength(50)]
        public string BillingLast { get; set; }

        [StringLength(50)]
        public string BillingEmailAddress { get; set; }

        [StringLength(50)]
        public string BillingAddr1 { get; set; }

        [StringLength(50)]
        public string BillingAddr2 { get; set; }

        [StringLength(50)]
        public string BillingCity { get; set; }

        [StringLength(50)]
        public string BillingState { get; set; }

        [StringLength(50)]
        public string BillingPostal { get; set; }

        [StringLength(50)]
        public string ShippingFirst { get; set; }

        [StringLength(50)]
        public string ShippingLast { get; set; }

        [StringLength(50)]
        public string ShippingEmailAddress { get; set; }

        [StringLength(50)]
        public string ShippingAddr1 { get; set; }

        [StringLength(50)]
        public string ShippingAddr2 { get; set; }

        [StringLength(50)]
        public string ShippingCity { get; set; }

        [StringLength(50)]
        public string ShippingState { get; set; }

        [StringLength(50)]
        public string ShippingPostal { get; set; }

        //[StringLength(50)]
        //public string CouponCode { get; set; }

        [Column(TypeName = "datetimeoffset")]
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

        [Column(TypeName = "datetimeoffset")]
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;

        public decimal SubTotal { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal Total { get; set; }

        [StringLength(100)]
        public string AuthorizationCode { get; set; }

        [StringLength(100)]
        public string ShippingProvider { get; set; }

        [StringLength(100)]
        public string TrackingCode { get; set; }

        [StringLength(200)]
        public string Notes { get; set; }

        public int SellerId { get; set; }

        public int CatalogId { get; set; }

        [Column(TypeName = "int")]
        public OrderStatuses Status { get; set; }
    }
}
