using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.MultiTenancy.Exceptions
{
    internal class TenantDatabaseCreationException : Exception
    {
        public TenantDatabaseCreationException(ITenant T) : base ($"impossibile creare il database per il tenant {T.Id}")
        {
            
        }
    }
}
