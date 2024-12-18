namespace OpenDMS.Domain.Infrastructure;
public interface IVirtualFileSystemProvider
{

    Task<IVirtualFileSystem> InstanceOf(string StoreIdentifier);
    IEnumerable<IVirtualFileSystem> GetAllTypes();
}
