using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DPLRef.eCommerce.Accessors.EntityFramework
{
    internal class Catalog
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Catalog()
        {
        }

        public int Id { get; set; }

        public int SellerId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsApproved { get; set; }

        [Column(TypeName = "datetimeoffset")]
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

        [Column(TypeName = "datetimeoffset")]
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;
    }


    internal class CatalogExtended 
    {
        public Catalog Catalog { get; set; }
        public string SellerName { get; set; }
    }
}
