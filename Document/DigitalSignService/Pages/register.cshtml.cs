using DigitalSignService.Interfaces;
using DigitalSignService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DesktopService.Pages
{
    public class registerModel : PageModel
    {
        public registerModel(IAppSettingService appSettingService)
        {
            AppSettingService = appSettingService;
        }

        public IAppSettingService AppSettingService { get; }

        public async Task<IActionResult> OnGet(string host, string clientSecret, string returnUrl)
        {
            await AppSettingService.SetSecret(host, clientSecret);
            return Redirect(returnUrl);
        }
    }
}
