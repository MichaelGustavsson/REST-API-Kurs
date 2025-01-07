using eshop.api.Data;
using eshop.api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Knyta samman applikation med vår databas...
builder.Services.AddDbContext<DataContext>(options =>
{
  options.UseSqlite(builder.Configuration.GetConnectionString("DevConnection"));
});

// Sätta upp hantering av användare...
builder.Services.AddIdentityCore<User>(options =>
{
  options.User.RequireUniqueEmail = true;
})
  .AddRoles<IdentityRole>()
  .AddEntityFrameworkStores<DataContext>();

builder.Services.AddControllers();

// Lägg till stöd för swagger...
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;
try
{
  var context = services.GetRequiredService<DataContext>();
  await context.Database.MigrateAsync();
  await Seed.LoadProducts(context);
  await Seed.LoadSalesOrders(context);
  await Seed.LoadOrderItems(context);
}
catch (Exception ex)
{
  Console.WriteLine("{0}", ex.Message);
  throw;
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
