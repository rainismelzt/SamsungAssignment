var builder = WebApplication.CreateBuilder(args);

// Add services and configuration from Startup
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

app.Run();