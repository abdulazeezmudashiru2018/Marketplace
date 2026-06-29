

using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? ProductName { get; set; }

        [Required]
        public decimal ProductPrice { get; set; }

        public string? ProductImage { get; set; }

        // --- NEW FIELDS ADDED FOR YOUR UI ---

        public string? Category { get; set; } // e.g., "Electronics"

        public string? Description { get; set; } // e.g., "High-quality headphones..."

        public double Rating { get; set; } // e.g., 4.5

        // ------------------------------------

        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
