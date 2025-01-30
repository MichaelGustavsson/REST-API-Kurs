using System.Reflection.Metadata;
using System.Text.Json;
using System.Threading.Tasks;
using eshop.api.Entities;
using eshop.api.ViewModels.Supplier;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eshop.api.Data;

public static class Seed
{
  private static readonly JsonSerializerOptions options = new()
  {
    PropertyNameCaseInsensitive = true
  };

  // Vi ska skapa rollerna Admin, User, SalesSupport...
  public static async Task LoadRoles(RoleManager<IdentityRole> rolemanager)
  {
    if (rolemanager.Roles.Any()) return;

    var admin = new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" };
    var user = new IdentityRole { Name = "User", NormalizedName = "USER" };
    var sales = new IdentityRole { Name = "SalesSupport", NormalizedName = "SALESSUPPORT" };

    await rolemanager.CreateAsync(admin);
    await rolemanager.CreateAsync(user);
    await rolemanager.CreateAsync(sales);
  }

  public static async Task LoadUsers(UserManager<User> userManager)
  {
    if (userManager.Users.Any()) return;

    var evert = new User
    {
      FirstName = "Evert",
      LastName = "Ljusberg",
      Email = "evert@gmail.com",
      UserName = "evert@gmail.com"
    };

    await userManager.CreateAsync(evert, "Password01!");
    await userManager.AddToRoleAsync(evert, "User");

    // Skapa en admin användare...
    var helena = new User
    {
      FirstName = "Helena",
      LastName = "Andersson",
      Email = "helena@gmail.com",
      UserName = "helena@gmail.com"
    };

    await userManager.CreateAsync(helena, "Password01!");
    await userManager.AddToRolesAsync(helena, ["User", "Admin", "SalesSupport"]);
  }

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

  public static async Task LoadAddressTypes(DataContext context)
  {
    if (context.AddressTypes.Any()) return;

    var json = await File.ReadAllTextAsync("Data/json/addressTypes.json");
    var types = JsonSerializer.Deserialize<List<AddressType>>(json, options);

    if (types is not null && types.Count > 0)
    {
      await context.AddressTypes.AddRangeAsync(types);
      await context.SaveChangesAsync();
    }
  }

  public static async Task LoadSuppliers(DataContext context)
  {
    if (context.Suppliers.Any()) return;

    var json = await File.ReadAllTextAsync("Data/json/suppliers.json");
    var suppliers = JsonSerializer.Deserialize<List<SupplierPostViewModel>>(json, options);

    if (suppliers is not null && suppliers.Count > 0)
    {
      foreach (var supplier in suppliers)
      {
        var newSupplier = new Supplier
        {
          Name = supplier.Name,
          Email = supplier.Email,
          Phone = supplier.Phone
        };

        await context.Suppliers.AddAsync(newSupplier);

        foreach (var address in supplier.Addresses)
        {
          var postalAddress = await context.PostalAddresses.FirstOrDefaultAsync(c => c.PostalCode.Replace(" ", "").Trim() == address.PostalCode.Replace(" ", "").Trim());
          var supplierAddress = await context.Addresses.FirstOrDefaultAsync(c => c.AddressLine.Trim().ToLower() == address.AddressLine.Trim().ToLower());

          // ?? = is null
          // = gör detta i fallet ovan är sant
          // if(postalAddress is null){gör vi detta}
          postalAddress ??= new PostalAddress { PostalCode = address.PostalCode.Replace(" ", "").Trim(), City = address.City.Trim() };
          supplierAddress ??= new Address { AddressLine = address.AddressLine, AddressTypeId = (int)address.AddressType, PostalAddress = postalAddress };

          await context.PostalAddresses.AddAsync(postalAddress);
          await context.Addresses.AddAsync(supplierAddress);
          newSupplier.SupplierAddresses.Add(new SupplierAddress { Address = supplierAddress, Supplier = newSupplier });
        }

        await context.SaveChangesAsync();
      }
    }
  }
}
