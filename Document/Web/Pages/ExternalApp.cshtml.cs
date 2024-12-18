using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.Interfaces;
using OpenDMS.CustomPages;

namespace Web.Pages
{
    public class ExternalAppModel : PageModel
    {
        private readonly ILoggedUserProfile userProfile;
        private readonly ICustomPageProvider customPageProvider;

        public string Title { get; set; }
        public string Icon{ get; set; }
        public string Tooltip { get; set; }
        public string URL { get; set; }


        public ExternalAppModel(ILoggedUserProfile userProfile, ICustomPageProvider customPageProvider)
        {
            this.userProfile = userProfile;
            this.customPageProvider = customPageProvider;
        }

        public async Task OnGet(string pageid)
        {
            var u = userProfile.Get();
            var p = await customPageProvider.GetPage(u, pageid);
            Title = p.Title;
            URL = p.URL;
            Tooltip = p.Tooltip;
            Icon = p.Icon;
        }
    }
}
