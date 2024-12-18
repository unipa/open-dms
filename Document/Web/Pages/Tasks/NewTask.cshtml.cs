using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Events.Types;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using Web.Model;

namespace Web.Pages
{
    [Authorize]
    public class NewTaskModel : PageModel
    {
        private readonly ILogger<NewTaskModel> _logger;
        private readonly ILoggedUserProfile userProfile;
        private readonly IACLService aCLService;
        private readonly IUISettingsRepository uISettingsRepository;
        private readonly IConfiguration _config;

        public bool CanAssignToAll { get; set; } = false;

        public NewTaskModel(ILogger<NewTaskModel> logger,
            ILoggedUserProfile userProfile,
            IACLService aCLService,
            IUISettingsRepository uISettingsRepository,
            IConfiguration config)
        {
            _logger = logger;
            this.userProfile = userProfile;
            this.aCLService = aCLService;
            this.uISettingsRepository = uISettingsRepository;
            _config = config;
        }

        public SharePanelViewModel ShareInfo { get; set; }

        public async Task OnGetAsync(int? parentTaskId)
        {
            ShareInfo = new SharePanelViewModel();

            try
            {
                ShareInfo.DocId = "";
                ShareInfo.Message = "";
                ShareInfo.To = "";
                ShareInfo.Cc = "";
                ShareInfo.Action = "*";
                ShareInfo.ParentTaskId = parentTaskId ?? 0;
                var u = userProfile.Get();
                CanAssignToAll = (await aCLService.GetAuthorization("", u, "Task.CanAssignToAll")) == AuthorizationType.Granted;
                ShareInfo.Companies = u.Companies.Where(c => !c.OffLine).ToList();// (await companyService.GetAll()).ToList();
                var valori = Enum.GetValues(typeof(ActionRequestType));
                var Stringhe = Enum.GetNames(typeof(ActionRequestType));
                ShareInfo.CompanyId = int.Parse("0" + (await uISettingsRepository.Get(User.Identity.Name, "Company.Active")));
                if (ShareInfo.CompanyId == 0) ShareInfo.CompanyId = ShareInfo.Companies[0].Id;

                ShareInfo.Actions = new List<SelectListItem>();
                //                ShareInfo.Actions.Add(new FieldTypeValue { Value = "", LookupValue = "Messaggio" });
                ShareInfo.Actions.Add(new SelectListItem { Value = "*", Text = "Generica", Selected=true });
                ShareInfo.Actions.Add(new SelectListItem { Value = EventType.Request.ToString(), Text = "Informazioni" });
                ShareInfo.Actions.Add(new SelectListItem { Value = EventType.View.ToString(), Text = "Presa Visione Allegato" });
                ShareInfo.Actions.Add(new SelectListItem { Value = EventType.Approval.ToString(), Text = "Approvazione / Autorizzazione" });
                ShareInfo.Actions.Add(new SelectListItem { Value = EventType.AddAttach.ToString(), Text = "Caricamento di uno o più allegati" });
                ShareInfo.Actions.Add(new SelectListItem { Value = EventType.AddDigitalSignature.ToString(), Text = "Firma Digitale Allegati" });
            }
            catch (Exception ex) { ShareInfo.ErrorMessage = ex.Message.ToString(); }

        }
    }
}
