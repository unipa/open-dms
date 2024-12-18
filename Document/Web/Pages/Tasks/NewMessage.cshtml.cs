using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Events.Types;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using Web.Model;

namespace Web.Pages
{
    [Authorize]
    public class NewMessageModel : PageModel
    {
        private readonly ILogger<NewMessageModel> _logger;
        private readonly ILoggedUserProfile userProfile;
        private readonly IUISettingsRepository uISettingsRepository;
        private readonly ICompanyService companyService;
        private readonly IACLService aCLService;
        private readonly IConfiguration _config;

        public bool CanAssignToAll { get; set; } = false;


        public NewMessageModel(ILogger<NewMessageModel> logger,
            ILoggedUserProfile userProfile,
            ICompanyService companyService,
            IUISettingsRepository uISettingsRepository,
            IACLService aCLService,
            IConfiguration config)
        {
            _logger = logger;
            this.userProfile = userProfile;
            this.uISettingsRepository = uISettingsRepository;
            this.companyService = companyService;
            this.aCLService = aCLService;
            _config = config;
        }

        public SharePanelViewModel ShareInfo { get; set; }

        public async Task OnGetAsync()
        {
            ShareInfo = new SharePanelViewModel();

            try
            {
                ShareInfo.DocId = "";
                ShareInfo.Message = "";
                ShareInfo.To = "";
                ShareInfo.Cc = "";
                var u = userProfile.Get();
                CanAssignToAll = (await aCLService.GetAuthorization("", u, "Task.CanAssignToAll")) == AuthorizationType.Granted;
                ShareInfo.Companies = (await companyService.GetAll()).ToList();
                ShareInfo.CompanyId = int.Parse("0" + (await uISettingsRepository.Get(User.Identity.Name, "Company.Active")));
                if (ShareInfo.CompanyId == 0) ShareInfo.CompanyId = ShareInfo.Companies[0].Id;

                var valori = Enum.GetValues(typeof(ActionRequestType));
                var Stringhe = Enum.GetNames(typeof(ActionRequestType));
            }
            catch (Exception ex) { ShareInfo.ErrorMessage = ex.Message.ToString(); }

        }
    }
}
