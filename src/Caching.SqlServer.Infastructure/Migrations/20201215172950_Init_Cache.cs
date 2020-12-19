using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.Options;

namespace Caching.SqlServer.Infastructure.Migrations
{
    internal partial class Init_Cache : Migration
    {
        readonly SqlServerCacheOptions _options;
        public Init_Cache(IOptions<SqlServerCacheOptions> options)
        {
            _options = options.Value;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: _options.SchemaName);

            migrationBuilder.CreateTable(
                name: _options.TableName,
                schema: _options.SchemaName,
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(449)", maxLength: 449, nullable: false),
                    Value = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ExpiresAtTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    SlidingExpirationInSeconds = table.Column<long>(type: "bigint", nullable: true),
                    AbsoluteExpiration = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{_options.TableName}", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: $"IX_{_options.TableName}_ExpiresAtTime",
                schema: _options.SchemaName,
                table: _options.TableName,
                column: "ExpiresAtTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: _options.TableName,
                schema: _options.SchemaName);
        }
    }
}
