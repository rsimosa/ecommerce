using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DPLRef.eCommerce.Accessors.EntityFramework
{
    internal class Seller
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Seller()
        {
        }

        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }

        public bool IsApproved { get; set; }

        public int BankRoutingNumber { get; set; }

        public int BankAccountNumber { get; set; }

        [StringLength(1000)]
        public string OrderNotificationEmail { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }
    }
}
