using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Services;
using Web.BL.Interface;
using Web.Model.Customize;
using Web.Pages.Shared.Components.CustomizeLeftPanel;

namespace Web.Pages.Shared.Components.CustomizeLeftPanelUtentiAdmin
{
    public class CustomizeLeftPanelUtentiAdminViewComponent : ViewComponent
    {
        private readonly ILogger<CustomizeLeftPanelViewComponent> _logger;
        private readonly ILoggedUserProfile loggedUserProfile;
        private readonly IUserService userService;
        private readonly ICustomizeLeftPanelBL _bl;

        public CustomizeLeftPanelUtentiAdminViewComponent(
            ILogger<CustomizeLeftPanelViewComponent> logger,
            ILoggedUserProfile loggedUserProfile,
            IUserService userService,
            ICustomizeLeftPanelBL bl)
        {
            _logger = logger;
            this.loggedUserProfile = loggedUserProfile;
            this.userService = userService;
            _bl = bl;
        }

        public async Task<IViewComponentResult> InvokeAsync(string UserId)
        {

            try
            {
                //INIZIALIZZO I DATI

                var VM = new CustomizeLeftPanelViewModel();
                var u = await userService.GetById(UserId);
                VM.UserId = UserId;
                VM.UserName = u.Contact.FullName;

                //ottengo tutte le tabelle
                VM.OptionPages = await _bl.GetOptionPagesForUser(UserId);

                if (VM.OptionPages.Count == 0)
                {
                    VM.ErrorMessage = "Non hai accesso a nessuna area del profilo.";
                    VM.Icon = "fa fa-alert";
                }

                return View(VM);
            }
            catch (Exception ex)
            {
                _logger.LogError("AdminController -> Index -> THROW an Exception : " + ex.Message);
                throw ex;
            }

        }
    }
}
