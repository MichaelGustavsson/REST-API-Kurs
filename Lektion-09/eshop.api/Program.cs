using System.Text;
using eshop.api.Data;
using eshop.api.Entities;
using eshop.api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
  // options.Password.RequireDigit = true;
  // options.Password.RequiredLength = 6;
  // options.Password.RequiredUniqueChars = 1;
  // options.Password.RequireLowercase = true;
  // options.Password.RequireNonAlphanumeric = true;
  // options.Password.RequireUppercase = true;

})
  .AddRoles<IdentityRole>()
  .AddEntityFrameworkStores<DataContext>();

// Dependency injection för vår TokenService...
builder.Services.AddScoped<TokenService>();

builder.Services.AddControllers();

// Lägg till stöd för swagger...
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Aktivera inloggningssäkerhet(Authentication)...
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options =>
  {
    options.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuer = false,
      ValidateAudience = false,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("tokenSettings:tokenKey").Value))
    };
  });

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
