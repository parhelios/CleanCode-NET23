var builder = WebApplication.CreateBuilder(args);

// L�gg till f�ljande om det inte redan finns
builder.Configuration.AddEnvironmentVariables();

// Anv�nd anslutningsstr�ngen
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Konfigurera databaskontexten
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseNpgsql(connectionString));

// Resten av din kod...
