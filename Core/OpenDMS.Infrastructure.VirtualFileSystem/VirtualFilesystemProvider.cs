

using Microsoft.Extensions.DependencyInjection;
using OpenDMS.Infrastructure.Database.DbContext;

namespace OpenDMS.Domain.Infrastructure;


public class VirtualFileSystemProvider : IVirtualFileSystemProvider
{
    private IServiceProvider serviceProvider;
    private  IDictionary<string, IVirtualFileSystem> Providers { get; set; } = new Dictionary<string, IVirtualFileSystem>();

    public VirtualFileSystemProvider(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;

        foreach (var M in serviceProvider.GetServices<IVirtualFileSystem>())
        {
            if (Providers.ContainsKey(M.Id)) Providers.Remove(M.Id);
            Providers.Add(M.Id, M);
        }
    }


    public IEnumerable<IVirtualFileSystem> GetAllTypes()
    {
        return Providers.Select(s=>s.Value).AsEnumerable();

    }

    public async Task<IVirtualFileSystem> InstanceOf(string dataType)
    {
        if (dataType is null) dataType = "";
        var found = Providers.ContainsKey(dataType);
        if (!found)
            return Providers.First().Value;
        return Providers[dataType];
    }



}
