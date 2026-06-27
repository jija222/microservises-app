using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Data
{
    public class OrderDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; } 
        public DbSet<Client> Clients { get; set; }

        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("orders");
            base.OnModelCreating(modelBuilder);

            // Настройка связей
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Client)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.ClientId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany(p => p.Orders)
                .HasForeignKey(o => o.ProductId);

            // Настройка ограничений
            modelBuilder.Entity<Client>().Property(c => c.Email).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnType("numeric(18,2)");

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Price = 500m },
                new Product { Id = 2, Price = 1500m },
                new Product { Id = 3, Price = 99.99m }
            );
            modelBuilder.Entity<Client>().HasData(new Client { Id = 1, Email = "test@mail.ru", PhoneNumber = "+79990000000" }
);
            base.OnModelCreating(modelBuilder);
        }
    }
}
