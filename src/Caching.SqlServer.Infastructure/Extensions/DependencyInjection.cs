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
        /// <summary>
        /// Adds Microsoft SQL Server caching infrastructure using <see cref="SqlServerCacheOptions"/> values set previously by <see cref="SqlServerCachingServicesExtensions.AddDistributedSqlServerCache(IServiceCollection, Action{SqlServerCacheOptions})"/> method"
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> of application services to be injected</param>
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

        /// <summary>
        /// Adds Microsoft SQL Server caching infrastructure using <see cref="SqlServerCacheOptions"/> values set previously by <see cref="SqlServerCachingServicesExtensions.AddDistributedSqlServerCache(IServiceCollection, Action{SqlServerCacheOptions})"/> method"
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> of application services to be injected</param>
        /// <param name="options">EF Core <see cref="DbContext"/> options</param>
        public static void AddSqlServerCachingInfrastructure(this IServiceCollection services, Action<DbContextOptionsBuilder> options)
        {
            services.AddDbContext<CacheDbContext>(options);
        }

        /// <summary>
        /// Executes migration to create the table for Microsoft SQL Server based caching
        /// </summary>
        /// <param name="app">ASP.NET Core pipeline <see cref="IApplicationBuilder"/> instance</param>
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
