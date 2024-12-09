using eshop.api.Entities;
using Microsoft.EntityFrameworkCore;

namespace eshop.api.Data;

public class DataContext : DbContext
{
  public DbSet<Product> Products { get; set; }
  public DataContext(DbContextOptions options) : base(options)
  {
  }
}
