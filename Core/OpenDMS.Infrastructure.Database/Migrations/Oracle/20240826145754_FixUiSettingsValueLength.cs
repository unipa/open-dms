using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenDMS.Infrastructure.Database.Migrations.Oracle
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
                keyValue: "637a4896-9b2f-4bf3-b8a1-951f63e02dda");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "UISettings",
                type: "NVARCHAR2(2000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "ACL.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7317), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7318) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Company.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7327), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7327) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Datasource.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7332), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7332) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.AddContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7161), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7161) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Authorize", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7156), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7156) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7139), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7139) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Delete", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7149), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7150) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Edit", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7144), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7144) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Execute", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7187), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7187) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.History", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7177), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7177) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.RemoveContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7166), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7166) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Share", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7171), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7171) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7126), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7126) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.ViewContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7134), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7134) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "DocumentType.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7313), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7313) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Mail.Console", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7353), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7353) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "MailServer.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7342), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7342) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Meta.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7322), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7323) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanCeateRootFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7202), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7202) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanHavePersonalFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7197), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7197) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Client", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7372), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7373) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CreateGenericDocument", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7182), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7182) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.RemoteSignatureService", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7368), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7368) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.SendMail", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7358), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7358) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Signature", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7363), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7363) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Roles.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7308), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7308) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Tables.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7337), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7337) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7237), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7237) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.CreateMessage", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7242), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7242) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7232), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7232) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7301), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7302) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInbox", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7222), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7223) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInboxCC", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7227), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7228) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewDown", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7212), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7212) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewSide", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7218), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7218) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewUp", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7207), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7207) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Template.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7347), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7347) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Workflow.Dashboard", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7192), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7192) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7436), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7437) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "workflow_architect", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7431), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7431) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$ACCOUNTING$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7386), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7386) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$GLOBAL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7105), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7108) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$HR$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7412), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7412) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MAIL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7418), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7418) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MANAGEMENT$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7392), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7393) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PROTOCOL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7379), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7380) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PURCHASE$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7405), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7406) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$SALES$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7398), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7399) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$WORKFLOW$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7420), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7420) });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(6809), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(6809) });

            migrationBuilder.InsertData(
                table: "LookupTables",
                columns: new[] { "Id", "TableId", "Annotation", "Description" },
                values: new object[] { "UserTask.Request", "$EVENTS$", "UserTask", "Request" });

            migrationBuilder.InsertData(
                table: "OrganizationNodes",
                columns: new[] { "Id", "ClosingNote", "CreationDate", "EndISODate", "LastUpdate", "LeftBound", "ParentUserGroupId", "RightBound", "StartISODate", "TaskReallocationProfile", "TaskReallocationStrategy", "UserGroupId" },
                values: new object[] { "63f35706-4b16-4b4f-9528-26f208ee61a4", null, new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7566), 99999999, new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(7567), 1, null, 2, 0, null, 0, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "$service$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(6788), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(6788) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(6771), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(6772) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "user",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(6800), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(6800) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "workflow_architect",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(6794), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(6794) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "$system$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(6831), new DateTime(2024, 8, 26, 14, 57, 53, 601, DateTimeKind.Utc).AddTicks(6832) });
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
                keyValue: "63f35706-4b16-4b4f-9528-26f208ee61a4");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "UISettings",
                type: "NVARCHAR2(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(2000)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "ACL.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2699), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2700) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Company.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2710), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2710) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Datasource.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2741), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2741) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.AddContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2587), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2587) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Authorize", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2581), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2582) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2538), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2538) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Delete", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2575), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2575) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Edit", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2569), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2570) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Execute", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2616), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2617) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.History", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2604), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2605) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.RemoveContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2593), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2593) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Share", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2598), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2599) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2523), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2523) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.ViewContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2532), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2532) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "DocumentType.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2694), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2694) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Mail.Console", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2763), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2763) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "MailServer.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2753), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2753) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Meta.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2705), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2705) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanCeateRootFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2633), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2634) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanHavePersonalFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2628), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2628) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Client", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2784), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2784) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CreateGenericDocument", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2610), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2611) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.RemoteSignatureService", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2779), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2779) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.SendMail", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2768), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2769) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Signature", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2773), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2774) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Roles.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2689), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2689) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Tables.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2747), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2748) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2672), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2673) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.CreateMessage", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2678), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2678) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2667), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2667) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2683), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2684) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInbox", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2655), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2656) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInboxCC", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2661), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2662) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewDown", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2644), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2645) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewSide", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2650), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2650) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewUp", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2639), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2639) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Template.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2757), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2758) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Workflow.Dashboard", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2622), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2622) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2853), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2854) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "workflow_architect", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2847), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2847) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$ACCOUNTING$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2798), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2799) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$GLOBAL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2494), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2503) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$HR$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2825), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2826) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MAIL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2833), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2833) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MANAGEMENT$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2805), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2806) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PROTOCOL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2791), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2792) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PURCHASE$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2819), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2820) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$SALES$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2812), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2812) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$WORKFLOW$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2835), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2835) });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2239), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2239) });

            migrationBuilder.InsertData(
                table: "OrganizationNodes",
                columns: new[] { "Id", "ClosingNote", "CreationDate", "EndISODate", "LastUpdate", "LeftBound", "ParentUserGroupId", "RightBound", "StartISODate", "TaskReallocationProfile", "TaskReallocationStrategy", "UserGroupId" },
                values: new object[] { "637a4896-9b2f-4bf3-b8a1-951f63e02dda", null, new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2957), 99999999, new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2958), 1, null, 2, 0, null, 0, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "$service$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2216), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2216) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2204), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2205) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "user",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2228), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2229) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "workflow_architect",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2223), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2223) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "$system$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2293), new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2294) });
        }
    }
}
