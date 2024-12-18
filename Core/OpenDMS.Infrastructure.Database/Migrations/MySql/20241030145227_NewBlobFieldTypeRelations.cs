using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenDMS.Infrastructure.Database.Migrations.MySql
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
                keyValue: "da10eccf-a29f-4081-a949-d22d93f3f52f");

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "ACL.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6086), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6086) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Company.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6098), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6098) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Datasource.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6103), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6104) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.AddContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5941), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5941) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Authorize", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5935), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5936) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5917), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5917) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Delete", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5929), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5929) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Edit", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5923), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5923) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Execute", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5972), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5972) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.History", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5960), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5960) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.RemoveContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5947), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5948) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Share", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5953), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5954) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5901), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5901) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.ViewContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5911), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5912) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "DocumentType.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6079), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6080) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Mail.Console", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6127), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6128) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "MailServer.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6115), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6115) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Meta.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6092), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6092) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanCeateRootFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5989), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5989) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanHavePersonalFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5983), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5984) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Client", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6151), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6151) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CreateGenericDocument", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5966), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5966) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.RemoteSignatureService", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6145), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6145) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.SendMail", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6133), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6133) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Signature", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6139), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6139) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Roles.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6049), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6049) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Tables.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6109), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6110) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6031), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6031) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.CreateMessage", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6037), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6037) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6025), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6026) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6043), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6043) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInbox", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6013), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6014) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInboxCC", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6020), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6020) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewDown", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6001), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6001) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewSide", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6007), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6008) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewUp", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5995), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5995) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Template.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6121), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6121) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Workflow.Dashboard", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5977), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5978) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6226), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6227) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "workflow_architect", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6220), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6220) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$ACCOUNTING$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6170), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6170) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$GLOBAL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5875), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5883) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$HR$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6198), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6199) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MAIL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6205), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6206) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MANAGEMENT$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6177), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6177) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PROTOCOL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6158), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6159) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PURCHASE$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6191), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6192) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$SALES$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6184), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6185) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$WORKFLOW$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6208), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6208) });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5636), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5636) });

            migrationBuilder.InsertData(
                table: "OrganizationNodes",
                columns: new[] { "Id", "ClosingNote", "CreationDate", "EndISODate", "LastUpdate", "LeftBound", "ParentUserGroupId", "RightBound", "StartISODate", "TaskReallocationProfile", "TaskReallocationStrategy", "UserGroupId" },
                values: new object[] { "6504ac7f-6791-435f-9a45-34169afec0fa", null, new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6338), 99999999, new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(6339), 1, null, 2, 0, null, 0, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "$service$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5590), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5590) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5578), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5579) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "user",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5625), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5625) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "workflow_architect",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5617), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5618) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "$system$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5662), new DateTime(2024, 10, 30, 14, 52, 26, 337, DateTimeKind.Utc).AddTicks(5662) });

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
                keyValue: "6504ac7f-6791-435f-9a45-34169afec0fa");

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "ACL.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4537), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4537) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Company.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4547), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4548) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Datasource.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4553), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4553) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.AddContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4402), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4402) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Authorize", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4396), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4397) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4379), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4379) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Delete", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4390), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4390) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Edit", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4384), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4384) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Execute", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4453), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4453) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.History", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4419), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4419) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.RemoveContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4407), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4408) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Share", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4413), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4413) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4364), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4365) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.ViewContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4373), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4373) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "DocumentType.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4531), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4531) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Mail.Console", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4576), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4576) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "MailServer.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4564), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4564) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Meta.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4542), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4542) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanCeateRootFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4470), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4470) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanHavePersonalFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4465), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4465) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Client", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4638), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4638) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CreateGenericDocument", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4424), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4425) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.RemoteSignatureService", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4632), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4632) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.SendMail", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4581), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4582) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Signature", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4587), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4587) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Roles.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4526), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4526) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Tables.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4558), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4558) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4509), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4509) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.CreateMessage", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4515), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4515) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4504), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4504) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4520), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4520) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInbox", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4493), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4493) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInboxCC", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4498), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4498) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewDown", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4481), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4481) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewSide", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4487), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4487) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewUp", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4476), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4476) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Template.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4569), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4569) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Workflow.Dashboard", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4459), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4459) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4704), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4704) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "workflow_architect", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4698), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4699) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$ACCOUNTING$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4652), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4653) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$GLOBAL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4340), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4349) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$HR$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4679), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4679) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MAIL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4685), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4685) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MANAGEMENT$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4658), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4659) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PROTOCOL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4645), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4646) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PURCHASE$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4672), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4673) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$SALES$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4665), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4665) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$WORKFLOW$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4687), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4687) });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4095), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4095) });

            migrationBuilder.InsertData(
                table: "OrganizationNodes",
                columns: new[] { "Id", "ClosingNote", "CreationDate", "EndISODate", "LastUpdate", "LeftBound", "ParentUserGroupId", "RightBound", "StartISODate", "TaskReallocationProfile", "TaskReallocationStrategy", "UserGroupId" },
                values: new object[] { "da10eccf-a29f-4081-a949-d22d93f3f52f", null, new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4766), 99999999, new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4766), 1, null, 2, 0, null, 0, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "$service$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4072), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4072) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4059), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4062) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "user",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4085), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4085) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "workflow_architect",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4078), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4078) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "$system$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4121), new DateTime(2024, 10, 28, 18, 2, 26, 51, DateTimeKind.Utc).AddTicks(4121) });
        }
    }
}
