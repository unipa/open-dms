using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenDMS.Infrastructure.Database.Migrations.SqlServer
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
                keyValue: "cee0422b-bdcc-46fe-8543-81053ab7e1b8");

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "ACL.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2563), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2563) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Company.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2574), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2574) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Datasource.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2580), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2580) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.AddContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2429), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2430) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Authorize", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2424), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2424) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2407), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2407) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Delete", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2417), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2418) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Edit", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2412), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2412) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Execute", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2457), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2458) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.History", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2447), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2447) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.RemoveContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2435), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2435) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Share", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2441), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2441) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2393), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2393) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.ViewContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2401), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2401) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "DocumentType.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2558), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2558) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Mail.Console", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2603), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2603) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "MailServer.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2591), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2591) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Meta.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2569), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2569) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanCeateRootFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2497), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2497) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanHavePersonalFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2491), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2492) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Client", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2655), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2655) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CreateGenericDocument", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2452), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2452) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.RemoteSignatureService", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2619), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2619) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.SendMail", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2608), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2609) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Signature", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2614), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2614) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Roles.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2552), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2553) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Tables.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2585), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2585) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2536), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2536) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.CreateMessage", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2541), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2542) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2530), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2531) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2547), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2547) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInbox", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2520), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2520) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInboxCC", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2525), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2525) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewDown", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2507), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2508) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewSide", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2514), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2514) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewUp", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2502), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2502) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Template.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2596), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2597) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Workflow.Dashboard", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2485), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2486) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2728), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2729) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "workflow_architect", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2722), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2722) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$ACCOUNTING$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2671), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2671) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$GLOBAL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2370), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2373) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$HR$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2703), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2703) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MAIL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2709), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2710) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MANAGEMENT$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2678), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2678) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PROTOCOL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2664), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2664) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PURCHASE$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2692), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2692) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$SALES$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2684), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2685) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$WORKFLOW$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2711), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2712) });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2133), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2134) });

            migrationBuilder.InsertData(
                table: "OrganizationNodes",
                columns: new[] { "Id", "ClosingNote", "CreationDate", "EndISODate", "LastUpdate", "LeftBound", "ParentUserGroupId", "RightBound", "StartISODate", "TaskReallocationProfile", "TaskReallocationStrategy", "UserGroupId" },
                values: new object[] { "eb02b39f-6309-4451-b19d-e803df8aa374", null, new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2835), 99999999, new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2836), 1, null, 2, 0, null, 0, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "$service$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2113), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2113) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2099), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2101) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "user",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2125), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2125) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "workflow_architect",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2119), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2119) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "$system$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2155), new DateTime(2024, 10, 30, 14, 53, 6, 913, DateTimeKind.Utc).AddTicks(2155) });

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
                keyValue: "eb02b39f-6309-4451-b19d-e803df8aa374");

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "ACL.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7993), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7993) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Company.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8005), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8005) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Datasource.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8011), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8011) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.AddContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7847), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7848) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Authorize", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7841), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7841) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7822), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7823) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Delete", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7834), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7834) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Edit", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7828), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7828) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Execute", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7899), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7900) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.History", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7866), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7866) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.RemoveContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7853), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7853) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Share", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7859), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7859) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7808), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7808) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.ViewContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7816), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7816) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "DocumentType.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7987), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7987) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Mail.Console", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8036), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8036) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "MailServer.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8023), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8023) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Meta.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7999), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7999) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanCeateRootFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7920), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7921) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanHavePersonalFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7914), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7914) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Client", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8082), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8082) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CreateGenericDocument", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7872), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7872) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.RemoteSignatureService", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8054), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8054) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.SendMail", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8042), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8042) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Signature", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8048), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8048) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Roles.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7981), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7981) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Tables.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8017), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8017) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7963), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7964) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.CreateMessage", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7969), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7970) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7958), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7958) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7975), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7976) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInbox", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7946), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7946) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInboxCC", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7952), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7952) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewDown", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7932), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7933) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewSide", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7939), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7939) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewUp", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7926), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7927) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Template.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8029), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8029) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Workflow.Dashboard", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7908), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7908) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8155), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8155) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "workflow_architect", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8148), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8149) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$ACCOUNTING$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8097), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8098) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$GLOBAL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7784), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7791) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$HR$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8127), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8127) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MAIL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8134), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8134) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MANAGEMENT$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8104), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8105) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PROTOCOL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8090), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8090) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PURCHASE$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8119), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8120) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$SALES$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8111), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8112) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$WORKFLOW$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8136), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8136) });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7543), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7543) });

            migrationBuilder.InsertData(
                table: "OrganizationNodes",
                columns: new[] { "Id", "ClosingNote", "CreationDate", "EndISODate", "LastUpdate", "LeftBound", "ParentUserGroupId", "RightBound", "StartISODate", "TaskReallocationProfile", "TaskReallocationStrategy", "UserGroupId" },
                values: new object[] { "cee0422b-bdcc-46fe-8543-81053ab7e1b8", null, new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8270), 99999999, new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(8270), 1, null, 2, 0, null, 0, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "$service$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7520), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7520) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7507), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7509) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "user",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7534), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7534) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "workflow_architect",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7527), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7527) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "$system$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7570), new DateTime(2024, 10, 28, 18, 3, 6, 879, DateTimeKind.Utc).AddTicks(7570) });
        }
    }
}
