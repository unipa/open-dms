using System.Xml.Serialization;

[XmlRoot("Response", Namespace = "http://www.kion.it/ns/xw")]
public class ResponseNewDocument
{
    [XmlElement("url")]
    public string Url { get; set; }

    [XmlElement("Document")]
    public DocumentNewDocument Document { get; set; }
}

public class DocumentNewDocument
{
    [XmlAttribute("physdoc")]
    public string Physdoc { get; set; }

    [XmlElement("doc")]
    public DocNewDoc doc { get; set; }
}

public class DocNewDoc
{
    [XmlAttribute("num_prot")]
    public string NumProt { get; set; }

    [XmlElement("prot_differito")]
    public string ProtDifferito { get; set; }

    [XmlElement("oggetto")]
    public string Oggetto { get; set; }

    [XmlElement("tipologia")]
    public TipologiaNewDocument Tipologia { get; set; }

    [XmlElement("mezzo_trasmissione")]
    public MezzoTrasmissioneNewDocument MezzoTrasmissione { get; set; }

    [XmlElement("classif")]
    public string Classif { get; set; }

    [XmlElement("note")]
    public string Note { get; set; }

    [XmlElement("riferimenti")]
    public string Riferimenti { get; set; }

    [XmlElement("allegato")]
    public string Allegato { get; set; }

    [XmlElement("voce_indice")]
    public string VoceIndice { get; set; }

    [XmlArray("rif_interni")]
    [XmlArrayItem("rif")]
    public RifInternoNewDocument[] RifInterni { get; set; }

    [XmlArray("rif_esterni")]
    [XmlArrayItem("rif")]
    public RifEsternoNewDocument[] RifEsterni { get; set; }

    [XmlElement("storia")]
    public StoriaNewDocument Storia { get; set; }
}

public class TipologiaNewDocument
{
    [XmlAttribute("cod")]
    public string Cod { get; set; }
}

public class MezzoTrasmissioneNewDocument
{
    [XmlAttribute("cod")]
    public string Cod { get; set; }

    [XmlAttribute("costo")]
    public string Costo { get; set; }

    [XmlAttribute("valuta")]
    public string Valuta { get; set; }
}

public class RifInternoNewDocument
{
    [XmlAttribute("nome_persona")]
    public string NomePersona { get; set; }

    [XmlAttribute("cod_persona")]
    public string CodPersona { get; set; }

    [XmlAttribute("cod_uff")]
    public string CodUff { get; set; }

    [XmlAttribute("nome_uff")]
    public string NomeUff { get; set; }

    [XmlAttribute("diritto")]
    public string Diritto { get; set; }
}

public class RifEsternoNewDocument
{
    [XmlAttribute("data_prot")]
    public string DataProt { get; set; }

    [XmlAttribute("n_prot")]
    public string NProt { get; set; }

    [XmlElement("nome")]
    public string Nome { get; set; }

    [XmlElement("indirizzo")]
    public IndirizzoNewDocument Indirizzo { get; set; }

    [XmlElement("referente")]
    public ReferenteNewDocument Referente { get; set; }

    [XmlElement("email_certificata")]
    public EmailCertificataNewDocument EmailCertificata { get; set; }
}

public class IndirizzoNewDocument
{
    [XmlAttribute("email")]
    public string Email { get; set; }

    [XmlAttribute("fax")]
    public string Fax { get; set; }

    [XmlAttribute("tel")]
    public string Tel { get; set; }

    [XmlText]
    public string Value { get; set; }
}

public class ReferenteNewDocument
{
    [XmlAttribute("cod")]
    public string Cod { get; set; }

    [XmlAttribute("nominativo")]
    public string Nominativo { get; set; }
}

public class EmailCertificataNewDocument
{
    [XmlAttribute("addr")]
    public string Addr { get; set; }
}

public class StoriaNewDocument
{
    [XmlElement("creazione")]
    public CreazioneNewDocument Creazione { get; set; }

    [XmlElement("responsabilita")]
    public ResponsabilitaNewDocument Responsabilita { get; set; }
}

public class CreazioneNewDocument
{
    [XmlAttribute("cod_oper")]
    public string CodOper { get; set; }

    [XmlAttribute("cod_uff_oper")]
    public string CodUffOper { get; set; }

    [XmlAttribute("oper")]
    public string Oper { get; set; }

    [XmlAttribute("uff_oper")]
    public string UffOper { get; set; }

    [XmlAttribute("data")]
    public string Data { get; set; }

    [XmlAttribute("ora")]
    public string Ora { get; set; }

    [XmlAttribute("versioneTitulus")]
    public string VersioneTitulus { get; set; }
}

public class ResponsabilitaNewDocument
{
    [XmlAttribute("cod_operatore")]
    public string CodOperatore { get; set; }

    [XmlAttribute("cod_uff")]
    public string CodUff { get; set; }

    [XmlAttribute("nome_uff")]
    public string NomeUff { get; set; }

    [XmlAttribute("operatore")]
    public string Operatore { get; set; }

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