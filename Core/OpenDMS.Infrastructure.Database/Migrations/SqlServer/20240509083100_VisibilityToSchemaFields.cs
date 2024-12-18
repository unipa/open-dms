using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenDMS.Infrastructure.Database.Migrations.SqlServer
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
                keyValue: "01f194ac-f594-45fc-91a0-e5c35de84bd3");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

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
                columns: new[] { "CreationDate", "Email", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2534), "", new DateTime(2024, 5, 9, 8, 31, 0, 52, DateTimeKind.Utc).AddTicks(2534) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrganizationNodes",
                keyColumn: "Id",
                keyValue: "8005f01e-41e6-4fc4-88b8-316865564ca0");

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
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 13, 20, DateTimeKind.Utc).AddTicks(9616), new DateTime(2024, 3, 15, 12, 58, 13, 20, DateTimeKind.Utc).AddTicks(9616) });

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
        }
    }
}
