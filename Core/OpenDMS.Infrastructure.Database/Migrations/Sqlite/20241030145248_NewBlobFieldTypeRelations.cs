using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenDMS.Infrastructure.Database.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class NewBlobFieldTypeRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrganizationNodes",
                keyColumn: "Id",
                keyValue: "e94cfe47-1dbb-4622-9880-b90d6dfcdfb5");

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "ACL.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7309), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7309) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Company.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7321), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7321) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Datasource.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7326), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7326) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.AddContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7169), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7169) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Authorize", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7164), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7164) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7148), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7148) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Delete", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7158), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7158) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Edit", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7153), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7153) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Execute", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7196), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7197) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.History", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7186), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7186) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.RemoveContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7175), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7175) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Share", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7180), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7180) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7133), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7133) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.ViewContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7142), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7142) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "DocumentType.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7271), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7271) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Mail.Console", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7348), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7349) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "MailServer.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7336), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7337) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Meta.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7316), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7316) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanCeateRootFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7212), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7212) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanHavePersonalFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7207), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7207) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Client", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7370), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7370) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CreateGenericDocument", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7191), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7191) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.RemoteSignatureService", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7364), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7365) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.SendMail", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7354), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7354) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Signature", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7359), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7360) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Roles.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7266), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7266) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Tables.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7331), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7331) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7250), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7250) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.CreateMessage", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7255), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7255) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7245), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7245) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7260), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7260) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInbox", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7234), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7234) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInboxCC", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7239), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7239) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewDown", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7223), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7223) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewSide", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7229), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7229) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewUp", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7217), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7217) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Template.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7342), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7342) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Workflow.Dashboard", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7202), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7202) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7437), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7437) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "workflow_architect", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7431), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7431) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$ACCOUNTING$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7384), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7385) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$GLOBAL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7073), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7076) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$HR$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7411), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7411) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MAIL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7417), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7417) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MANAGEMENT$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7391), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7391) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PROTOCOL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7377), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7377) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PURCHASE$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7404), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7405) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$SALES$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7397), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7397) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$WORKFLOW$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7419), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7420) });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(6874), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(6874) });

            migrationBuilder.InsertData(
                table: "OrganizationNodes",
                columns: new[] { "Id", "ClosingNote", "CreationDate", "EndISODate", "LastUpdate", "LeftBound", "ParentUserGroupId", "RightBound", "StartISODate", "TaskReallocationProfile", "TaskReallocationStrategy", "UserGroupId" },
                values: new object[] { "80701909-90dd-4433-a442-f4eebf27413f", null, new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7543), 99999999, new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(7544), 1, null, 2, 0, null, 0, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "$service$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(6827), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(6827) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(6814), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(6816) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "user",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(6861), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(6862) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "workflow_architect",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(6833), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(6834) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "$system$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(6900), new DateTime(2024, 10, 30, 14, 52, 48, 282, DateTimeKind.Utc).AddTicks(6900) });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentBlobFields_DocumentTypeId",
                table: "DocumentBlobFields",
                column: "DocumentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentBlobFields_DocumentTypes_DocumentTypeId",
                table: "DocumentBlobFields",
                column: "DocumentTypeId",
                principalTable: "DocumentTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentBlobFields_Documents_DocumentId",
                table: "DocumentBlobFields",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentBlobFields_DocumentTypes_DocumentTypeId",
                table: "DocumentBlobFields");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentBlobFields_Documents_DocumentId",
                table: "DocumentBlobFields");

            migrationBuilder.DropIndex(
                name: "IX_DocumentBlobFields_DocumentTypeId",
                table: "DocumentBlobFields");

            migrationBuilder.DeleteData(
                table: "OrganizationNodes",
                keyColumn: "Id",
                keyValue: "80701909-90dd-4433-a442-f4eebf27413f");

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "ACL.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8418), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8418) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Company.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8429), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8429) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Datasource.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8434), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8434) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.AddContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8287), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8288) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Authorize", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8282), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8282) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8265), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8265) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Delete", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8276), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8276) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Edit", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8271), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8271) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Execute", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8314), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8315) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.History", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8304), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8304) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.RemoveContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8293), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8293) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Share", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8298), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8298) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8251), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8251) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.ViewContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8260), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8260) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "DocumentType.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8412), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8413) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Mail.Console", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8456), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8456) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "MailServer.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8445), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8445) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Meta.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8423), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8424) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanCeateRootFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8330), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8331) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanHavePersonalFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8325), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8325) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Client", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8477), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8478) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CreateGenericDocument", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8309), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8310) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.RemoteSignatureService", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8472), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8472) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.SendMail", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8461), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8461) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Signature", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8466), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8467) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Roles.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8407), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8407) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Tables.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8439), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8439) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8369), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8369) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.CreateMessage", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8395), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8396) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8363), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8364) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8402), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8402) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInbox", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8353), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8353) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInboxCC", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8358), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8358) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewDown", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8341), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8341) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewSide", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8348), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8348) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewUp", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8336), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8336) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Template.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8450), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8450) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Workflow.Dashboard", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8320), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8320) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8567), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8567) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "workflow_architect", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8539), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8540) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$ACCOUNTING$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8491), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8492) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$GLOBAL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8234), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8237) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$HR$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8519), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8519) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MAIL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8525), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8526) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MANAGEMENT$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8498), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8498) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PROTOCOL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8484), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8485) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PURCHASE$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8512), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8513) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$SALES$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8505), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8505) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$WORKFLOW$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8528), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8528) });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8004), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8004) });

            migrationBuilder.InsertData(
                table: "OrganizationNodes",
                columns: new[] { "Id", "ClosingNote", "CreationDate", "EndISODate", "LastUpdate", "LeftBound", "ParentUserGroupId", "RightBound", "StartISODate", "TaskReallocationProfile", "TaskReallocationStrategy", "UserGroupId" },
                values: new object[] { "e94cfe47-1dbb-4622-9880-b90d6dfcdfb5", null, new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8671), 99999999, new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8672), 1, null, 2, 0, null, 0, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "$service$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(7981), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(7981) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(7964), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(7966) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "user",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(7995), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(7996) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "workflow_architect",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(7987), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(7988) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "$system$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8029), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8029) });
        }
    }
}
