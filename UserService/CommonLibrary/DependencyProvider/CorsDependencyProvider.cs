using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommonLibrary.DependencyProvider
{
    public static class CorsDependencyProvider
    {
        private class CorsSettings
        {
            public string[] AngularOrigins { get; set; } = [];
        }

        public static IServiceCollection AddAngularCorsPolicy(this IServiceCollection services, IConfiguration configuration)
        {
            var corsSettings = new CorsSettings();
            configuration.GetSection("Cors").Bind(corsSettings);

            services.AddCors(options =>
            {
                options.AddPolicy("AngularCors", policy =>
                {
                    policy.WithOrigins(corsSettings.AngularOrigins)
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            return services;
        }
    }
}
