using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OpenDMS.CustomPages.Models;
using OpenDMS.CustomPages;
using OpenDMS.Core.Interfaces;
using Elastic.Clients.Elasticsearch;

namespace Web.Pages.Shared.Components.MainMenuPanel
{

    [Authorize]
    public class MainMenuPanelViewComponent : ViewComponent
    {
        private readonly ILoggedUserProfile userProfile;
        private readonly ICustomPageProvider customPageProvider;


        public MainMenuPanelViewComponent(
            ILoggedUserProfile userProfile, 
            ICustomPageProvider customPageProvider)
        {
            this.userProfile = userProfile;
            this.customPageProvider = customPageProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var u = userProfile.Get();
            if (u != null)
            {
                var UserName = u.UserInfo?.Contact.FullName ?? "";
                ViewData["UserName"] = UserName;
                var menu = await customPageProvider.GetPages(u, "");
                foreach (var m in menu)
                {
                    if (m.URL.ToLower().StartsWith("http"))
                    {
                        m.URL = "/ExternalApp?pageId=" + m.PageId;
                    }
                }
                return View(menu);
            }
            else
            {
                var menu = new List<CustomPageDTO>();
                ViewData["UserName"] = "";
                return View(menu);
            }

        }

    }
}