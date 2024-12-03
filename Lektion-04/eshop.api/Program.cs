using eshop.api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Knyta samman applikation med vår databas...
builder.Services.AddDbContext<DataContext>(options =>
{
  options.UseSqlite(builder.Configuration.GetConnectionString("DevConnection"));
});

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
