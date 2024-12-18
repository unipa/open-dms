namespace OpenDMS.TenantManager.API.DTOs
{
    /// <summary>
    /// modello da utilizzare per creare un nuovo tenant
    /// </summary>
    public class TenantCreationDTO
    {
        /// Identificativo univoco del tenant 
        public string Id { get; set; } = "";
        /// Descrizione del tenant
        public string Description { get; set; } = "";
        /// <summary>
        /// Connectionstring per il collegamento al database del tenant. 
        /// Per il provider Sqlite, corrisponde al nome del file database locale.
        /// </summary>

        public string ConnectionString { get; set; } = "";
        public string Provider { get; set; } = "";
        /// Modalità di connessione al database. Per maggiori informazioni vedere <seealso cref="DatabaseConnectionMode"/>
        public DatabaseConnectionMode DatabaseConnectionStrategy { get; set; } = DatabaseConnectionMode.Create;
        /// flag che permette di mettere offline il tenant.
        public bool Offline { get; set; } = false;
        public string RootFolder { get; set; } = "";
        /// <summary>
        /// URL del tenant
        /// </summary>
        public string URL { get; set; } = "";

        /// <summary>
        /// Nome del realm da utilizzare per l'autenticazione del tenant
        /// </summary>
        public string Realm { get; set; } = "";

        /// <summary>
        /// Identificativo del client di Keycloak da utilizzare per l'autenticazione del tenant
        /// </summary>
        public string ClientId { get; set; } = "";

        /// <summary>
        /// ClientSecret per l'accesso al realm del tenant
        /// </summary>
        public string ClientSecret { get; set; } = "";

    }
}
