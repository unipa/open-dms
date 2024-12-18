using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenDMS.Infrastructure.Database.Migrations.Sqlite
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
                keyValue: "c89f2022-da46-436f-b120-5e40f88855df");

            migrationBuilder.AddColumn<int>(
                name: "ProcessDataId",
                table: "TaskItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProcessInstances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DocumentId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProcessDefinitionId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProcessImageId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProcessInstanceId = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    ProcessEngineId = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    ProcessKey = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    EventName = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    StartUser = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StopDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessInstances", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "ACL.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(245), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(245) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Company.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(254), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(254) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Datasource.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(259), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(259) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.AddContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(103), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(103) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Authorize", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(98), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(98) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(82), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(82) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Delete", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(92), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(92) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Edit", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(87), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(87) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Execute", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(128), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(128) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.History", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(118), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(119) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.RemoveContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(107), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(108) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Share", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(112), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(113) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(65), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(65) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.ViewContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(77), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(77) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "DocumentType.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(240), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(240) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Mail.Console", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(279), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(279) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "MailServer.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(268), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(269) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Meta.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(249), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(249) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanCeateRootFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(143), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(143) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanHavePersonalFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(138), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(139) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Client", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(299), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(300) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CreateGenericDocument", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(123), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(124) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.RemoteSignatureService", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(294), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(294) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.SendMail", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(283), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(284) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Signature", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(289), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(289) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Roles.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(235), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(235) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Tables.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(263), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(264) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(179), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(179) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.CreateMessage", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(224), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(224) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(174), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(175) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(229), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(230) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInbox", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(164), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(164) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInboxCC", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(169), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(169) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewDown", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(153), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(154) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewSide", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(159), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(159) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewUp", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(148), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(148) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Template.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(273), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(273) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Workflow.Dashboard", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(133), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(133) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(391), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(391) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "workflow_architect", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(384), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(385) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$ACCOUNTING$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(313), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(313) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$GLOBAL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(35), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(43) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$HR$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(339), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(340) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MAIL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(346), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(346) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MANAGEMENT$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(319), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(320) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PROTOCOL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(306), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(306) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PURCHASE$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(333), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(333) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$SALES$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(326), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(326) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$WORKFLOW$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(348), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(349) });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 151, DateTimeKind.Utc).AddTicks(9757), new DateTime(2024, 3, 15, 12, 57, 54, 151, DateTimeKind.Utc).AddTicks(9758) });

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
                values: new object[] { "8f3aee64-083b-496b-b54c-4deb7aeb755b", null, new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(479), 99999999, new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(480), 1, null, 2, 0, null, 0, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "$service$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 151, DateTimeKind.Utc).AddTicks(9733), new DateTime(2024, 3, 15, 12, 57, 54, 151, DateTimeKind.Utc).AddTicks(9733) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 151, DateTimeKind.Utc).AddTicks(9718), new DateTime(2024, 3, 15, 12, 57, 54, 151, DateTimeKind.Utc).AddTicks(9720) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "user",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 151, DateTimeKind.Utc).AddTicks(9746), new DateTime(2024, 3, 15, 12, 57, 54, 151, DateTimeKind.Utc).AddTicks(9746) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "workflow_architect",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 151, DateTimeKind.Utc).AddTicks(9740), new DateTime(2024, 3, 15, 12, 57, 54, 151, DateTimeKind.Utc).AddTicks(9740) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "$system$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 151, DateTimeKind.Utc).AddTicks(9794), new DateTime(2024, 3, 15, 12, 57, 54, 151, DateTimeKind.Utc).AddTicks(9795) });

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
                keyValue: "8f3aee64-083b-496b-b54c-4deb7aeb755b");

            migrationBuilder.DropColumn(
                name: "ProcessDataId",
                table: "TaskItems");

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "ACL.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7687), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7687) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Company.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7696), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7696) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Datasource.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7701), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7701) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.AddContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7586), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7587) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Authorize", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7581), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7582) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7538), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7539) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Delete", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7576), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7576) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Edit", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7570), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7570) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Execute", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7613), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7613) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.History", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7602), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7602) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.RemoveContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7592), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7592) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Share", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7596), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7597) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7522), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7522) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.ViewContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7533), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7533) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "DocumentType.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7682), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7682) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Mail.Console", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7753), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7753) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "MailServer.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7743), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7743) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Meta.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7692), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7692) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanCeateRootFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7627), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7627) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanHavePersonalFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7622), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7622) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Client", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7773), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7773) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CreateGenericDocument", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7608), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7608) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.RemoteSignatureService", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7768), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7768) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.SendMail", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7758), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7758) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Signature", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7763), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7763) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Roles.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7677), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7677) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Tables.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7737), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7738) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7663), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7664) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.CreateMessage", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7668), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7668) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7659), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7659) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7672), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7673) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInbox", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7648), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7648) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInboxCC", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7653), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7653) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewDown", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7637), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7637) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewSide", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7643), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7643) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewUp", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7632), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7632) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Template.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7747), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7748) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Workflow.Dashboard", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7617), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7618) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7833), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7833) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "workflow_architect", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7827), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7827) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$ACCOUNTING$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7785), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7785) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$GLOBAL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7493), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7498) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$HR$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7810), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7810) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MAIL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7815), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7816) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MANAGEMENT$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7791), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7791) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PROTOCOL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7779), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7779) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PURCHASE$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7804), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7804) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$SALES$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7797), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7797) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$WORKFLOW$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7817), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7817) });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7239), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7240) });

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
                values: new object[] { "c89f2022-da46-436f-b120-5e40f88855df", null, new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7916), 99999999, new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7916), 1, null, 2, 0, null, 0, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "$service$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7217), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7217) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7202), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7206) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "user",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7229), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7230) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "workflow_architect",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7223), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7224) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "$system$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7294), new DateTime(2024, 2, 29, 14, 43, 34, 782, DateTimeKind.Utc).AddTicks(7294) });

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupRoles_StartISODate_UserGroupId_UserId_RoleId",
                table: "UserGroupRoles",
                columns: new[] { "StartISODate", "UserGroupId", "UserId", "RoleId" },
                unique: true);
        }
    }
}
