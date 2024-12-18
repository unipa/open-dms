using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.Interfaces;
using OpenDMS.CustomPages;
using OpenDMS.CustomPages.Models;
using OpenDMS.Domain.Entities.Settings;
using System.Runtime.CompilerServices;

namespace Web.Pages.Home
{
    public class IndexModel : PageModel
    {
        private readonly ICustomPageProvider customPageProvider;
        private readonly ILoggedUserProfile loggedUserProfile;

        public List<CustomPageDTO> MenuItems { get; set; }


        public IndexModel(
            ICustomPageProvider customPageProvider,
            ILoggedUserProfile loggedUserProfile
            )
        {
            this.customPageProvider = customPageProvider;
            this.loggedUserProfile = loggedUserProfile;
        }

        public async Task OnGetAsync()
        {
            MenuItems =  (await customPageProvider.GetPages(loggedUserProfile.Get())).Where(m=>m.Alignment >= 0).ToList();
        }
    }
}
