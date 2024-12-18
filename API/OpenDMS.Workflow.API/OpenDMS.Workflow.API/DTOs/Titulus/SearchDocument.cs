using System.Globalization;
using System.Xml.Serialization;


namespace OpenDMS.Workflow.API.DTOs.Titulus
{
    [XmlRoot("Response", Namespace = "http://www.kion.it/ns/xw")]
    public class ResponseSearch
    {
        [XmlAttribute("pageCount")]
        public int pageCount { get; set; }

        [XmlAttribute("pageIndex")]
        public int pageIndex { get; set; }

        [XmlAttribute("seleId")]
        public string seleId { get; set; }

        [XmlAttribute("seleSize")]
        public string seleSize { get; set; }

        [XmlElement("doc")]
        public DocSearch Doc { get; set; }
    }

    public class DocSearch
    {
        [XmlAttribute("physdoc")]
        public string Physdoc { get; set; }

        [XmlAttribute("nrecord")]
        public string Nrecord { get; set; }

        [XmlAttribute("tipo")]
        public string Tipo { get; set; }

        [XmlAttribute("anno")]
        public string Anno { get; set; }

        [XmlAttribute("cod_amm_aoo")]
        public string CodAmmAoo { get; set; }

        [XmlAttribute("num_prot")]
        public string NumProt { get; set; }

        [XmlAttribute("data_prot")]
        public string DataProt { get; set; }

        [XmlAttribute("annullato")]
        public string Annullato { get; set; }

        [XmlElement("oggetto")]
        public string oggetto { get; set; }

        [XmlElement("classif")]
        public string classif { get; set; }

        [XmlElement("rif_interni")]
        public RifInterniSearch rif_interni { get; set; }

        [XmlElement("rif_esterni")]
        public RifEsterniSearch rif_esterni { get; set; }

        [XmlElement("files")]
        public FilesSearch? Files { get; set; }
        public string titulusDocumentURL { get; set; }

    }

    public class FilesSearch
    {
        [XmlElement("file")]
        public FileSearch[] file { get; set; }
    }

    public class FileSearch
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("title")]
        public string Title { get; set; }
        public string downloadUrl { get; set; }
        public bool Principale { get; set; }
    }

    public class RifEsterniSearch
    {
        [XmlElement("rif")]
        public RifSearch[] Rif { get; set; }
    }

    public class RifSearch
    {
        [XmlElement("nome")]
        public string Nome { get; set; }
    }

    public class RifInterniSearch
    {
        [XmlElement("rif")]
        public RifInternoSearch[] RifList { get; set; }
    }

    public class RifInternoSearch
    {
        [XmlAttribute("nome_persona")]
        public string nome_persona { get; set; }

        [XmlAttribute("cod_persona")]
        public string cod_persona { get; set; }

        [XmlAttribute("cod_uff")]
        public string cod_uff { get; set; }

        [XmlAttribute("nome_uff")]
        public string nome_uff { get; set; }

        [XmlAttribute("diritto")]
        public string diritto { get; set; }

        [XmlAttribute("cod_fasc")]
        public string cod_fasc { get; set; }

    }
}

