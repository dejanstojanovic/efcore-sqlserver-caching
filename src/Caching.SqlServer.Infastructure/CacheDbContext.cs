using Caching.SqlServer.Infastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.Options;

namespace Caching.SqlServer.Infastructure
{
    public partial class CacheDbContext : DbContext
    {
        readonly IEntityTypeConfiguration<Cache> _configuration;
        readonly IOptions<SqlServerCacheOptions> _cacheOptions;

        public IOptions<SqlServerCacheOptions> CacheOptions
        {
            get => _cacheOptions;
        }

        public CacheDbContext(
            IOptions<SqlServerCacheOptions> cacheOptions,
            IEntityTypeConfiguration<Cache> configuration) : base()
        {
            _configuration = configuration;
            _cacheOptions = cacheOptions;
        }

        public CacheDbContext(
            IOptions<SqlServerCacheOptions> cacheOptions,
            DbContextOptions<CacheDbContext> options,
            IEntityTypeConfiguration<Cache> configuration)
            : base(options)
        {
            _cacheOptions = cacheOptions;
            _configuration = configuration;
        }

        public virtual DbSet<Cache> Cache { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ReplaceService<IMigrationsAssembly, CacheMigrationAssembly>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(_configuration);
        }
    }
}
