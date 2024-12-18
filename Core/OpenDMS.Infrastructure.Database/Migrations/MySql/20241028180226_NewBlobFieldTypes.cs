using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OpenDMS.Infrastructure.Database.Migrations.MySql
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
                keyValue: "6273a941-b61f-4470-978d-d9cf03c2f938");

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
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    FieldIndex = table.Column<int>(type: "int", nullable: false),
                    DocumentTypeId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FieldName = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FieldTypeId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastUpdateUser = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentBlobFields", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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
                keyValue: "da10eccf-a29f-4081-a949-d22d93f3f52f");

            migrationBuilder.DropColumn(
                name: "BlobId",
                table: "DocumentFields");

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "ACL.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6057), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6058) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Company.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6092), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6093) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Datasource.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6098), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6098) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.AddContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5959), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5959) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Authorize", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5954), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5954) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5938), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5938) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Delete", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5948), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5949) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Edit", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5943), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5943) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Execute", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5984), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5984) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.History", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5974), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5974) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.RemoveContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5964), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5964) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Share", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5969), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5969) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5897), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5897) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.ViewContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5905), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5905) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "DocumentType.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6053), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6053) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Mail.Console", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6119), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6119) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "MailServer.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6108), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6108) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Meta.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6062), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6063) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanCeateRootFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5998), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5998) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanHavePersonalFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5993), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5993) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Client", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6138), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6138) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CreateGenericDocument", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5979), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5979) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.RemoteSignatureService", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6133), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6133) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.SendMail", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6123), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6124) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Signature", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6128), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6128) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Roles.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6048), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6048) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Tables.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6103), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6104) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6033), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6033) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.CreateMessage", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6038), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6038) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6028), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6029) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6043), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6043) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInbox", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6019), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6019) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInboxCC", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6024), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6024) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewDown", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6008), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6008) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewSide", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6014), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6014) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewUp", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6003), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6003) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Template.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6113), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6113) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Workflow.Dashboard", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5989), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5989) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6202), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6202) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "workflow_architect", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6195), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6195) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$ACCOUNTING$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6151), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6152) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$GLOBAL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5881), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5883) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$HR$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6177), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6177) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MAIL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6183), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6183) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MANAGEMENT$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6157), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6158) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PROTOCOL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6145), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6145) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PURCHASE$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6171), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6171) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$SALES$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6164), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6164) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$WORKFLOW$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6185), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6185) });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5658), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5658) });

            migrationBuilder.InsertData(
                table: "OrganizationNodes",
                columns: new[] { "Id", "ClosingNote", "CreationDate", "EndISODate", "LastUpdate", "LeftBound", "ParentUserGroupId", "RightBound", "StartISODate", "TaskReallocationProfile", "TaskReallocationStrategy", "UserGroupId" },
                values: new object[] { "6273a941-b61f-4470-978d-d9cf03c2f938", null, new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6303), 99999999, new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(6303), 1, null, 2, 0, null, 0, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "$service$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5635), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5635) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5624), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5625) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "user",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5647), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5647) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "workflow_architect",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5641), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5642) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "$system$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5702), new DateTime(2024, 8, 26, 14, 57, 1, 664, DateTimeKind.Utc).AddTicks(5702) });
        }
    }
}
