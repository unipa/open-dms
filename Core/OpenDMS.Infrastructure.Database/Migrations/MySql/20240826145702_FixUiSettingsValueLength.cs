using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenDMS.Infrastructure.Database.Migrations.MySql
{
    /// <inheritdoc />
    public partial class FixUiSettingsValueLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrganizationNodes",
                keyColumn: "Id",
                keyValue: "cb493763-db38-48be-83a0-d6466b89404b");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "UISettings",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "ACL.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6057), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6058) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Company.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6092), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6093) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Datasource.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6098), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6098) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.AddContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5959), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5959) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Authorize", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5954), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5954) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5938), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5938) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Delete", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5948), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5949) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Edit", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5943), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5943) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Execute", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5984), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5984) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.History", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5974), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5974) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.RemoveContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5964), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5964) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Share", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5969), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5969) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5897), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5897) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.ViewContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5905), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5905) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "DocumentType.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6053), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6053) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Mail.Console", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6119), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6119) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "MailServer.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6108), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6108) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Meta.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6062), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6063) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanCeateRootFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5998), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5998) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanHavePersonalFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5993), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5993) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Client", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6138), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6138) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CreateGenericDocument", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5979), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5979) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.RemoteSignatureService", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6133), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6133) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.SendMail", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6123), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6124) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Signature", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6128), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6128) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Roles.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6048), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6048) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Tables.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6103), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6104) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6033), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6033) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.CreateMessage", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6038), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6038) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6028), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6029) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6043), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6043) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInbox", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6019), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6019) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInboxCC", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6024), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6024) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewDown", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6008), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6008) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewSide", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6014), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6014) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewUp", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6003), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6003) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Template.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6113), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6113) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Workflow.Dashboard", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5989), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5989) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6202), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6202) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "workflow_architect", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6195), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6195) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$ACCOUNTING$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6151), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6152) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$GLOBAL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5881), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5883) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$HR$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6177), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6177) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MAIL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6183), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6183) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MANAGEMENT$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6157), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6158) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PROTOCOL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6145), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6145) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PURCHASE$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6171), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6171) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$SALES$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6164), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6164) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$WORKFLOW$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6185), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6185) });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5658), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5658) });

            migrationBuilder.InsertData(
                table: "LookupTables",
                columns: new[] { "Id", "TableId", "Annotation", "Description" },
                values: new object[] { "UserTask.Request", "$EVENTS$", "UserTask", "Request" });

            migrationBuilder.InsertData(
                table: "OrganizationNodes",
                columns: new[] { "Id", "ClosingNote", "CreationDate", "EndISODate", "LastUpdate", "LeftBound", "ParentUserGroupId", "RightBound", "StartISODate", "TaskReallocationProfile", "TaskReallocationStrategy", "UserGroupId" },
                values: new object[] { "6273a941-b61f-4470-978d-d9cf03c2f938", null, new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6303), 99999999, new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6303), 1, null, 2, 0, null, 0, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "$service$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5635), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5635) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5624), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5625) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "user",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5647), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5647) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "workflow_architect",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5641), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5642) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "$system$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5702), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5702) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LookupTables",
                keyColumns: new[] { "Id", "TableId" },
                keyValues: new object[] { "UserTask.Request", "$EVENTS$" });

            migrationBuilder.DeleteData(
                table: "OrganizationNodes",
                keyColumn: "Id",
                keyValue: "6273a941-b61f-4470-978d-d9cf03c2f938");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "UISettings",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "ACL.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9625) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Company.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9637), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9637) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Datasource.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9643), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9643) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.AddContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9508), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9508) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Authorize", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9501), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9502) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9444), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9444) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Delete", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9456), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9456) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Edit", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9450), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9450) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Execute", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9538), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9538) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.History", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9526), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9526) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.RemoveContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9513), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9514) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Share", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9519), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9520) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9428), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9429) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.ViewContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9438), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9438) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "DocumentType.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9619), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9620) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Mail.Console", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9701), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9701) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "MailServer.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9655), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9655) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Meta.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9631), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9631) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanCeateRootFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9555), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9555) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanHavePersonalFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9549), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9549) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Client", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9751), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9752) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CreateGenericDocument", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9532), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9532) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.RemoteSignatureService", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9719), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9719) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.SendMail", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9707), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9707) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Signature", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9713), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9713) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Roles.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9613), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9614) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Tables.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9649), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9649) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9595), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9595) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.CreateMessage", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9601), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9601) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9589), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9589) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9607), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9607) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInbox", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9577), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9577) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInboxCC", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9583), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9583) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewDown", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9566), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9566) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewSide", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9572), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9572) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewUp", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9560), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9561) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Template.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9694), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9694) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Workflow.Dashboard", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9544), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9544) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9823), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9823) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "workflow_architect", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9816), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9817) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$ACCOUNTING$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9767), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9768) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$GLOBAL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9395), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9401) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$HR$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9795), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9796) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MAIL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9803), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9803) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MANAGEMENT$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9774), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9775) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PROTOCOL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9759), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9760) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PURCHASE$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9788), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9789) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$SALES$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9781), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9782) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$WORKFLOW$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9805), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9805) });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9070), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9070) });

            migrationBuilder.InsertData(
                table: "OrganizationNodes",
                columns: new[] { "Id", "ClosingNote", "CreationDate", "EndISODate", "LastUpdate", "LeftBound", "ParentUserGroupId", "RightBound", "StartISODate", "TaskReallocationProfile", "TaskReallocationStrategy", "UserGroupId" },
                values: new object[] { "cb493763-db38-48be-83a0-d6466b89404b", null, new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9915), 99999999, new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9915), 1, null, 2, 0, null, 0, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "$service$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9041), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9041) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9027), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9029) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "user",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9055), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9055) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "workflow_architect",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9048), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9048) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "$system$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9105), new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9105) });
        }
    }
}
