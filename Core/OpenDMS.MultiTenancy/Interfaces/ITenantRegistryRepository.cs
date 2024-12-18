namespace OpenDMS.MultiTenancy.Interfaces;

public interface ITenantRegistryRepository<TTenant> where TTenant : ITenant
{
    Task<int> Create(TTenant tenant);
    Task<int> ConnectOrCreate(TTenant tenant);
    Task<int> TryConnect(TTenant tenant);
    Task<int> Delete(string id);
    Task<int> Delete(TTenant tenant);
    TTenant? GetById(string id);
    TTenant GetDefault();
    IList<TTenant?> GetAll(bool Online = false);
    Task<TTenant?> GetByIdAsync(string di);
    Task<int> Update(TTenant tenant);
    Task<bool> TestConnection(TTenant T);
    Task<bool> CreateOrUpdateDatabase(TTenant T);

}