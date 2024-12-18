using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Repositories;
using Web.BL.Interface;

namespace Web.BL
{
    public class TemplateNotificheBL : ITemplateNotificheBL
    {
        private readonly IConfiguration _config;
        private readonly string Host;
        private readonly ICompanyService companyRepo;
        private readonly IAppSettingsRepository appSettingRepo;

        public TemplateNotificheBL(IConfiguration config, IHttpContextAccessor accessor, ICompanyService companyRepo, IAppSettingsRepository appSettingRepo)
        {
            _config = config;
            Host = (string)_config.GetValue(typeof(string), "Endpoint:AdminService");
            this.companyRepo = companyRepo;
            this.appSettingRepo = appSettingRepo;
        }

        public async Task<IEnumerable<Company>> GetCompanies()
        {
            return await companyRepo.GetAll();
        }

        public async Task<Company> GetCompany(int Id)
        {
            var result = await companyRepo.GetById(Id);
            return result == null ? throw new Exception("La Company indicata non è stata trovata.") : result;
        }

        public async Task<string> Get(int CompanyId, string Key)
        {
            return await appSettingRepo.Get(CompanyId, Key);
        }

        public async Task Set(int CompanyId, string Key, string value)
        {
            await appSettingRepo.Set(CompanyId, Key, value);
        }

    }
}
