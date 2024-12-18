namespace Core.TitulusIntegration.Models
{
    public class ProtocolData
    {
        public string IdInterno { get; set; } = "";
        public string URL { get; set; } = "";
        public string Oggetto { get; set; } = "";
        public string Tipo { get; set; } = "";
        public string Anno { get; set; } = "";
        public string physdoc { get; set; } = "";

        public string Protocollo { get; set; } = "";
        public string Registro { get; set; } = "";
        public DateTime DataProtocollo { get; set; } = DateTime.MinValue;
        public int NumeroProtocollo { get; set; } = 0;
        public string ProtocolloEsterno { get; set; } = "";
        public DateTime DataProtocolloEsterno { get; set; } = DateTime.MinValue;
        public string Repertorio { get; set; } = "";
    }
}