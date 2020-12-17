using Caching.SqlServer.Infastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.Options;

namespace Caching.SqlServer.Infastructure.Configurations
{
    public class CacheConfiguration : IEntityTypeConfiguration<Cache>
    {
        readonly SqlServerCacheOptions _options;
        public CacheConfiguration(IOptions<SqlServerCacheOptions> options)
        {
            _options = options.Value;
        }

        public void Configure(EntityTypeBuilder<Cache> builder)
        {
            builder.ToTable(name:_options.TableName, schema: _options.SchemaName);

            builder.HasIndex(e => e.ExpiresAtTime);

            builder.Property(e => e.Id)
                .IsRequired()
                .HasMaxLength(449);

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Value).IsRequired();
        }
    }
}
