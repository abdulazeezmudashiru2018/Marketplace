
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketPlace.Models 
{
    public class SalesHistory
    {
        public int Id { get; set; }

        [Required]
        public string? CustomerName { get; set; } 

        public string? CustomerPhone { get; set; } 
        public string? CustomerAddress { get; set; }

        [Required]
        public string? InvoiceNumber { get; set; }
        [Required]
        public string? DateTime { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal GrandTotal { get; set; }

        public string? AmountInWords { get; set; }

        public string? SalesItemsJson { get; set; }
    }
}