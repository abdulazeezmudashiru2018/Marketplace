using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Models
{
    public class MarketPlaceDbContext : DbContext
    {
        public MarketPlaceDbContext(
            DbContextOptions<MarketPlaceDbContext> options
        ) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }


        // Added so checkout can save orders
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }


        // ADD THIS LINE 👇
        public DbSet<SalesHistory> SalesHistories { get; set; }

    }

}



