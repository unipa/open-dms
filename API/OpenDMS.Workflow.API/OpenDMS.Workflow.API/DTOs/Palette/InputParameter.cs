namespace OpenDMS.Workflow.API.DTOs.Palette
{
    public class InputParameter
    {
        /// <summary>
        /// Nome del parametro richiesto dal worker
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// true=parametro obbligatorio
        /// </summary>
        public bool Required { get; set; } = false;

        /// <summary>
        /// 0 = Input, 1=textarea, 2=select
        /// </summary>
        public int InputType { get; set; } = 0;

        /// <summary>
        /// Nome descrittivo del parametro
        /// </summary>
        public string Label { get; set; } = "";
        /// <summary>
        /// Descrizione estesa del parametro
        /// Viene visualizzato come help su richiesta
        /// </summary>
        public string Description { get; set; } = "";
        /// <summary>
        /// Valori predefiniti del parametrom nel caso di campo di select
        /// </summary>
        public string Values { get; set; } = "";
        /// <summary>
        /// Valore di default del parametro
        /// </summary>
        public string DefaultValue { get; set; } = "";

        /// <summary>
        /// Id della tabella da utilizzare con il servizio "search" per recuperare valori tabellari per il campo (es. tipologie, utenti, ruoli,..)
        /// </summary>
        public string LookupTableId { get; set; } = "";

    }
}
