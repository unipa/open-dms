using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Infrastructure;
using OpenDMS.Domain.Infrastructure.Encrypters;
using OpenDMS.Domain.Infrastructure.Providers;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy;
using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.Startup;

public static class TenantExtensions
{
    public static IServiceCollection AddMultiTenancy(this IServiceCollection services, IConfiguration configuration)
    {
        Tenant tenant = new Tenant(configuration[StaticConfiguration.CONST_TENANT_PROVIDER], configuration[StaticConfiguration.CONST_TENANT_DATABASE]);
        services.AddHttpContextAccessor();

        //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<IApplicationDbContextFactory, ApplicationDbContextFactory>();
        services.AddTransient<IMasterTenantDbContextFactory>(s => new MasterTenantDbContextFactory(() => { return tenant; }));
        services.AddTransient<ITenantRegistryRepository<Tenant>, TenantRegistryRepository>();
        services.AddTransient<ITenantContext<Tenant>, TenantContext>();

   //     services.AddTransient<IVirtualFileSystem, OSFileSystem>();
   //     services.AddTransient<IVirtualFileSystem, EncryptedOSFileSystem>();
   //     services.AddTransient<IFileEncryptor, FastFileEncryptor>(); 
   //     services.AddTransient<IVirtualFileSystemProvider, VirtualFileSystemProvider>();



        var resolver = configuration[StaticConfiguration.CONST_TENANT_RESOLVER]?.ToLower();
        switch (resolver)
        {
            case "domain":
            case "host":
                services.AddTransient<ITenantResolver, DomainTenantResolver>();
                break;
            case "claim":
                services.AddTransient<ITenantResolver, ClaimResolver>();
                break;
            case "path":
                services.AddTransient<ITenantResolver, PathResolver>();
                break;
            case "header":
                services.AddTransient<ITenantResolver, HeaderResolver>();
                break;
            case "querystring":
                services.AddTransient<ITenantResolver, QueryStringResolver>();
                break;
            default:
                services.AddTransient<ITenantResolver, NullTenantResolver>();
                break;
        }
        return services;
    }


    public static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder app)
    {
        //using var scope = app.ApplicationServices.CreateScope();
        //using var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContextFactory>().GetDbContext();
        //try
        //{
        //    dbContext.Database.Migrate();
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine(ex.ToString());
        //    var pending = dbContext.Database.GetPendingMigrations().Count();
        //    if (pending > 0) 
        //        throw ex;
        //};
        
        //app.UseMiddleware<MultitenantMiddleware>();
        return app;
    }



}

