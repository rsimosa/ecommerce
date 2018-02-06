using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DPLRef.eCommerce.Accessors.EntityFramework
{
    internal class CartItem
    {
        public int Id { get; set; }

        public Guid CartId { get; set; }

        public int CatalogId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }
        
    }
}
