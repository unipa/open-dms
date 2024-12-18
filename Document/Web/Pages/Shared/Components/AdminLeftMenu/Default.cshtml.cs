using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.Filters;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Models;
using Web.BL.Interface;
using Web.Model.Admin;

namespace Web.Pages.Shared.Components.AdminLeftMenu
{
    [Authorization(":admin")]
    public class AdminLeftMenuViewComponent : ViewComponent
    {
        private readonly ILoggedUserProfile userProfile;
        private readonly ILogger<AdminLeftMenuViewComponent> _logger;
        private readonly IAdminLeftMenuBL _bl;

        public AdminLeftMenuViewComponent(ILoggedUserProfile userProfile,  ILogger<AdminLeftMenuViewComponent> logger, IAdminLeftMenuBL bl)
        {
            this.userProfile = userProfile;
            _logger = logger;
            _bl = bl;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            try
            {
                //INIZIALIZZO I DATI
                var u = userProfile.Get();
                var VM = new AdminLeftMenuViewModel();

                VM.UserId = u.userId;

                //ottengo tutte le tabelle
                VM.Aree = await _bl.GetAreas(u);

                if (VM.Aree.Count == 0)
                {
                    VM.ErrorMessage = "Non hai accesso a nessuna area amministrativa";
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