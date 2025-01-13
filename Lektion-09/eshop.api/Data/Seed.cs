using System.Text.Json;
using eshop.api.Entities;
using Microsoft.Extensions.Options;

namespace eshop.api.Data;

public static class Seed
{
  private static readonly JsonSerializerOptions options = new()
  {
    PropertyNameCaseInsensitive = true
  };
  public static async Task LoadProducts(DataContext context)
  {
    if (context.Products.Any()) return;

    var json = File.ReadAllText("Data/json/products.json");
    var products = JsonSerializer.Deserialize<List<Product>>(json, options);

    if (products is not null && products.Count > 0)
    {
      await context.Products.AddRangeAsync(products);
      await context.SaveChangesAsync();
    }
  }

  public static async Task LoadSalesOrders(DataContext context)
  {
    // var options = new JsonSerializerOptions
    // {
    //   PropertyNameCaseInsensitive = true
    // };

    if (context.SalesOrders.Any()) return;

    var json = File.ReadAllText("Data/json/orders.json");
    var orders = JsonSerializer.Deserialize<List<SalesOrder>>(json, options);

    if (orders is not null && orders.Count > 0)
    {
      await context.SalesOrders.AddRangeAsync(orders);
      await context.SaveChangesAsync();
    }
  }

  public static async Task LoadOrderItems(DataContext context)
  {
    if (context.OrderItems.Any()) return;

    var json = File.ReadAllText("Data/json/orderitems.json");
    var orderitems = JsonSerializer.Deserialize<List<OrderItem>>(json, options);

    if (orderitems is not null && orderitems.Count > 0)
    {
      await context.OrderItems.AddRangeAsync(orderitems);
      await context.SaveChangesAsync();
    }
  }
}
