namespace OpenDMS.Workflow.API.DTOs.Palette
{
    public class OutputParameter
    {
        /// <summary>
        /// Nome del parametro di output
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Valore di default.
        /// Rappresenta il nome della variabile globale che si vuole preimpostare
        /// </summary>
        public string DefaultValue { get; set; }
        /// <summary>
        /// Indica se inpostare o meno il campo di output sul PropertyPanel
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// Nome descrittivo del campo di output
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// Descrizione del campo di output
        /// Viene visualizzato come help su richiesta
        /// </summary>
        public string Description { get; set; }
    }

}

