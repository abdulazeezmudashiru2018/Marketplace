using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

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

        // Sales History
        public DbSet<SalesHistory> SalesHistories { get; set; }

        // This prevents the PendingModelChangesWarning crash on Render
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }
    }
}