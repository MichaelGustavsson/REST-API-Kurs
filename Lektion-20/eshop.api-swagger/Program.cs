using System.Reflection;
using eshop.api;
using eshop.api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

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
builder.Services.AddSwaggerGen(options =>
{
  options.SwaggerDoc("v1", new OpenApiInfo
  {
    Title = "eShop amazing API",
    Version = "1.0",
    Description = "An api for serving eShop client apps",
    Contact = new OpenApiContact
    {
      Name = "Michael Gustavsson",
      Email = "michael.gustavsson@gmail.com"
    }
  });

  var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
  var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

  options.IncludeXmlComments(xmlPath);
});

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
