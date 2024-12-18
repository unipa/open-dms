using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OpenDMS.Infrastructure.Database.Migrations.Oracle
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
                keyValue: "63f35706-4b16-4b4f-9528-26f208ee61a4");

            migrationBuilder.AddColumn<int>(
                name: "BlobId",
                table: "DocumentFields",
                type: "NUMBER(10)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DocumentBlobFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    DocumentId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    FieldIndex = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DocumentTypeId = table.Column<string>(type: "NVARCHAR2(64)", maxLength: 64, nullable: true),
                    FieldName = table.Column<string>(type: "NVARCHAR2(64)", maxLength: 64, nullable: true),
                    FieldTypeId = table.Column<string>(type: "NVARCHAR2(64)", maxLength: 64, nullable: true),
                    Value = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    LastUpdateUser = table.Column<string>(type: "NVARCHAR2(64)", maxLength: 64, nullable: true)
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
                unique: true,
                filter: "\"FieldName\" IS NOT NULL");

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
                keyValue: "89249bb1-6440-4a9e-be67-7af86aed6b6d");

            migrationBuilder.DropColumn(
                name: "BlobId",
                table: "DocumentFields");

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
    }
}
