using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DPLRef.eCommerce.Accessors.EntityFramework
{
    internal class Cart
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cart()
        {
        }

        public Guid Id { get; set; }

        public int CatalogId { get; set; }

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

        [Column(TypeName = "datetimeoffset")]
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

        [Column(TypeName = "datetimeoffset")]
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;
    }
}
