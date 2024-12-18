using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using OpenDMS.Domain;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.Domain.Entities.OrganizationUnits;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Entities.Tasks;
using OpenDMS.Domain.Entities.Users;
using OpenDMS.Domain.Entities.V2;
using OpenDMS.Domain.Entities.Workflow;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Events.Types;
using OpenDMS.Domain.QueryBuilder;
using OpenDMS.MultiTenancy.DbContexts;
using OpenDMS.MultiTenancy.Interfaces;
using System.Data;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OpenDMS.Infrastructure.Database.DbContext;

public class ApplicationDbContext : MultiTenantDbContext
{
    protected readonly ILogger<MultiTenantDbContext> logger;

    //IMPOSTAZIONI
    public DbSet<Counter> Counters { get; set; }
    public DbSet<AppSetting> AppSettings { get; set; }
    public DbSet<UserSetting> UserSettings { get; set; }
    public DbSet<UISetting> UISettings { get; set; }
    public DbSet<TranslatedText> TranslatedTexts { get; set; }
    public DbSet<TaskListCustomFilter> UserFilters { get; set; }
    public DbSet<CustomPage> CustomPages { get; set; }

    public DbSet<ExternalDatasource> ExternalDataSources { get; set; }
    public DbSet<FieldType> FieldTypes { get; set; }
    public DbSet<LookupTable> LookupTables { get; set; }
    //    public DbSet<ApplicationProcess> ApplicationProcesses { get; set; }

    //TIPOLOGIE DOCUMENTALI
    public DbSet<DocumentType> DocumentTypes { get; set; }
    public DbSet<DocumentTypeField> DocumentTypeFields { get; set; }
    public DbSet<DocumentTypeWorkflow> DocumentTypeWorkflows { get; set; }

