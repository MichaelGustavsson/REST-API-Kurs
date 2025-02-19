using eshop.api;
using eshop.api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Knyta samman applikation med vår databas...
builder.Services.AddDbContext<DataContext>(options =>
{
  options.UseSqlite(builder.Configuration.GetConnectionString("DevConnection"));
});


// Dependency injection...
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();

builder.Services.AddControllers();

// Lägg till stöd för swagger...
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Pipeline...
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

/* ENDAST FÖR TEST VID UTVECKLING... */
using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;
try
{
  var context = services.GetRequiredService<DataContext>();

  await context.Database.MigrateAsync();
  await Seed.LoadAddressTypes(context);
  await Seed.LoadSuppliers(context);
}
catch (Exception ex)
{
  Console.WriteLine("{0}", ex.Message);
  throw;
}

/* STOP INLÄSNING DUMMY DATA... */

app.MapControllers();

app.Run();
