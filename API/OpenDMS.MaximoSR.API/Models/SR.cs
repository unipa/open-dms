namespace OpenDMS.MaximoSR.API.Models
{
    public class SR
    {
        public string Iddms { get; set; } = Guid.NewGuid().ToString();
        public string Origine { get; set; } = "DMS";
        public string Studente { get; set; }
        public string Reportedpriority { get; set; }
        public string Description { get; set; }
        public string Assetnum { get; set; }
        public string Assetorgid { get; set; } = "UNIPA";
        public string Assetsiteid { get; set; } = "UNP";
        public string Worktype { get; set; } = "GUAST";


        public SR(string Studente, string Reportedpriority, string Description, string Assetnum)
        {
            this.Studente = Studente;
            this.Reportedpriority = Reportedpriority;
            this.Description = Description;
            this.Assetnum = Assetnum;
        }

    }
}
