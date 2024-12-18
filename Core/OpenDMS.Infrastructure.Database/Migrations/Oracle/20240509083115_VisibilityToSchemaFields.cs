using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenDMS.Infrastructure.Database.Migrations.Oracle
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
                keyValue: "0c412e26-34e7-4d3b-9401-f2c5fa55a275");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "NVARCHAR2(2000)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Visibility",
                table: "DocumentTypeFields",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

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
                columns: new[] { "CreationDate", "Email", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2293), "", new DateTime(2024, 5, 9, 8, 31, 14, 885, DateTimeKind.Utc).AddTicks(2294) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrganizationNodes",
                keyColumn: "Id",
                keyValue: "637a4896-9b2f-4bf3-b8a1-951f63e02dda");

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
                values: new object[] { new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7158), new DateTime(2024, 3, 15, 12, 58, 30, 995, DateTimeKind.Utc).AddTicks(7159) });

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
        }
    }
}
