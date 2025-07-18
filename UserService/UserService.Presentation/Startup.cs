using CommonLibrary.DependencyProvider;
using Microsoft.OpenApi.Models;
using UserService.Core.EntityMapper;
using UserService.Core.Interface;
using UserService.Database;
using UserService.Infrastructure.Repository;
using UserService.Infrastructure.Service;
using UserService.Presentation.Middleware;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddConfiguredDbContext<UserDbContext>(_configuration);
        services.AddScoped<IUserDataRepository, UserDataRepository>();
        services.AddScoped<IUserDataService, UserDataService>();

        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        services.AddControllers();
        services.AddAngularCorsPolicy(_configuration);

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
        });
    }

    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
            });
        }

        app.UseCors("AngularCors");

        app.UseMiddleware<HtmlSanitizerMiddleware>();

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.MapControllers();
    }
}
