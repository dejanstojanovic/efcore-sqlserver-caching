using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.Options;
using Caching.SqlServer.Infastructure.Models;
using Caching.SqlServer.Infastructure.Configurations;
using Caching.SqlServer.Infastructure.Options;

namespace Caching.SqlServer.Infastructure.Extensions
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds Microsoft SQL Server caching infrastructure using <see cref="SqlServerCacheOptions"/> values set previously by <see cref="SqlServerCachingServicesExtensions.AddDistributedSqlServerCache(IServiceCollection, Action{SqlServerCacheOptions})"/> method"
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> of application services to be injected</param>
        /// <param name="dbContextOptions">DbContext options for cache db context</param>
        public static void AddSqlServerCachingInfrastructure(this IServiceCollection services, Action<SqlServerCacheDbContextOptions> dbContextOptions = null)
        {
            services.AddScoped<IEntityTypeConfiguration<Cache>, CacheConfiguration>();

            if(dbContextOptions!=null)
                services.Configure<SqlServerCacheDbContextOptions>(dbContextOptions);

            using (var provider = services.BuildServiceProvider())
            {
                var cacheOptions = provider.GetService<IOptions<SqlServerCacheOptions>>().Value;
                var cacheDbContextOptions = provider.GetService<IOptions<SqlServerCacheDbContextOptions>>().Value;
                services.AddDbContext<CacheDbContext>(options =>
                {
                    options.UseSqlServer(cacheOptions.ConnectionString, o =>
                    {
                        o.MigrationsHistoryTable(
                            cacheDbContextOptions.MigrationHistoryTable,
                            cacheOptions.SchemaName);
                    });
                });
            }
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
