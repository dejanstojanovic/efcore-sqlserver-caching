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
    public class CacheMigrationAssembly : MigrationsAssembly
    {
        readonly DbContext _context;
        readonly SqlServerCacheOptions _cacheOptions;

        public CacheMigrationAssembly(
            IOptions<SqlServerCacheOptions> cacheOptions,
            ICurrentDbContext currentContext,
            IDbContextOptions options,
            IMigrationsIdGenerator idGenerator,
            IDiagnosticsLogger<DbLoggerCategory.Migrations> logger) : base(currentContext, options, idGenerator, logger)
        {
            _cacheOptions = cacheOptions.Value;
        }
        public override Migration CreateMigration(TypeInfo migrationClass, string activeProvider)
        {
            var hasCtorWithDbContext = migrationClass
                    .GetConstructor(new[] { typeof(DbContext) }) != null;

            if (hasCtorWithDbContext)
            {
                var instance = (Migration)Activator.CreateInstance(migrationClass.AsType(), _cacheOptions);
                instance.ActiveProvider = activeProvider;
                return instance;
            }

            return base.CreateMigration(migrationClass, activeProvider);
        }
    }
}
