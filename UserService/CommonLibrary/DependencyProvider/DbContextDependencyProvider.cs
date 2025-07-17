using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommonLibrary.DependencyProvider
{
    public static class DbContextDependencyProvider
    {
        private const string DefaultConnectionStringName = "UserDbConnectionString";

        public static IServiceCollection AddConfiguredDbContext<TContext>(
            this IServiceCollection services,
            IConfiguration configuration,
            string? connectionStringName = null)
            where TContext : DbContext
        {
            var connName = connectionStringName ?? DefaultConnectionStringName;
            var connectionString = configuration.GetConnectionString(connName);

            services.AddDbContext<TContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }
    }
}
