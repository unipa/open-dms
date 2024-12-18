using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenDMS.Infrastructure.Database.Migrations.SqlServer
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
                keyValue: "e40ab004-c155-4f4c-90d3-5fa72b506c93");

            migrationBuilder.AlterColumn<string>(
                name: "EventId",
                table: "TaskItems",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128,
                oldNullable: true);

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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    ProcessDefinitionId = table.Column<int>(type: "int", nullable: false),
                    ProcessImageId = table.Column<int>(type: "int", nullable: false),
                    ProcessInstanceId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ProcessEngineId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ProcessKey = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EventName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    StartUser = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StopDate = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(180), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(181) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Company.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(191), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(191) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Datasource.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(196), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(196) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.AddContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(53), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(53) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Authorize", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(47), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(48) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(30), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(31) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Delete", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(41), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(41) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Edit", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(36), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(36) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Execute", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(103), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(103) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.History", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(69), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(69) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.RemoveContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(58), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(58) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Share", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(63), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(63) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(13), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(13) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.ViewContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(25), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(25) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "DocumentType.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(175), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(176) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Mail.Console", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(218), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(218) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "MailServer.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(206), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(207) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Meta.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(185), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(185) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanCeateRootFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(118), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(118) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanHavePersonalFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(113), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(114) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Client", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(278), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(279) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CreateGenericDocument", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(98), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(98) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.RemoteSignatureService", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(273), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(273) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.SendMail", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(223), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(223) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Signature", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(267), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(267) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Roles.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(171), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(171) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Tables.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(201), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(201) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(156), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(156) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.CreateMessage", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(160), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(161) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(151), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(151) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(166), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(166) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInbox", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(140), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(140) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInboxCC", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(145), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(146) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewDown", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(129), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(129) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewSide", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(135), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(135) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewUp", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(123), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(124) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Template.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(212), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(212) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Workflow.Dashboard", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(108), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(109) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(342), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(342) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "workflow_architect", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(336), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(336) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$ACCOUNTING$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(292), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(292) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$GLOBAL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 20, DateTimeKind.Utc).AddTicks(9974), new DateTime(2024, 3, 15, 12, 58, 13, 20, DateTimeKind.Utc).AddTicks(9987) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$HR$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(317), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(318) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MAIL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(324), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(324) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MANAGEMENT$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(299), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(299) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PROTOCOL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(285), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(285) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PURCHASE$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(311), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(311) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$SALES$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(304), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(305) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$WORKFLOW$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(326), new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(326) });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 20, DateTimeKind.Utc).AddTicks(9616), new DateTime(2024, 3, 15, 12, 58, 13, 20, DateTimeKind.Utc).AddTicks(9616) });

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
                values: new object[] { "01f194ac-f594-45fc-91a0-e5c35de84bd3", null, new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(447), 99999999, new DateTime(2024, 3, 15, 12, 58, 13, 21, DateTimeKind.Utc).AddTicks(447), 1, null, 2, 0, null, 0, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "$service$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 20, DateTimeKind.Utc).AddTicks(9592), new DateTime(2024, 3, 15, 12, 58, 13, 20, DateTimeKind.Utc).AddTicks(9593) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 20, DateTimeKind.Utc).AddTicks(9575), new DateTime(2024, 3, 15, 12, 58, 13, 20, DateTimeKind.Utc).AddTicks(9578) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "user",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 20, DateTimeKind.Utc).AddTicks(9605), new DateTime(2024, 3, 15, 12, 58, 13, 20, DateTimeKind.Utc).AddTicks(9606) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "workflow_architect",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 20, DateTimeKind.Utc).AddTicks(9599), new DateTime(2024, 3, 15, 12, 58, 13, 20, DateTimeKind.Utc).AddTicks(9600) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "$system$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 20, DateTimeKind.Utc).AddTicks(9662), new DateTime(2024, 3, 15, 12, 58, 13, 20, DateTimeKind.Utc).AddTicks(9663) });

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
                unique: true,
                filter: "[UserId] IS NOT NULL AND [UserGroupId] IS NOT NULL AND [RoleId] IS NOT NULL");

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
                keyValue: "01f194ac-f594-45fc-91a0-e5c35de84bd3");

            migrationBuilder.DropColumn(
                name: "ProcessDataId",
                table: "TaskItems");

            migrationBuilder.AlterColumn<string>(
                name: "EventId",
                table: "TaskItems",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "ACL.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7699), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7699) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Company.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7710), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7710) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Datasource.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7715), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7715) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.AddContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7558), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7558) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Authorize", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7552), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7553) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7534), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7534) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Delete", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7546), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7546) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Edit", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7539), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7540) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Execute", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7610), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7610) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.History", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7599), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7600) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.RemoveContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7564), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7564) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Share", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7569), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7569) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7521), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7521) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.ViewContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7529), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7529) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "DocumentType.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7693), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7693) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Mail.Console", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7736), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7737) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "MailServer.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7725), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7725) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Meta.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7704), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7705) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanCeateRootFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7625), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7626) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanHavePersonalFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7620), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7621) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Client", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7781), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7781) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CreateGenericDocument", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7605), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7605) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.RemoteSignatureService", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7776), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7776) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.SendMail", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7741), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7742) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Signature", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7770), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7771) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Roles.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7688), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7688) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Tables.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7720), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7721) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7670), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7670) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.CreateMessage", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7676), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7676) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7664), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7664) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7682), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7682) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInbox", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7652), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7653) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInboxCC", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7658), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7659) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewDown", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7637), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7637) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewSide", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7647), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7647) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewUp", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7631), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7632) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Template.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7730), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7731) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Workflow.Dashboard", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7615), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7615) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7848), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7848) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "workflow_architect", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7841), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7841) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$ACCOUNTING$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7794), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7794) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$GLOBAL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7493), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7498) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$HR$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7822), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7822) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MAIL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7828), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7829) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MANAGEMENT$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7801), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7801) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PROTOCOL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7787), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7788) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PURCHASE$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7815), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7816) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$SALES$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7807), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7808) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$WORKFLOW$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7830), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7831) });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7237), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7237) });

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
                values: new object[] { "e40ab004-c155-4f4c-90d3-5fa72b506c93", null, new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7935), 99999999, new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7936), 1, null, 2, 0, null, 0, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "$service$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7215), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7215) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7198), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7201) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "user",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7227), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7228) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "workflow_architect",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7222), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7222) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "$system$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7264), new DateTime(2024, 2, 29, 14, 43, 51, 895, DateTimeKind.Utc).AddTicks(7265) });

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupRoles_StartISODate_UserGroupId_UserId_RoleId",
                table: "UserGroupRoles",
                columns: new[] { "StartISODate", "UserGroupId", "UserId", "RoleId" },
                unique: true,
                filter: "[UserGroupId] IS NOT NULL AND [UserId] IS NOT NULL AND [RoleId] IS NOT NULL");
        }
    }
}
