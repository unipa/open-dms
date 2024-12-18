
namespace OpenDMS.MultiTenancy.Interfaces;

public interface ITenantContext<TTenant> where  TTenant :  class, ITenant
{

    TTenant Tenant { get; }
    void Find(string tenant);
    void SetTenant(TTenant tenant);
    //    void SetTenant(TTenant tenant);
}
