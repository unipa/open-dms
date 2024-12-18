using Microsoft.AspNetCore.Http;

namespace OpenDMS.MultiTenancy.Interfaces;

public class MultitenantMiddleware// : IMiddleware
{
   
    private readonly RequestDelegate _next;
    private readonly ITenantContext<Tenant> tenantContext;
    private readonly ITenantResolver tenantResolver;

    public MultitenantMiddleware(RequestDelegate next, ITenantContext<Tenant> tenantContext, ITenantResolver tenantResolver)
    {
        _next = next;
        this.tenantContext = tenantContext;
        this.tenantResolver = tenantResolver;
    }
    //public MultitenantMiddleware(RequestDelegate next)
    //{
    //    _next = next;
    //}


    //public async Task InvokeAsync(HttpContext context, ITenantContext<Tenant> tenantContext, ITenantResolver tenantResolver)
    //{
    //    var id = await tenantResolver.ResolveAsync();
    //    if (!String.IsNullOrEmpty(id))
    //    {
    //        tenantContext.Find(id);

    //    }
    //    await _next(context);
    //}

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var id = await tenantResolver.ResolveAsync();
        if (!String.IsNullOrEmpty(id))
        {
            tenantContext.Find(id);

        }
        await _next(context);
    }
}
