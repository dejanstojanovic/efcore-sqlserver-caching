using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.Options;

namespace Caching.SqlServer.Infastructure.Migrations
{
    [DbContext(typeof(CacheDbContext))]
    partial class CacheDbContextModelSnapshot : ModelSnapshot
    {
        readonly SqlServerCacheOptions _options;
        public CacheDbContextModelSnapshot(IOptions<SqlServerCacheOptions> options)
        {
            _options = options.Value;
        }

        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Caching.SqlServer.Infastructure.Models.Cache", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(449)
                        .HasColumnType("nvarchar(449)");

                    b.Property<DateTimeOffset?>("AbsoluteExpiration")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("ExpiresAtTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<long?>("SlidingExpirationInSeconds")
                        .HasColumnType("bigint");

                    b.Property<byte[]>("Value")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.HasIndex("ExpiresAtTime");

                    b.ToTable(_options.TableName, _options.SchemaName);
                });
        }
    }
}
