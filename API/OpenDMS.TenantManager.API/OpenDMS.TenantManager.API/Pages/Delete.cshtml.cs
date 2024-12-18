using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.MultiTenancy;
using OpenDMS.MultiTenancy.Exceptions;
using OpenDMS.MultiTenancy.Interfaces;
using OpenDMS.TenantManager.API.Controllers;
using OpenDMS.TenantManager.API.DTOs;
using System.Net;

namespace IdentiyMultiTenant.Pages
{
    //[Authorize("admin")]
    public class DeleteModel : PageModel
    {
        private readonly ILogger<DeleteModel> _logger;
        private readonly ITenantRegistryRepository<Tenant> tenantRepo;

        [BindProperty]
        public TenantUpdateModel tenant { get; set; }

        [BindProperty]
        public string Error { get; set; } = "";


        public DeleteModel(ILogger<DeleteModel> logger, ITenantRegistryRepository<Tenant> tenantRepo)
        {
            _logger = logger;
            this.tenantRepo = tenantRepo;
        }

        public async Task OnGetAsync(string id)
        {
            var T = tenantRepo.GetById(id);
            tenant = new TenantUpdateModel();
            tenant.Id = id;
            tenant.Description = T.Description;
            tenant.Offline = T.Offline;
            tenant.ClientSecret = "";
            tenant.Realm = "";
            tenant.ClientId = "";

        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var Tenant = tenantRepo.GetById(tenant.Id);
                if (Tenant == null) throw new TenantNotFoundException(tenant.Id);
                Tenant.Description = tenant.Description;
                Tenant.Offline = !tenant.Offline;
                await tenantRepo.Delete (Tenant);
                return RedirectToPage("/NewTenant");
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return Page();
            }
        }

    }
}