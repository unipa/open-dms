using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OpenDMS.Infrastructure.Database.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class NewBlobFieldTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrganizationNodes",
                keyColumn: "Id",
                keyValue: "4c9fd124-ac8a-420d-b8f4-8d959d790cdc");

            migrationBuilder.AddColumn<int>(
                name: "BlobId",
                table: "DocumentFields",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DocumentBlobFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DocumentId = table.Column<int>(type: "INTEGER", nullable: false),
                    FieldIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    DocumentTypeId = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    FieldName = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    FieldTypeId = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    Value = table.Column<string>(type: "TEXT", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastUpdateUser = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentBlobFields", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "ACL.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8418), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8418) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Company.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8429), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8429) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Datasource.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8434), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8434) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.AddContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8287), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8288) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Authorize", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8282), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8282) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8265), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8265) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Delete", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8276), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8276) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Edit", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8271), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8271) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Execute", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8314), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8315) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.History", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8304), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8304) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.RemoveContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8293), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8293) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Share", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8298), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8298) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8251), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8251) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.ViewContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8260), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8260) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "DocumentType.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8412), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8413) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Mail.Console", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8456), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8456) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "MailServer.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8445), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8445) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Meta.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8423), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8424) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanCeateRootFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8330), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8331) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanHavePersonalFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8325), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8325) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Client", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8477), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8478) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CreateGenericDocument", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8309), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8310) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.RemoteSignatureService", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8472), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8472) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.SendMail", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8461), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8461) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Signature", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8466), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8467) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Roles.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8407), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8407) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Tables.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8439), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8439) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8369), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8369) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.CreateMessage", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8395), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8396) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8363), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8364) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8402), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8402) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInbox", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8353), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8353) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInboxCC", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8358), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8358) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewDown", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8341), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8341) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewSide", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8348), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8348) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewUp", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8336), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8336) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Template.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8450), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8450) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Workflow.Dashboard", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8320), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8320) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8567), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8567) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "workflow_architect", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8539), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8540) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$ACCOUNTING$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8491), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8492) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$GLOBAL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8234), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8237) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$HR$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8519), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8519) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MAIL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8525), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8526) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MANAGEMENT$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8498), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8498) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PROTOCOL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8484), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8485) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PURCHASE$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8512), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8513) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$SALES$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8505), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8505) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$WORKFLOW$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8528), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8528) });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8004), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8004) });

            migrationBuilder.InsertData(
                table: "LookupTables",
                columns: new[] { "Id", "TableId", "Annotation", "Description" },
                values: new object[,]
                {
                    { "Document.CreationAsDraft", "$EVENTS$", "Document", "CreationAsDraft" },
                    { "UserTask.Viewed", "$EVENTS$", "UserTask", "UserTaskViewed" }
                });

            migrationBuilder.InsertData(
                table: "OrganizationNodes",
                columns: new[] { "Id", "ClosingNote", "CreationDate", "EndISODate", "LastUpdate", "LeftBound", "ParentUserGroupId", "RightBound", "StartISODate", "TaskReallocationProfile", "TaskReallocationStrategy", "UserGroupId" },
                values: new object[] { "e94cfe47-1dbb-4622-9880-b90d6dfcdfb5", null, new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8671), 99999999, new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8672), 1, null, 2, 0, null, 0, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "$service$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(7981), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(7981) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(7964), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(7966) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "user",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(7995), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(7996) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "workflow_architect",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(7987), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(7988) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "$system$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8029), new DateTime(2024, 10, 28, 18, 2, 47, 485, DateTimeKind.Utc).AddTicks(8029) });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFields_BlobId",
                table: "DocumentFields",
                column: "BlobId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentBlobFields_DocumentId_FieldIndex",
                table: "DocumentBlobFields",
                columns: new[] { "DocumentId", "FieldIndex" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentBlobFields_DocumentId_FieldName",
                table: "DocumentBlobFields",
                columns: new[] { "DocumentId", "FieldName" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentFields_DocumentBlobFields_BlobId",
                table: "DocumentFields",
                column: "BlobId",
                principalTable: "DocumentBlobFields",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentFields_DocumentBlobFields_BlobId",
                table: "DocumentFields");

            migrationBuilder.DropTable(
                name: "DocumentBlobFields");

            migrationBuilder.DropIndex(
                name: "IX_DocumentFields_BlobId",
                table: "DocumentFields");

            migrationBuilder.DeleteData(
                table: "LookupTables",
                keyColumns: new[] { "Id", "TableId" },
                keyValues: new object[] { "Document.CreationAsDraft", "$EVENTS$" });

            migrationBuilder.DeleteData(
                table: "LookupTables",
                keyColumns: new[] { "Id", "TableId" },
                keyValues: new object[] { "UserTask.Viewed", "$EVENTS$" });

            migrationBuilder.DeleteData(
                table: "OrganizationNodes",
                keyColumn: "Id",
                keyValue: "e94cfe47-1dbb-4622-9880-b90d6dfcdfb5");

            migrationBuilder.DropColumn(
                name: "BlobId",
                table: "DocumentFields");

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
    }
}
