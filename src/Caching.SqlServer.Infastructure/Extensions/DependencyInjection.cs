using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.Options;
using Caching.SqlServer.Infastructure.Models;
using Caching.SqlServer.Infastructure.Configurations;

namespace Caching.SqlServer.Infastructure.Extensions
{
    public static class DependencyInjection
    {
        public static void AddSqlServerCachingInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IEntityTypeConfiguration<Cache>, CacheConfiguration>();

            using (var provider = services.BuildServiceProvider())
            {
                var connectionString = provider.GetService<IOptions<SqlServerCacheOptions>>().Value.ConnectionString;
                services.AddDbContext<CacheDbContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });
            }
        }

        public static void AddSqlServerCachingInfrastructure(this IServiceCollection services, Action<DbContextOptionsBuilder> options)
        {
            services.AddDbContext<CacheDbContext>(options);
        }

        public static void SetupSqlServerCachingInfrastructure(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<CacheDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
