using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.MultiTenancy;
using OpenDMS.MultiTenancy.Interfaces;
using OpenDMS.TenantManager.API.Controllers;
using OpenDMS.TenantManager.API.DTOs;
using System.Data.Common;
using System.Net;

namespace IdentiyMultiTenant.Pages
{
    //[Authorize("admin")]
    public class NewTenantComponentModel : PageModel
    {
        private readonly ILogger<NewTenantComponentModel> _logger;
        private readonly IConfiguration config;
        private readonly ITenantRegistryRepository<Tenant> tenantRepo;

        [BindProperty]
        public TenantCreationDTO tenant { get; set; }

        [BindProperty]
        public string Error { get; set; } = "";

        public NewTenantComponentModel(ILogger<NewTenantComponentModel> logger, IConfiguration config, ITenantRegistryRepository<Tenant> tenantRepo)
        {
            _logger = logger;
            this.config = config;
            this.tenantRepo = tenantRepo;
        }

        public async Task OnGetAsync()
        {
            tenant = new TenantCreationDTO();
            tenant.Provider = config["provider"];
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(tenant.Id))
                {
                    throw new Exception(nameof(tenant.Id) + " non può essere vuoto");
                }
                Tenant T = new Tenant(tenant.Description, tenant.Provider, tenant.ConnectionString, tenant.Offline);
                T.Id = tenant.Id;
                if (string.IsNullOrEmpty(T.Provider))
                {
                    T.Provider = tenantRepo.GetDefault().Provider;
                }
                else
                {
                    if (!T.Provider.Equals(tenantRepo.GetDefault().Provider, StringComparison.InvariantCultureIgnoreCase) && String.IsNullOrEmpty(T.ConnectionString))
                        throw new ArgumentNullException(nameof(T.ConnectionString));
                }
                if (string.IsNullOrEmpty(T.ConnectionString))
                {
                    if (T.Provider.ToLower() != "memory")
                    {
                        DbConnectionStringBuilder builder = new DbConnectionStringBuilder();
                        builder.ConnectionString = tenantRepo.GetDefault().ConnectionString;

                        builder["database"] = config["TenantPrefix"] + T.Id;
                        T.ConnectionString = builder.ConnectionString;
                    }
                    else
                    {
                        T.ConnectionString = T.Id;
                    }
                }
                else
                {
                    if (T.ConnectionString.IndexOf(";") < 0)
                    {
                        if (T.Provider.ToLower() != "memory")
                        {
                            DbConnectionStringBuilder builder = new DbConnectionStringBuilder();
                            builder.ConnectionString = tenantRepo.GetDefault().ConnectionString;
                            builder["database"] = T.ConnectionString;
                            T.ConnectionString = builder.ConnectionString;
                        }
                        else
                        {
                            T.ConnectionString = T.Id;
                        }
                    }


                }
                T.OpenIdConnectClientId = tenant.ClientId;
                T.OpenIdConnectClientSecret = tenant.ClientSecret;
                T.OpenIdConnectAuthority = tenant.Realm;
                T.URL = tenant.URL;

                if (tenant.DatabaseConnectionStrategy == DatabaseConnectionMode.Connect)
                {
                    await tenantRepo.TryConnect(T);
                }
                else
                if (tenant.DatabaseConnectionStrategy == DatabaseConnectionMode.ConnectOrCreate)
                {
                    await tenantRepo.ConnectOrCreate(T);
                }
                else
                {
                    await tenantRepo.Create(T);
                }
                return RedirectToPage("/");
            }
            catch (Exception ex){
                Error = ex.Message;
                return Page();
            }
        }

    }
}