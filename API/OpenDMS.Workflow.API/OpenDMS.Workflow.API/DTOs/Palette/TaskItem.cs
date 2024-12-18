namespace OpenDMS.Workflow.API.DTOs.Palette
{
    public class TaskItem
    {
        /// <summary>
        /// Identificativo univoco del servizio task
        /// </summary>
        public string TaskServiceId { get; set; } = "";

        /// <summary>
        /// Identificativo univoco del task
        /// </summary>
        public string Id { get; set; } = "";
        /// <summary>
        /// Nome del gruppo di appartenenza
        /// </summary>
        public string Group { get; set; } = "";
        /// <summary>
        /// Nome del task
        /// </summary>
        public string Name { get; set; } = "";


        /// <summary>
        /// Parametri da passare al jobworker per identificare 
        /// un task in modo univoco
        /// </summary>
        public string Header { get; set; } = "";

        /// <summary>
        /// Tipo di autenticazione richiesta
        /// </summary>
        public int AuthenticationType { get; set; } = 0;

        /// <summary>
        /// Nome del jobworker associato
        /// </summary>
        public string JobWorker { get; set; } = "";

        /// <summary>
        /// Nome del Tag XML da generare (es. zeebe:serviceTask)
        /// </summary>
        public string TagType { get; set; } = "";

        /// <summary>
        /// Nome descrittivo del task
        /// </summary>
        public string Label { get; set; } = "";
        /// <summary>
        /// Descrizione del task
        /// Viene visualizzato come help su richiesta
        /// </summary>
        public string Description { get; set; } = "";

        /// <summary>
        /// Icona associata al task
        /// </summary>
        public string Icon { get; set; } = "";

        /// <summary>
        ///  Colore del task
        /// </summary>
        public string ColorFill { get; set; } = "";

        /// <summary>
        ///  Colore del task
        /// </summary>
        public string ColorStroke { get; set; } = "";

        /// <summary>
        /// Elenco dei parametri di input
        /// </summary>
        public List<InputParameter> Inputs { get; set; } = new();

        /// <summary>
        /// Elenco dei parametri di output
        /// </summary>
        public List<OutputParameter> Outputs { get; set; } = new();

    }
}
