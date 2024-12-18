using Microsoft.Extensions.DependencyInjection;
using OpenDMS.Core.Interfaces;

namespace OpenDMS.Core.Managers;

public class ViewServiceResolver : IViewServiceResolver
{
    private readonly List<ISearchService> services;
    private readonly IServiceProvider serviceProvider;

    public ViewServiceResolver(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
        //        this.services = services;
    }


    public async Task<ISearchService> GetSearchService(string viewId)
    {
        //var type = typeof(ISearchService);
        //var types = AppDomain.CurrentDomain.GetAssemblies()
        //    .SelectMany(s => s.GetTypes())
        //    .Where(p => type.IsAssignableFrom(p));
        using (var scope = serviceProvider.CreateScope())
        {
            var types = scope.ServiceProvider.GetServices<ISearchService>();
            foreach (var s in types)
            {
                //var s = (ISearchService)(scope.ServiceProvider.GetRequiredService<ISearchService>(t));
                if (s != null)
                    if (s.ResolveView(viewId))
                        return s;
            }
        }
        throw new KeyNotFoundException();
    }

}