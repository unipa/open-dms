using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenDMS.Infrastructure.Database.Migrations.MySql
{
    /// <inheritdoc />
    public partial class VisibilityToSchemaFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrganizationNodes",
                keyColumn: "Id",
                keyValue: "e21349f7-ec0c-469b-a44e-e4f4f2aa19f1");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Visibility",
                table: "DocumentTypeFields",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                table: "AppSettings",
                keyColumns: new[] { "CompanyId", "Name" },
                keyValues: new object[] { 0, "Template.Exception.Body" },
                column: "Value",
                value: "{{description}}");

            migrationBuilder.UpdateData(
                table: "AppSettings",
                keyColumns: new[] { "CompanyId", "Name" },
                keyValues: new object[] { 0, "Template.NoReferents.Body" },
                column: "Value",
                value: "Il fascicolo è: {{Document.Description}}<br/><br/>{{Task.Description}}<br/> {{Sender.FullName}}");

            migrationBuilder.UpdateData(
                table: "AppSettings",
                keyColumns: new[] { "CompanyId", "Name" },
                keyValues: new object[] { 0, "Template.Notification.Body" },
                column: "Value",
                value: "{{description}}<br/><br/>{{Sender.FullName}}");

            migrationBuilder.UpdateData(
                table: "AppSettings",
                keyColumns: new[] { "CompanyId", "Name" },
                keyValues: new object[] { 0, "Template.Notification.CC.Body" },
                column: "Value",
                value: "{{description}}<br/><br/>{{Sender.FullName}}");

            migrationBuilder.UpdateData(
                table: "AppSettings",
                keyColumns: new[] { "CompanyId", "Name" },
                keyValues: new object[] { 0, "Template.Notification.CC.Title" },
                column: "Value",
                value: "Hai un nuovo messaggio: {{title}}");

            migrationBuilder.UpdateData(
                table: "AppSettings",
                keyColumns: new[] { "CompanyId", "Name" },
                keyValues: new object[] { 0, "Template.Notification.Title" },
                column: "Value",
                value: "Hai una nuova attività: {{title}}");

            migrationBuilder.UpdateData(
                table: "AppSettings",
                keyColumns: new[] { "CompanyId", "Name" },
                keyValues: new object[] { 0, "Template.Referents.Body" },
                column: "Value",
                value: "Il fascicolo è: {{Document.Description}}<br/><br/>{{Task.Description}}<br/> {{Sender.FullName}}");

            migrationBuilder.UpdateData(
                table: "AppSettings",
                keyColumns: new[] { "CompanyId", "Name" },
                keyValues: new object[] { 0, "Template.Referents.Changes.Body" },
                column: "Value",
                value: "Il documento modificato è: {{Document.Description}}<br/>");

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
                columns: new[] { "CreationDate", "Email", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9105), "", new DateTime(2024, 5, 9, 8, 30, 25, 290, DateTimeKind.Utc).AddTicks(9105) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrganizationNodes",
                keyColumn: "Id",
                keyValue: "cb493763-db38-48be-83a0-d6466b89404b");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Visibility",
                table: "DocumentTypeFields");

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
                table: "AppSettings",
                keyColumns: new[] { "CompanyId", "Name" },
                keyValues: new object[] { 0, "Template.Exception.Body" },
                column: "Value",
                value: "{description}");

            migrationBuilder.UpdateData(
                table: "AppSettings",
                keyColumns: new[] { "CompanyId", "Name" },
                keyValues: new object[] { 0, "Template.NoReferents.Body" },
                column: "Value",
                value: "Il fascicolo è: {Document.Description}<br/><br/>{Task.Description}<br/> {Sender.FullName}");

            migrationBuilder.UpdateData(
                table: "AppSettings",
                keyColumns: new[] { "CompanyId", "Name" },
                keyValues: new object[] { 0, "Template.Notification.Body" },
                column: "Value",
                value: "{description}<br/><br/>{Sender.FullName}");

            migrationBuilder.UpdateData(
                table: "AppSettings",
                keyColumns: new[] { "CompanyId", "Name" },
                keyValues: new object[] { 0, "Template.Notification.CC.Body" },
                column: "Value",
                value: "{description}<br/><br/>{Sender.FullName}");

            migrationBuilder.UpdateData(
                table: "AppSettings",
                keyColumns: new[] { "CompanyId", "Name" },
                keyValues: new object[] { 0, "Template.Notification.CC.Title" },
                column: "Value",
                value: "Hai un nuovo messaggio: {Task.Title}");

            migrationBuilder.UpdateData(
                table: "AppSettings",
                keyColumns: new[] { "CompanyId", "Name" },
                keyValues: new object[] { 0, "Template.Notification.Title" },
                column: "Value",
                value: "Hai una nuova attività: {Task.Title}");

            migrationBuilder.UpdateData(
                table: "AppSettings",
                keyColumns: new[] { "CompanyId", "Name" },
                keyValues: new object[] { 0, "Template.Referents.Body" },
                column: "Value",
                value: "Il fascicolo è: {Document.Description}<br/><br/>{Task.Description}<br/> {Sender.FullName}");

            migrationBuilder.UpdateData(
                table: "AppSettings",
                keyColumns: new[] { "CompanyId", "Name" },
                keyValues: new object[] { 0, "Template.Referents.Changes.Body" },
                column: "Value",
                value: "Il documento modificato è: {Document.Description}<br/>");

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(6773), new DateTime(2024, 3, 15, 12, 57, 33, 531, DateTimeKind.Utc).AddTicks(6773) });

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
        }
    }
}
