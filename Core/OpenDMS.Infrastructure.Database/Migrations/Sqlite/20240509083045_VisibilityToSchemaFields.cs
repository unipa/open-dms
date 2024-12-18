using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenDMS.Infrastructure.Database.Migrations.Sqlite
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
                keyValue: "8f3aee64-083b-496b-b54c-4deb7aeb755b");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Visibility",
                table: "DocumentTypeFields",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "ACL.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(725), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(725) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Company.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(736), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(736) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Datasource.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(742), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(742) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.AddContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(564), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(564) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Authorize", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(559), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(559) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(541), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(541) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Delete", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(552), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(553) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Edit", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(547), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(547) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Execute", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(593), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(594) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.History", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(582), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(583) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.RemoveContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(570), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(570) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Share", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(575), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(576) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(526), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(526) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.ViewContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(535), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(535) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "DocumentType.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(719), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(719) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Mail.Console", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(764), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(764) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "MailServer.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(752), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(753) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Meta.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(730), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(730) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanCeateRootFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(610), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(610) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanHavePersonalFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(605), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(605) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Client", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(786), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(786) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CreateGenericDocument", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(588), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(588) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.RemoteSignatureService", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(780), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(781) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.SendMail", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(769), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(770) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Signature", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(775), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(775) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Roles.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(713), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(713) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Tables.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(747), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(747) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(696), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(697) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.CreateMessage", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(702), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(702) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(691), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(692) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(708), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(708) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInbox", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(680), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(680) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInboxCC", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(686), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(686) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewDown", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(620), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(621) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewSide", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(673), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(673) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewUp", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(615), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(615) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Template.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(758), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(759) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Workflow.Dashboard", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(599), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(599) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(882), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(882) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "workflow_architect", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(875), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(876) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$ACCOUNTING$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(800), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(800) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$GLOBAL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(494), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(505) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$HR$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(855), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(856) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MAIL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(863), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(863) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MANAGEMENT$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(806), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(806) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PROTOCOL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(793), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(793) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PURCHASE$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(821), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(821) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$SALES$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(813), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(813) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$WORKFLOW$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(865), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(865) });

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
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(196), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(197) });

            migrationBuilder.InsertData(
                table: "OrganizationNodes",
                columns: new[] { "Id", "ClosingNote", "CreationDate", "EndISODate", "LastUpdate", "LeftBound", "ParentUserGroupId", "RightBound", "StartISODate", "TaskReallocationProfile", "TaskReallocationStrategy", "UserGroupId" },
                values: new object[] { "adb0483a-3905-410b-8d22-1e4febfe7208", null, new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(1025), 99999999, new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(1026), 1, null, 2, 0, null, 0, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "$service$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(169), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(170) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(153), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(157) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "user",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(183), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(183) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "workflow_architect",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(176), new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(177) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "$system$",
                columns: new[] { "CreationDate", "Email", "LastUpdate" },
                values: new object[] { new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(231), "", new DateTime(2024, 5, 9, 8, 30, 44, 685, DateTimeKind.Utc).AddTicks(232) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrganizationNodes",
                keyColumn: "Id",
                keyValue: "adb0483a-3905-410b-8d22-1e4febfe7208");

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
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(245), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(245) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Company.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(254), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(254) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Datasource.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(259), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(259) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.AddContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(103), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(103) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Authorize", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(98), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(98) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(82), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(82) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Delete", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(92), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(92) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Edit", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(87), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(87) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Execute", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(128), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(128) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.History", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(118), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(119) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.RemoveContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(107), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(108) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.Share", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(112), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(113) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(65), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(65) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Document.ViewContent", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(77), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(77) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "DocumentType.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(240), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(240) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Mail.Console", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(279), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(279) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "MailServer.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(268), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(269) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Meta.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(249), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(249) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanCeateRootFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(143), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(143) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CanHavePersonalFolder", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(138), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(139) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Client", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(299), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(300) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.CreateGenericDocument", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(123), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(124) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.RemoteSignatureService", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(294), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(294) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.SendMail", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(283), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(284) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Profile.Signature", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(289), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(289) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Roles.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(235), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(235) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Tables.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(263), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(264) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(179), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(179) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.CreateMessage", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(224), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(224) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Task.View", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(174), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(175) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(229), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(230) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInbox", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(164), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(164) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ReadInboxCC", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(169), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(169) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewDown", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(153), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(154) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewSide", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(159), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(159) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Team.ViewUp", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(148), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(148) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Template.Admin", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(273), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(273) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$GLOBAL$", "Workflow.Dashboard", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(133), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(133) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "admin", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(391), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(391) });

            migrationBuilder.UpdateData(
                table: "ACLPermissions",
                keyColumns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType" },
                keyValues: new object[] { "$WORKFLOW$", "Document.Create", "workflow_architect", 2 },
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(384), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(385) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$ACCOUNTING$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(313), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(313) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$GLOBAL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(35), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(43) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$HR$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(339), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(340) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MAIL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(346), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(346) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$MANAGEMENT$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(319), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(320) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PROTOCOL$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(306), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(306) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$PURCHASE$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(333), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(333) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$SALES$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(326), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(326) });

            migrationBuilder.UpdateData(
                table: "ACLs",
                keyColumn: "Id",
                keyValue: "$WORKFLOW$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(348), new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(349) });

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
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 151, DateTimeKind.Utc).AddTicks(9757), new DateTime(2024, 3, 15, 12, 57, 54, 151, DateTimeKind.Utc).AddTicks(9758) });

            migrationBuilder.InsertData(
                table: "OrganizationNodes",
                columns: new[] { "Id", "ClosingNote", "CreationDate", "EndISODate", "LastUpdate", "LeftBound", "ParentUserGroupId", "RightBound", "StartISODate", "TaskReallocationProfile", "TaskReallocationStrategy", "UserGroupId" },
                values: new object[] { "8f3aee64-083b-496b-b54c-4deb7aeb755b", null, new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(479), 99999999, new DateTime(2024, 3, 15, 12, 57, 54, 152, DateTimeKind.Utc).AddTicks(480), 1, null, 2, 0, null, 0, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "$service$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 151, DateTimeKind.Utc).AddTicks(9733), new DateTime(2024, 3, 15, 12, 57, 54, 151, DateTimeKind.Utc).AddTicks(9733) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 151, DateTimeKind.Utc).AddTicks(9718), new DateTime(2024, 3, 15, 12, 57, 54, 151, DateTimeKind.Utc).AddTicks(9720) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "user",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 151, DateTimeKind.Utc).AddTicks(9746), new DateTime(2024, 3, 15, 12, 57, 54, 151, DateTimeKind.Utc).AddTicks(9746) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "workflow_architect",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 151, DateTimeKind.Utc).AddTicks(9740), new DateTime(2024, 3, 15, 12, 57, 54, 151, DateTimeKind.Utc).AddTicks(9740) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "$system$",
                columns: new[] { "CreationDate", "LastUpdate" },
                values: new object[] { new DateTime(2024, 3, 15, 12, 57, 54, 151, DateTimeKind.Utc).AddTicks(9794), new DateTime(2024, 3, 15, 12, 57, 54, 151, DateTimeKind.Utc).AddTicks(9795) });
        }
    }
}
