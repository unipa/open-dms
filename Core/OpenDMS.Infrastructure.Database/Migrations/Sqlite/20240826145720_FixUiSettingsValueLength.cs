using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenDMS.Infrastructure.Database.Migrations.Sqlite
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
                keyValue: "adb0483a-3905-410b-8d22-1e4febfe7208");

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "ACL.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6538), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6539) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Company.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6549), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6550) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Datasource.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6555), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6555) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.AddContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6403), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6403) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Authorize", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6397), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6397) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6380), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6380) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Delete", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6391), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6391) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Edit", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6385), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6386) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Execute", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6430), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6431) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.History", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6420), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6420) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.RemoveContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6408), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6408) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Share", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6413), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6414) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6366), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6366) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.ViewContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6375), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6375) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "DocumentType.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6533), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6533) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Mail.Console", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6578), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6578) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "MailServer.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6566), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6566) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Meta.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6544), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6544) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanCeateRootFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6447), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6447) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanHavePersonalFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6441), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6442) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Client", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6599), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6600) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CreateGenericDocument", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6425), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6425) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.RemoteSignatureService", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6594), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6594) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.SendMail", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6583), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6583) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Signature", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6588), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6589) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Roles.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6527), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6527) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Tables.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6560), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6560) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6511), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6511) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.CreateMessage", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6517), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6517) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6506), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6506) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6522), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6522) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInbox", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6469), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6469) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInboxCC", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6500), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6500) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewDown", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6458), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6458) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewSide", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6464), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6464) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewUp", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6452), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6452) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Template.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6571), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6572) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Workflow.Dashboard", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6436), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6436) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6699), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6699) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "workflow_architect", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6692), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6692) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$ACCOUNTING$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6614), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6615) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$GLOBAL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6341), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6344) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$HR$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6642), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6643) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MAIL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6675), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6677) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MANAGEMENT$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6621), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6622) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PROTOCOL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6607), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6608) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PURCHASE$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6636), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6636) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$SALES$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6628), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6629) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$WORKFLOW$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6679), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6680) });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6049), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6050) });

            migrationBuilder.InsertData(
                table: "LookupTables",
                columns: new[] { "Id", "TableId", "Annotation", "Description" },
                values: new object[] { "UserTask.Request", "$EVENTS$", "UserTask", "Request" });

            migrationBuilder.InsertData(
                table: "OrganizationNodes",
                columns: new[] { "Id", "ClosingNote", "CreationDate", "EndISODate", "LastUpdate", "LeftBound", "ParentUserGroupId", "RightBound", "StartISODate", "TaskReallocationProfile", "TaskReallocationStrategy", "UserGroupId" },
                values: new object[] { "4c9fd124-ac8a-420d-b8f4-8d959d790cdc", null, new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6830), 99999999, new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6830), 1, null, 2, 0, null, 0, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "$service$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6025), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6025) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6012), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6014) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "user",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6038), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6038) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "workflow_architect",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6031), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6032) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "$system$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6083), new DateTime(2024, 8, 26, 14, 57, 20, 182, DateTimeKind.Utc).AddTicks(6084) });
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
                keyValue: "4c9fd124-ac8a-420d-b8f4-8d959d790cdc");

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "ACL.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(725), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(725) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Company.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(736), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(736) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Datasource.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(742), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(742) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.AddContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(564), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(564) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Authorize", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(559), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(559) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(541), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(541) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Delete", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(552), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(553) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Edit", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(547), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(547) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Execute", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(593), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(594) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.History", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(582), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(583) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.RemoveContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(570), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(570) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Share", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(575), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(576) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(526), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(526) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.ViewContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(535), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(535) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "DocumentType.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(719), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(719) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Mail.Console", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(764), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(764) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "MailServer.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(752), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(753) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Meta.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(730), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(730) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanCeateRootFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(610), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(610) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanHavePersonalFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(605), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(605) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Client", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(786), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(786) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CreateGenericDocument", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(588), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(588) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.RemoteSignatureService", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(780), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(781) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.SendMail", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(769), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(770) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Signature", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(775), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(775) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Roles.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(713), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(713) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Tables.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(747), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(747) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(696), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(697) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.CreateMessage", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(702), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(702) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(691), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(692) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(708), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(708) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInbox", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(680), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(680) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInboxCC", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(686), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(686) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewDown", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(620), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(621) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewSide", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(673), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(673) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewUp", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(615), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(615) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Template.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(758), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(759) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Workflow.Dashboard", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(599), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(599) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(882), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(882) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "workflow_architect", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(875), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(876) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$ACCOUNTING$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(800), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(800) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$GLOBAL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(494), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(505) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$HR$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(855), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(856) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MAIL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(863), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(863) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MANAGEMENT$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(806), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(806) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PROTOCOL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(793), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(793) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PURCHASE$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(821), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(821) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$SALES$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(813), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(813) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$WORKFLOW$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(865), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(865) });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(196), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(197) });

            migrationBuilder.InsertData(
                table: "OrganizationNodes",
                columns: new[] { "Id", "ClosingNote", "CreationDate", "EndISODate", "LastUpdate", "LeftBound", "ParentUserGroupId", "RightBound", "StartISODate", "TaskReallocationProfile", "TaskReallocationStrategy", "UserGroupId" },
                values: new object[] { "adb0483a-3905-410b-8d22-1e4febfe7208", null, new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(1025), 99999999, new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(1026), 1, null, 2, 0, null, 0, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "$service$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(169), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(170) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(153), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(157) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "user",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(183), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(183) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "workflow_architect",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(176), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(177) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "$system$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(231), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(232) });
        }
    }
}
