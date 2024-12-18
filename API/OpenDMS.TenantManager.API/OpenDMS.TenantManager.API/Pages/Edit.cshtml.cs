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
    public class EditModel : PageModel
    {
        private readonly ILogger<EditModel> _logger;
        private readonly ITenantRegistryRepository<Tenant> tenantRepo;

        [BindProperty]
        public TenantUpdateModel tenant { get; set; }

        [BindProperty]
        public string Error { get; set; } = "";


        public EditModel(ILogger<EditModel> logger, ITenantRegistryRepository<Tenant> tenantRepo)
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
            tenant.Realm = T.OpenIdConnectAuthority;
            tenant.RootFolder = T.RootFolder;
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
                if (string.IsNullOrWhiteSpace(tenant.RootFolder))
                    tenant.Description = tenantRepo.GetDefault().RootFolder + "/" + tenant.Id;
                Tenant.Description = tenant.Description;
                Tenant.Offline = !tenant.Offline;
                //Tenant.ClientId = tenant.ClientId;
                //Tenant.ClientSecret = tenant.ClientSecret;
                //Tenant.Realm = tenant.Realm;
                Tenant.URL = tenant.URL;
                Tenant.RootFolder = tenant.RootFolder;
                await tenantRepo.Update(Tenant);
                ViewData["Refresh"] = "true";
                return Page();
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return Page();
            }
        }

    }
}