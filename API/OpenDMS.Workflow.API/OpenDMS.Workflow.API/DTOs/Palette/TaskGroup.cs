namespace OpenDMS.Workflow.API.DTOs.Palette
{
    public class TaskGroup
    {
        /// <summary>
        /// Id del servizio interfacciato
        /// </summary>
        public string Id { get; set; } = "";

        /// <summary>
        /// Nome del servizio
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Descrizione del servizio
        /// </summary>
        public string Description { get; set; } = "";

        /// <summary>
        /// Elenco di task associati
        /// </summary>
        public List<TaskItem> Tasks { get; set; } = new();
    }
}
