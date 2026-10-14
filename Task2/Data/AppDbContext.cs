using Microsoft.EntityFrameworkCore;
using Task2.Entities;

namespace Task2.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasQueryFilter(product => !product.IsDeleted);
            entity.HasIndex(product => product.Name);
            entity.Property(product => product.RowVersion).IsRowVersion();
            entity.HasOne(product => product.Category)
                .WithMany(category => category.Products)
                .HasForeignKey(product => product.CategoryId);
        });

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Electronics" },
            new Category { Id = 2, Name = "Home & Kitchen" });

        modelBuilder.Entity<Product>().HasData(
            new { Id = 1, Name = "Wireless Headphones", Price = 79.99m, CategoryId = 1, IsDeleted = false },
            new { Id = 2, Name = "USB-C Charger", Price = 24.99m, CategoryId = 1, IsDeleted = false },
            new { Id = 3, Name = "Bluetooth Speaker", Price = 49.99m, CategoryId = 1, IsDeleted = false },
            new { Id = 4, Name = "Coffee Maker", Price = 89.99m, CategoryId = 2, IsDeleted = false },
            new { Id = 5, Name = "Desk Lamp", Price = 34.99m, CategoryId = 2, IsDeleted = false });
    }
}
