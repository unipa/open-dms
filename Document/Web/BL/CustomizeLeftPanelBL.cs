using OpenDMS.Core.BusinessLogic;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Services;
using sun.misc;
using Web.BL.Interface;
using Web.Model.Customize;

namespace Web.BL
{
    public class CustomizeLeftPanelBL : ICustomizeLeftPanelBL
    {
        private readonly IConfiguration _config;
        private readonly IACLService aclService;
        private readonly ILoggedUserProfile userContext;
        private readonly IUserService _userService;

        public CustomizeLeftPanelBL(IConfiguration config,
            IUserService userService,
            IACLService aclService,
            ILoggedUserProfile userContext)
        {
            _config = config;
            this.aclService = aclService;
            this.userContext = userContext;
            _userService = userService;
        }

        private string GetUrl(string path)
        {
            return _config["URL_HOST"] + _config["PATH_BASE"] + path;
        }

        public async Task<List<OptionPage>> GetOptionPages(string UserId)
        {
            //Group : 1 o 2  ; indica se la voce deve essere posta sopra il divisore o sotto
            var u = userContext.Get();
            var CanHaveRemoteSignature = (await aclService.GetAuthorization("$GLOBAL$", u, PermissionType.Profile_CanHaveRemoteSignature)) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted;
            var CanHaveSignature = (await aclService.GetAuthorization("$GLOBAL$", u, PermissionType.Profile_CanHaveSignature)) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted;
            var CanSendMail = (await aclService.GetAuthorization("$GLOBAL$", u, PermissionType.Profile_CanSendMail)) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted;
            var CanHaveClient = (await aclService.GetAuthorization("$GLOBAL$", u, PermissionType.Profile_CanHaveClient)) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted;


            //var ProfiloUtente = new OptionPage()
            //{
            //    Id = "liCustomize-ProfiloUtente",
            //    Name = "Profilo Utente",
            //    Class = "row",
            //    Href = "/Customize/Index", // "/Customize/Index"
            //    Icon = "fa fa-lg fa-info-circle",
            //    Color = "#fff",
            //    Description = "Gestione Dati Anagrafici",
            //    Group = 1
            //};

            var Firme = new OptionPage()
                {
                    Id = "liCustomize-FirmeUtente",
                    Name = "Firme",
                    Class = "row",
                    Href = "/Customize/Firme",
                    Icon = "fa fa-lg fa-pencil",
                    Color = "#fff",
                    Description = "Gestione Firme personali",
                    Group = 1
                };

            var Homepage= new OptionPage()
            {
                Id = "liCustomize-HomePage",
                Name = "Home Page",
                Class = "row",
                Href = "/Customize/HomePage",
                Icon = "fa fa-lg fa-home",
                Color = "#fff",
                Description = "Scelta della home page",
                Group = 1
            };
            var Avatar = new OptionPage()
            {
                Id = "liCustomize-Avatar",
                Name = "Immagine",
                Class = "row",
                Href = "/Customize/Avatar",
                Icon = "fa fa-lg fa-user",
                Color = "#fff",
                Description = "Gestione immagine profilo",
                Group = 1
            };
                var UserContactDigitalAddress = new OptionPage()
                {
                    Id = "liCustomize-UserEmail",
                    Name = "Mail/PEC",
                    Class = "row",
                    Href = "/Customize/Email",
                    Icon = "fa fa-lg fa-envelope",
                    Color = "#fff",
                    Description = "Gestione delle EMail e PEC Personali",
                    Group = 1
                };
            var Authorizations = new OptionPage()
            {
                Id = "liCustomize-Autorizzazioni",
                Name = "Autorizzazioni",
                Class = "row",
                Href = "/Customize/Authorization",
                Icon = "fa fa-lg fa-lock",
                Color = "#fff",
                Description = "Dettaglio Autorizzazioni",
                Group = 2
            };
            var Roles = new OptionPage()
            {
                Id = "liCustomize-Roles",
                Name = "Ruoli Applicativi",
                Class = "row",
                Href = "/Customize/roles",
                Icon = "fa fa-lg fa-user-md",
                Color = "#fff",
                Description = "Dettaglio dei ruoli applicativi",
                Group = 2
            };
            var Nodes = new OptionPage()
            {
                Id = "liCustomize-Strutture",
                Name = "Strutture",
                Class = "row",
                Href = "/Customize/Groups",
                Icon = "fa fa-lg fa-sitemap",
                Color = "#fff",
                Description = "Dettaglio delle strutture dell'utente",
                Group = 2
            };
            var Client = new OptionPage()
            {
                Id = "liCustomize-Client",
                Name = "Applicazioni",
                Class = "row",
                Href = "/Customize/AddOn",
                Icon = "fa fa-lg fa-puzzle-piece",
                Color = "#fff",
                Description = "Applicazioni per estensione delle funzionalità",
                Group = 2
            };

            var list = new List<OptionPage>() {
         //       ProfiloUtente,
                Avatar,
            };
            list.Add(Homepage);
            if (CanSendMail) list.Add(UserContactDigitalAddress);
            if (CanHaveSignature || CanHaveRemoteSignature) list.Add(Firme);
            list.Add(Roles);
            list.Add(Nodes);
            list.Add(Authorizations);
            if (CanHaveClient) list.Add(Client);

            return list;
        }

