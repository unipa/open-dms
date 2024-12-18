using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenDMS.Infrastructure.Database.Migrations.SqlServer
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
                keyValue: "8005f01e-41e6-4fc4-88b8-316865564ca0");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "UISettings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "ACL.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9646), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9647) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Company.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9657), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9657) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Datasource.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9662), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9662) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.AddContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9474), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9474) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Authorize", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9468), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9469) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9452), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9452) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Delete", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9462), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9462) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Edit", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9457), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9457) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Execute", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9566), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9567) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.History", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9490), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9490) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.RemoveContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9479), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9479) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Share", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9484), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9484) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9437), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9437) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.ViewContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9446), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9446) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "DocumentType.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9641), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9642) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Mail.Console", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9684), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9684) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "MailServer.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9672), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9673) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Meta.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9651), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9652) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanCeateRootFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9583), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9583) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanHavePersonalFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9578), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9578) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Client", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9755), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9756) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CreateGenericDocument", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9495), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9495) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.RemoteSignatureService", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9749), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9750) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.SendMail", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9689), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9689) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Signature", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9694), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9695) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Roles.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9636), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9636) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Tables.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9667), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9667) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9620), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9621) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.CreateMessage", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9626) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9615), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9615) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9631), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9631) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInbox", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9605), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9605) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInboxCC", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9610), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9610) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewDown", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9593), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9594) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewSide", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9600), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9600) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewUp", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9588), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9589) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Template.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9678), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9678) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Workflow.Dashboard", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9573), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9573) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9821), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9822) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "workflow_architect", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9815), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9816) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$ACCOUNTING$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9770), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9770) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$GLOBAL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9420), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9423) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$HR$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9796), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9797) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MAIL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9803), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9803) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MANAGEMENT$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9776), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9777) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PROTOCOL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9763), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9764) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PURCHASE$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9790), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9791) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$SALES$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9783), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9783) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$WORKFLOW$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9805), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9805) });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9159), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9160) });

            migrationBuilder.InsertData(
                table: "LookupTables",
                columns: new[] { "Id", "TableId", "Annotation", "Description" },
                values: new object[] { "UserTask.Request", "$EVENTS$", "UserTask", "Request" });

            migrationBuilder.InsertData(
                table: "OrganizationNodes",
                columns: new[] { "Id", "ClosingNote", "CreationDate", "EndISODate", "LastUpdate", "LeftBound", "ParentUserGroupId", "RightBound", "StartISODate", "TaskReallocationProfile", "TaskReallocationStrategy", "UserGroupId" },
                values: new object[] { "88264912-6588-49bc-9db6-cb9de0060f90", null, new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9882), 99999999, new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9883), 1, null, 2, 0, null, 0, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "$service$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9139), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9139) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9123), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9130) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "user",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9150), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9151) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "workflow_architect",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9145), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9145) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "$system$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9180), new DateTime(2024, 8, 26, 14, 57, 38, 166, DateTimeKind.Utc).AddTicks(9180) });
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
                keyValue: "88264912-6588-49bc-9db6-cb9de0060f90");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "UISettings",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "ACL.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2964), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2965) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Company.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3007), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3007) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Datasource.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3012), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3013) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.AddContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2859), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2859) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Authorize", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2854), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2854) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2837), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2837) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Delete", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2848), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2848) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Edit", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2842), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2843) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Execute", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2886), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2886) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.History", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2875), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2875) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.RemoveContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2864), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2864) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Share", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2869), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2869) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2790), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2790) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.ViewContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2830), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2830) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "DocumentType.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2959), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2959) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Mail.Console", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3034), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3034) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "MailServer.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3023), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3023) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Meta.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3001), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3001) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanCeateRootFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2901), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2901) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanHavePersonalFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2896), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2896) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Client", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3056), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3056) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CreateGenericDocument", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2880), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2881) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.RemoteSignatureService", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3050), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3051) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.SendMail", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3039), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3039) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Signature", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3045), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3045) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Roles.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2954), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2954) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Tables.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3017), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3018) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2938), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2939) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.CreateMessage", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2943), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2943) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2933), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2933) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2949), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2949) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInbox", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2922), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2923) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInboxCC", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2928), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2928) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewDown", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2911), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2912) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewSide", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2917), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2918) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewUp", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2906), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2906) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Template.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3028), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3028) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Workflow.Dashboard", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2891), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2891) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3123), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3123) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "workflow_architect", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3116), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3116) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$ACCOUNTING$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3070), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3070) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$GLOBAL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2757), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2762) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$HR$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3096), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3097) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MAIL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3103), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3104) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MANAGEMENT$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3076), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3077) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PROTOCOL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3063), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3063) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PURCHASE$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3090), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3090) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$SALES$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3083), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3083) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$WORKFLOW$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3105), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3106) });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2495), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2495) });

            migrationBuilder.InsertData(
                table: "OrganizationNodes",
                columns: new[] { "Id", "ClosingNote", "CreationDate", "EndISODate", "LastUpdate", "LeftBound", "ParentUserGroupId", "RightBound", "StartISODate", "TaskReallocationProfile", "TaskReallocationStrategy", "UserGroupId" },
                values: new object[] { "8005f01e-41e6-4fc4-88b8-316865564ca0", null, new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3216), 99999999, new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(3216), 1, null, 2, 0, null, 0, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "$service$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2443), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2443) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2430), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2432) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "user",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2457), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2457) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "workflow_architect",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2449), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2450) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "$system$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2534), new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2534) });
        }
    }
}
