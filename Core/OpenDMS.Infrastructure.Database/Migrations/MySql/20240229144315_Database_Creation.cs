using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OpenDMS.Infrastructure.Database.Migrations.MySql
{
    /// <inheritdoc />
    public partial class Database_Creation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ACLs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACLs", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AppSettings",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettings", x => new { x.CompanyId, x.Name });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Theme = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Logo = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ERP = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AOO = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OffLine = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExternalReference = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RootOrganizationNode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContactType = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FriendlyName = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SearchName = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CountryCode = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LicTradNum = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FiscalCode = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IPACode = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Avatar = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sex = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SurName = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirstName = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationUser = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastUpdateUser = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdateErrors = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Annotation = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contacts_Contacts_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Counters",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CounterId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Counters", x => new { x.CompanyId, x.CounterId, x.Year });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CustomPages",
                columns: table => new
                {
                    PageId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentPageId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HeaderPageId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Icon = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ToolTip = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    URL = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Target = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Permissions = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Alignment = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    IncludeSubMenus = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    BadgeURL = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomPages", x => x.PageId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CustomTaskEndpoints",
                columns: table => new
                {
                    Endpoint = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Id = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tasks = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EndPointDescriptor = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CustomTaskEndpointType = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomTaskEndpoints", x => x.Endpoint);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DistributedLocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ObjectId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RecordId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ServiceId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributedLocks", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DocumentTypeWorkflows",
                columns: table => new
                {
                    DocumentTypeId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EventName = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProcessKey = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypeWorkflows", x => new { x.DocumentTypeId, x.EventName });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ExternalDataSources",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Driver = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Provider = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Connection = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserName = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalDataSources", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FieldTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CategoryId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataType = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DefaultValue = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tag = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Customized = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastUpdateUser = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreationUser = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CustomProperties = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ColumnWidth = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Searchable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Encrypted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ControlType = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Histories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DeputyUserId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Details = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EventType = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WorkflowId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TaskId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Histories", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VersionNumber = table.Column<int>(type: "int", nullable: false),
                    RevisionNumber = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsLinked = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    OriginalPath = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileManager = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileNameHash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileExtension = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Hash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Owner = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OriginalFileName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CheckOutUser = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PreservationUser = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PreservationPDV = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PreservationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    SignatureSession = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SignatureUser = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SignatureDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Signatures = table.Column<int>(type: "int", nullable: false),
                    SendingUser = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SendingDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    SendingIdentifier = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DeletionUser = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PreservationStatus = table.Column<int>(type: "int", nullable: false),
                    SignatureStatus = table.Column<int>(type: "int", nullable: false),
                    SendingStatus = table.Column<int>(type: "int", nullable: false),
                    StoringStatus = table.Column<int>(type: "int", nullable: false),
                    IndexingStatus = table.Column<int>(type: "int", nullable: false),
                    PreviewStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LookupTables",
                columns: table => new
                {
                    TableId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Id = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Annotation = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LookupTables", x => new { x.TableId, x.Id });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MailEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UIDL = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MessageId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MessageUID = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileHash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileManager = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FilePath = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MailType = table.Column<int>(type: "int", nullable: false),
                    SubType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    MailServerId = table.Column<int>(type: "int", nullable: false),
                    Direction = table.Column<int>(type: "int", nullable: false),
                    InternalMailAddress = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MessageDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ExternalMailAddress = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MessageTitle = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    NumberOfAttachments = table.Column<int>(type: "int", nullable: false),
                    TransmissionDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RetryValue = table.Column<int>(type: "int", nullable: false),
                    LastException = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IMAPFolder = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsSPAM = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsInfected = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    WorkerId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastRunningUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ArchivingDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PurgedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    ProtocolNumber = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProtocolURL = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimUser = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeletionUser = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ArchivingUser = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MailboxId = table.Column<int>(type: "int", nullable: false),
                    ExternalMailAddressCC = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExternalMailAddressCCr = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LinkAttachments = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IncludePDFPreview = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ImageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MailEntries_MailEntries_ParentId",
                        column: x => x.ParentId,
                        principalTable: "MailEntries",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MailServers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Domain = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InboxServer = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InboxServerPort = table.Column<int>(type: "int", nullable: false),
                    InboxSSL = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    InboxProtocol = table.Column<int>(type: "int", nullable: false),
                    SMTPServer = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SMTPServerPort = table.Column<int>(type: "int", nullable: false),
                    SMTPServerSSL = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastConnection = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastConnectionStatus = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    MailType = table.Column<int>(type: "int", nullable: false),
                    AuthenticationType = table.Column<int>(type: "int", nullable: false),
                    TenantID = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClientID = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClientSecret = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailServers", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Postit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Message = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PageIndex = table.Column<int>(type: "int", nullable: false),
                    Left = table.Column<int>(type: "int", nullable: false),
                    Top = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postit", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RoleName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExternalApp = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TaskItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    FromUserId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TaskType = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PriorityId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FormKey = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Variables = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EventId = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NotifyExecution = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    NotifyExpiration = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MessageTemplate = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExecutionPercentage = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExecutionDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ValidationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ProcessDefinitionId = table.Column<int>(type: "int", nullable: false),
                    ProcessDefinitionKey = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProcessImageId = table.Column<int>(type: "int", nullable: false),
                    JobInstanceId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProcessInstanceId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskItems", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TranslatedTexts",
                columns: table => new
                {
                    LanguageId = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CategoryId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Text = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranslatedTexts", x => new { x.LanguageId, x.CategoryId, x.Text });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UISettings",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UISettings", x => new { x.CompanyId, x.UserId, x.Name });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserFilters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RoleId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GroupId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Position = table.Column<int>(type: "int", nullable: false),
                    Icon = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SystemFilter = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Name = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SerializedFilters = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFilters", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShortName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClosingDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ClosingUser = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationUser = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExternalId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExternalApp = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NotificationStrategy = table.Column<int>(type: "int", nullable: false),
                    NotificationProfile = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NotificationStrategyCC = table.Column<int>(type: "int", nullable: false),
                    Visible = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Closed = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    ContactId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AttributeId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => new { x.CompanyId, x.ContactId, x.AttributeId });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ACLPermissions",
                columns: table => new
                {
                    ACLId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProfileId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProfileType = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Authorization = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACLPermissions", x => new { x.ACLId, x.ProfileId, x.ProfileType, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_ACLPermissions_ACLs_ACLId",
                        column: x => x.ACLId,
                        principalTable: "ACLs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    ContentType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CategoryId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Owner = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Icon = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IconColor = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InitialStatus = table.Column<int>(type: "int", nullable: false),
                    Internal = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ToBeSigned = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ToBePublished = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ToBePreserved = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ToBeIndexed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AcceptedExtensions = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaxVersions = table.Column<int>(type: "int", nullable: false),
                    FileNamingTemplate = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ACLId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileManager = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PersonalData = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Reserved = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LabelPosition = table.Column<int>(type: "int", nullable: false),
                    LabelX = table.Column<int>(type: "int", nullable: false),
                    LabelY = table.Column<int>(type: "int", nullable: false),
                    ExpirationStrategy = table.Column<int>(type: "int", nullable: false),
                    ExpirationDays = table.Column<int>(type: "int", nullable: false),
                    ExpirationTolerance = table.Column<int>(type: "int", nullable: false),
                    DescriptionLabel = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DocumentDateLabel = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DocumentNumberLabel = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DocumentNumberDataType = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DetailPage = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DocumentNumberMandatory = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DescriptionMandatory = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ConvertToPDF = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Direction = table.Column<int>(type: "int", nullable: false),
                    CreationFormKey = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentTypes_ACLs_ACLId",
                        column: x => x.ACLId,
                        principalTable: "ACLs",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ContactAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ContactId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    City = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Province = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CAP = table.Column<string>(type: "varchar(9)", maxLength: 9, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SearchName = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationUser = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastUpdateUser = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactAddresses_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ContactAddressRules",
                columns: table => new
                {
                    AddresType = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContactId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationUser = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastUpdateUser = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactAddressRules", x => new { x.AddresType, x.Address });
                    table.ForeignKey(
                        name: "FK_ContactAddressRules_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ContactDigitalAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ContactId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DigitalAddressType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SearchName = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationUser = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastUpdateUser = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactDigitalAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactDigitalAddresses_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContactId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExternalApp = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CustomTaskGroups",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EndpointId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsCustom = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CustomTaskEndpointEndpoint = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomTaskGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomTaskGroups_CustomTaskEndpoints_CustomTaskEndpointEndpo~",
                        column: x => x.CustomTaskEndpointEndpoint,
                        principalTable: "CustomTaskEndpoints",
                        principalColumn: "Endpoint");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HistoryDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EntryId = table.Column<int>(type: "int", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    ImageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoryDocuments_Histories_EntryId",
                        column: x => x.EntryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HistoryRecipients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EntryId = table.Column<int>(type: "int", nullable: false),
                    ProfileId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProfileType = table.Column<int>(type: "int", nullable: false),
                    CC = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryRecipients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoryRecipients_Histories_EntryId",
                        column: x => x.EntryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Mailboxes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MailAddress = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayName = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserSignature = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MailServerId = table.Column<int>(type: "int", nullable: false),
                    Account = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TokenId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RefreshToken = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Validated = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastCredentialUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UserId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    LastSendingError = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastSendingDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastReceivingError = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastReceivingDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ReadOnlyProfiles = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SendEnabledProfiles = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DraftEnabledProfiles = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EnableDownload = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeleteDownloadedMail = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DownloadImapFolders = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SaveToImapFolder = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirstReceivingMessageDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DaysToRead = table.Column<int>(type: "int", nullable: false),
                    GracePeriod = table.Column<int>(type: "int", nullable: false),
                    EmptyTrash = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastDeletionDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ReaderWorkerId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SenderWorkerId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EraserWorkerId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NextReaderDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    NextSenderDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    NextEraserDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IdleTime = table.Column<int>(type: "int", nullable: false),
                    DocumentType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SaveAsDocument = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mailboxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mailboxes_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mailboxes_MailServers_MailServerId",
                        column: x => x.MailServerId,
                        principalTable: "MailServers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TaskRecipients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TaskItemId = table.Column<int>(type: "int", nullable: false),
                    ProfileId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProfileType = table.Column<int>(type: "int", nullable: false),
                    CC = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskRecipients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskRecipients_TaskItems_TaskItemId",
                        column: x => x.TaskItemId,
                        principalTable: "TaskItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TaskItemId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RoleId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Read = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CC = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ManagerId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Percentage = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    NotificationType = table.Column<int>(type: "int", nullable: false),
                    NotificationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ClaimDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FirstExecutionDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastExecutionDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ValidationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Variables = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTasks_TaskItems_TaskItemId",
                        column: x => x.TaskItemId,
                        principalTable: "TaskItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OrganizationNodes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LeftBound = table.Column<int>(type: "int", nullable: false),
                    RightBound = table.Column<int>(type: "int", nullable: false),
                    UserGroupId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentUserGroupId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartISODate = table.Column<int>(type: "int", nullable: false),
                    EndISODate = table.Column<int>(type: "int", nullable: false),
                    TaskReallocationStrategy = table.Column<int>(type: "int", nullable: false),
                    TaskReallocationProfile = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClosingNote = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationNodes_UserGroups_ParentUserGroupId",
                        column: x => x.ParentUserGroupId,
                        principalTable: "UserGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrganizationNodes_UserGroups_UserGroupId",
                        column: x => x.UserGroupId,
                        principalTable: "UserGroups",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    ExternalId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContentType = table.Column<int>(type: "int", nullable: false),
                    DocumentTypeId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MasterDocumentId = table.Column<int>(type: "int", nullable: true),
                    DocumentStatus = table.Column<int>(type: "int", nullable: false),
                    IconColor = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Icon = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DocumentNumber = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DocumentFormattedNumber = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DocumentNumberDataType = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DocumentDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ExpirationStrategy = table.Column<int>(type: "int", nullable: false),
                    ExpirationTolerance = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastUpdateUser = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImageId = table.Column<int>(type: "int", nullable: true),
                    FolderId = table.Column<int>(type: "int", nullable: true),
                    Referents = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReferentsCC = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConsolidationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ACLId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Reserved = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PersonalData = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Owner = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProtocolNumber = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProtocolImageId = table.Column<int>(type: "int", nullable: false),
                    ExternalProtocolUid = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExternalProtocolURL = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProtocolDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ProtocolCustomProperties = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProtocolUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProtocolStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_DocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Documents_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DocumentTypeFields",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DocumentTypeId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FieldName = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FieldIndex = table.Column<int>(type: "int", nullable: false),
                    FieldTypeId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DefaultValue = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Width = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Mandatory = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Editable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Tag = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Preservable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastUpdateUser = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Encrypted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypeFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentTypeFields_DocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DocumentTypeFields_FieldTypes_FieldTypeId",
                        column: x => x.FieldTypeId,
                        principalTable: "FieldTypes",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserGroupRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserGroupId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartISODate = table.Column<int>(type: "int", nullable: false),
                    EndISODate = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroupRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGroupRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserGroupRoles_UserGroups_UserGroupId",
                        column: x => x.UserGroupId,
                        principalTable: "UserGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserGroupRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CustomTaskItems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GroupId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EndpointId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Label = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Icon = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Color = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AuthenticationType = table.Column<int>(type: "int", nullable: false),
                    JobWorker = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InputVariables = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OutputVariables = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    TaskGroupId = table.Column<string>(type: "varchar(64)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomTaskItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomTaskItems_CustomTaskGroups_TaskGroupId",
                        column: x => x.TaskGroupId,
                        principalTable: "CustomTaskGroups",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TaskProgress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserTaskId = table.Column<int>(type: "int", nullable: true),
                    TaskItemId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Message = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Percentage = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskProgress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskProgress_TaskItems_TaskItemId",
                        column: x => x.TaskItemId,
                        principalTable: "TaskItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskProgress_UserTasks_UserTaskId",
                        column: x => x.UserTaskId,
                        principalTable: "UserTasks",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DocumentFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    FieldIndex = table.Column<int>(type: "int", nullable: false),
                    Chunk = table.Column<int>(type: "int", nullable: false),
                    DocumentTypeId = table.Column<string>(type: "varchar(64)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FieldName = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FieldTypeId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FormattedValue = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LookupValue = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Encrypted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Customized = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Internal = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Tag = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastUpdateUser = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentFields_DocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DocumentFields_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DocumentPermissions",
                columns: table => new
                {
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    ProfileId = table.Column<string>(type: "varchar(129)", maxLength: 129, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProfileType = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Authorization = table.Column<int>(type: "int", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentPermissions", x => new { x.DocumentId, x.ProfileId, x.ProfileType, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_DocumentPermissions_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DocumentRecipients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    RecipientType = table.Column<int>(type: "int", nullable: false),
                    ProfileId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProfileType = table.Column<int>(type: "int", nullable: false),
                    InitialProfileId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastUpdateUser = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentRecipients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentRecipients_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DocumentRelationships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    ImageId = table.Column<int>(type: "int", nullable: false),
                    AttachmentId = table.Column<int>(type: "int", nullable: false),
                    AttachmentImageId = table.Column<int>(type: "int", nullable: false),
                    IsLinked = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreationUser = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentRelationships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentRelationships_Documents_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentRelationships_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FolderContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    FolderId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreationUser = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolderContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FolderContents_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FolderContents_Documents_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ImageVersions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    ImageId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreationUser = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageVersions_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImageVersions_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TaskAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TaskItemId = table.Column<int>(type: "int", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    IsLinked = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskAttachments_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskAttachments_TaskItems_TaskItemId",
                        column: x => x.TaskItemId,
                        principalTable: "TaskItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "ACLs",
                columns: new[] { "Id", "CreationDate", "Description", "LastUpdate", "Name" },
                values: new object[,]
                {
                    { "$ACCOUNTING$", new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8771), "", new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8772), "Contabilità" },
                    { "$GLOBAL$", new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8344), "", new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8353), "Permessi Globali" },
                    { "$HR$", new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8812), "", new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8813), "Personale" },
                    { "$MAIL$", new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8822), "", new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8822), "Corrispondenza" },
                    { "$MANAGEMENT$", new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8781), "", new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8782), "Amministrazione" },
                    { "$PROTOCOL$", new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8759), "", new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8760), "Protocollo" },
                    { "$PURCHASE$", new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8803), "", new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8803), "Acquisti" },
                    { "$SALES$", new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8793), "", new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8793), "Vendite" },
                    { "$WORKFLOW$", new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8824), "", new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8825), "Workflow" }
                });

            migrationBuilder.InsertData(
                table: "AppSettings",
                columns: new[] { "CompanyId", "Name", "Value" },
                values: new object[,]
                {
                    { 0, "Template.Attachment.Linked", "" },
                    { 0, "Template.Attachment.NotFound", "" },
                    { 0, "Template.Exception.Body", "{description}" },
                    { 0, "Template.Exception.Title", "Si è verificato un errore" },
                    { 0, "Template.NewMail.Body", "Ricordati di assegnarlo a chi di competenza o di gestirlo tu stesso." },
                    { 0, "Template.NewMail.Title", "Hai un messaggio di posta elettronica da gestire" },
                    { 0, "Template.NoReferents.Body", "Il fascicolo è: {Document.Description}<br/><br/>{Task.Description}<br/> {Sender.FullName}" },
                    { 0, "Template.NoReferents.Title", "Non sei più il referente di un fascicolo" },
                    { 0, "Template.Notification.Body", "{description}<br/><br/>{Sender.FullName}" },
                    { 0, "Template.Notification.CC.Body", "{description}<br/><br/>{Sender.FullName}" },
                    { 0, "Template.Notification.CC.Title", "Hai un nuovo messaggio: {Task.Title}" },
                    { 0, "Template.Notification.Title", "Hai una nuova attività: {Task.Title}" },
                    { 0, "Template.Referents.Body", "Il fascicolo è: {Document.Description}<br/><br/>{Task.Description}<br/> {Sender.FullName}" },
                    { 0, "Template.Referents.Changes.Body", "Il documento modificato è: {Document.Description}<br/>" },
                    { 0, "Template.Referents.Changes.Title", "E' stato modificato un documento di cui sei il referente" },
                    { 0, "Template.Referents.Title", "Sei il nuovo referente di un fascicolo" }
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "AOO", "Description", "ERP", "ExternalReference", "Logo", "OffLine", "RootOrganizationNode", "Theme" },
                values: new object[] { 1, "", "Ambiente Principale", "", "", "", false, "", "" });

            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "Id", "Annotation", "Avatar", "ContactType", "CountryCode", "CreationDate", "CreationUser", "Deleted", "FirstName", "FiscalCode", "FriendlyName", "FullName", "IPACode", "LastUpdate", "LastUpdateUser", "LicTradNum", "ParentId", "SearchName", "Sex", "SurName", "UpdateErrors" },
                values: new object[] { "00000000-0000-0000-0000-000000000000", null, null, 1, null, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8036), null, false, null, null, "Utente di Sistema", "Utente di Sistema", null, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8037), null, null, null, "Utente di Sistema", null, null, null });

            migrationBuilder.InsertData(
                table: "CustomPages",
                columns: new[] { "PageId", "Alignment", "BadgeURL", "HeaderPageId", "Icon", "IncludeSubMenus", "LastUpdate", "ParentPageId", "Permissions", "Position", "Target", "Title", "ToolTip", "URL" },
                values: new object[,]
                {
                    { "Admin", -1, "", "", "fa fa-sliders", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", ":admin", 0, "", "Amministrazione", "Amministrazione", "/Admin/Home/Index" },
                    { "Folders", 0, "", "", "fa fa-folder", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "", 1, "", "Documenti", "Documenti", "/" },
                    { "Mail", 0, "/internalapi/mailentry/badge", "", "fa fa-envelope", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Mail.Console", 2, "", "Posta Elettronica", "Posta Elettronica", "/mail" },
                    { "Tasks", 0, "/internalapi/tasklist/badge", "", "fa fa-bell", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Task.View", 0, "", "Attività", "Attività", "/tasks" },
                    { "WF", 0, "", "", "fa fa-sitemap", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Workflow.Dashboard", 100, "", "Processi", "Dashboard Processi", "/BPMMonitor" }
                });

            migrationBuilder.InsertData(
                table: "LookupTables",
                columns: new[] { "Id", "TableId", "Annotation", "Description" },
                values: new object[,]
                {
                    { "$ACCOUNTING$", "$CATEGORIES$", null, "Contabilità" },
                    { "$HR$", "$CATEGORIES$", null, "Personale" },
                    { "$MAIL$", "$CATEGORIES$", null, "Corrispondenza" },
                    { "$MANAGEMENT$", "$CATEGORIES$", null, "Amministrazione" },
                    { "$PROTOCOL$", "$CATEGORIES$", null, "Protocollo" },
                    { "$PURCHASE$", "$CATEGORIES$", null, "Acquisti" },
                    { "$SALES$", "$CATEGORIES$", null, "Vendite" },
                    { "$TEMPLATES$", "$CATEGORIES$", null, "Modelli" },
                    { "$WORKFLOW$", "$CATEGORIES$", null, "Processi" },
                    { "Application.Exception", "$EVENTS$", "Application", "Exception" },
                    { "Application.Timer", "$EVENTS$", "Application", "Timer" },
                    { "Document.AddAttach", "$EVENTS$", "Document", "AddAttach" },
                    { "Document.AddBiometricalSignature", "$EVENTS$", "Document", "AddBiometricalSignature" },
                    { "Document.AddCheckSign", "$EVENTS$", "Document", "AddCheckSign" },
                    { "Document.AddDigitalSignature", "$EVENTS$", "Document", "AddDigitalSignature" },
                    { "Document.AddLink", "$EVENTS$", "Document", "AddLink" },
                    { "Document.AddPreservationSignature", "$EVENTS$", "Document", "AddPreservationSignature" },
                    { "Document.AddProtocolSign", "$EVENTS$", "Document", "AddProtocolSign" },
                    { "Document.AddRemoteDigitalSignature", "$EVENTS$", "Document", "AddRemoteDigitalSignature" },
                    { "Document.AddRevision", "$EVENTS$", "Document", "AddRevision" },
                    { "Document.AddSignatureField", "$EVENTS$", "Document", "AddSignatureField" },
                    { "Document.AddStamp", "$EVENTS$", "Document", "AddStamp" },
                    { "Document.AddTextToContent", "$EVENTS$", "Document", "AddTextToContent" },
                    { "Document.AddToFolder", "$EVENTS$", "Document", "AddToFolder" },
                    { "Document.AddUserSignature", "$EVENTS$", "Document", "AddUserSignature" },
                    { "Document.AddVersion", "$EVENTS$", "Document", "AddVersion" },
                    { "Document.CheckIn", "$EVENTS$", "Document", "CheckIn" },
                    { "Document.CheckOut", "$EVENTS$", "Document", "CheckOut" },
                    { "Document.Classification", "$EVENTS$", "Document", "Classify" },
                    { "Document.CommentAdded", "$EVENTS$", "Document", "Comment" },
                    { "Document.Convert", "$EVENTS$", "Document", "Convert" },
                    { "Document.Copy", "$EVENTS$", "Document", "Copy" },
                    { "Document.Creation", "$EVENTS$", "Document", "Creation" },
                    { "Document.DefaultContent", "$EVENTS$", "Document", "DefaultContent" },
                    { "Document.Delete", "$EVENTS$", "Document", "Delete" },
                    { "Document.Download", "$EVENTS$", "Document", "Download" },
                    { "Document.EraseContent", "$EVENTS$", "Document", "EraseContent" },
                    { "Document.ExcludeFromSending", "$EVENTS$", "Document", "ExcludeFromSending" },
                    { "Document.Expiration", "$EVENTS$", "Document", "Expiration" },
                    { "Document.ExpirationDateUpdated", "$EVENTS$", "Document", "ExpirationUpdated" },
                    { "Document.HighlightContent", "$EVENTS$", "Document", "HighlightContent" },
                    { "Document.NoContentUpdate", "$EVENTS$", "Document", "NoContentUpdate" },
                    { "Document.ObscureContent", "$EVENTS$", "Document", "ObscureContent" },
                    { "Document.PermissionChanged", "$EVENTS$", "Document", "Authorize" },
                    { "Document.PrepareForSending", "$EVENTS$", "Document", "PrepareForSending" },
                    { "Document.Preservation", "$EVENTS$", "Document", "Preservation" },
                    { "Document.Print", "$EVENTS$", "Document", "Print" },
                    { "Document.ProcessStarted", "$EVENTS$", "Document", "RunProcess" },
                    { "Document.Protocol", "$EVENTS$", "Document", "Protocol" },
                    { "Document.Publish", "$EVENTS$", "Document", "Publish" },
                    { "Document.RemoveAttach", "$EVENTS$", "Document", "AttachRemoved" },
                    { "Document.RemoveFromFolder", "$EVENTS$", "Document", "RemoveFromFolder" },
                    { "Document.RemoveLink", "$EVENTS$", "Document", "LinkRemoved" },
                    { "Document.RemoveRevision", "$EVENTS$", "Document", "RemoveRevision" },
                    { "Document.RemoveVersion", "$EVENTS$", "Document", "RemoveVersion" },
                    { "Document.Restore", "$EVENTS$", "Document", "UnDelete" },
                    { "Document.Send", "$EVENTS$", "Document", "EMail" },
                    { "Document.Share", "$EVENTS$", "Document", "Share" },
                    { "Document.StatusChanged", "$EVENTS$", "Document", "ChangeStatus" },
                    { "Document.Update", "$EVENTS$", "Document", "Update" },
                    { "Document.View", "$EVENTS$", "Document", "View" },
                    { "Task.Created", "$EVENTS$", "Task", "TaskCreated" },
                    { "Task.Deleted", "$EVENTS$", "Task", "TaskDeleted" },
                    { "Task.Executed", "$EVENTS$", "Task", "TaskExecuted" },
                    { "Task.Expired", "$EVENTS$", "Task", "TaskExpired" },
                    { "Task.Running", "$EVENTS$", "Task", "TaskRunning" },
                    { "Task.Validated", "$EVENTS$", "Task", "TaskValidated" },
                    { "UserTask.Approve", "$EVENTS$", "UserTask", "Approval" },
                    { "UserTask.Claimed", "$EVENTS$", "UserTask", "UserTaskClaimed" },
                    { "UserTask.Executed", "$EVENTS$", "UserTask", "UserTaskExecuted" },
                    { "UserTask.Reassigned", "$EVENTS$", "UserTask", "UserTaskReassigned" },
                    { "UserTask.Refuse", "$EVENTS$", "UserTask", "Refuse" },
                    { "UserTask.Rejected", "$EVENTS$", "UserTask", "UserTaskRejected" },
                    { "UserTask.Released", "$EVENTS$", "UserTask", "UserTaskReleased" },
                    { "ACL.Admin", "$PERMISSIONS$", null, "Amministrazione - Gestione Permessi su Tipologie" },
                    { "Company.Admin", "$PERMISSIONS$", null, "Amministrazione - Gestione Sistemi Informativi" },
                    { "Datasource.Admin", "$PERMISSIONS$", null, "Amministrazione - Gestione Database Esterni" },
                    { "Document.AddContent", "$PERMISSIONS$", null, "Documento - Gestione Contenuti" },
                    { "Document.Authorize", "$PERMISSIONS$", null, "Documento - Gestione Permessi" },
                    { "Document.Create", "$PERMISSIONS$", null, "Documento - Creazione" },
                    { "Document.Delete", "$PERMISSIONS$", null, "Documento - Cancellazione" },
                    { "Document.Edit", "$PERMISSIONS$", null, "Documento - Modifica Metadati" },
                    { "Document.Execute", "$PERMISSIONS$", null, "Processo - Esecuzione flussi" },
                    { "Document.History", "$PERMISSIONS$", null, "Documento - Accesso Registro Cronistoria" },
                    { "Document.RemoveContent", "$PERMISSIONS$", null, "Documento - Rimozione Contenuti" },
                    { "Document.Share", "$PERMISSIONS$", null, "Documento - Condivisione" },
                    { "Document.View", "$PERMISSIONS$", null, "Documento - Visibilità" },
                    { "Document.ViewContent", "$PERMISSIONS$", null, "Documento - Visibilità Contenuto" },
                    { "DocumentType.Admin", "$PERMISSIONS$", null, "Amministrazione - Gestione Tipologie" },
                    { "Mail.Console", "$PERMISSIONS$", null, "Posta Elettronica - Accesso alla Dashboard" },
                    { "MailServer.Admin", "$PERMISSIONS$", null, "Amministrazione - Gestione MailServer" },
                    { "Meta.Admin", "$PERMISSIONS$", null, "Amministrazione - Gestione Metadati" },
                    { "Profile.CanCeateRootFolder", "$PERMISSIONS$", null, "Fascicolo - Creazione Fascicoli Condivisi" },
                    { "Profile.CanHavePersonalFolder", "$PERMISSIONS$", null, "Fascicolo - Gestione Fascicolo Personale" },
                    { "Profile.Client", "$PERMISSIONS$", null, "App - Download Client di Firma e CheckIn/CheckOut" },
                    { "Profile.CreateGenericDocument", "$PERMISSIONS$", null, "Documento - Creazione Documento Generico" },
                    { "Profile.RemoteSignatureService", "$PERMISSIONS$", null, "Firme - Accesso Firme Remote" },
                    { "Profile.SendMail", "$PERMISSIONS$", null, "Posta Elettronica - Autorizzato all'invio" },
                    { "Profile.Signature", "$PERMISSIONS$", null, "Firme - Gestione Firma Autografa" },
                    { "Roles.Admin", "$PERMISSIONS$", null, "Amministrazione - Gestione Ruoli" },
                    { "Tables.Admin", "$PERMISSIONS$", null, "Amministrazione - Gestione Tabelle Interne" },
                    { "Task.Create", "$PERMISSIONS$", null, "Task - Creazione Nuove Attività" },
                    { "Task.CreateMessage", "$PERMISSIONS$", null, "Task - Creazione Nuovi Messaggi" },
                    { "Task.View", "$PERMISSIONS$", null, "Task - Accesso alla Dashboard" },
                    { "Team.Admin", "$PERMISSIONS$", null, "Amministrazione - Gestione Organigramma" },
                    { "Team.ReadInbox", "$PERMISSIONS$", null, "Organigramma - Ricezione Notifiche di Struttura" },
                    { "Team.ReadInboxCC", "$PERMISSIONS$", null, "Organigramma - Ricezione Notifche CC di Struttura" },
                    { "Team.ViewDown", "$PERMISSIONS$", null, "Organigramma - Visione Strutture Inferiori" },
                    { "Team.ViewSide", "$PERMISSIONS$", null, "Organigramma - Visione Strutture Paritetiche" },
                    { "Team.ViewUp", "$PERMISSIONS$", null, "Organigramma - Visione Strutture Superiori" },
                    { "Template.Admin", "$PERMISSIONS$", null, "Amministrazione - Gestione Template Notifiche" },
                    { "Workflow.Dashboard", "$PERMISSIONS$", null, "Processo - Accesso alla Dashboard" },
                    { "$CATEGORIES$", "$TABLES$", null, "Categorie Documentali" },
                    { "$EVENTS$", "$TABLES$", null, "Eventi" },
                    { "$PERMISSIONS$", "$TABLES$", null, "Permessi" },
                    { "$TABLES$", "$TABLES$", null, "Registro delle Tabelle" },
                    { "$TASK.GROUPS$", "$TABLES$", null, "Categorie Task" },
                    { "$TASK.PRIORITIES$", "$TABLES$", null, "Priorità" },
                    { "100.NONE", "$TASK.GROUPS$", null, "Attività Generica" },
                    { "100.LOW", "$TASK.PRIORITIES$", null, "Bassa" },
                    { "200.MEDIUM", "$TASK.PRIORITIES$", null, "Media" },
                    { "300.HIGH", "$TASK.PRIORITIES$", null, "Alta" },
                    { "400.URGENT", "$TASK.PRIORITIES$", null, "Urgente" }
                });

            migrationBuilder.InsertData(
                table: "OrganizationNodes",
                columns: new[] { "Id", "ClosingNote", "CreationDate", "EndISODate", "LastUpdate", "LeftBound", "ParentUserGroupId", "RightBound", "StartISODate", "TaskReallocationProfile", "TaskReallocationStrategy", "UserGroupId" },
                values: new object[] { "cc474ead-94d4-4dec-832d-00f45dbc82e7", null, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(9043), 99999999, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(9044), 1, null, 2, 0, null, 0, null });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreationDate", "Deleted", "DeletionDate", "ExternalApp", "LastUpdate", "RoleName" },
                values: new object[,]
                {
                    { "$service$", new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8007), false, null, "", new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8007), "Servizio Applicativo" },
                    { "admin", new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(7982), false, null, "", new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(7985), "Amministratore" },
                    { "user", new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8021), false, null, "", new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8022), "Utente Interattivo" },
                    { "workflow_architect", new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8014), false, null, "", new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8014), "Progettista di Workflow" }
                });

            migrationBuilder.InsertData(
                table: "ACLPermissions",
                columns: new[] { "ACLId", "PermissionId", "ProfileId", "ProfileType", "Authorization", "CreationDate", "LastUpdate" },
                values: new object[,]
                {
                    { "$GLOBAL$", "ACL.Admin", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8602), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8602) },
                    { "$GLOBAL$", "Company.Admin", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8621), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8622) },
                    { "$GLOBAL$", "Datasource.Admin", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8630), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8630) },
                    { "$GLOBAL$", "Document.AddContent", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8453), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8453) },
                    { "$GLOBAL$", "Document.Authorize", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8413), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8413) },
                    { "$GLOBAL$", "Document.Create", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8395), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8395) },
                    { "$GLOBAL$", "Document.Delete", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8406), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8406) },
                    { "$GLOBAL$", "Document.Edit", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8400), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8400) },
                    { "$GLOBAL$", "Document.Execute", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8483), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8484) },
                    { "$GLOBAL$", "Document.History", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8471), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8471) },
                    { "$GLOBAL$", "Document.RemoveContent", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8459), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8460) },
                    { "$GLOBAL$", "Document.Share", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8465), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8465) },
                    { "$GLOBAL$", "Document.View", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8377), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8378) },
                    { "$GLOBAL$", "Document.ViewContent", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8389), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8389) },
                    { "$GLOBAL$", "DocumentType.Admin", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8592), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8593) },
                    { "$GLOBAL$", "Mail.Console", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8709), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8709) },
                    { "$GLOBAL$", "MailServer.Admin", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8648), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8649) },
                    { "$GLOBAL$", "Meta.Admin", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8612), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8612) },
                    { "$GLOBAL$", "Profile.CanCeateRootFolder", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8502), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8502) },
                    { "$GLOBAL$", "Profile.CanHavePersonalFolder", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8496), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8496) },
                    { "$GLOBAL$", "Profile.Client", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8748), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8748) },
                    { "$GLOBAL$", "Profile.CreateGenericDocument", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8477), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8478) },
                    { "$GLOBAL$", "Profile.RemoteSignatureService", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8739), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8740) },
                    { "$GLOBAL$", "Profile.SendMail", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8721), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8721) },
                    { "$GLOBAL$", "Profile.Signature", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8730), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8731) },
                    { "$GLOBAL$", "Roles.Admin", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8583), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8584) },
                    { "$GLOBAL$", "Tables.Admin", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8639), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8639) },
                    { "$GLOBAL$", "Task.Create", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8557), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8557) },
                    { "$GLOBAL$", "Task.CreateMessage", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8567), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8567) },
                    { "$GLOBAL$", "Task.View", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8547), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8547) },
                    { "$GLOBAL$", "Team.Admin", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8575), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8575) },
                    { "$GLOBAL$", "Team.ReadInbox", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8529), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8529) },
                    { "$GLOBAL$", "Team.ReadInboxCC", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8537), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8538) },
                    { "$GLOBAL$", "Team.ViewDown", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8513), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8514) },
                    { "$GLOBAL$", "Team.ViewSide", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8519), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8520) },
                    { "$GLOBAL$", "Team.ViewUp", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8508), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8508) },
                    { "$GLOBAL$", "Template.Admin", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8657), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8658) },
                    { "$GLOBAL$", "Workflow.Dashboard", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8489), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8489) },
                    { "$WORKFLOW$", "Document.Create", "admin", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8851), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8851) },
                    { "$WORKFLOW$", "Document.Create", "workflow_architect", 2, 1, new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8841), new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8841) }
                });

            migrationBuilder.InsertData(
                table: "DocumentTypes",
                columns: new[] { "Id", "ACLId", "AcceptedExtensions", "CategoryId", "CompanyId", "ContentType", "ConvertToPDF", "CreationFormKey", "Description", "DescriptionLabel", "DescriptionMandatory", "DetailPage", "Direction", "DocumentDateLabel", "DocumentNumberDataType", "DocumentNumberLabel", "DocumentNumberMandatory", "ExpirationDays", "ExpirationStrategy", "ExpirationTolerance", "FileManager", "FileNamingTemplate", "Icon", "IconColor", "InitialStatus", "Internal", "LabelPosition", "LabelX", "LabelY", "MaxVersions", "Name", "Owner", "PersonalData", "Reserved", "ToBeIndexed", "ToBePreserved", "ToBePublished", "ToBeSigned" },
                values: new object[,]
                {
                    { "$DIAGRAM$", "$WORKFLOW$", ".bpmn", "$WORKFLOW$", 0, 3, false, "$NEW-DIAGRAM-TEMPLATE$", "", "Descrizione", true, "/Details/Process", 0, "", null, "", false, 0, 0, 0, "", "", "fa fa-cogs", "", 0, false, 0, 0, 0, 0, "Processo", "", false, false, false, false, true, false },
                    { "$DMN$", "$WORKFLOW$", ".dmn", "$WORKFLOW$", 0, 5, false, "$NEW-DMN-TEMPLATE$", "", "Descrizione", true, "/Details/DMN", 0, "", null, "ID Univoco", false, 0, 0, 0, "", "", "fa fa-question-circle", "", 0, false, 0, 0, 0, 0, "Matrice Decisionale", "", false, false, false, false, true, false },
                    { "$FORM$", "$WORKFLOW$", ".form,.html,.formio", "$WORKFLOW$", 0, 4, false, "$NEW-FORM-TEMPLATE$", "", "Descrizione", true, "/Details/Form", 0, "", null, "", false, 0, 0, 0, "", "", "fa fa-table", "", 0, false, 0, 0, 0, 0, "Modulo Digitale", "", false, false, false, false, true, false },
                    { "$MAIL-INBOUND$", "$MAIL$", ".eml", "$MAIL$", 0, 8, false, "", "", "Oggetto", true, "/Details/Mail", 0, "Data Ricezione", null, "", false, 0, 0, 0, "", "", "fa fa-envelope", "", 0, false, 0, 0, 0, 0, "EMail Ricevuta", "", false, false, true, false, false, false },
                    { "$MAIL-OUTBOUND$", "$MAIL$", ".eml", "$MAIL$", 0, 8, false, "", "", "Oggetto", true, "/Details/Mail", 0, "Data Invio", null, "", false, 0, 0, 0, "", "", "fa fa-send", "", 0, false, 0, 0, 0, 0, "EMail Spedita", "", false, false, true, false, true, false },
                    { "$NOTA-INGRESSO$", "$PROTOCOL$", "", "$PROTOCOL$", 0, 1, false, "", "", "Oggetto di Protocollo", true, "", 1, "Data Documento", null, "", false, 0, 0, 0, "", "", "", "", 0, false, 0, 0, 0, 0, "Nota In Ingresso", "", false, false, true, false, false, false },
                    { "$NOTA-INTERNA$", "$PROTOCOL$", "", "$PROTOCOL$", 0, 1, false, "", "", "Oggetto di Protocollo", true, "", 0, "Data Documento", null, "", false, 0, 0, 0, "", "", "", "", 0, false, 0, 0, 0, 0, "Nota Interna", "", false, false, true, false, false, false },
                    { "$NOTA-USCITA$", "$PROTOCOL$", "", "$PROTOCOL$", 0, 1, false, "", "", "Oggetto di Protocollo", true, "", 2, "Data Documento", null, "", false, 0, 0, 0, "", "", "", "", 0, false, 0, 0, 0, 0, "Nota In Uscita", "", false, false, true, false, true, false },
                    { "$PEC-INBOUND$", "$MAIL$", ".eml", "$MAIL$", 0, 8, false, "", "", "Oggetto", true, "/Details/Mail", 0, "Data Ricezione", null, "", false, 0, 0, 0, "", "", "fa fa-envelope", "", 0, false, 0, 0, 0, 0, "PEC Ricevuta", "", false, false, true, false, false, false },
                    { "$PEC-OUTBOUND$", "$MAIL$", ".eml", "$MAIL$", 0, 8, false, "", "", "Oggetto", true, "/Details/Mail", 0, "Data Invio", null, "", false, 0, 0, 0, "", "", "fa fa-send", "", 0, false, 0, 0, 0, 0, "PEC Spedita", "", false, false, true, false, true, false },
                    { "$TEMPLATE$", "$WORKFLOW$", ".html,.txt,.docx,.xlsx,.pdf", "$WORKFLOW$", 0, 4, false, "$NEW-TEMPLATE-TEMPLATE$", "", "Descrizione", true, "/Details/Template", 0, "", null, "", false, 0, 0, 0, "", "", "fa fa-edit", "", 0, false, 0, 0, 0, 0, "Template", "", false, false, false, false, true, false }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ContactId", "CreationDate", "Deleted", "DeletionDate", "ExternalApp", "LastUpdate" },
                values: new object[] { "$system$", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8069), false, null, "", new DateTime(2024, 2, 29, 14, 43, 15, 678, DateTimeKind.Utc).AddTicks(8070) });

            migrationBuilder.CreateIndex(
                name: "IX_ContactAddresses_ContactId",
                table: "ContactAddresses",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactAddressRules_ContactId",
                table: "ContactAddressRules",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactDigitalAddresses_ContactId",
                table: "ContactDigitalAddresses",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactDigitalAddresses_SearchName_Address",
                table: "ContactDigitalAddresses",
                columns: new[] { "SearchName", "Address" });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_CountryCode_LicTradNum",
                table: "Contacts",
                columns: new[] { "CountryCode", "LicTradNum" });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_FiscalCode",
                table: "Contacts",
                column: "FiscalCode");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_ParentId",
                table: "Contacts",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_SearchName",
                table: "Contacts",
                column: "SearchName");

            migrationBuilder.CreateIndex(
                name: "IX_CustomPages_ParentPageId_Alignment_Position",
                table: "CustomPages",
                columns: new[] { "ParentPageId", "Alignment", "Position" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomTaskGroups_CustomTaskEndpointEndpoint",
                table: "CustomTaskGroups",
                column: "CustomTaskEndpointEndpoint");

            migrationBuilder.CreateIndex(
                name: "IX_CustomTaskItems_TaskGroupId",
                table: "CustomTaskItems",
                column: "TaskGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DistributedLocks_ObjectId_RecordId",
                table: "DistributedLocks",
                columns: new[] { "ObjectId", "RecordId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFields_DocumentId_FieldIndex_Chunk",
                table: "DocumentFields",
                columns: new[] { "DocumentId", "FieldIndex", "Chunk" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFields_DocumentId_FieldName_Chunk",
                table: "DocumentFields",
                columns: new[] { "DocumentId", "FieldName", "Chunk" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFields_DocumentTypeId",
                table: "DocumentFields",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFields_FieldTypeId_FormattedValue",
                table: "DocumentFields",
                columns: new[] { "FieldTypeId", "FormattedValue" });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentRecipients_DocumentId_RecipientType_ProfileId_Profil~",
                table: "DocumentRecipients",
                columns: new[] { "DocumentId", "RecipientType", "ProfileId", "ProfileType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentRelationships_AttachmentId",
                table: "DocumentRelationships",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentRelationships_DocumentId_AttachmentId",
                table: "DocumentRelationships",
                columns: new[] { "DocumentId", "AttachmentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DocumentTypeId_DocumentStatus",
                table: "Documents",
                columns: new[] { "DocumentTypeId", "DocumentStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ExternalId_DocumentStatus",
                table: "Documents",
                columns: new[] { "ExternalId", "DocumentStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_FolderId",
                table: "Documents",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ImageId",
                table: "Documents",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_MasterDocumentId",
                table: "Documents",
                column: "MasterDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTypeFields_DocumentTypeId_FieldName",
                table: "DocumentTypeFields",
                columns: new[] { "DocumentTypeId", "FieldName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTypeFields_FieldTypeId",
                table: "DocumentTypeFields",
                column: "FieldTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTypes_ACLId",
                table: "DocumentTypes",
                column: "ACLId");

            migrationBuilder.CreateIndex(
                name: "IX_FolderContents_DocumentId",
                table: "FolderContents",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_FolderContents_FolderId",
                table: "FolderContents",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_FolderContents_FolderId_DocumentId",
                table: "FolderContents",
                columns: new[] { "FolderId", "DocumentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistoryDocuments_EntryId",
                table: "HistoryDocuments",
                column: "EntryId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryRecipients_EntryId",
                table: "HistoryRecipients",
                column: "EntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_FileName_FileNameHash",
                table: "Images",
                columns: new[] { "FileName", "FileNameHash" });

            migrationBuilder.CreateIndex(
                name: "IX_Images_Hash",
                table: "Images",
                column: "Hash");

            migrationBuilder.CreateIndex(
                name: "IX_ImageVersions_DocumentId_ImageId",
                table: "ImageVersions",
                columns: new[] { "DocumentId", "ImageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImageVersions_ImageId",
                table: "ImageVersions",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Mailboxes_CompanyId",
                table: "Mailboxes",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Mailboxes_MailAddress",
                table: "Mailboxes",
                column: "MailAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mailboxes_MailServerId",
                table: "Mailboxes",
                column: "MailServerId");

            migrationBuilder.CreateIndex(
                name: "IX_MailEntries_ParentId",
                table: "MailEntries",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_MailServers_Domain",
                table: "MailServers",
                column: "Domain",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationNodes_LeftBound",
                table: "OrganizationNodes",
                column: "LeftBound",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationNodes_ParentUserGroupId",
                table: "OrganizationNodes",
                column: "ParentUserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationNodes_RightBound",
                table: "OrganizationNodes",
                column: "RightBound",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationNodes_StartISODate_UserGroupId",
                table: "OrganizationNodes",
                columns: new[] { "StartISODate", "UserGroupId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationNodes_UserGroupId",
                table: "OrganizationNodes",
                column: "UserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAttachments_DocumentId",
                table: "TaskAttachments",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAttachments_TaskItemId",
                table: "TaskAttachments",
                column: "TaskItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskProgress_TaskItemId",
                table: "TaskProgress",
                column: "TaskItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskProgress_UserTaskId",
                table: "TaskProgress",
                column: "UserTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskRecipients_TaskItemId",
                table: "TaskRecipients",
                column: "TaskItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupRoles_RoleId",
                table: "UserGroupRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupRoles_StartISODate_UserGroupId_UserId_RoleId",
                table: "UserGroupRoles",
                columns: new[] { "StartISODate", "UserGroupId", "UserId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupRoles_UserGroupId",
                table: "UserGroupRoles",
                column: "UserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupRoles_UserId",
                table: "UserGroupRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_ExternalId_ExternalApp",
                table: "UserGroups",
                columns: new[] { "ExternalId", "ExternalApp" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ContactId",
                table: "Users",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_TaskItemId",
                table: "UserTasks",
                column: "TaskItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ACLPermissions");

            migrationBuilder.DropTable(
                name: "AppSettings");

            migrationBuilder.DropTable(
                name: "ContactAddresses");

            migrationBuilder.DropTable(
                name: "ContactAddressRules");

            migrationBuilder.DropTable(
                name: "ContactDigitalAddresses");

            migrationBuilder.DropTable(
                name: "Counters");

            migrationBuilder.DropTable(
                name: "CustomPages");

            migrationBuilder.DropTable(
                name: "CustomTaskItems");

            migrationBuilder.DropTable(
                name: "DistributedLocks");

            migrationBuilder.DropTable(
                name: "DocumentFields");

            migrationBuilder.DropTable(
                name: "DocumentPermissions");

            migrationBuilder.DropTable(
                name: "DocumentRecipients");

            migrationBuilder.DropTable(
                name: "DocumentRelationships");

            migrationBuilder.DropTable(
                name: "DocumentTypeFields");

            migrationBuilder.DropTable(
                name: "DocumentTypeWorkflows");

            migrationBuilder.DropTable(
                name: "ExternalDataSources");

            migrationBuilder.DropTable(
                name: "FolderContents");

            migrationBuilder.DropTable(
                name: "HistoryDocuments");

            migrationBuilder.DropTable(
                name: "HistoryRecipients");

            migrationBuilder.DropTable(
                name: "ImageVersions");

            migrationBuilder.DropTable(
                name: "LookupTables");

            migrationBuilder.DropTable(
                name: "Mailboxes");

            migrationBuilder.DropTable(
                name: "MailEntries");

            migrationBuilder.DropTable(
                name: "OrganizationNodes");

            migrationBuilder.DropTable(
                name: "Postit");

            migrationBuilder.DropTable(
                name: "TaskAttachments");

            migrationBuilder.DropTable(
                name: "TaskProgress");

            migrationBuilder.DropTable(
                name: "TaskRecipients");

            migrationBuilder.DropTable(
                name: "TranslatedTexts");

            migrationBuilder.DropTable(
                name: "UISettings");

            migrationBuilder.DropTable(
                name: "UserFilters");

            migrationBuilder.DropTable(
                name: "UserGroupRoles");

            migrationBuilder.DropTable(
                name: "UserSettings");

            migrationBuilder.DropTable(
                name: "CustomTaskGroups");

            migrationBuilder.DropTable(
                name: "FieldTypes");

            migrationBuilder.DropTable(
                name: "Histories");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "MailServers");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "UserTasks");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "UserGroups");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "CustomTaskEndpoints");

            migrationBuilder.DropTable(
                name: "DocumentTypes");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "TaskItems");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "ACLs");
        }
    }
}
