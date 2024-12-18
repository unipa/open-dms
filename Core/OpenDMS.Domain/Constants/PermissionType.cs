namespace OpenDMS.Domain.Constants
{
    public class PermissionType
    {
  

        public const string CanViewContent = "Document.ViewContent";
        public const string CanView = "Document.View";
        public const string CanEdit = "Document.Edit";
        public const string CanCreate = "Document.Create";
        public const string CanDelete = "Document.Delete";
        public const string CanAuthorize = "Document.Authorize";
        public const string CanAddContent = "Document.AddContent";
        public const string CanRemoveContent = "Document.RemoveContent";
        public const string CanShare = "Document.Share";
        public const string CanViewRegistry = "Document.History";
        public const string CanProtocol = "Document.Protocol";
        public const string CanUpdateProtocol = "Document.UpdateProtocol";
        public const string CanExecute = "Document.Execute";
        public const string CanPublish = "Document.Publish";

        public const string WorkflowDashboard = "Workflow.Dashboard";
        public const string WorkflowCanDeploy = "Workflow.CanDeploy";
        public const string WorkflowCanCustomize = "Workflow.CanCustomize";

        //     public const string PreferredInCreate = "Document.PreferredInCreate";

        // Permessi di gruppo
        public const string CanAdminGroups = "Team.Admin";
        public const string CanViewUp = "Team.ViewUp";
        public const string CanViewDown = "Team.ViewDown";
        public const string CanViewSide = "Team.ViewSide";
        public const string CanViewInBox = "Team.ReadInbox";
        public const string CanViewInBoxCC = "Team.ReadInboxCC";

        // Permessi di Task
        public const string Task_CanView = "Task.View";
        public const string Task_CanViewUsers = "Task.ViewUses";
        public const string Task_CanCreateTask = "Task.Create";
        public const string Task_CanCreateMessage = "Task.CreateMessage";


        // Permessi Mail
        public const string MailConsole = "Mail.Console";


        // Permessi di Amministrativi
        public const string CanAdminRoles = "Roles.Admin";
        public const string CanAdminACL = "ACL.Admin";
        public const string CanAdminDocumentTypes = "DocumentType.Admin";
        public const string CanAdminCompanies = "Company.Admin";
        public const string CanAdminDatasources = "Datasource.Admin";
        public const string CanAdminMeta = "Meta.Admin";

        public const string CanAdminTables = "Tables.Admin";
        public const string CanAdminMailServers = "MailServer.Admin";
        public const string CanAdminMessageTemplates = "Template.Admin";


        //Profilo Utente
        public const string Profile_CanHavePersonalFolder = "Profile.CanHavePersonalFolder";
        public const string Profile_CanCreateRootFolder = "Profile.CanCeateRootFolder";
        public const string Profile_CanCreateGenericDocument = "Profile.CreateGenericDocument";
        public const string Profile_CanSendMail = "Profile.SendMail";
        public const string Profile_CanHaveSignature = "Profile.Signature";
        public const string Profile_CanHaveClient = "Profile.Client";
        public const string Profile_CanHaveRemoteSignature = "Profile.RemoteSignatureService";
        public const string Profile_CanHandleDocuments = "Profile.CanHandleDocuments";


 

        public static Dictionary<string, string> Name = new Dictionary<string, string>()
        {
            { CanView, "Documento - Visibilità" },
            { CanViewContent, "Documento - Visibilità Contenuto" },
            { CanCreate, "Documento - Creazione" },
            { CanEdit, "Documento - Modifica Metadati" },
            { CanDelete, "Documento - Cancellazione" },
            { CanAuthorize, "Documento - Gestione Permessi" },
            { CanAddContent, "Documento - Gestione Contenuti" },
            { CanRemoveContent, "Documento - Rimozione Contenuti" },
            { CanShare, "Documento - Condivisione" },
            { CanViewRegistry, "Documento - Accesso Registro Cronistoria" },
 //           { CanProtocol, "Documento - Protocollazione" },
            //{ CanUpdateProtocol, "Documento - Gestione Dati di Protocollo" },
  //          { PreferredInCreate, "Documento - Mostra tra i preferiti in creazione" } ,
            { Profile_CanCreateGenericDocument, "Documento - Creazione Documento Generico" } ,

            { CanExecute, "Processo - Esecuzione flussi" },
            { WorkflowDashboard, "Processo - Accesso alla Dashboard" },

            { Profile_CanHavePersonalFolder, "Fascicolo - Gestione Fascicolo Personale" },
            { Profile_CanCreateRootFolder, "Fascicolo - Creazione Fascicoli Condivisi" },

            { CanViewUp, "Organigramma - Visione Strutture Superiori" },
            { CanViewDown, "Organigramma - Visione Strutture Inferiori" },
            { CanViewSide, "Organigramma - Visione Strutture Paritetiche" },
            { CanViewInBox, "Organigramma - Ricezione Notifiche di Struttura" },
            { CanViewInBoxCC, "Organigramma - Ricezione Notifche CC di Struttura" },

// Permessi Applicativi

            { Task_CanView, "Task - Accesso alla Dashboard" },
//            { Task_CanViewUsers, "Task - Accesso Task Colleghi" },
            { Task_CanCreateTask, "Task - Creazione Nuove Attività" },
            { Task_CanCreateMessage, "Task - Creazione Nuovi Messaggi" },

            { CanAdminGroups, "Amministrazione - Gestione Organigramma" },
            { CanAdminRoles, "Amministrazione - Gestione Ruoli" },
            { CanAdminDocumentTypes, "Amministrazione - Gestione Tipologie" },
            { CanAdminACL, "Amministrazione - Gestione Permessi su Tipologie" },
            { CanAdminMeta, "Amministrazione - Gestione Metadati" },
            { CanAdminCompanies, "Amministrazione - Gestione Sistemi Informativi" },
            { CanAdminDatasources, "Amministrazione - Gestione Database Esterni" },
            { CanAdminTables, "Amministrazione - Gestione Tabelle Interne" },
            { CanAdminMailServers, "Amministrazione - Gestione MailServer" },
            { CanAdminMessageTemplates, "Amministrazione - Gestione Template Notifiche" },

            { MailConsole, "Posta Elettronica - Accesso alla Dashboard" },
            { Profile_CanSendMail, "Posta Elettronica - Autorizzato all'invio" },

            { Profile_CanHaveSignature, "Firme - Gestione Firma Autografa" },
            { Profile_CanHaveRemoteSignature, "Firme - Accesso Firme Remote" },
            { Profile_CanHaveClient, "App - Download Client di Firma e CheckIn/CheckOut" },
        };
    }
}
