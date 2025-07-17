using UserService.Core.EntityMapper;
using UserService.Core.Interface;
using UserService.Database;
using UserService.Infrastructure.Repository;
using UserService.Infrastructure.Service;
using CommonLibrary.DependencyProvider;
using UserService.Presentation.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddConfiguredDbContext<UserDbContext>(builder.Configuration);
builder.Services.AddScoped<IUserDataRepository, UserDataRepository>();
builder.Services.AddScoped<IUserDataService, UserDataService>();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});
builder.Services.AddControllers();
builder.Services.AddAngularCorsPolicy(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AngularCors");

app.UseMiddleware<HtmlSanitizerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
