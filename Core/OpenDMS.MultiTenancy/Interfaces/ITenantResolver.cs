namespace OpenDMS.MultiTenancy.Interfaces;

public interface ITenantResolver
{
    string Resolve();
    Task<string> ResolveAsync();
}
