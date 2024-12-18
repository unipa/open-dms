using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenDMS.Infrastructure.Database.Migrations.Oracle
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
                keyValue: "f23de01a-968f-4e7a-9719-e593aa161c10");

            migrationBuilder.AlterColumn<string>(
                name: "EventId",
                table: "TaskItems",
                type: "NVARCHAR2(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(128)",
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProcessDataId",
                table: "TaskItems",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProcessInstances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    DocumentId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ProcessDefinitionId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ProcessImageId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ProcessInstanceId = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: true),
                    ProcessEngineId = table.Column<string>(type: "NVARCHAR2(64)", maxLength: 64, nullable: true),
                    ProcessKey = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: true),
                    EventName = table.Column<string>(type: "NVARCHAR2(64)", maxLength: 64, nullable: true),
                    StartUser = table.Column<string>(type: "NVARCHAR2(64)", maxLength: 64, nullable: true),
                    StartDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    StopDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
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
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7647), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7647) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Company.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7657), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7657) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Datasource.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7701), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7702) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.AddContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7537), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7537) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Authorize", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7531), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7531) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7489), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7489) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Delete", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7525), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7526) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Edit", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7519), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7520) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Execute", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7565), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7565) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.History", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7554), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7554) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.RemoveContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7542), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7542) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Share", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7548), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7548) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7470), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7471) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.ViewContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7483), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7484) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "DocumentType.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7641), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7642) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Mail.Console", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7723), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7724) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "MailServer.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7713), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7713) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Meta.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7652), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7652) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanCeateRootFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7581), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7581) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanHavePersonalFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7576), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7576) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Client", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7745), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7745) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CreateGenericDocument", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7559), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7560) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.RemoteSignatureService", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7739), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7740) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.SendMail", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7729), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7729) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Signature", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7734), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7734) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Roles.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7636), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7636) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Tables.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7708), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7708) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7619), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7620) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.CreateMessage", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7625), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7625) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7614), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7614) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7630), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7631) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInbox", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7604), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7604) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInboxCC", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7609), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7609) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewDown", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7592), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7593) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewSide", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7598), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7599) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewUp", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7587), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7587) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Template.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7718), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7718) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Workflow.Dashboard", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7570), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7571) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7812), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7812) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "workflow_architect", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7805), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7806) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$ACCOUNTING$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7758), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7759) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$GLOBAL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7441), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7448) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$HR$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7786), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7786) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MAIL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7792), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7793) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MANAGEMENT$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7765), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7765) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PROTOCOL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7751), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7752) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PURCHASE$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7779), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7780) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$SALES$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7772), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7772) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$WORKFLOW$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7794), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7794) });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7158), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7159) });

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
                values: new object[] { "0c412e26-34e7-4d3b-9401-f2c5fa55a275", null, new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7952), 99999999, new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7952), 1, null, 2, 0, null, 0, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "$service$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7136), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7137) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7124), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7125) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "user",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7150), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7150) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "workflow_architect",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7143), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7144) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "$system$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7216), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7216) });

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
                filter: "\"UserId\" IS NOT NULL AND \"UserGroupId\" IS NOT NULL AND \"RoleId\" IS NOT NULL");

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
                keyValue: "0c412e26-34e7-4d3b-9401-f2c5fa55a275");

            migrationBuilder.DropColumn(
                name: "ProcessDataId",
                table: "TaskItems");

            migrationBuilder.AlterColumn<string>(
                name: "EventId",
                table: "TaskItems",
                type: "NVARCHAR2(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "ACL.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4753), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4753) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Company.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4763), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4763) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Datasource.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4768), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4768) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.AddContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4650), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4650) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Authorize", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4645), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4645) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4601), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4602) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Delete", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4612), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4613) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Edit", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4607), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4607) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Execute", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4675), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4675) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.History", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4665), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4665) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.RemoveContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4655), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4655) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Share", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4659), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4660) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4586), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4586) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.ViewContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4596), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4596) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "DocumentType.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4748), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4748) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Mail.Console", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4819), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4819) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "MailServer.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4778), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4779) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Meta.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4758), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4758) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanCeateRootFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4690), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4690) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanHavePersonalFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4685), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4685) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Client", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4840), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4840) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CreateGenericDocument", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4670), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4670) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.RemoteSignatureService", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4834), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4835) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.SendMail", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4824), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4825) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Signature", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4829), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4829) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Roles.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4743), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4743) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Tables.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4773), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4774) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4727), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4727) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.CreateMessage", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4733), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4733) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4722), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4722) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4738), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4738) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInbox", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4711), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4712) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInboxCC", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4717), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4718) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewDown", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4701), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4701) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewSide", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4706), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4707) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewUp", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4695), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4696) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Template.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4812), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4813) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Workflow.Dashboard", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4680), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4681) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4904), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4905) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "workflow_architect", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4898), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4898) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$ACCOUNTING$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4853), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4854) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$GLOBAL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4556), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4564) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$HR$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4878), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4879) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MAIL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4884), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4885) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MANAGEMENT$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4859), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4859) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PROTOCOL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4846), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4847) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PURCHASE$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4872), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4872) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$SALES$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4865), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4865) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$WORKFLOW$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4886), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4886) });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4300), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4300) });

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
                values: new object[] { "f23de01a-968f-4e7a-9719-e593aa161c10", null, new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4994), 99999999, new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4994), 1, null, 2, 0, null, 0, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "$service$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4279), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4279) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4264), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4265) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "user",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4291), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4291) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "workflow_architect",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4285), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4285) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "$system$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4334), new DateTime(2024, 2, 29, 14, 44, 8, 788, DateTimeKind.Utc).AddTicks(4334) });

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupRoles_StartISODate_UserGroupId_UserId_RoleId",
                table: "UserGroupRoles",
                columns: new[] { "StartISODate", "UserGroupId", "UserId", "RoleId" },
                unique: true,
                filter: "\"UserGroupId\" IS NOT NULL AND \"UserId\" IS NOT NULL AND \"RoleId\" IS NOT NULL");
        }
    }
}
