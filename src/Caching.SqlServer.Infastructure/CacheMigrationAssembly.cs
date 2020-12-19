using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;

namespace Caching.SqlServer.Infastructure
{
    internal class CacheMigrationAssembly : MigrationsAssembly
    {
        readonly IOptions<SqlServerCacheOptions> _cacheOptions;
        readonly CacheDbContext _cacheDbContext;

        public CacheMigrationAssembly(
            ICurrentDbContext currentContext,
            IDbContextOptions options,
            IMigrationsIdGenerator idGenerator,
            IDiagnosticsLogger<DbLoggerCategory.Migrations> logger) : base(currentContext, options, idGenerator, logger)
        {
            _cacheDbContext = currentContext.Context as CacheDbContext;
            _cacheOptions = _cacheDbContext.CacheOptions;

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
