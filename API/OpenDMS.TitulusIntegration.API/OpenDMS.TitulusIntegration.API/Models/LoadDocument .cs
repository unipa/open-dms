using System.Xml.Serialization;


namespace OpenDMS.TitulusIntegration.API.Models.LoadDocument
{
    [XmlRoot("Response", Namespace = "http://www.kion.it/ns/xw")]
    public class Response
    {
        [XmlAttribute("canSee")]
        public bool CanSee { get; set; }

        [XmlAttribute("canLinkFolder")]
        public bool CanLinkFolder { get; set; }
        [XmlElement("url")]
        public string Url { get; set; }

        [XmlElement("Document")]
        public Document Document { get; set; }
    }

    public class Document
    {
        [XmlAttribute("physdoc")]
        public string Physdoc { get; set; }

        [XmlElement("doc")]
        public Doc Doc { get; set; }
    }

    public class Doc
    {
        [XmlAttribute("data_prot")]
        public string DataProt { get; set; }

        [XmlAttribute("annullato")]
        public string Annullato { get; set; }

        [XmlAttribute("iu_name")]
        public string IuName { get; set; }

        [XmlAttribute("schema_version")]
        public string SchemaVersion { get; set; }

        [XmlAttribute("checksum")]
        public string Checksum { get; set; }

        [XmlAttribute("checksum_ver")]
        public int ChecksumVer { get; set; }

        [XmlAttribute("nrecord")]
        public string Nrecord { get; set; }

        [XmlAttribute("tipo")]
        public string Tipo { get; set; }

        [XmlAttribute("cod_amm_aoo")]
        public string CodAmmAoo { get; set; }

        [XmlAttribute("anno")]
        public int Anno { get; set; }

        [XmlAttribute("num_prot")]
        public string NumProt { get; set; }

        [XmlAttribute("scarto")]
        public string Scarto { get; set; }

        [XmlElement("classif")]
        public string classif { get;set; }

        [XmlElement("files")]
        public Files Files { get; set; }

        [XmlElement("mezzo_trasmissione")]
        public MezzoTrasmissione mezzoTrasmissione { get; set; }

        [XmlElement("rif_esterni")]
        public RifEsterni rif_esterni { get; set; }

        [XmlElement("rif_interni")]
        public RifInterni rif_interni { get; set; }

        [XmlElement("storia")]
        public Storia storia { get; set; }

        [XmlElement("oggetto")]
        public string oggetto { get; set; }
    }

    public class Files
    {
        [XmlElement("file", Namespace = "http://www.kion.it/ns/xw")]
        public File[] file { get; set; }
    }

    public class File
    {
        [XmlAttribute("principale")]
        public bool Principale { get; set; }

        [XmlAttribute("index")]
        public string Index { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("signed")]
        public bool Signed { get; set; }

        [XmlAttribute("title")]
        public string Title { get; set; }
        
        [XmlElement("chkin")]
        public Chkin Chkin {  get;set;}
    }

    public class Chkin
    {
        [XmlAttribute("operatore")]
        public string Operatore { get; set; }

        [XmlAttribute("cod_operatore")]
        public string CodOperatore { get; set; }

        [XmlAttribute("data")]
        public string Data { get; set; }

        [XmlAttribute("ora")]
        public string Ora { get; set; }
    }

    public class MezzoTrasmissione
    {
        [XmlAttribute("cod")]
        public string cod { get; set; }
    }

    public class RifEsterni
    {
        [XmlElement("rif")]
        public Rif[] Rif { get; set; }
    }

    public class Rif
    {
        [XmlElement("nome")]
        public Nome Nome { get; set; }
    }

