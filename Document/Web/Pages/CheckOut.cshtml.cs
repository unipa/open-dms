
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;

namespace Web.Pages
{
    public class CheckOutModel : PageModel
    {
        private readonly ILoggedUserProfile userContext;
        private readonly IUserService userService;
        private readonly IDocumentService documentService;
        private readonly IAppSettingsRepository appSettings;
        private readonly IUserSettingsRepository userSettings;

        public int DocumentId { get; set; }
        public int UserPort { get; set; }

        public CheckOutModel(ILoggedUserProfile userContext,
            IUserService userService,
            IDocumentService documentService,
            IAppSettingsRepository appSettings,
            IUserSettingsRepository userSettings)
        {
            this.userContext = userContext;
            this.userService = userService;
            this.documentService = documentService;
            this.appSettings = appSettings;
            this.userSettings = userSettings;
        }

        public async Task OnGet(int documentId)
        {
            var u = userContext.Get();
            var user = await userService.GetById(u.userId);
            int.TryParse( await userSettings.Get(user.ContactId, OpenDMS.Domain.Constants.UserAttributes.CONST_DIGITALSIGNATURE_PORT), out int port);
            
            UserPort = port > 0 ? port : 5000;
            DocumentId = documentId;
        }
    }
}
