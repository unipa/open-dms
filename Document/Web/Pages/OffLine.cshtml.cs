using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Domain.Repositories;
using System.Diagnostics;

namespace Web.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class OfflineModel : PageModel
    {
        public string? Timeout { get; set; }

        private readonly ILogger<OfflineModel> _logger;
        private readonly IAppSettingsRepository appSettingsRepository;

        public OfflineModel(ILogger<OfflineModel> logger, IAppSettingsRepository appSettingsRepository)
        {
            _logger = logger;
            this.appSettingsRepository = appSettingsRepository;
        }

        public async Task OnGetAsync()
        {
            Timeout = await appSettingsRepository.Get("Offline");
        }
    }
}