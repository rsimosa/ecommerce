using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DPLRef.eCommerce.Accessors.EntityFramework
{
    internal class OrderLine
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int FromCartItemId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "datetimeoffset")]
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

        [Column(TypeName = "datetimeoffset")]
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;


        public decimal UnitPrice { get; set; }

        //public decimal Tax { get; set; }

        //public decimal DiscountAmount { get; set; }

        public decimal ExtendedPrice { get; set; }

    }
}
