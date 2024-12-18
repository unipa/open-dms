using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OpenDMS.Infrastructure.Database.Migrations.SqlServer
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
                keyValue: "88264912-6588-49bc-9db6-cb9de0060f90");

            migrationBuilder.AddColumn<int>(
                name: "BlobId",
                table: "DocumentFields",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DocumentBlobFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    FieldIndex = table.Column<int>(type: "int", nullable: false),
                    DocumentTypeId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    FieldName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    FieldTypeId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateUser = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
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
                filter: "[FieldName] IS NOT NULL");

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
                keyValue: "cee0422b-bdcc-46fe-8543-81053ab7e1b8");

            migrationBuilder.DropColumn(
                name: "BlobId",
                table: "DocumentFields");

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
    }
}
