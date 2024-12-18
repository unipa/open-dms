using OpenDMS.Domain.Entities.Settings;

namespace Web.BL.Interface
{
    public interface ITemplateNotificheBL
    {
        Task<string> Get(int CompanyId, string Key);
        Task<IEnumerable<Company>> GetCompanies();
        Task<Company> GetCompany(int Id);
        Task Set(int CompanyId, string Key, string value);
    }
}