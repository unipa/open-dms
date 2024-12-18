namespace RemoteSignInfocert.Models.VM
{
    public class FileListVM
    {
        public FileListVM(string fileName, string document, string type, string dimension, string creationTime, bool isErasable)
        {
            FileName = fileName;
            Document = document;
            Type = type;
            Dimension = dimension;
            CreationTime = creationTime;
            this.isErasable = isErasable;
        }

        public string FileName { get; set; }
        public string Document { get; set; }
        public string Type { get; set; }
        public string Dimension { get; set; }
        public string CreationTime { get; set; }
        public bool isErasable { get; set; }

    }
}
