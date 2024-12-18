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
    public class EditComponentModel : PageModel
    {
        private readonly ILogger<EditComponentModel> _logger;
        private readonly ITenantRegistryRepository<Tenant> tenantRepo;

        [BindProperty]
        public TenantUpdateModel tenant { get; set; }

        [BindProperty]
        public string Error { get; set; } = "";


        public EditComponentModel(ILogger<EditComponentModel> logger, ITenantRegistryRepository<Tenant> tenantRepo)
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
            tenant.ClientSecret = T.OpenIdConnectClientSecret;
            tenant.Realm = T.OpenIdConnectAuthority;
            tenant.ClientId = T.OpenIdConnectClientId; 
            tenant.URL= T.URL;

        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(tenant.Id))
                {
                    return BadRequest(nameof(tenant.Id) + " non può essere vuoto");
                }
                var Tenant = tenantRepo.GetById(tenant.Id);
                if (Tenant == null) throw new TenantNotFoundException(tenant.Id);
                Tenant.Description = tenant.Description;
                Tenant.Offline = !tenant.Offline;
                Tenant.OpenIdConnectClientId = tenant.ClientId;
                Tenant.OpenIdConnectClientSecret = tenant.ClientSecret;
                Tenant.OpenIdConnectAuthority = tenant.Realm;
                Tenant.URL = tenant.URL;
                await tenantRepo.Update(Tenant);
                return RedirectToPage("/");
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return Page();
            }
        }

    }
}