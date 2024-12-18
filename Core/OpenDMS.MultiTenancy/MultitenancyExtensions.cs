using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OpenDMS.MultiTenancy.DbContexts;
using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.MultiTenancy;

public static class TenantExtension
{

    public static IServiceCollection AddMultiTenancy<TStore, TTenantFactory>(this IServiceCollection services, Tenant T)
        where TStore : class, ITenantRegistryRepository<Tenant>
        where TTenantFactory : class, IApplicationDbContextFactory
    {
        services.AddHttpContextAccessor();
        //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        //services.AddDbContext<TenantRegistryDbContext>();
        //services.AddDbContext<SqlServerTenantRegistryDbContext>();
        //services.AddDbContext<MySqlTenantRegistryDbContext>();
        //services.AddDbContext<OracleTenantRegistryDbContext>();
        //services.AddDbContext<SqliteTenantRegistryDbContext>();

        services.AddTransient<IApplicationDbContextFactory, TTenantFactory>();
        services.AddTransient<IMasterTenantDbContextFactory>(s => new MasterTenantDbContextFactory(()=> { return T; }));
        services.AddTransient<ITenantRegistryRepository<Tenant>, TStore>();

        services.AddTransient<ITenantContext<Tenant>,TenantContext>();

        return services;
    }
 
    public static IServiceCollection WithResolver<TResolver>(this IServiceCollection services) where TResolver : class, ITenantResolver
    {
        services.AddTransient<ITenantResolver, TResolver>();
        return services;
    }




    public static IApplicationBuilder UseMultiTenancy (this IApplicationBuilder app)
    {
        app.UseMiddleware<MultitenantMiddleware>();
        return app;
    }



}