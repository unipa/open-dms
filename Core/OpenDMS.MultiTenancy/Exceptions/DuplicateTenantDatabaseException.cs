using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.MultiTenancy.Exceptions
{
    public class DuplicateTenantDatabaseException : Exception
    {
        public DuplicateTenantDatabaseException(ITenant T) : base ($"Il database indicato per il tenant {T.Id} esiste già")
        {
        }
    }
}
