using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenDMS.MultiTenancy.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class DatabaseCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tenants",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    ConnectionString = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    Provider = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Offline = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OpenIdConnectAuthority = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    URL = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    OpenIdConnectClientId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    OpenIdConnectClientSecret = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    ChallengeScheme = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    RootFolder = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tenants", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tenants");
        }
    }
}