    //ACL
    public DbSet<ACL> ACLs { get; set; }
    public DbSet<ACLPermission> ACLPermissions { get; set; }

    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }


    //ORGANIGRAMMA
    public DbSet<OrganizationNode> OrganizationNodes { get; set; }
    public DbSet<UserGroup> UserGroups { get; set; }
    public DbSet<UserGroupRole> UserGroupRoles { get; set; }

    //DOCUMENTO
    public DbSet<Company> Companies { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<DocumentImage> Images { get; set; }
    public DbSet<DocumentPermission> DocumentPermissions { get; set; }
    public DbSet<FolderContent> FolderContents { get; set; }
    public DbSet<DocumentRelationship> DocumentRelationships { get; set; }
    public DbSet<DocumentField> DocumentFields { get; set; }
    public DbSet<DocumentBlobField> DocumentBlobFields { get; set; }
    public DbSet<DocumentRecipient> DocumentRecipients { get; set; }
    public DbSet<ImageVersion> ImageVersions { get; set; }
    //    public DbSet<DocumentProcessInstance> DocumentProcesses { get; set; }
    public DbSet<PostIt> Postit { get; set; }


    //CRONISTORIA
    public DbSet<HistoryEntry> Histories { get; set; }
    public DbSet<HistoryRecipient> HistoryRecipients { get; set; }
    public DbSet<HistoryDocument> HistoryDocuments { get; set; }



    //CONTATTI
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<ContactAddress> ContactAddresses { get; set; }
    public DbSet<ContactDigitalAddress> ContactDigitalAddresses { get; set; }
    public DbSet<ContactAddressRule> ContactAddressRules { get; set; }

    //HUMAN TASKS
    public DbSet<TaskItem> TaskItems { get; set; }
    public DbSet<UserTask> UserTasks { get; set; }
    public DbSet<TaskProgress> TaskProgress { get; set; }
    public DbSet<TaskAttachment> TaskAttachments { get; set; }
    public DbSet<TaskRecipient> TaskRecipients { get; set; }
    //public DbSet<TaskVariable> TaskVariables { get; set; }


    /* MAIL */
    public DbSet<Mailbox> Mailboxes { get; set; }
    public DbSet<MailServer> MailServers { get; set; }
    public DbSet<MailEntry> MailEntries { get; set; }

    /* WORKFLOW */
    public DbSet<CustomTaskEndpoint> CustomTaskEndpoints { get; set; }
    public DbSet<CustomTaskGroup> CustomTaskGroups { get; set; }
    public DbSet<CustomTaskItem> CustomTaskItems { get; set; }
    public DbSet<ProcessInstance> ProcessInstances { get; set; }



    public DbSet<DistributedLock> DistributedLocks { get; set; }


    public ApplicationDbContext(ITenantContext<Tenant> tenantContext, ILogger<MultiTenantDbContext> logger) : base(tenantContext)
    {
        this.logger = logger;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));

    }



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Contact>()
           .HasMany(j => j.Contacts)
           .WithOne(j => j.Parent)
           .HasForeignKey(j => j.ParentId);

        ////appSettings.Add(new AppSetting() { t.API.TenantManager", Value = "" });
        //builder.Entity<AppSetting>().HasData((new AppSetting() { Name = "Endpoint.API.Admin", Value = "" }));
        //builder.Entity<AppSetting>().HasData((new AppSetting() { Name = "Endpoint.API.Search", Value = "" }));
        //builder.Entity<AppSetting>().HasData((new AppSetting() { Name = "Endpoint.API.Preview", Value = "" }));
        //builder.Entity<AppSetting>().HasData((new AppSetting() { Name = "Endpoint.API.Identity", Value = "" }));
        //builder.Entity<AppSetting>().HasData((new AppSetting() { Name = "Endpoint.API.Workflow", Value = "" }));
        //builder.Entity<AppSetting>().HasData((new AppSetting() { Name = "Endpoint.API.Documents", Value = "" }));
        //builder.Entity<AppSetting>().HasData((new AppSetting() { Name = "Endpoint.API.TaskList", Value = "" }));

        //builder.Entity<Company>().HasData(new Company() { Id = 0, Description = "Ambiente Condiviso", RootOrganizationNode = "", OffLine = false });
        builder.Entity<Company>().HasData((new Company() { Id = 1, Description = "Ambiente Principale" }));

        builder.Entity<LookupTable>().HasData((new LookupTable() { TableId = "$TABLES$", Id = "$TABLES$", Description = "Registro delle Tabelle" }));
        builder.Entity<LookupTable>().HasData((new LookupTable() { TableId = "$TABLES$", Id = "$CATEGORIES$", Description = "Categorie Documentali" }));
        builder.Entity<LookupTable>().HasData((new LookupTable() { TableId = "$TABLES$", Id = "$PERMISSIONS$", Description = "Permessi" }));
        builder.Entity<LookupTable>().HasData((new LookupTable() { TableId = "$TABLES$", Id = "$EVENTS$", Description = "Eventi" }));
        //builder.Entity<LookupTable>().HasData((new LookupTable() { TableId = "$TABLES$", Id = "$REGISTRIES$", Description = "Registri di Protocollo" }));
        builder.Entity<LookupTable>().HasData((new LookupTable() { TableId = "$TABLES$", Id = TaskConstants.CONST_TASK_GROUPS, Description = "Categorie Task" }));
        builder.Entity<LookupTable>().HasData((new LookupTable() { TableId = "$TABLES$", Id = TaskConstants.CONST_TASK_PRIORITIES, Description = "Priorità" }));

        builder.Entity<LookupTable>().HasData((new LookupTable() { TableId = "$CATEGORIES$", Id = "$PURCHASE$", Description = "Acquisti" }));
        builder.Entity<LookupTable>().HasData((new LookupTable() { TableId = "$CATEGORIES$", Id = "$SALES$", Description = "Vendite" }));
        builder.Entity<LookupTable>().HasData((new LookupTable() { TableId = "$CATEGORIES$", Id = "$HR$", Description = "Personale" }));
        builder.Entity<LookupTable>().HasData((new LookupTable() { TableId = "$CATEGORIES$", Id = "$MANAGEMENT$", Description = "Amministrazione" }));
        builder.Entity<LookupTable>().HasData((new LookupTable() { TableId = "$CATEGORIES$", Id = "$ACCOUNTING$", Description = "Contabilità" }));
        builder.Entity<LookupTable>().HasData((new LookupTable() { TableId = "$CATEGORIES$", Id = "$TEMPLATES$", Description = "Modelli" }));
        builder.Entity<LookupTable>().HasData((new LookupTable() { TableId = "$CATEGORIES$", Id = "$PROTOCOL$", Description = "Protocollo" }));
        builder.Entity<LookupTable>().HasData((new LookupTable() { TableId = "$CATEGORIES$", Id = "$MAIL$", Description = "Corrispondenza" }));
        builder.Entity<LookupTable>().HasData((new LookupTable() { TableId = "$CATEGORIES$", Id = "$WORKFLOW$", Description = "Processi" }));

        builder.Entity<LookupTable>().HasData((new LookupTable() { TableId = TaskConstants.CONST_TASK_GROUPS, Id = "100.NONE", Description = "Attività Generica" }));


        builder.Entity<LookupTable>().HasData((new LookupTable() { TableId = TaskConstants.CONST_TASK_PRIORITIES, Id = "100.LOW", Description = "Bassa" }));
        builder.Entity<LookupTable>().HasData((new LookupTable() { TableId = TaskConstants.CONST_TASK_PRIORITIES, Id = "200.MEDIUM", Description = "Media" }));
        builder.Entity<LookupTable>().HasData((new LookupTable() { TableId = TaskConstants.CONST_TASK_PRIORITIES, Id = "300.HIGH", Description = "Alta" }));
        builder.Entity<LookupTable>().HasData((new LookupTable() { TableId = TaskConstants.CONST_TASK_PRIORITIES, Id = "400.URGENT", Description = "Urgente" }));


        //builder.Entity<LookupTable>().HasData((new LookupTable() { TableId = "$REGISTRIES$", Id = "GEN", Description = "Generale" }));
        //builder.Entity<LookupTable>().HasData((new LookupTable() { TableId = "$REGISTRIES$", Id = "DIG", Description = "Digitale" }));
        //builder.Entity<LookupTable>().HasData((new LookupTable() { TableId = "$REGISTRIES$", Id = "RIS", Description = "Riservato" }));

        // Creazione Tabella  Permessi
        foreach (var p in PermissionType.Name.Keys)
            builder.Entity<LookupTable>().HasData((new LookupTable() { TableId = "$PERMISSIONS$", Id = p, Description = PermissionType.Name[p] }));

        // Creazione tabella Eventi
        foreach (var p in typeof(EventType).GetFields(BindingFlags.Static | BindingFlags.Public))
        {
            var value = p.GetRawConstantValue().ToString();
            string group = value.IndexOf('.') > 0 ? value.Substring(0, value.IndexOf('.')) : "";
            builder.Entity<LookupTable>().HasData((new LookupTable() { TableId = "$EVENTS$", Id = value, Description = p.Name, Annotation = group }));
        }


        // Creazione Gruppi di Utenti
        builder.Entity<Role>().HasData((new Role() { Id = SpecialUser.AdminRole, RoleName = "Amministratore" }));
        builder.Entity<Role>().HasData((new Role() { Id = SpecialUser.ServiceRole, RoleName = "Servizio Applicativo" }));
        builder.Entity<Role>().HasData((new Role() { Id = SpecialUser.WorkflowArchitect, RoleName = "Progettista di Workflow" }));
        builder.Entity<Role>().HasData((new Role() { Id = SpecialUser.User, RoleName = "Utente Interattivo" }));

        // Creazione utente 
        var adminId = "00000000-0000-0000-0000-000000000000";// Guid.NewGuid().ToString();
        builder.Entity<Contact>().HasData((new Contact() { Id = adminId, ContactType = ContactType.Contact, FullName = "Utente di Sistema", FriendlyName = "Utente di Sistema", SearchName = "Utente di Sistema" }));
        builder.Entity<User>().HasData((new User() { Id = SpecialUser.SystemUser, ContactId = adminId }));

        //        builder.Entity<ContactDigitalAddress>().HasData(new ContactDigitalAddress() { Id=1, ContactId = adminId, Address = "admin@local" , Name = "Mail", SearchName = "Mail", CreationUser = "admin", DigitalAddressType = Domain.Enumerators.DigitalAddressType.Email });
        //        builder.Entity<UserSetting>().HasData(new UserSetting() { ContactId = adminId, AttributeId = UserAttributes.CONST_NOTIFICATION_TYPE, CompanyId = 0, Value = "Mail" });
        //        builder.Entity<UserSetting>().HasData(new UserSetting() { ContactId = adminId, AttributeId = UserAttributes.CONST_NOTIFICATION_MAIL, CompanyId = 0, Value = "admin@local" });
        //        builder.Entity<MailServer>().HasData(new MailServer() { Id=1, AuthenticationType=AuthenticationType.None, Domain= "local", MailType=MailType.Mail, Status = MailServerStatus.Active });

        // Creazione Templates
        builder.Entity<AppSetting>().HasData(new AppSetting() { Name = NotificationConstants.CONST_TEMPLATE_NOTIFY + ".Title", CompanyId = 0, Value = "Hai una nuova attività: {{title}}" });
        builder.Entity<AppSetting>().HasData(new AppSetting() { Name = NotificationConstants.CONST_TEMPLATE_NOTIFY + ".Body", CompanyId = 0, Value = "{{description}}<br/><br/>{{Sender.FullName}}" });
        builder.Entity<AppSetting>().HasData(new AppSetting() { Name = NotificationConstants.CONST_TEMPLATE_NOTIFY + ".CC.Title", CompanyId = 0, Value = "Hai un nuovo messaggio: {{title}}" });
        builder.Entity<AppSetting>().HasData(new AppSetting() { Name = NotificationConstants.CONST_TEMPLATE_NOTIFY + ".CC.Body", CompanyId = 0, Value = "{{description}}<br/><br/>{{Sender.FullName}}" });
        builder.Entity<AppSetting>().HasData(new AppSetting() { Name = NotificationConstants.CONST_TEMPLATE_NOTIFY_EXCEPTION + ".Title", CompanyId = 0, Value = "Si è verificato un errore" });
        builder.Entity<AppSetting>().HasData(new AppSetting() { Name = NotificationConstants.CONST_TEMPLATE_NOTIFY_EXCEPTION + ".Body", CompanyId = 0, Value = "{{description}}" });

        builder.Entity<AppSetting>().HasData(new AppSetting() { Name = NotificationConstants.CONST_TEMPLATE_NEWMAIL + ".Title", CompanyId = 0, Value = "Hai un messaggio di posta elettronica da gestire" });
        builder.Entity<AppSetting>().HasData(new AppSetting() { Name = NotificationConstants.CONST_TEMPLATE_NEWMAIL + ".Body", CompanyId = 0, Value = "Ricordati di assegnarlo a chi di competenza o di gestirlo tu stesso." });

        builder.Entity<AppSetting>().HasData(new AppSetting() { Name = NotificationConstants.CONST_TEMPLATE_ATTACHMENT_LINKED, CompanyId = 0, Value = "" });
        builder.Entity<AppSetting>().HasData(new AppSetting() { Name = NotificationConstants.CONST_TEMPLATE_ATTACHMENT_NOTFOUND, CompanyId = 0, Value = "" });

        builder.Entity<AppSetting>().HasData(new AppSetting() { Name = NotificationConstants.CONST_TEMPLATE_REFERENT + ".Title", CompanyId = 0, Value = "Sei il nuovo referente di un fascicolo" });
        builder.Entity<AppSetting>().HasData(new AppSetting() { Name = NotificationConstants.CONST_TEMPLATE_REFERENT + ".Body", CompanyId = 0, Value = "Il fascicolo è: {{Document.Description}}<br/><br/>{{Task.Description}}<br/> {{Sender.FullName}}" });
        builder.Entity<AppSetting>().HasData(new AppSetting() { Name = NotificationConstants.CONST_TEMPLATE_NO_REFERENT + ".Title", CompanyId = 0, Value = "Non sei più il referente di un fascicolo" });
        builder.Entity<AppSetting>().HasData(new AppSetting() { Name = NotificationConstants.CONST_TEMPLATE_NO_REFERENT + ".Body", CompanyId = 0, Value = "Il fascicolo è: {{Document.Description}}<br/><br/>{{Task.Description}}<br/> {{Sender.FullName}}" });

        builder.Entity<AppSetting>().HasData(new AppSetting() { Name = NotificationConstants.CONST_TEMPLATE_REFERENT_CHANGES + ".Title", CompanyId = 0, Value = "E' stato modificato un documento di cui sei il referente" });
        builder.Entity<AppSetting>().HasData(new AppSetting() { Name = NotificationConstants.CONST_TEMPLATE_REFERENT_CHANGES + ".Body", CompanyId = 0, Value = "Il documento modificato è: {{Document.Description}}<br/>" });

        // Creazione Custom Pages Iniziali
        builder.Entity<CustomPage>().HasData(new CustomPage() { Alignment = -1, Icon = "fa fa-sliders", PageId = "Admin", Title = "Amministrazione", URL = "/Admin/Home/Index", ToolTip = "Amministrazione", Permissions = ":admin" });
        builder.Entity<CustomPage>().HasData(new CustomPage() { Alignment = 0, Icon = "fa fa-bell", PageId = "Tasks", Title = "Attività", URL = "/tasks", ToolTip = "Attività", BadgeURL = "/internalapi/tasklist/badge", Permissions = "Task.View" });
        builder.Entity<CustomPage>().HasData(new CustomPage() { Alignment = 0, Icon = "fa fa-folder", PageId = "Folders", Title = "Documenti", URL = "/", ToolTip = "Documenti", Position = 1, Permissions = "" });
        builder.Entity<CustomPage>().HasData(new CustomPage() { Alignment = 0, Icon = "fa fa-envelope", PageId = "Mail", Title = "Posta Elettronica", URL = "/mail", BadgeURL = "/internalapi/mailentry/badge", ToolTip = "Posta Elettronica", Position = 2, Permissions = "Mail.Console" });
        builder.Entity<CustomPage>().HasData(new CustomPage() { Alignment = 0, Icon = "fa fa-sitemap", PageId = "WF", Title = "Processi", URL = "/BPMMonitor", ToolTip = "Dashboard Processi", Position = 100, Permissions = "Workflow.Dashboard" });

        var RoleAdmin = SpecialUser.AdminRole;
        var ACLGLobal = "$GLOBAL$";
        builder.Entity<ACL>().HasData((new ACL() { Id = ACLGLobal, Name = "Permessi Globali" }));
        foreach (var p in PermissionType.Name.Keys)
        {
            builder.Entity<ACLPermission>().HasData((new ACLPermission()
            {
                ACLId = ACLGLobal,
                ProfileId = RoleAdmin,
                ProfileType = ProfileType.Role,
                PermissionId = p,
                Authorization = AuthorizationType.Granted
            }));
        }
        var ACLProtocol = "$PROTOCOL$";
        builder.Entity<ACL>().HasData((new ACL() { Id = ACLProtocol, Name = "Protocollo" }));
        //var ACLTemplates = "$TEMPLATE$";
        //builder.Entity<ACL>().HasData((new ACL() { Id = ACLTemplates, Name = "Templates" }));
        var ACLAccounting = "$ACCOUNTING$";
        builder.Entity<ACL>().HasData((new ACL() { Id = ACLAccounting, Name = "Contabilità" }));
        var ACLManagement = "$MANAGEMENT$";
        builder.Entity<ACL>().HasData((new ACL() { Id = ACLManagement, Name = "Amministrazione" }));
        var ACLSales = "$SALES$";
        builder.Entity<ACL>().HasData((new ACL() { Id = ACLSales, Name = "Vendite" }));
        var ACLPurchase = "$PURCHASE$";
        builder.Entity<ACL>().HasData((new ACL() { Id = ACLPurchase, Name = "Acquisti" }));
        var ACLHr = "$HR$";
        builder.Entity<ACL>().HasData((new ACL() { Id = ACLHr, Name = "Personale" }));
        var ACLMail = "$MAIL$";
        builder.Entity<ACL>().HasData((new ACL() { Id = ACLMail, Name = "Corrispondenza" }));
        var ACLWorkflow = "$WORKFLOW$";

        var acl = (new ACL() { Id = ACLWorkflow, Name = "Workflow" });
        builder.Entity<ACL>().HasData(acl);
        builder.Entity<ACLPermission>().HasData((new ACLPermission() { ACLId = ACLWorkflow, Authorization = AuthorizationType.Granted, PermissionId = PermissionType.CanCreate, ProfileId = SpecialUser.WorkflowArchitect, ProfileType = ProfileType.Role }));
        builder.Entity<ACLPermission>().HasData((new ACLPermission() { ACLId = ACLWorkflow, Authorization = AuthorizationType.Granted, PermissionId = PermissionType.CanCreate, ProfileId = SpecialUser.AdminRole, ProfileType = ProfileType.Role }));

        string GroupRoles = null;
        builder.Entity<OrganizationNode>().HasData((new OrganizationNode() { LeftBound = 1, RightBound = 2, StartISODate = 0, EndISODate = 99999999, UserGroupId = GroupRoles }));

        builder.Entity<DocumentType>().HasData((new DocumentType() { Id = "$NOTA-INGRESSO$", ACLId = ACLProtocol, CategoryId = ACLProtocol, ContentType = ContentType.Document, Name = "Nota In Ingresso", DescriptionLabel = "Oggetto di Protocollo", DocumentDateLabel = "Data Documento", ToBeIndexed = true, LabelPosition = LabelPosition.Manuale, Direction = 1, DescriptionMandatory = true }));
        builder.Entity<DocumentType>().HasData((new DocumentType() { Id = "$NOTA-USCITA$", ACLId = ACLProtocol, CategoryId = ACLProtocol, ContentType = ContentType.Document, Name = "Nota In Uscita", DescriptionLabel = "Oggetto di Protocollo", DocumentDateLabel = "Data Documento", ToBeIndexed = true, ToBePublished = true, LabelPosition = LabelPosition.Manuale, Direction = 2, DescriptionMandatory = true }));
        builder.Entity<DocumentType>().HasData((new DocumentType() { Id = "$NOTA-INTERNA$", ACLId = ACLProtocol, CategoryId = ACLProtocol, ContentType = ContentType.Document, Name = "Nota Interna", DescriptionLabel = "Oggetto di Protocollo", DocumentDateLabel = "Data Documento", ToBeIndexed = true, LabelPosition = LabelPosition.Manuale, Direction = 0, DescriptionMandatory = true }));

        //builder.Entity<DocumentType>().HasData((new DocumentType() { Id = "$WORKFLOW$", ACLId = ACLWorkflow, CategoryId = ACLWorkflow, ContentType = ContentType.Folder, Name = "Fascicolo Processo", DocumentNumberLabel="ID Univoco", DescriptionLabel = "Descrizione", DocumentDateLabel = "", ToBeIndexed = true }));
        builder.Entity<DocumentType>().HasData((new DocumentType() { Id = "$DIAGRAM$", ACLId = ACLWorkflow, CategoryId = ACLWorkflow, ContentType = ContentType.Workflow, Name = "Processo", DocumentNumberLabel = "", DescriptionLabel = "Descrizione", DocumentDateLabel = "", ToBeIndexed = false, ToBePublished = true, AcceptedExtensions = ".bpmn", CreationFormKey = "$NEW-DIAGRAM-TEMPLATE$", DetailPage = "/Details/Process", Icon = "fa fa-cogs", DescriptionMandatory = true }));
        //        builder.Entity<DocumentType>().HasData((new DocumentType() { Id = "$DOCUMENT-DIAGRAM$", ACLId = ACLWorkflow, GroupName = ACLWorkflow, ContentType = ContentType.Document, Name = "Processo Automatico su Documento", DocumentNumberLabel = "ID Univoco", DescriptionLabel = "Descrizione", DocumentDateLabel = "", ToBeIndexed = true, ProtocolDirection = "-", ProtocolRegister = "",  ArchivingStrategy = ArchivingStrategy.Template, AcceptedExtensions = ".bpmn" }));
        builder.Entity<DocumentType>().HasData((new DocumentType() { Id = "$FORM$", ACLId = ACLWorkflow, CategoryId = ACLWorkflow, ContentType = ContentType.Form, Name = "Modulo Digitale", DocumentNumberLabel = "", DescriptionLabel = "Descrizione", DocumentDateLabel = "", ToBeIndexed = false, ToBePublished = true, AcceptedExtensions = ".form,.html,.formio", CreationFormKey = "$NEW-FORM-TEMPLATE$", DetailPage = "/Details/Form", Icon = "fa fa-table", DescriptionMandatory = true }));
        builder.Entity<DocumentType>().HasData((new DocumentType() { Id = "$TEMPLATE$", ACLId = ACLWorkflow, CategoryId = ACLWorkflow, ContentType = ContentType.Template, Name = "Template", DocumentNumberLabel = "", DescriptionLabel = "Descrizione", DocumentDateLabel = "", ToBeIndexed = false, ToBePublished = true, AcceptedExtensions = ".html,.txt,.docx,.xlsx,.pdf", CreationFormKey = "$NEW-TEMPLATE-TEMPLATE$", DetailPage = "/Details/Template", Icon = "fa fa-edit", DescriptionMandatory = true }));
        //        builder.Entity<DocumentType>().HasData((new DocumentType() { Id = "$FORM-HTML$", ACLId = ACLWorkflow, GroupName = ACLWorkflow, ContentType = ContentType.Document, Name = "Form Html", DocumentNumberLabel = "ID Univoco", DescriptionLabel = "Descrizione", DocumentDateLabel = "", ToBeIndexed = true, ProtocolDirection = "-", ProtocolRegister = "",  ArchivingStrategy = ArchivingStrategy.Template, AcceptedExtensions = ".formhtml" }));
        builder.Entity<DocumentType>().HasData((new DocumentType() { Id = "$DMN$", ACLId = ACLWorkflow, CategoryId = ACLWorkflow, ContentType = ContentType.DMN, Name = "Matrice Decisionale", DocumentNumberLabel = "ID Univoco", DescriptionLabel = "Descrizione", DocumentDateLabel = "", ToBeIndexed = false, AcceptedExtensions = ".dmn", ToBePublished = true, CreationFormKey = "$NEW-DMN-TEMPLATE$", DetailPage = "/Details/DMN", Icon = "fa fa-question-circle", DescriptionMandatory = true }));

        builder.Entity<DocumentType>().HasData((new DocumentType() { Id = "$MAIL-INBOUND$", ACLId = ACLMail, CategoryId = ACLMail, ContentType = ContentType.Email, Name = "EMail Ricevuta", DocumentNumberLabel = "", DescriptionLabel = "Oggetto", Direction = 1, DocumentDateLabel = "Data Ricezione", ToBeIndexed = true, AcceptedExtensions = ".eml", DetailPage = "/Details/Mail", Icon = "fa fa-envelope", DescriptionMandatory = true }));
        builder.Entity<DocumentType>().HasData((new DocumentType() { Id = "$MAIL-OUTBOUND$", ACLId = ACLMail, CategoryId = ACLMail, ContentType = ContentType.Email, Name = "EMail Spedita", DocumentNumberLabel = "", DescriptionLabel = "Oggetto", Direction = 2, DocumentDateLabel = "Data Invio", ToBeIndexed = true, ToBePublished = true, AcceptedExtensions = ".eml", DetailPage = "/Details/Mail", Icon = "fa fa-send", DescriptionMandatory = true }));
        builder.Entity<DocumentType>().HasData((new DocumentType() { Id = "$PEC-INBOUND$", ACLId = ACLMail, CategoryId = ACLMail, ContentType = ContentType.Email, Name = "PEC Ricevuta", DocumentNumberLabel = "", DescriptionLabel = "Oggetto", Direction = 1, DocumentDateLabel = "Data Ricezione", ToBeIndexed = true, AcceptedExtensions = ".eml", DetailPage = "/Details/Mail", Icon = "fa fa-envelope", DescriptionMandatory = true }));
        builder.Entity<DocumentType>().HasData((new DocumentType() { Id = "$PEC-OUTBOUND$", ACLId = ACLMail, CategoryId = ACLMail, ContentType = ContentType.Email, Name = "PEC Spedita", DocumentNumberLabel = "", DescriptionLabel = "Oggetto", Direction = 2, DocumentDateLabel = "Data Invio", ToBeIndexed = true, ToBePublished = true, AcceptedExtensions = ".eml", DetailPage = "/Details/Mail", Icon = "fa fa-send", DescriptionMandatory = true }));

        //builder.Entity<DocumentImage>().HasData(new DocumentImage() { Id = 1, FileExtension = ".bpm", FileManager = "", FileName = "/templates/BPMN_Documentale.html", OriginalFileName = "/templates/BPMN_Documentale.html", VersionNumber = 1, RevisionNumber = 0 });
        //builder.Entity<DocumentImage>().HasData(new DocumentImage() { Id = 1, FileExtension = ".formhtml", FileManager = "", FileName = "/templates/BPMN_Documentale.html", OriginalFileName = "/templates/BPMN_Documentale.html", VersionNumber = 1, RevisionNumber = 0 });
        //builder.Entity<DocumentImage>().HasData(new DocumentImage() { Id = 1, FileExtension = ".dmn", FileManager = "", FileName = "/templates/BPMN_Documentale.html", OriginalFileName = "/templates/BPMN_Documentale.html", VersionNumber = 1, RevisionNumber = 0 });

        //builder.Entity<DocumentImage>().HasData(new DocumentImage() { Id = 1, FileExtension = ".formhtml", FileManager = "", FileName = "/templates/BPMN_Documentale.html", OriginalFileName = "/templates/BPMN_Documentale.html", VersionNumber = 1, RevisionNumber = 0 });
        //builder.Entity<DocumentImage>().HasData(new DocumentImage() { Id = 1, FileExtension = ".formhtml", FileManager = "", FileName = "/templates/BPMN_Documentale.html", OriginalFileName = "/templates/BPMN_Documentale.html", VersionNumber = 1, RevisionNumber = 0 });

        //var ProcessFolder = new Document() { Id = 1, DocumentTypeId = null, CompanyId = 0, ACLId = ACLWorkflow, Description = "Archivio Processi", DocumentDate = DateTime.MinValue, DocumentNumber = "Processi", DocumentStatus = DocumentStatus.Active, ExternalId = "$WORKFLOW-FOLDER$", Icon = "fa fa-cogs", ContentType = ContentType.Folder, Owner = SpecialUser.SystemUser };
        //var NewForm = new Document() { Id = 2, DocumentTypeId = null, CompanyId = 0, ACLId = ACLWorkflow, Description = "Modello Creazione Form", DocumentDate = DateTime.MinValue, DocumentNumber = "", DocumentStatus = DocumentStatus.Active, ExternalId = "$NEW-FORM$", Icon = "fa fa-table", ContentType = ContentType.Form, Owner=SpecialUser.SystemUser};
        //var NewDMN = new Document() { Id = 3, DocumentTypeId = null, CompanyId = 0, ACLId = ACLWorkflow, Description = "Modello Creazione DMN", DocumentDate = DateTime.MinValue, DocumentNumber = "", DocumentStatus = DocumentStatus.Active, ExternalId = "$NEW-DMN$", Icon = "fa fa-question-circle", ContentType = ContentType.DMN, Owner = SpecialUser.SystemUser };
        //var NewProcess = new Document() { Id = 4, DocumentTypeId = null, CompanyId = 0, ACLId = ACLWorkflow, Description = "Modello Creazione Processi", DocumentDate = DateTime.MinValue, DocumentNumber = "", DocumentStatus = DocumentStatus.Active, ExternalId = "$NEW-PROCESS$", Icon = "fa fa-cogs", ContentType = ContentType.Workflow, Owner = SpecialUser.SystemUser };
        //var NewTemplate = new Document() { Id = 5, DocumentTypeId = null, CompanyId = 0, ACLId = ACLWorkflow, Description = "Modello Creazione Templates", DocumentDate = DateTime.MinValue, DocumentNumber = "", DocumentStatus = DocumentStatus.Active, ExternalId = "$NEW-TEMPLATE$", Icon = "fa fa-edit", ContentType = ContentType.Template, Owner = SpecialUser.SystemUser };



        //builder.Entity<Document>().HasData(NewForm);
        //builder.Entity<Document>().HasData(NewDMN);
        //builder.Entity<Document>().HasData(NewProcess);
        //builder.Entity<Document>().HasData(NewTemplate);
        //builder.Entity<Document>().HasData(ProcessFolder);

        //builder.Entity<DocumentPermission>().HasData(new DocumentPermission() { DocumentId = NewForm.Id, ProfileId = SpecialUser.AdminRole, ProfileType = ProfileType.Role, PermissionId = PermissionType.CanView, Authorization = AuthorizationType.Granted });
        //builder.Entity<DocumentPermission>().HasData(new DocumentPermission() { DocumentId = NewForm.Id, ProfileId = SpecialUser.AdminRole, ProfileType = ProfileType.Role, PermissionId = PermissionType.CanViewContent, Authorization = AuthorizationType.Granted });
        //builder.Entity<DocumentPermission>().HasData(new DocumentPermission() { DocumentId = NewDMN.Id, ProfileId = SpecialUser.AdminRole, ProfileType = ProfileType.Role, PermissionId = PermissionType.CanView, Authorization = AuthorizationType.Granted });
        //builder.Entity<DocumentPermission>().HasData(new DocumentPermission() { DocumentId = NewDMN.Id, ProfileId = SpecialUser.AdminRole, ProfileType = ProfileType.Role, PermissionId = PermissionType.CanViewContent, Authorization = AuthorizationType.Granted });
        //builder.Entity<DocumentPermission>().HasData(new DocumentPermission() { DocumentId = NewProcess.Id, ProfileId = SpecialUser.AdminRole, ProfileType = ProfileType.Role, PermissionId = PermissionType.CanView, Authorization = AuthorizationType.Granted });
        //builder.Entity<DocumentPermission>().HasData(new DocumentPermission() { DocumentId = NewProcess.Id, ProfileId = SpecialUser.AdminRole, ProfileType = ProfileType.Role, PermissionId = PermissionType.CanViewContent, Authorization = AuthorizationType.Granted });
        //builder.Entity<DocumentPermission>().HasData(new DocumentPermission() { DocumentId = NewTemplate.Id, ProfileId = SpecialUser.AdminRole, ProfileType = ProfileType.Role, PermissionId = PermissionType.CanView, Authorization = AuthorizationType.Granted });
        //builder.Entity<DocumentPermission>().HasData(new DocumentPermission() { DocumentId = NewTemplate.Id, ProfileId = SpecialUser.AdminRole, ProfileType = ProfileType.Role, PermissionId = PermissionType.CanViewContent, Authorization = AuthorizationType.Granted });

        //builder.Entity<DocumentPermission>().HasData(new DocumentPermission() { DocumentId = ProcessFolder.Id, ProfileId = SpecialUser.WorkflowArchitect, ProfileType = ProfileType.Role, PermissionId = PermissionType.CanView, Authorization = AuthorizationType.Granted });
        //builder.Entity<DocumentPermission>().HasData(new DocumentPermission() { DocumentId = ProcessFolder.Id, ProfileId = SpecialUser.WorkflowArchitect, ProfileType = ProfileType.Role, PermissionId = PermissionType.CanViewContent, Authorization = AuthorizationType.Granted });
        //builder.Entity<DocumentPermission>().HasData(new DocumentPermission() { DocumentId = ProcessFolder.Id, ProfileId = SpecialUser.AdminRole, ProfileType = ProfileType.Role, PermissionId = PermissionType.CanView, Authorization = AuthorizationType.Granted });
        //builder.Entity<DocumentPermission>().HasData(new DocumentPermission() { DocumentId = ProcessFolder.Id, ProfileId = SpecialUser.AdminRole, ProfileType = ProfileType.Role, PermissionId = PermissionType.CanViewContent, Authorization = AuthorizationType.Granted });

        //builder.Entity<Document>().HasData((new Document() { DocumentTypeId = " ", CompanyId = 0, Description = "Archivio Reports", DocumentDate = DateTime.MinValue, DocumentNumber = "Processi", DocumentStatus = DocumentStatus.Active, ExternalId = "$WORKFLOW-FOLDER$", Icon = "fa fa-cogs", ContentType = ContentType.Folder }));
    }

    public virtual string FormatDate(DateTime Date)
    {
        return Date.ToString("yyyyMMdd");
    }

    public virtual string GetSql(string Fields, string Tables, string Where, string GroupBy, string Having, String OrderBy, int Skip, int Take)
    {
        string Sql = "SELECT " + Fields + " FROM " + Tables;
        if (!String.IsNullOrEmpty(Where)) Sql += " WHERE " + Where;
        if (!String.IsNullOrEmpty(GroupBy)) Sql += " GROUP BY " + GroupBy;
        if (!String.IsNullOrEmpty(Having)) Sql += " Having " + Having;
        if (!String.IsNullOrEmpty(OrderBy)) Sql += " ORDER BY " + OrderBy;
        if (Take > 0)
            Sql += $" OFFSET {Skip} FETCH NEXT {Take} ROWS ONLY";
        return Sql;
    }

    public virtual IEnumerable<QueryRow> Select(string sql)
    {
        IDbConnection connection = Database.GetDbConnection();
        IDbCommand command = connection.CreateCommand();
        command.CommandTimeout = 60000;
        command.CommandType = CommandType.Text;
        connection.Open();
        command.CommandText = sql;
        using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection))
        {
            while (reader != null && reader.Read())
            {
                QueryRow R = new QueryRow();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string k = reader.GetName(i);
                    string v = reader[i].ToString();
                    if (reader[i].GetType() == typeof(DateTime))
                        v = reader.GetDateTime(i).ToString("s");
                    R.Add(k, v);
                }
                yield return R;
            }
        }
    }

    public virtual async Task<List<T>> Select<T>(string Fields, string Tables, string Where, String GroupBy, String Having, String OrderBy, int Skip, int Take, Func<Dictionary<string, string>, int, Task<T>> map)
    {
        try
        {
            List<T> Rows = new List<T>();
            IDbConnection connection = Database.GetDbConnection();
            IDbCommand command = connection.CreateCommand();
            command.CommandTimeout = 60000;
            command.CommandType = CommandType.Text;
            connection.Open();
            command.CommandText = GetSql(Fields, Tables, Where, GroupBy, Having, OrderBy, Skip, Take);
            List<Dictionary<string, string>> readers = new List<Dictionary<string, string>>();
            using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection))
            {
                while (reader != null && reader.Read())
                {
                    Dictionary<string, string> R = new Dictionary<string, string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string v = reader[i].ToString();
                        if (reader[i].GetType() == typeof(DateTime))
                            //v = reader.GetDateTime(i).ToString("dd/MM/yyyy hh:mm:ss");
                            v = reader.GetDateTime(i).ToString("s");
                        R.Add(reader.GetName(i), v);
                    }
                    readers.Add(R);
                }
            }
            int rowNumber = 0;
            foreach (var R in readers)
            {
                var row = await map(R, rowNumber++);
                Rows.Add(row);
            }
            return Rows;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "SELECT");
            throw;
        }

    }

    public virtual int Count(string sql)
    {
        //string Sql = "SELECT COUNT(*) FROM " + Tables;
        //if (!String.IsNullOrEmpty(Where)) Sql += " WHERE " + Where;
        IDbConnection connection = Database.GetDbConnection();
        try
        {
            IDbCommand command = connection.CreateCommand();
            command.CommandTimeout = 60000;
            command.CommandType = CommandType.Text;
            connection.Open();
            command.CommandText = sql;
            var o = command.ExecuteScalar();
            int r = (int)Convert.ChangeType(o, typeof(int));
            connection.Close();
            return r;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "SELECT");
            throw;
        }
        finally { 
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }
    }




    public int NextID(int CompanyId, string CounterId, int Year)
    {
        int r = 0;
        using (var T = Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
        {
            var c = Counters.Find(CompanyId, CounterId, Year);
            if (c != null)
            {
                r = c.Value;
            }
            else
            {
                c = new Counter() { CompanyId = CompanyId, CounterId = CounterId, Year = Year };
                Counters.Add(c);
            }
            r++;
            c.Value = r;
            SaveChanges();
        }
        return r;
    }
    public int LastID(int CompanyId, string CounterId, int Year)
    {
        int r = 0;
        var c = Counters.Find(CompanyId, CounterId, Year);
        if (c != null)
        {
            r = c.Value;
        }
        return r;
    }
    public void SetID(int CompanyId, string CounterId, int Year, int Value)
    {
        using (var T = Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
        {
            var c = Counters.Find(CompanyId, CounterId, Year);
            if (c == null)
            {
                c = new Counter() { CompanyId = CompanyId, CounterId = CounterId, Year = Year };
                Counters.Add(c);
            }
            c.Value = Value;
            SaveChanges();
        }
    }
    public void ResetID(int CompanyId, string CounterId, int Year)
    {
        SetID(CompanyId, CounterId, Year, 0);
    }


    public int NextID(string CounterId, int Year)
    {
        return NextID(0, CounterId, Year);
    }
    public int LastID(string CounterId, int Year)
    {
        return LastID(0, CounterId, Year);
    }
    public void SetID(string CounterId, int Year, int Value)
    {
        SetID(0, CounterId, Year, Value);

    }
    public void ResetID(string CounterId, int Year)
    {
        SetID(0, CounterId, Year, 0);
    }


    public int NextID(string CounterId)
    {
        return NextID(0, CounterId, 0);
    }
    public int LastID(string CounterId)
    {
        return LastID(0, CounterId, 0);
    }
    public void SetID(string CounterId, int Value)
    {
        SetID(0, CounterId, 0, Value);

    }
    public void Reset(string CounterId)
    {
        SetID(0, CounterId, 0, 0);
    }


    public override async Task<bool> CreateDatabase(Tenant T)
    {
        try
        {
            if (!Database.IsInMemory()) await Database.MigrateAsync();
            AppSettings.Add((new AppSetting() { Name = "Tenant.RootPath", Value = T.RootFolder }));
            AppSettings.Add((new AppSetting() { Name = "Tenant.IAM.URL", Value = T.URL }));
            AppSettings.Add((new AppSetting() { Name = "Tenant.IAM.Realm", Value = T.OpenIdConnectAuthority }));
            AppSettings.Add((new AppSetting() { Name = "Tenant.IAM.ClientId", Value = T.OpenIdConnectClientId }));
            AppSettings.Add((new AppSetting() { Name = "Tenant.IAM.ClientSecret", Value = T.OpenIdConnectClientSecret }));
            SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            throw;
        }
    }


    public virtual string GetTextFilter(string Field, OperatorType operatorType, string[] values)
    {
        string s = Field;
        string V = values[0];
        switch (operatorType)
        {
            case OperatorType.NotEqualTo:
                s += "<>" + V.Quoted(); break;
            case OperatorType.GreaterThan:
                s += ">" + V.Quoted(); break;
            case OperatorType.LessThan:
                s += "<" + V.Quoted(); break;
            case OperatorType.GreaterThanOrEqualTo:
                s += ">=" + V.Quoted(); break;
            case OperatorType.LessThanOrEqualTo:
                s += "<=" + V.Quoted(); break;
            case OperatorType.Contains:
                s += " LIKE " + ("%" + V.Replace(" ", "%") + "%").Quoted(); break;
            case OperatorType.NotContains:
                s += " NOT LIKE " + ("%" + V.Replace(" ", "%") + "%").Quoted(); break;
            case OperatorType.StarstWith:
                s += " LIKE " + (V + "%").Quoted(); break;
            case OperatorType.EndsWith:
                s += " LIKE " + ("%" + V).Quoted(); break;
            default:
                if (values.Length > 1)
                    s += " IN (" + string.Join(',', values.Select(a => a.Quoted())) + ")";
                else
                    s += "=" + V.Quoted();
                break;
        }

        return s;
    }

    public virtual string GetDateFilter(string Field, OperatorType operatorType, string[] values)
    {
        string s = Field;
        DateTime dt1 = DateTime.MinValue;
        DateTime dt2 = DateTime.MaxValue;
        List<string> NValues = new();
        for (int i = 0; i < values.Length; i++)
        {
            NValues.Add(values[i]);
            string h = i > 0 ? "T23:59.59" : NValues.Count > 0 ? "T00:00.00" : "";
            string d = i > 0 ? "2999-12-31" : "1900-01-01";
            var t = NValues[i].IndexOf("T");
            if (t > 0)
            {
                h = NValues[i].Substring(t);
                NValues[i] = NValues[i].Substring(0, t);
            }
            if (int.TryParse(NValues[i], out int it1))
                NValues[i] = ((it1 / 10000).ToString() + "-" + ((it1 / 100) % 100).ToString().PadLeft(2, '0') + "-" + (it1 % 100).ToString().PadLeft(2, '0') + h).Quoted();
            else
                if (DateTime.TryParse(NValues[i], out dt1))
                NValues[i] = (dt1.ToString("yyyy-MM-dd") + h).Quoted();
            else
                NValues[i] = (d + h).Quoted();
        }
        if (NValues.Count < 2) NValues.Append("'2999-12-31T23:59.59'");
        switch (operatorType)
        {
            case OperatorType.NotEqualTo:
                s += "<>" + NValues[0]; break;
            case OperatorType.GreaterThan:
                s += ">" + NValues[0]; break;
            case OperatorType.LessThan:
                s += "<" + NValues[0]; break;
            case OperatorType.GreaterThanOrEqualTo:
                s += ">=" + NValues[0]; break;
            case OperatorType.LessThanOrEqualTo:
                s += "<=" + NValues[0]; break;
            case OperatorType.Contains:
            case OperatorType.In:
                s += " BETWEEN " + NValues[0] + " AND " + NValues[1]; break;
            default:
                s += "=" + NValues[0]; break;
        }

        return s;
    }

    public virtual string GetNumberFilter(string Field, OperatorType operatorType, string[] values)
    {
        string s = Field;
        switch (operatorType)
        {
            case OperatorType.NotEqualTo:
                s += "<>" + values[0]; break;
            case OperatorType.GreaterThan:
                s += ">" + values[0]; break;
            case OperatorType.LessThan:
                s += "<" + values[0]; break;
            case OperatorType.GreaterThanOrEqualTo:
                s += ">=" + values[0]; break;
            case OperatorType.LessThanOrEqualTo:
                s += "<=" + values[0]; break;
            case OperatorType.In:
            case OperatorType.Contains:
                s += " IN (" + string.Join(',', values) + ")"; break;
            default:
                s += "=" + values[0]; break;
        }
        return s;
    }
}
