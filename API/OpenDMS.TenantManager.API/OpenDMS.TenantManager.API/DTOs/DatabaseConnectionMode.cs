namespace OpenDMS.TenantManager.API.DTOs
{
    /// <summary>
    /// Definisce la strategia utilizzata per creare il tenant
    /// 0=Crea un nuovo database per il tenant.
    /// 1=Utilizza un database esistente
    /// 2=Utilizza un database esistente o lo crea se non lo trova 
    /// </summary>
    public enum DatabaseConnectionMode
    {
        /// 0=Crea un nuovo database per il tenant.
        Create = 0,
        /// 1=Utilizza un database esistente
        Connect = 1,
        /// 2=Utilizza un database esistente o lo crea se non lo trova 
        ConnectOrCreate = 2
    }
}