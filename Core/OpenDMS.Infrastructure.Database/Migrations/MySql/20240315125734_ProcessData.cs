using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenDMS.Infrastructure.Database.Migrations.MySql
{
    /// <inheritdoc />
    public partial class ProcessData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserGroupRoles_StartISODate_UserGroupId_UserId_RoleId",
                table: "UserGroupRoles");

            migrationBuilder.DeleteData(
                table: "OrganizationNodes",
                keyColumn: "Id",
                keyValue: "cc474ead-94d4-4dec-832d-00f45dbc82e7");

            migrationBuilder.AlterColumn<string>(
                name: "EventId",
                table: "TaskItems",
                type: "varchar(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "ProcessDataId",
                table: "TaskItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProcessInstances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    ProcessDefinitionId = table.Column<int>(type: "int", nullable: false),
                    ProcessImageId = table.Column<int>(type: "int", nullable: false),
                    ProcessInstanceId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProcessEngineId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProcessKey = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EventName = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartUser = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    StopDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessInstances", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "ACL.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7297), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7297) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Company.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7308), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7308) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Datasource.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7313), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7313) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.AddContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7191), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7191) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Authorize", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7157), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7158) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7141), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7141) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Delete", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7151), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7151) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Edit", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7146), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7146) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Execute", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7219), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7220) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.History", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7208), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7209) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.RemoveContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7196), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7197) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Share", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7202), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7202) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7125), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7125) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.ViewContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7135), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7135) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "DocumentType.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7292), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7292) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Mail.Console", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7365), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7365) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "MailServer.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7324), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7324) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Meta.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7302), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7303) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanCeateRootFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7235), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7236) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanHavePersonalFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7230), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7230) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Client", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7385), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7385) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CreateGenericDocument", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7214), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7214) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.RemoteSignatureService", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7379), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7380) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.SendMail", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7370), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7370) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Signature", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7375), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7375) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Roles.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7287), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7287) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Tables.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7318), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7319) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7272), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7272) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.CreateMessage", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7277), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7278) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7266), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7267) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7282), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7282) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInbox", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7255), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7256) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInboxCC", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7260), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7261) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewDown", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7245), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7245) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewSide", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7251), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7251) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewUp", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7240), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7241) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Template.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7357), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7358) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Workflow.Dashboard", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7225), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7225) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7464), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7464) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "workflow_architect", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7458), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7458) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$ACCOUNTING$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7400), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7401) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$GLOBAL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7088), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7097) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$HR$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7427), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7427) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MAIL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7433), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7433) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MANAGEMENT$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7406), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7407) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PROTOCOL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7392), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7393) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PURCHASE$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7420), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7421) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$SALES$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7413), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7414) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$WORKFLOW$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7446), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7447) });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(6773), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(6773) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: "$MAIL-INBOUND$",
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: "$MAIL-OUTBOUND$",
                column: "Direction",
                value: 2);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: "$PEC-INBOUND$",
                column: "Direction",
                value: 1);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: "$PEC-OUTBOUND$",
                column: "Direction",
                value: 2);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: "$TEMPLATE$",
                column: "ContentType",
                value: 6);

            migrationBuilder.InsertData(
                table: "OrganizationNodes",
                columns: new[] { "Id", "ClosingNote", "CreationDate", "EndISODate", "LastUpdate", "LeftBound", "ParentUserGroupId", "RightBound", "StartISODate", "TaskReallocationProfile", "TaskReallocationStrategy", "UserGroupId" },
                values: new object[] { "e21349f7-ec0c-469b-a44e-e4f4f2aa19f1", null, new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7655), 99999999, new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(7656), 1, null, 2, 0, null, 0, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "$service$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(6747), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(6748) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(6733), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(6735) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "user",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(6760), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(6761) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "workflow_architect",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(6754), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(6754) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "$system$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(6812), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(6812) });

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupRoles_StartISODate_UserId_RoleId",
                table: "UserGroupRoles",
                columns: new[] { "StartISODate", "UserId", "RoleId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupRoles_StartISODate_UserId_UserGroupId",
                table: "UserGroupRoles",
                columns: new[] { "StartISODate", "UserId", "UserGroupId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupRoles_StartISODate_UserId_UserGroupId_RoleId",
                table: "UserGroupRoles",
                columns: new[] { "StartISODate", "UserId", "UserGroupId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcessInstances_DocumentId",
                table: "ProcessInstances",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessInstances_ProcessDefinitionId",
                table: "ProcessInstances",
                column: "ProcessDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessInstances_ProcessInstanceId",
                table: "ProcessInstances",
                column: "ProcessInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessInstances_ProcessKey",
                table: "ProcessInstances",
                column: "ProcessKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessInstances");

            migrationBuilder.DropIndex(
                name: "IX_UserGroupRoles_StartISODate_UserId_RoleId",
                table: "UserGroupRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserGroupRoles_StartISODate_UserId_UserGroupId",
                table: "UserGroupRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserGroupRoles_StartISODate_UserId_UserGroupId_RoleId",
                table: "UserGroupRoles");

            migrationBuilder.DeleteData(
                table: "OrganizationNodes",
                keyColumn: "Id",
                keyValue: "e21349f7-ec0c-469b-a44e-e4f4f2aa19f1");

            migrationBuilder.DropColumn(
                name: "ProcessDataId",
                table: "TaskItems");

            migrationBuilder.AlterColumn<string>(
                name: "EventId",
                table: "TaskItems",
                type: "varchar(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(64)",
                oldMaxLength: 64,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "ACL.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8602), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8602) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Company.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8621), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8622) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Datasource.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8630), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8630) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.AddContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8453), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8453) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Authorize", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8413), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8413) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8395), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8395) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Delete", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8406), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8406) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Edit", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8400), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8400) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Execute", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8483), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8484) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.History", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8471), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8471) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.RemoveContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8459), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8460) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Share", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8465), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8465) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8377), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8378) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.ViewContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8389), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8389) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "DocumentType.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8592), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8593) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Mail.Console", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8709), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8709) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "MailServer.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8648), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8649) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Meta.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8612), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8612) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanCeateRootFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8502), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8502) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanHavePersonalFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8496), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8496) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Client", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8748), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8748) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CreateGenericDocument", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8477), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8478) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.RemoteSignatureService", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8739), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8740) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.SendMail", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8721), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8721) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Signature", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8730), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8731) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Roles.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8583), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8584) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Tables.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8639), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8639) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8557), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8557) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.CreateMessage", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8567), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8567) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8547), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8547) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8575), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8575) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInbox", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8529), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8529) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInboxCC", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8537), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8538) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewDown", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8513), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8514) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewSide", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8519), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8520) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewUp", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8508), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8508) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Template.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8657), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8658) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Workflow.Dashboard", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8489), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8489) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8851), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8851) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "workflow_architect", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8841), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8841) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$ACCOUNTING$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8771), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8772) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$GLOBAL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8344), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8353) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$HR$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8812), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8813) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MAIL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8822), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8822) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MANAGEMENT$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8781), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8782) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PROTOCOL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8759), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8760) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PURCHASE$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8803), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8803) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$SALES$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8793), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8793) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$WORKFLOW$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8824), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8825) });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8036), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8037) });

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: "$MAIL-INBOUND$",
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: "$MAIL-OUTBOUND$",
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: "$PEC-INBOUND$",
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: "$PEC-OUTBOUND$",
                column: "Direction",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DocumentTypes",
                keyColumn: "Id",
                keyValue: "$TEMPLATE$",
                column: "ContentType",
                value: 4);

            migrationBuilder.InsertData(
                table: "OrganizationNodes",
                columns: new[] { "Id", "ClosingNote", "CreationDate", "EndISODate", "LastUpdate", "LeftBound", "ParentUserGroupId", "RightBound", "StartISODate", "TaskReallocationProfile", "TaskReallocationStrategy", "UserGroupId" },
                values: new object[] { "cc474ead-94d4-4dec-832d-00f45dbc82e7", null, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(9043), 99999999, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(9044), 1, null, 2, 0, null, 0, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "$service$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8007), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8007) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(7982), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(7985) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "user",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8021), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8022) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "workflow_architect",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8014), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8014) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "$system$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8069), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8070) });

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupRoles_StartISODate_UserGroupId_UserId_RoleId",
                table: "UserGroupRoles",
                columns: new[] { "StartISODate", "UserGroupId", "UserId", "RoleId" },
                unique: true);
        }
    }
}
