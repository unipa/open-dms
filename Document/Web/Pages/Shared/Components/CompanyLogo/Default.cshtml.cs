using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Repositories;

namespace Web.Pages.Shared.Components.CompanyLogo
{

    public class CompanyLogo : ViewComponent
    {
        private readonly ICompanyService companyService;
        private readonly IUISettingsRepository uiSettingsRepository;
        private readonly ILoggedUserProfile userContext;


        public class CompanyInfo
        {
            public string Name { get; set; }
            public int Id { get; set; }
            public string URL { get; set; }
            public bool OnLine { get; set; }
            public string LastVisit { get; set; }
        }

        public string Logo { get; set; }
        public string CompanyName { get; set; }
        public List<CompanyInfo> Companies { get; set; } = new();


        public CompanyLogo(
            ICompanyService companyService,
            IUISettingsRepository uiSettingsRepository,
            ILoggedUserProfile usercontext)
        {
            this.companyService = companyService;
            this.uiSettingsRepository = uiSettingsRepository;
            this.userContext = usercontext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int companyId = 0;
            CompanyName = "";
            Logo = "";
            var u = userContext.Get();
            if (u != null)
            {
                var companies = u.Companies; 
                var id = await uiSettingsRepository.Get(u.userId, "Company.Active");
                foreach (var d in companies)
                {
                    try
                    {
                        CompanyInfo F = new();
                        F.Name = d.Description;
                        F.Id = d.Id;
                        F.OnLine = !d.OffLine;
                        F.URL = d.Logo;
                        if (String.IsNullOrEmpty(F.URL)) F.URL = "/css/logo.png";
                        F.LastVisit = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                        Companies.Add(F);

                        if (F.Id == companyId)
                        {
                            Logo = F.URL;
                            CompanyName = F.Name;
                        }

                    }
                    catch { };
                }
            }

            return View(this);
        }
    }

}
