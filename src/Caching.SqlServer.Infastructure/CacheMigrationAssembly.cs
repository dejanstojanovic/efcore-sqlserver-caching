using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;
using System.Linq;

namespace Caching.SqlServer.Infastructure
{
    public class CacheMigrationAssembly : MigrationsAssembly
    {
        readonly SqlServerCacheOptions _cacheOptions;

        public CacheMigrationAssembly(
            //IOptions<SqlServerCacheOptions> cacheOptions,
            IServiceProvider serviceProvider,
            ICurrentDbContext currentContext,
            IDbContextOptions options,
            IMigrationsIdGenerator idGenerator,
            IDiagnosticsLogger<DbLoggerCategory.Migrations> logger) : base(currentContext, options, idGenerator, logger)
        {
            var cacheOptions = serviceProvider.GetRequiredService<IOptions<SqlServerCacheOptions>>();
            _cacheOptions = cacheOptions.Value;
        }
        public override Migration CreateMigration(TypeInfo migrationClass, string activeProvider)
        {
            var hasCtorWithCacheOptions = migrationClass
                    .GetConstructor(new[] { typeof(IOptions<SqlServerCacheOptions>) }) != null;

            if (hasCtorWithCacheOptions)
            {
                var instance = (Migration)Activator.CreateInstance(migrationClass.AsType(), _cacheOptions);
                instance.ActiveProvider = activeProvider;
                return instance;
            }

            return base.CreateMigration(migrationClass, activeProvider);
        }
    }
}
