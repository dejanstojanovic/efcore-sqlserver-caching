using Microsoft.Extensions.Options;
using System;

namespace Caching.SqlServer.Infastructure.Options
{
    public class SqlServerCacheDbContextOptions : IOptions<SqlServerCacheDbContextOptions>
    {
        public SqlServerCacheDbContextOptions()
        {
            MigrationHistoryTable = "__EFMigrationsHistory";
        }
        public String MigrationHistoryTable { get; set; }
        public SqlServerCacheDbContextOptions Value => this;
    }
}
