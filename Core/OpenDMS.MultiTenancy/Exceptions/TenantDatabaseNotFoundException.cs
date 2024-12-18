using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.MultiTenancy.Exceptions
{
    public class TenantDatabaseNotFoundException : Exception
    {
        public TenantDatabaseNotFoundException(ITenant T) : base ($"Il database indicato per il tenant {T.Id} non è stato trovato")
        {
        }
    }
}