        public async Task<List<OptionPage>> GetOptionPagesForUser(string UserId)
        {
            //Group : 1 o 2  ; indica se la voce deve essere posta sopra il divisore o sotto
            var u = await _userService.GetUserProfile(UserId);
            var CanHaveRemoteSignature = (await aclService.GetAuthorization("$GLOBAL$", u, PermissionType.Profile_CanHaveRemoteSignature)) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted;
            var CanHaveSignature = (await aclService.GetAuthorization("$GLOBAL$", u, PermissionType.Profile_CanHaveSignature)) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted;
            var CanSendMail = (await aclService.GetAuthorization("$GLOBAL$", u, PermissionType.Profile_CanSendMail)) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted;
            var CanHaveClient = (await aclService.GetAuthorization("$GLOBAL$", u, PermissionType.Profile_CanHaveClient)) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted;

            var ProfiloUtente = new OptionPage()
            {
                Id = "liCustomize-ProfiloUtente",
                Name = "Profilo Utente",
                Class = "row",
                Href = $"/Admin/Utenti/{UserId}/Info",
                Icon = "fa fa-user-circle",
                Color = "#fff",
                Description = "Gestione Dati Anagrafici",
                Group = 1
            };

            var Firme = new OptionPage()
            {
                Id = "liCustomize-FirmeUtente",
                Name = "Firme",
                Class = "row",
                Href = $"/Admin/Utenti/{UserId}/Firme",
                Icon = "fa fa-lg fa-pencil",
                Color = "#fff",
                Description = "Gestione Firme personali",
                Group = 1
            };

            var UserContactDigitalAddress = new OptionPage()
            {
                Id = "liCustomize-UserEmail",
                Name = "Mail/PEC",
                Class = "row",
                Href = $"/Admin/Utenti/{UserId}/Email",
                Icon = "fa fa-lg fa-envelope",
                Color = "#fff",
                Description = "Gestione delle EMail e PEC Personali",
                Group = 1
            };
            var Authorizations = new OptionPage()
            {
                Id = "liCustomize-Autorizzazioni",
                Name = "Autorizzazioni",
                Class = "row",
                Href = $"/Admin/Utenti/{UserId}/Authorization",
                Icon = "fa fa-lg fa-lock",
                Color = "#fff",
                Description = "Dettaglio Autorizzazioni",
                Group = 2
            };
            var Roles = new OptionPage()
            {
                Id = "liCustomize-Roles",
                Name = "Ruoli Applicativi",
                Class = "row",
                Href = $"/Admin/Utenti/{UserId}/Roles",
                Icon = "fa fa-lg fa-user-md",
                Color = "#fff",
                Description = "Dettaglio dei ruoli applicativi",
                Group = 2
            };
            var Nodes = new OptionPage()
            {
                Id = "liCustomize-Strutture",
                Name = "Strutture",
                Class = "row",
                Href = $"/Admin/Utenti/{UserId}/Groups",
                Icon = "fa fa-lg fa-sitemap",
                Color = "#fff",
                Description = "Dettaglio delle strutture dell'utente",
                Group = 2
            };


            var list = new List<OptionPage>();

            list.Add(ProfiloUtente);
            if (CanSendMail) list.Add(UserContactDigitalAddress);
            if (CanHaveSignature || CanHaveRemoteSignature) list.Add(Firme);
            list.Add(Roles);
            list.Add(Nodes);
            list.Add(Authorizations);

            return list;
        }

    }
}
