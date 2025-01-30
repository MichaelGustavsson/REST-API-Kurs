using eshop.api.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eshop.api.Data;

public class DataContext(DbContextOptions options) : IdentityDbContext<User>(options)
{
  public DbSet<Product> Products { get; set; }
  public DbSet<SalesOrder> SalesOrders { get; set; }
  public DbSet<OrderItem> OrderItems { get; set; }
  public DbSet<Ingredient> Ingredients { get; set; }
  public DbSet<Supplier> Suppliers { get; set; }
  public DbSet<SupplierIngredient> SupplierIngredients { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<OrderItem>().HasKey(o => new { o.ProductId, o.SalesOrderId });
    modelBuilder.Entity<SupplierIngredient>().HasKey(s => new { s.SupplierId, s.IngredientId });
    base.OnModelCreating(modelBuilder);
  }
}
