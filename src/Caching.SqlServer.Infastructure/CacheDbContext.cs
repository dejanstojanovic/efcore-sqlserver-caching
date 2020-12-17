using Caching.SqlServer.Infastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Caching.SqlServer.Infastructure
{
    public partial class CacheDbContext : DbContext
    {
        readonly IEntityTypeConfiguration<Cache> _configuration;
        public CacheDbContext(IEntityTypeConfiguration<Cache> configuration)
        {
            _configuration = configuration;
        }

        public CacheDbContext(DbContextOptions<CacheDbContext> options, IEntityTypeConfiguration<Cache> configuration)
            : base(options)
        {
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
