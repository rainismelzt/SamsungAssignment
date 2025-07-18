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
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    // Register services
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddConfiguredDbContext<UserDbContext>(Configuration);

        services.AddScoped<IUserDataRepository, UserDataRepository>();
        services.AddScoped<IUserDataService, UserDataService>();

        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        services.AddControllers();
        services.AddAngularCorsPolicy(Configuration);

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
        });
    }

    // Configure middleware pipeline
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