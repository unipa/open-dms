using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.Filters;
using OpenDMS.MultiTenancy.Interfaces;
using OpenDMS.TenantManager.API.DTOs;

namespace IdentiyMultiTenant.Pages
{
    [Authorization(":admin")]
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ITenantRegistryRepository<Tenant> tenantRepo;

        [BindProperty]
        public IList<Tenant> tenants { get; set; }

        [BindProperty]
        public bool OnlyOnline { get; set; } = true;

        public IndexModel(ILogger<IndexModel> logger, ITenantRegistryRepository<Tenant> tenantRepo)
        {
            _logger = logger;
            this.tenantRepo = tenantRepo;
        }

        public async Task OnGetAsync()
        {
            tenants = tenantRepo.GetAll(false); // OnlyOnline);
        }
        public async Task OnPostOffLineAsync(bool onlyOnline)
        {
            OnlyOnline = onlyOnline;
            await OnGetAsync();
        }

        public PartialViewResult OnGetNewTenant()
        {
            return Partial("Components/NewTenant", new TenantCreationDTO());
        }

    }
}