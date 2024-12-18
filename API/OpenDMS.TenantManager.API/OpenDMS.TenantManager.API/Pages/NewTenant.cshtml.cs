using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.Filters;
using OpenDMS.MultiTenancy;
using OpenDMS.MultiTenancy.Interfaces;
using OpenDMS.TenantManager.API.Controllers;
using OpenDMS.TenantManager.API.DTOs;
using System.Data.Common;
using System.Net;
using System.Runtime.CompilerServices;

namespace IdentiyMultiTenant.Pages
{
    [Authorization(":admin")]
    public class NewTenantModel : PageModel
    {
        private readonly ILogger<NewTenantModel> _logger;
        private readonly IConfiguration config;
        private readonly ITenantRegistryRepository<Tenant> tenantRepo;

        [BindProperty]
        public TenantCreationDTO tenant { get; set; }

        [BindProperty]
        public string Error { get; set; } = "";

        public NewTenantModel(ILogger<NewTenantModel> logger, IConfiguration config, ITenantRegistryRepository<Tenant> tenantRepo)
        {
            _logger = logger;
            this.config = config;
            this.tenantRepo = tenantRepo;
        }

        public async Task OnGetAsync()
        {
            tenant = new TenantCreationDTO();
            tenant.Provider = config["provider"];



            //var api = $"{config["Keycloak:auth-server-url"]}admin/realms/{config["Keycloak:realm"]}/clients";
            //var h = new HttpClient();
            //h.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Authorization", (await HttpContext.GetTokenAsync("JWToken")));
            //var r = await h.GetAsync(api);
            //var clients = await r.Content.ReadFromJsonAsync<List<string>>();
            Error = "";
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(tenant.Id))
                {
                    throw new Exception(nameof(tenant.Id) + " non può essere vuoto");
                }
                if (string.IsNullOrWhiteSpace(tenant.Description))
                    tenant.Description = tenant.Id;
                if (string.IsNullOrWhiteSpace(tenant.RootFolder))
                    tenant.Description = tenantRepo.GetDefault().RootFolder + "/" + tenant.Id;
                Tenant T = new Tenant(tenant.Description + "", tenant.Provider, tenant.ConnectionString, tenant.Offline);
                T.Id = tenant.Id;
                T.OpenIdConnectClientId = tenant.ClientId + "";
                T.OpenIdConnectClientSecret = tenant.ClientSecret + "";
                T.OpenIdConnectAuthority = tenant.Realm + "";
                T.URL = tenant.URL + "";
                T.RootFolder = "/"; // tenant.RootFolder;
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

                tenant.DatabaseConnectionStrategy = DatabaseConnectionMode.ConnectOrCreate;

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
                ViewData["Refresh"] = "true";
                return Page();
            }
            catch (Exception ex){
                Error = ex.StackTrace;
                return Page();
            }
        }

    }
}