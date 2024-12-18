using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Models;
using Web.BL.Interface;
using Web.Model.Admin;

namespace Web.BL
{
    public class AdminLeftMenuBL : IAdminLeftMenuBL
    {
        private readonly IACLService aCLService;
        private readonly IConfiguration _config;
        private readonly string Host;
        private readonly string UserPermissionEndpoint;

        public AdminLeftMenuBL(IACLService aCLService, IConfiguration config)
        {
            this.aCLService = aCLService;
            _config = config;
            UserPermissionEndpoint = (string)_config.GetValue(typeof(string), "Endpoint:UserService");
            Host = (string)_config.GetValue(typeof(string), "Endpoint:AdminService");
        }

        //public string GetUrl(string path)
        //{
        //    return _config["URL_HOST"] + _config["PATH_BASE"] + path;
        //}

        public async Task<List<Area>> GetAreas(UserProfile UserId)
        {

            //Sistema informativo

            var BancheDati = new Area()
            {
                Id = "liadmin-bd",
                Name = "Sistema Informativo",
                Class = "mnuAdminBancheDati",
                Group = "Sistema informativo",
                Href = "/Admin/Company/Index",
                Icon = "fa fa-building",
                Color = "#fff",
                Description = "Gestisce i dati principali del sistema Informativo"
            };

            var UtenteDiSistema = new Area()
            {
                Id = "liadmin-SystemUser",
                Name = "Utente di Sistema",
                Class = "mnuAdminUserProfile",
                Group = "Impostazioni Globali",
                Href = "/Admin/SystemUser/Index",
                Icon = "fa fa-user",
                Color = "#fff",
                Description = "Configura le caratteristiche dell'utente interno di sistema"
            };
            var ProcessiSuDocumenti = new Area()
            {
                Id = "liadmin-bd",
                Name = "Processi documentali",
                Class = "mnuAdminProcessiSuDocumenti",
                Group = "Sistema informativo",
                Href = "/Admin/DocProcesses/Index",
                Icon = "fa fa-cogs",
                Color = "#fff",
                Description = "Configura l'avvio di processi in risposta a eventi documentali"
            };

            var MailServer = new Area()
            {
                Id = "liadmin-mailserver",
                Name = "Server di Posta",
                Class = "mnuAdminMailServer",
                Group = "Impostazioni Globali",
                Href = "/Admin/MailServer/Index",
                Icon = "fa fa-inbox",
                Color = "",
                Description = "Configurzione dei server di Posta Elettronica/PEC"
            };

            var Organigramma = new Area()
            {
                Id = "liadmin-organigramma",
                Name = "Organigramma",
                Class = "mnuAdminOrganigramma",
                Group = "Impostazioni Globali",
                Href = "/Admin/Organigramma/Index",
                Icon = "fa fa-sitemap",// "it-inbox",
                Color = "",
                Description = "Organigramma",
            };

            var TemplateNotifiche = new Area()
            {
                Id = "liadmin-templatenotifiche",
                Name = "Tasks",
                Class = "mnuAdminTemplateNotifiche",
                Group = "Impostazioni Globali",
                Href = "/Admin/TemplateNotifiche/Index",
                Icon = "fa fa-bell",
                Color = "icon-white",
                Description = "Impostazioni Generali e dei Template di Notifica"
            };

            var DatabaseEsterni = new Area()
            {
                Id = "liadmin-database",
                Name = "Database Esterni",
                Class = "mnuAdminDatabaseEsterni",
                Group = "Sistema informativo",
                Href = "/Admin/DatabaseEsterni/Index",
                Icon = "fa fa-database",
                Color = "",
                Description = "Configura la connessione a fonti dati esterne"
            };

            var Metadati = new Area()
            {
                Id = "liadmin-metadata",
                Name = "Metadati",
                Class = "mnuAdminMetadati",
                Group = "Sistema informativo",
                Href = "/Admin/Metadati/Index",
                Icon = "fa fa-tags",
                Color = "",
                Description = "Configura i metadati delle tipologie"
            };

            var Ruoli= new Area()
            {
                Id = "liadmin-ruoli",
                Name = "Ruoli",
                Class = "mnuAdminRuoli",
                Group = "Sistema informativo",
                Href = "/Admin/Roles/Index",
                Icon = "fa fa-user-md",
                Color = "",
                Description = "Configura i ruoli applicativi"
            };

            var Permessi = new Area()
            {
                Id = "liadmin-acl",
                Name = "ACL",
                Class = "mnuAdminPermessi",
                Group = "Sistema informativo",
                Href = "/Admin/Permessi/Index",
                Icon = "fa fa-unlock-alt",
                Color = "",
                Description = "Configura i permessi ACL"
            };

            var PermessiGlobali = new Area()
            {
                Id = "liadmin-aclGlobali",
                Name = "Permessi",
                Class = "mnuAdminPermessiGlobali",
                Group = "Sistema informativo",
                Href = "/Admin/PermessiGlobali/Index",
                Icon = "fa fa-lock",
                Color = "",
                Description = "Configura i permessi globali"
            };

            var TabelleInterne = new Area()
            {
                Id = "liadmin-table",
                Name = "Tabelle Interne",
                Class = "mnuAdminTabelleInterne",
                Group = "Sistema informativo",
                Href = "/Admin/TabelleInterne/Index",
                Icon = "fa fa-table",
                Color = "",
                Description = "Configura le Tabelle Interne"
            };

            var Tipologie = new Area()
            {
                Id = "liadmin-Tipologie",
                Name = "Tipologie",
                Class = "mnuAdminTipologie",
                Group = "Sistema informativo",
                Href = "/Admin/Tipologie/Index",
                Icon = "fa fa-folder-o",
                Color = "",
                Description = "Configura le tipologie documentali"
            };

            var Utenti = new Area()
            {
                Id = "liadmin-Utenti",
                Name = "Utenti",
                Class = "mnuAdminUtenti",
                Group = "Sistema informativo",
                Href = "/Admin/Utenti/Index?pageSize=5&pageNumber=1",
                Icon = "fa fa-users",
                Color = "#fff",
                Description = "Gestisce le utenze registrate"
            };

            var sql = new Area()
            {
                Id = "liadmin-sql",
                Name = "Console SQL",
                Class = "mnuAdminSql",
                Group = "Sistema informativo",
                Href = "/Admin/sql/Index",
                Icon = "fa fa-database",
                Color = "#fff",
                Description = "Permette di interagire con il database"
            };

            var list = new List<Area>() { };
            if (await aCLService.GetAuthorization("$GLOBAL$", UserId, PermissionType.CanAdminCompanies) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted)
                list.Add(BancheDati);
            if (await aCLService.GetAuthorization("$GLOBAL$", UserId, PermissionType.CanAdminMailServers) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted)
                list.Add(MailServer);
            if (await aCLService.GetAuthorization("$GLOBAL$", UserId, PermissionType.CanAdminDatasources) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted)
                list.Add(DatabaseEsterni);
            if (await aCLService.GetAuthorization("$GLOBAL$", UserId, PermissionType.CanAdminMeta) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted)
                list.Add(Metadati);
            if (await aCLService.GetAuthorization("$GLOBAL$", UserId, PermissionType.CanAdminACL) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted)
                list.Add(Permessi);
            if (await aCLService.GetAuthorization("$GLOBAL$", UserId, PermissionType.CanAdminACL) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted)
                list.Add(PermessiGlobali);
            if (await aCLService.GetAuthorization("$GLOBAL$", UserId, PermissionType.CanAdminTables) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted)
                list.Add(TabelleInterne);
            if (await aCLService.GetAuthorization("$GLOBAL$", UserId, PermissionType.CanAdminGroups) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted)
                list.Add(Organigramma);
            if (await aCLService.GetAuthorization("$GLOBAL$", UserId, PermissionType.CanAdminMessageTemplates) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted)
                list.Add(TemplateNotifiche);
            if (await aCLService.GetAuthorization("$GLOBAL$", UserId, PermissionType.CanAdminDocumentTypes) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted)
                list.Add(Tipologie);
            if (await aCLService.GetAuthorization("$GLOBAL$", UserId, PermissionType.CanAdminDocumentTypes) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted)
                list.Add(ProcessiSuDocumenti);
            if (await aCLService.GetAuthorization("$GLOBAL$", UserId, PermissionType.CanAdminRoles) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted)
                list.Add(Ruoli);
            if (UserId.Roles.Any(r=>r.Id.ToLower().Equals("admin")))
            {
                list.Add(UtenteDiSistema);
                list.Add(sql);
                list.Add(Utenti);
            }
            return list;
        }

}
}
