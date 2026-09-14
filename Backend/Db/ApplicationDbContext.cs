using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Db
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<DeliveryLocation> DeliveryLocations { get; set; }

        // Catalog & Inventory
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Capacity> Capacities { get; set; }
        public DbSet<ConnectivityType> ConnectivityTypes { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }

        // Orders & Transactions
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Store User Role Enum as string in SQL database
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            // Store Enums as strings in SQL database for readability
            modelBuilder.Entity<Transaction>()
                .Property(t => t.PaymentStatus)
                .HasConversion<string>();

            modelBuilder.Entity<Order>()
                .Property(o => o.DeliveryMethod)
                .HasConversion<string>();

            modelBuilder.Entity<Order>()
                .Property(o => o.OrderStatus)
                .HasConversion<string>();

            // Prevent multiple cascade delete paths on Order -> DeliveryLocation
            modelBuilder.Entity<Order>()
                .HasOne(o => o.DeliveryLocation)
                .WithMany(d => d.Orders)
                .HasForeignKey(o => o.DeliveryLocationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
