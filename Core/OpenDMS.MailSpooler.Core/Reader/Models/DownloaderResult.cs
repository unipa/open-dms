namespace OpenDMS.MailSpooler.Core.Reader.Models
{
    public class DownloaderResult
    {
        public IList<int> Messaggi { get; set; } = new List<int>();
        public string Folder { get; set; }
        public int Letti { get; set; }
        public int Scaricati { get; set; }
        public DateTime FirstDate { get; set; }
    }


}