    public class Nome
    {
        [XmlAttribute("cod")]
        public string Cod { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

    public class RifInterni
    {
        [XmlElement("rif")]
        public RifInterno[] RifList { get; set; }
    }

    public class RifInterno
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

    public class Storia
    {
        [XmlElement("creazione")]
        public Creazione Creazione { get; set; }

        [XmlElement("responsabilita")]
        public Responsabilita Responsabilita { get; set; }

        [XmlElement("assegnazione_cc")]
        public AssegnazioneCC AssegnazioneCC { get; set; }

        [XmlElement("protocollazione")]
        public Protocollazione Protocollazione { get; set; }

        [XmlElement("ultima_modifica")]
        public UltimaModifica UltimaModifica { get; set; }

        [XmlElement("in_fascicolo")]
        public InFascicolo InFascicolo { get; set; }
    }

    public class Creazione
    {
        [XmlAttribute("oper")]
        public string Oper { get; set; }

        [XmlAttribute("cod_oper")]
        public string CodOper { get; set; }

        [XmlAttribute("uff_oper")]
        public string UffOper { get; set; }

        [XmlAttribute("cod_uff_oper")]
        public string CodUffOper { get; set; }

        [XmlAttribute("data")]
        public string Data { get; set; }

        [XmlAttribute("ora")]
        public string Ora { get; set; }

        [XmlAttribute("versioneTitulus")]
        public string VersioneTitulus { get; set; }
    }

    public class Responsabilita
    {
        [XmlAttribute("cod_uff")]
        public string CodUff { get; set; }

        [XmlAttribute("nome_uff")]
        public string NomeUff { get; set; }

        [XmlAttribute("operatore")]
        public string Operatore { get; set; }

        [XmlAttribute("cod_operatore")]
        public string CodOperatore { get; set; }

        [XmlAttribute("data")]
        public string Data { get; set; }

        [XmlAttribute("ora")]
        public string Ora { get; set; }

        [XmlAttribute("cod_persona")]
        public string CodPersona { get; set; }

        [XmlAttribute("nome_persona")]
        public string NomePersona { get; set; }

        [XmlAttribute("versioneTitulus")]
        public string VersioneTitulus { get; set; }
    }

    public class AssegnazioneCC
    {
        [XmlAttribute("cod_uff")]
        public string CodUff { get; set; }

        [XmlAttribute("nome_uff")]
        public string NomeUff { get; set; }

        [XmlAttribute("operatore")]
        public string Operatore { get; set; }

        [XmlAttribute("cod_operatore")]
        public string CodOperatore { get; set; }

        [XmlAttribute("data")]
        public string Data { get; set; }

        [XmlAttribute("ora")]
        public string Ora { get; set; }

        [XmlAttribute("cod_persona")]
        public string CodPersona { get; set; }

        [XmlAttribute("nome_persona")]
        public string NomePersona { get; set; }

        [XmlAttribute("versioneTitulus")]
        public string VersioneTitulus { get; set; }
    }

    public class Protocollazione
    {
        [XmlAttribute("oper")]
        public string Oper { get; set; }

        [XmlAttribute("cod_oper")]
        public string CodOper { get; set; }

        [XmlAttribute("uff_oper")]
        public string UffOper { get; set; }

        [XmlAttribute("cod_uff_oper")]
        public string CodUffOper { get; set; }

        [XmlAttribute("data")]
        public string Data { get; set; }

        [XmlAttribute("ora")]
        public string Ora { get; set; }

        [XmlAttribute("versioneTitulus")]
        public string VersioneTitulus { get; set; }
    }

    public class UltimaModifica
    {
        [XmlAttribute("oper")]
        public string Oper { get; set; }

        [XmlAttribute("cod_oper")]
        public string CodOper { get; set; }

        [XmlAttribute("uff_oper")]
        public string UffOper { get; set; }

        [XmlAttribute("cod_uff_oper")]
        public string CodUffOper { get; set; }

        [XmlAttribute("data")]
        public string Data { get; set; }

        [XmlAttribute("ora")]
        public string Ora { get; set; }

        [XmlAttribute("versioneTitulus")]
        public string VersioneTitulus { get; set; }

        [XmlAttribute("azione")]
        public string Azione { get; set; }
    }

    public class InFascicolo
    {
        [XmlAttribute("operatore")]
        public string Operatore { get; set; }

        [XmlAttribute("cod_operatore")]
        public string CodOperatore { get; set; }

        [XmlAttribute("data")]
        public string Data { get; set; }

        [XmlAttribute("ora")]
        public string Ora { get; set; }

        [XmlAttribute("tipo")]
        public string Tipo { get; set; }

        [XmlAttribute("codice")]
        public string Codice { get; set; }
    }
}

