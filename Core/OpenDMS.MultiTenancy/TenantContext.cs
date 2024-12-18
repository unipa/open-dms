using Microsoft.EntityFrameworkCore;
using OpenDMS.MultiTenancy.DbContexts;
using OpenDMS.MultiTenancy.Exceptions;
using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.MultiTenancy;

public class TenantContext : ITenantContext<Tenant>
{
    private readonly IMasterTenantDbContextFactory tenantFactory;
    private Tenant _tenant = default!;
    private TenantRegistryDbContext context;

    public TenantContext(IMasterTenantDbContextFactory tenantFactory) 
    {
        this.tenantFactory = tenantFactory;
        this.context = this.tenantFactory.GetDbContext();
    }


    public Tenant Tenant
    {
        get
        {
            if (_tenant == null)
                _tenant = context.tenantFactory.tenant;
            return _tenant;
        }
    }

    public void Find(string id)
    {
        _tenant =  context.tenants.FirstOrDefault(t => t.Id == id);
        if (_tenant == null) throw new TenantNotFoundException(nameof(id));
    }
    public void SetTenant(Tenant T)
    {
        _tenant = T;
    }
    //public void SetTenant(Tenant t)
    //{
    //    _tenant = t;
    //    _tenant = tenantFactory.GetDbContext().GetById(tenantId);
    //    if (_tenant == null) throw new TenantNotFoundException(nameof(tenantId));
    //}


}
