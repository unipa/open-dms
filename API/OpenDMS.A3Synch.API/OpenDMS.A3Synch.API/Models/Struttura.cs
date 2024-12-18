namespace A3Synch.Models
{
    public class Struttura
    {
        public string rn { get; set; }
        public int id { get; set; }
        public string? id_struttura { get; set; }
        public string id_tipo_struttura { get; set; }
        public string denominazione { get; set; }
        public string descrizione { get; set; }
        public string inizio_validita { get; set; }
        public string fine_validita { get; set; }

        public int LeftBound { get; set; }
        public int RightBound { get; set; }
    }
}