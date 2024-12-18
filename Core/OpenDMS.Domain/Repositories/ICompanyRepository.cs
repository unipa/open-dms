using OpenDMS.Domain.Entities.Settings;

namespace OpenDMS.Domain.Repositories;


/// <summary>
/// Descrizione di riepilogo per BancheDatiDAO
/// </summary>
public interface ICompanyRepository
{
    Task<Company> GetById(int cbd);
    Task<int> Insert(Company bd);
    Task<int> Update(Company bd);
    Task<int> Delete(int CompanyId);
    Task<IList<Company>> GetAll();

}


