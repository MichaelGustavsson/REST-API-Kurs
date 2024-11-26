var builder = WebApplication.CreateBuilder(args);

// Lägg till stöd för API Controllers...
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();