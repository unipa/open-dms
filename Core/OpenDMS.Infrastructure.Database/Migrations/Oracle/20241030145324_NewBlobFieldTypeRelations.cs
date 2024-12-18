using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenDMS.Infrastructure.Database.Migrations.Oracle
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
                keyValue: "89249bb1-6440-4a9e-be67-7af86aed6b6d");

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "ACL.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3109), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3109) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Company.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3121), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3121) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Datasource.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3127), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3127) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.AddContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2972), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2972) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Authorize", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2966), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2966) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2949), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2949) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Delete", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2960), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2960) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Edit", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2954), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2955) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Execute", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3000), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3001) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.History", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2989), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2989) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.RemoveContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2977), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2978) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Share", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2983), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2983) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2932), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2932) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.ViewContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2943), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2943) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "DocumentType.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3079), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3079) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Mail.Console", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3150), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3150) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "MailServer.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3138), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3138) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Meta.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3116), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3116) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanCeateRootFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3017), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3017) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanHavePersonalFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3012), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3012) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Client", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3172), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3172) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CreateGenericDocument", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2995), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2995) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.RemoteSignatureService", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3167), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3167) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.SendMail", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3155), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3156) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Signature", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3161), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3161) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Roles.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3073), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3073) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Tables.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3132), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3133) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3057), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3057) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.CreateMessage", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3062), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3062) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3051), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3051) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3068), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3068) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInbox", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3040), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3040) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInboxCC", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3046), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3046) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewDown", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3028), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3029) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewSide", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3035), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3035) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewUp", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3023), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3023) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Template.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3143), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3144) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Workflow.Dashboard", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3006), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3006) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3241), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3241) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "workflow_architect", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3234), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3235) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$ACCOUNTING$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3187), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3187) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$GLOBAL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2870), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2874) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$HR$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3214), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3215) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MAIL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3221), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3222) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MANAGEMENT$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3193), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3194) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PROTOCOL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3179), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3180) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PURCHASE$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3208), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3208) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$SALES$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3200), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3201) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$WORKFLOW$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3223), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3223) });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2674), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2675) });

            migrationBuilder.InsertData(
                table: "OrganizationNodes",
                columns: new[] { "Id", "ClosingNote", "CreationDate", "EndISODate", "LastUpdate", "LeftBound", "ParentUserGroupId", "RightBound", "StartISODate", "TaskReallocationProfile", "TaskReallocationStrategy", "UserGroupId" },
                values: new object[] { "7f8c97ef-0764-4614-8933-c057d9dafa01", null, new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3317), 99999999, new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(3317), 1, null, 2, 0, null, 0, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "$service$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2629), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2629) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2618), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2619) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "user",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2662), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2662) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "workflow_architect",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2636), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2636) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "$system$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2698), new DateTime(2024, 10, 30, 14, 53, 24, 420, DateTimeKind.Utc).AddTicks(2698) });

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
                keyValue: "7f8c97ef-0764-4614-8933-c057d9dafa01");

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "ACL.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8332), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8332) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Company.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8342), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8343) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Datasource.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8348), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8348) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.AddContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8193), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8193) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Authorize", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8188), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8188) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8171), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8171) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Delete", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8182), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8182) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Edit", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8177), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8177) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Execute", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8220), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8220) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.History", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8210), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8210) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.RemoveContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8198), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8199) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Share", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8204), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8204) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8145), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8146) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.ViewContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8165), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8166) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "DocumentType.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8327), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8327) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Mail.Console", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8370), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8370) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "MailServer.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8358), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8359) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Meta.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8337), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8337) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanCeateRootFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8268), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8268) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanHavePersonalFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8230), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8231) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Client", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8390), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8391) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CreateGenericDocument", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8215), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8215) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.RemoteSignatureService", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8385), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8385) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.SendMail", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8375), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8375) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Signature", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8380), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8380) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Roles.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8322), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8322) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Tables.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8353), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8353) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8306), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8306) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.CreateMessage", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8311), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8312) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8301), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8301) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8316), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8317) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInbox", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8290), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8291) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInboxCC", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8295), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8296) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewDown", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8279), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8279) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewSide", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8285), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8285) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewUp", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8274), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8274) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Template.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8364), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8364) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Workflow.Dashboard", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8225), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8226) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8481), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8481) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "workflow_architect", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8474), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8475) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$ACCOUNTING$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8427), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8428) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$GLOBAL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8114), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8117) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$HR$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8455), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8456) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MAIL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8462), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8462) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MANAGEMENT$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8436), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8436) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PROTOCOL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8398), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8399) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PURCHASE$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8449), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8450) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$SALES$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8442), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8443) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$WORKFLOW$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8463), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8464) });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(7883), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(7884) });

            migrationBuilder.InsertData(
                table: "OrganizationNodes",
                columns: new[] { "Id", "ClosingNote", "CreationDate", "EndISODate", "LastUpdate", "LeftBound", "ParentUserGroupId", "RightBound", "StartISODate", "TaskReallocationProfile", "TaskReallocationStrategy", "UserGroupId" },
                values: new object[] { "89249bb1-6440-4a9e-be67-7af86aed6b6d", null, new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8547), 99999999, new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(8547), 1, null, 2, 0, null, 0, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "$service$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(7857), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(7858) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(7834), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(7834) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "user",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(7871), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(7871) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "workflow_architect",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(7865), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(7865) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "$system$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(7902), new DateTime(2024, 10, 28, 18, 3, 25, 405, DateTimeKind.Utc).AddTicks(7903) });
        }
    }
}
