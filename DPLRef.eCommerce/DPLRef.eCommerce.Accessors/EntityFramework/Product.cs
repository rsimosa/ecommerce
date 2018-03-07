using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DPLRef.eCommerce.Accessors.EntityFramework
{
    internal class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
        }

        public int Id { get; set; }

        public int CatalogId { get; set; }

        [StringLength(50)]
        public string SellerProductId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public string Summary { get; set; }

        public string Detail { get; set; }

        public decimal Price { get; set; }

        [Column(TypeName = "datetimeoffset")]
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

        [Column(TypeName = "datetimeoffset")]
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;

        [StringLength(50)]
        public string SupplierName { get; set; }

        public decimal ShippingWeight { get; set; }

        public bool IsAvailable { get; set; }

        public bool IsDownloadable { get; set; }

    }
}
