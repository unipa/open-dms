using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.MultiTenancy.Exceptions
{
    public class TenantNotFoundException : Exception
    {
        public TenantNotFoundException(string T) : base ($"Il tenant {T} non è stato trovato")
        {
        }
    }
}
