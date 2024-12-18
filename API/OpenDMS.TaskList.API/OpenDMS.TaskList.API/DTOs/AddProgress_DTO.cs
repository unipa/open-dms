namespace OpenDMS.TaskList.API.DTOs
{
    public class AddProgress_DTO
    {
        /// <summary>
        /// Messaggio descrittivo del progresso effettuato
        /// </summary>
        public string message { get; set; } = "";

        /// <summary>
        /// Percentuale di completamento del processo
        /// </summary>
        public decimal percentage { get; set; } = 0;

        /// <summary>
        /// Stato corrente delle variabili del form 
        /// </summary>
        public string variables { get; set; } = null;
    }
}
