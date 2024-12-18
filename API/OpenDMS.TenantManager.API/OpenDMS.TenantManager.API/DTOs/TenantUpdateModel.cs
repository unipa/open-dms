namespace OpenDMS.TenantManager.API.DTOs
{
    /// <summary>
    /// modello da utilizzare per aggiornare un nuovo tenant
    /// </summary>
    public class TenantUpdateModel
    {
        /// Nome identificativo del tenant
        public string Id { get; set; } = "";
        /// Descrizione del tenant
        public string Description { get; set; } = "";
        /// flag che permette di mettere offline il tenant.
        public bool Offline { get; set; } = false;

        /// <summary>
        /// URL del tenant
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// Nome del realm da utilizzare per l'autenticazione del tenant
        /// </summary>
        public string Realm { get; set; }

        public string RootFolder { get; set; } = "";
        
        /// <summary>
        /// Identificativo del client di Keycloak da utilizzare per l'autenticazione del tenant
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// ClientSecret per l'accesso al realm del tenant
        /// </summary>
        public string ClientSecret { get; set; }
    }
}
