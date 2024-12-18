using Core.TitulusIntegration.Models;
using Newtonsoft.Json;
using System.ComponentModel.Design;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using TitulusIntegration.Titulus;
using TitulusIntegration.TitulusACL;
using TitulusIntegration.TitulusOrgani;
using AttachmentBean = TitulusIntegration.Titulus.AttachmentBean;

namespace Core.TitulusIntegration;

public class Titulus4 : IDisposable
{
    private Titulus4Client client;
    private Acl4Client clientACL;
    private TitulusOrgani4Client clientOrgani;

    private readonly string URL;
    private readonly string uid;
    private readonly string pwd;

    private bool disposing = false;

    public Titulus4(string URL, string uid, string pwd) 
    {
        ServicePointManager.Expect100Continue = true;
        //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
        //       | SecurityProtocolType.Tls11
        //       | SecurityProtocolType.Tls12
        //       | SecurityProtocolType.Ssl3;

        ServicePointManager.ServerCertificateValidationCallback +=
        (se, cert, chain, sslerror) =>
        {
            return true;
        };

        ServicePointManager.Expect100Continue = true;
        clientACL = null; //
        clientOrgani = null;
        this.URL = URL;
        this.uid = uid;
        this.pwd = pwd;

        var binding = new CustomBinding(
            new TextMessageEncodingBindingElement(MessageVersion.Soap11, System.Text.Encoding.UTF8),
            new HttpsTransportBindingElement()
            {
                AuthenticationScheme = AuthenticationSchemes.Basic,
                MaxReceivedMessageSize = 314572800
            });


        client = new(binding, new EndpointAddress(URL + "/titulus_ws/services/Titulus4"));// Titulus4Client.EndpointConfiguration.Titulus4, URL + "/titulus_ws/services/Titulus4");
        client.ClientCredentials.UserName.UserName = uid;
        client.ClientCredentials.UserName.Password = pwd;
        client.Open();

    }

    public Titulus4(string URL, string uid)
    {
        ServicePointManager.Expect100Continue = true;
        //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
        //       | SecurityProtocolType.Tls11
        //       | SecurityProtocolType.Tls12
        //       | SecurityProtocolType.Ssl3;

        ServicePointManager.ServerCertificateValidationCallback +=
        (se, cert, chain, sslerror) =>
        {
            return true;
        };

        ServicePointManager.Expect100Continue = true;
        clientACL = null; //
        clientOrgani = null;
        this.URL = URL;
        this.uid = uid;
        this.pwd = "";

        var binding = new CustomBinding(
            new TextMessageEncodingBindingElement(MessageVersion.Soap11, System.Text.Encoding.UTF8),
            new HttpsTransportBindingElement()
            {
                AuthenticationScheme = AuthenticationSchemes.Basic,
                MaxReceivedMessageSize = 314572800
            });

        client = new(binding, new EndpointAddress(URL + "/titulus_ws/services/Titulus4"));// Titulus4Client.EndpointConfiguration.Titulus4, URL + "/titulus_ws/services/Titulus4");
        client.ClientCredentials.UserName.UserName = uid;
        //client.ClientCredentials.UserName.Password = pwd;
        client.Open();
    }
    public void Dispose()
    {
        if (disposing) return;
        disposing = true;
        if (client != null && client.State == System.ServiceModel.CommunicationState.Opened)
            client.Close();

        client = null;

        if (clientACL != null && clientACL.State == System.ServiceModel.CommunicationState.Opened)
            clientACL.Close();
        clientACL = null; //

        if (clientOrgani != null && clientOrgani.State == System.ServiceModel.CommunicationState.Opened)
            clientOrgani.Close();
        clientOrgani = null;
    }


    public Acl4Client ACL
    {
        get
        {
            if (clientACL == null)
            {
                var binding = new CustomBinding(
                    new TextMessageEncodingBindingElement(MessageVersion.Soap11, System.Text.Encoding.UTF8),
                    new HttpsTransportBindingElement()
                    {
                        AuthenticationScheme = AuthenticationSchemes.Basic,
                        MaxReceivedMessageSize = 314572800
                    });
                clientACL = new Acl4Client(binding, new EndpointAddress(URL + "/titulus_ws/services/Acl4"));
                clientACL.ClientCredentials.UserName.UserName = uid;
                clientACL.ClientCredentials.UserName.Password = pwd;
                clientACL.Open();
            }
            return clientACL;
        }
    }
    //public TitulusOrgani4Client Organi
    //{
    //    get
    //    {
    //        if (clientOrgani == null)
    //        {
    //            clientOrgani = new(TitulusOrgani4Client.EndpointConfiguration.TitulusOrgani4, URL + "/titulus_ws/services/TitulusOrgani4");
    //            clientOrgani = new();
    //            clientOrgani.ClientCredentials.UserName.UserName = uid;
    //            clientOrgani.ClientCredentials.UserName.Password = pwd;
    //            clientOrgani.Open();
    //        }
    //        return clientOrgani;
    //    }
    //}


    public string GetFolderMetadada(long id)
    {
        return client.getFolder(id.ToString());
    }

    public ProtocolData GetProtocolData(long physdoc)
    {
        ProtocolData DatiProtocollo = new();

        var res = client.loadDocument(physdoc.ToString(), false);
        return GetProtocolFromResponse(res);
    }


    public ProtocolData GetProtocolData(string protocollo)
    {
        ProtocolData DatiProtocollo = new();

        var res = client.search("([docnumprot]="+protocollo+")", null,null);

        return GetProtocolFromResponse(res);
    }


    public string GetFileBase64(string fileID)
    {
        var attach = client.getAttachment(fileID);
        return Convert.ToBase64String(attach.content);
    }
    public string GetDocumentUrl(long physdoc)
    {
        return client.getDocumentURL(physdoc.ToString());
    }

    public void AttachFile(string documentId, string UserId, string description, string filename, byte[] file)
    {
        var idTitulus = documentId;
        var attachmentBeans = new List<AttachmentBean>();
        var attach = new AttachmentBean();
        attach.content = file;
        attach.fileName = filename;
        attach.description = description;
        attach.mimeType = MimeKit.MimeTypes.GetMimeType(filename);  //MimeMapping.GetMimeMapping(filename);
        attachmentBeans.Add(attach);
        var _params = new SaveParams();
        _params.pdfConversion = true;
        var res = client.checkInContentFiles(idTitulus, attachmentBeans.ToArray(), _params);
    }


    public ProtocolData SaveInboundProtocol(InboundDocument documentoDaProtocollare, bool draft)
    {
        //preparazione riferimento interni ed esterni
        var riferimentiInterni = new List<XElement>();
        var riferimentiEsterni = new List<XElement>();
        XElement minuta = null;

        bool voceIndiceIsNeeded = false;
        bool classifIsNeeded = false;
        bool tipologiaIsNeeded = true;
        bool mezzoIsNeeded = true;
        bool noteIsNeeded = true;

        var voceIndiceValue = documentoDaProtocollare.VoceIndice;

        indice_titolarioCompilazione_automaticaClassif classif = null;
        string tipoDocTitulus = "";

        tipoDocTitulus = "arrivo";
        classifIsNeeded = true;
        var titolari = StaticGetCustomData<IndiceTitolari>(CustomDataEnum.IndiceTitolario, voceIndiceValue); //.Titolari?.FirstOrDefault().compilazione_automatica.classif;
        if (titolari.Titolari.Count > 0)
            classif = titolari.Titolari[0].compilazione_automatica.classif;

        tipologiaIsNeeded = false;
        mezzoIsNeeded = false;
        noteIsNeeded = false;

        //riferimenti interni
        var dirittoIngresso = "RPA";

        var Destinatari = documentoDaProtocollare.PersoneInterne; // GetExtraInfoProperty("Destinatari", documentoDaProtocollare.ExtraProtocolInfo).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        if (Destinatari.Count() > 0)
        {
            foreach (var destinatario in Destinatari)
            {
                var IdentificativoPersona = destinatario;
                var DatiPersonaInterna = StaticGetCustomData<PersoneInterne>(documentoDaProtocollare.CriterioRicercaPersoneInterne, IdentificativoPersona).persone.FirstOrDefault();
                if (DatiPersonaInterna == null)
                    throw new Exception("Non è stato indicato un destinatario interno valido per: "+ IdentificativoPersona);

                //var ufficioPersona = UfficiDestinatari.FirstOrDefault(u => u.Contains(personaInterna.cod_uff));
                if (!String.IsNullOrEmpty(DatiPersonaInterna.cod_uff))
                {
                    var UfficiopersonaInterna = StaticGetCustomData<StruttureInterne>(CustomDataEnum.StruttureInterneDaCodice, DatiPersonaInterna.cod_uff).strutture.FirstOrDefault();
                    if (UfficiopersonaInterna != null)
                    {
                        var nomeUfficio = UfficiopersonaInterna.nome.Value; //  UfficiDestinatari.FirstOrDefault(u => u.Contains(personaInterna.cod_uff));//.Split('_').FirstOrDefault();

                        riferimentiInterni.Add(GeneraElementoRifInterno(dirittoIngresso, DatiPersonaInterna.matricola, DatiPersonaInterna.cod_uff, DatiPersonaInterna.cognome + " " + DatiPersonaInterna.nome, nomeUfficio));
                        dirittoIngresso = "CC"; //dopo aver dato al primo destinatario il diritto RPA
                    }
                    else
                        throw new Exception("Non è stato trovato l'ufficio del destinatario interno: "+ IdentificativoPersona + " - codice: " + DatiPersonaInterna.cod_uff);
                } else
                    throw new Exception("Non è stato trovato l'ufficio del destinatario interno: " + IdentificativoPersona);
            }
        }

        //destinatari 
        var DestinatariS = documentoDaProtocollare.StruttureInterne; // GetExtraInfoProperty("Destinatari", documentoDaProtocollare.ExtraProtocolInfo).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        //var UfficiDestinatari = documentoDaProtocollare.StruttureInterne; // ProtocolImpl.GetExtraInfoProperty("UfficiDestinatari", documentoDaProtocollare.ExtraProtocolInfo).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        if (DestinatariS.Count() > 0)
        {
            foreach (var destinatario in DestinatariS)
            {
                var IdentificativoStruttura = destinatario;
                var UfficiopersonaInterna = StaticGetCustomData<StruttureInterne>(CustomDataEnum.StruttureInterneDaCodice, IdentificativoStruttura).strutture.FirstOrDefault();
                if (UfficiopersonaInterna != null)
                {
                    var nomeUfficio = UfficiopersonaInterna.nome.Value; //  UfficiDestinatari.FirstOrDefault(u => u.Contains(personaInterna.cod_uff));//.Split('_').FirstOrDefault();
                    if (!String.IsNullOrEmpty(UfficiopersonaInterna.cod_responsabile))
                    {
                        var DatiPersonaInterna = StaticGetCustomData<PersoneInterne>(documentoDaProtocollare.CriterioRicercaPersoneInterne, UfficiopersonaInterna.cod_responsabile).persone.FirstOrDefault();
                        //var ufficioPersona = UfficiDestinatari.FirstOrDefault(u => u.Contains(personaInterna.cod_uff));
                        if (DatiPersonaInterna != null)
                        {
                            if (!String.IsNullOrEmpty(DatiPersonaInterna.cod_uff))
                            {
                                riferimentiInterni.Add(GeneraElementoRifInterno(dirittoIngresso, DatiPersonaInterna.matricola, UfficiopersonaInterna.cod_uff, DatiPersonaInterna.cognome + " " + DatiPersonaInterna.nome, nomeUfficio));
                                dirittoIngresso = "CC"; //dopo aver dato al primo destinatario il diritto RPA
                            }
                            else
                                throw new Exception("Non risulta associato il codice ufficio interno: " + IdentificativoStruttura + " - " + nomeUfficio + " - al responsabile codice: " + UfficiopersonaInterna.cod_responsabile + DatiPersonaInterna.cognome + " " + DatiPersonaInterna.nome);
                        }
                        else
                            throw new Exception("Non è stato trovato il responsabile dell'ufficio interno: " + IdentificativoStruttura + " - " + nomeUfficio + " - codice: "+ UfficiopersonaInterna.cod_responsabile);
                    }
//                    else
//                        throw new Exception("Non è stato trovato il responsabile dell'ufficio interno: " + IdentificativoStruttura+ " - "+ nomeUfficio);
                }
            }
        }
        //destinatari 
         //var UfficiDestinatari = documentoDaProtocollare.StruttureInterne; // ProtocolImpl.GetExtraInfoProperty("UfficiDestinatari", documentoDaProtocollare.ExtraProtocolInfo).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);


        //destinatari in copia conoscenza

        var DestinatariCC = documentoDaProtocollare.PersoneInterneCC;// ProtocolImpl.GetExtraInfoProperty("DestinatariCC", documentoDaProtocollare.ExtraProtocolInfo).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var destinatarioCC in DestinatariCC)
        {
            var IdentificativoPersonaCC = destinatarioCC;
            var DatiPersonaInterna = StaticGetCustomData<PersoneInterne>(CustomDataEnum.PersoneInterneDaCognome, IdentificativoPersonaCC).persone.FirstOrDefault();
            if (DatiPersonaInterna != null)
            {
                //                throw new Exception("Non è stato trovato il destinatario interno in CC: " + IdentificativoPersonaCC);
                //            }
                if (!String.IsNullOrEmpty(DatiPersonaInterna.cod_uff))
                {
                    var UfficioPersonaInterna = StaticGetCustomData<StruttureInterne>(CustomDataEnum.StruttureInterneDaCodice, DatiPersonaInterna.cod_uff).strutture.FirstOrDefault();
                    var nomeUfficio = UfficioPersonaInterna.nome.Value; //  UfficiDestinatari.FirstOrDefault(u => u.Contains(personaInterna.cod_uff));//.Split('_').FirstOrDefault();
                    riferimentiInterni.Add(GeneraElementoRifInterno("CC", DatiPersonaInterna.matricola, DatiPersonaInterna.cod_uff, DatiPersonaInterna.cognome + " " + DatiPersonaInterna.nome, nomeUfficio));
                }
                else
                    throw new Exception("Non è stato trovato il codice ufficio del destinatario interno in CC: " + IdentificativoPersonaCC);
            }
        }

        //mittenti
        var IdentificativoMittente = documentoDaProtocollare.PersonaEsterna; // ProtocolImpl.GetExtraInfoProperty("mittente", documentoDaProtocollare.ExtraProtocolInfo).Split('_'); //nome_cognome_matricola
        if (string.IsNullOrEmpty(IdentificativoMittente))
            throw new Exception("Non è stato indicato il mittente (PersonaEsterna)");
        
        var DatiPersonaEsterna = StaticGetCustomData<PersoneEsterne>(documentoDaProtocollare.CriterioRicercaPersonaEsterna, IdentificativoMittente).persone.FirstOrDefault();
        if (DatiPersonaEsterna != null)
        {
            var IdentificativoStrutturaMittente = documentoDaProtocollare.StrutturaEsterna; // ProtocolImpl.GetExtraInfoProperty("strutturaMittente", documentoDaProtocollare.ExtraProtocolInfo).Split('_');// nome_coduff
            var DatiUfficioEsterno = StaticGetCustomData<StruttureEsterne>(documentoDaProtocollare.CriterioRicercaStrutturaEsterna, IdentificativoStrutturaMittente).strutture.FirstOrDefault();
            if (DatiUfficioEsterno != null)
            {
                riferimentiEsterni.Add(GeneraElementoRifEsterno(
                nomeCod: DatiUfficioEsterno.cod_uff,
                nomePreserve: DatiUfficioEsterno.nome,
                referenteCod: DatiPersonaEsterna.matricola,
                referenteNominativo: DatiPersonaEsterna.nome + " " + DatiPersonaEsterna.cognome
                ));
            } else
            {
                riferimentiEsterni.Add(GeneraElementoRifEsterno(
                nomeCod: "",
                nomePreserve: "",
                referenteCod: DatiPersonaEsterna.matricola,
                referenteNominativo: DatiPersonaEsterna.nome + " " + DatiPersonaEsterna.cognome
                ));
                throw new Exception("Il mittente indicato - " + IdentificativoMittente + " - non ha una struttura associata : (" + IdentificativoStrutturaMittente + ")");

            }
            //generazione della stringa xml doc
        } else
            throw new Exception("Il mittente indicato non è stato trovato in anagrafica: ("+IdentificativoMittente+")");

        string generatedXML = GenerateXML(
            draft: draft,
            minuta: minuta,
            tipo: tipoDocTitulus,
            oggettoValue: documentoDaProtocollare.Oggetto,
            tipologiaCod: (tipologiaIsNeeded) ? documentoDaProtocollare.Tipologia : null,
            mezzoTrasmissioneCod: (mezzoIsNeeded) ? documentoDaProtocollare.MezzoTrasmissione : null,
            repertorioValue: documentoDaProtocollare.NumeroRepertorio,
            repertorioCod: documentoDaProtocollare.Repertorio,
            noteValue: (noteIsNeeded) ? documentoDaProtocollare.Note : null,
            voceIndiceValue: (voceIndiceIsNeeded) ? voceIndiceValue : null,
            classifCod: (classifIsNeeded) ? classif.cod : null,
            classifValue: (classifIsNeeded) ? classif.Value : null,
            rifInterniList: riferimentiInterni,
            rifEsterniList: riferimentiEsterni
        );
        //preparazione attachments

        // ** COMMENTARE SOLO PER TEST
        // ** RIPROSTINARE PRIMA DI COMPILARE
        // ** 
        var res = client.saveDocument(generatedXML, documentoDaProtocollare.Attachments.ToArray(), null);
        //var res = "";
        return GetProtocolFromResponse(res);
    }

    public ProtocolData SaveOutboundProtocol(OutboundDocument documentoDaProtocollare, bool draft)
    {
        //preparazione riferimento interni ed esterni
        var riferimentiInterni = new List<XElement>();
        var riferimentiEsterni = new List<XElement>();
        XElement minuta = null;

        bool voceIndiceIsNeeded = false;
        bool classifIsNeeded = false;
        bool tipologiaIsNeeded = true;
        bool mezzoIsNeeded = true;
        bool noteIsNeeded = true;

        var voceIndiceValue = documentoDaProtocollare.VoceIndice;
        indice_titolarioCompilazione_automaticaClassif classif = null;
        if (!String.IsNullOrEmpty(voceIndiceValue))
        {
            var titolari = StaticGetCustomData<IndiceTitolari>(CustomDataEnum.IndiceTitolario, voceIndiceValue); //.Titolari?.FirstOrDefault().compilazione_automatica.classif;
            if (titolari.Titolari.Count > 0)
                classif = titolari.Titolari[0].compilazione_automatica.classif;
        }

        string tipoDocTitulus = "";

        tipoDocTitulus = "partenza";
        voceIndiceIsNeeded = true;

        //destinatari 
        var DestinatariU = documentoDaProtocollare.PersoneEsterne;
        var UfficiDestinatariU = documentoDaProtocollare.StruttureEsterne;

        var DestinatariCCU = documentoDaProtocollare.PersoneEsterneCC;
        var UfficiDestinatariCCU = documentoDaProtocollare.StruttureEsterneCC;

        //        if (DestinatariU.Count() < 1)
        //            throw new Exception("Non è stato indicato nessun destinatario.");

        var isCC = false;



        for (int i = 0; i < UfficiDestinatariU.Count; i++)
        {
            try
            {
                var IdentificativoStrutturaEsterna = UfficiDestinatariU[i];
                var DatiUfficioEsterno = StaticGetCustomData<StruttureEsterne>(documentoDaProtocollare.CriterioRicercaStrutturaEsterna, IdentificativoStrutturaEsterna).strutture.FirstOrDefault();
                //var Ufficio = UfficiDestinatariU[i].Split('_'); //nome_cod
                if (DatiUfficioEsterno != null)
                {
                    string EmailPersona = "";
                    string PecPersona = "";
                    string nome = "";
                    string matricola = "";
                    var IdentificativoPersonaEsterna = DestinatariU.Count() > i ? DestinatariU[i] : "";//.Split('_').LastOrDefault();
                    if (!string.IsNullOrEmpty(IdentificativoPersonaEsterna))
                    {
                        var DatiPersonaEsterna = StaticGetCustomData<PersoneEsterne>(documentoDaProtocollare.CriterioRicercaPersonaEsterna, IdentificativoPersonaEsterna).persone.FirstOrDefault();
                        if (DatiPersonaEsterna != null)
                        {

                            if (DatiPersonaEsterna.recapito != null && DatiPersonaEsterna.recapito.email != null && !string.IsNullOrEmpty(DatiPersonaEsterna.recapito.email.addr))
                                EmailPersona = DatiPersonaEsterna.recapito.email.addr;

                            if (DatiPersonaEsterna.recapito != null && DatiPersonaEsterna.recapito.email_certificata != null && !string.IsNullOrEmpty(DatiPersonaEsterna.recapito.email_certificata.addr))
                                PecPersona = DatiPersonaEsterna.recapito.email_certificata.addr;
                            nome = DatiPersonaEsterna.cognome + " " + DatiPersonaEsterna.nome;
                            matricola = DatiPersonaEsterna.matricola;
                        }
                    }
                    riferimentiEsterni.Add(GeneraElementoRifEsterno(
                            nomeCod: DatiUfficioEsterno.cod_uff,
                            nomePreserve: DatiUfficioEsterno.nome,
                            referenteNominativo: nome,
                            referenteCod: matricola,
                            indirizzoEmail: EmailPersona,
                            indirizzoEmailCert: PecPersona,
                            copiaConoscenza: isCC
                            ));
                    isCC = true; //dopo aver dato al primo destinatario il diritto RPA
                }
            }
            catch (Exception ex) { }
        }

        //destinatari in copia conoscenza


        for (int i = 0; i < UfficiDestinatariCCU.Count; i++)
        {
            try
            {
                var IdentificativoStrutturaEsterna = UfficiDestinatariCCU[i];
                var Ufficio = StaticGetCustomData<StruttureEsterne>(documentoDaProtocollare.CriterioRicercaStrutturaEsterna, IdentificativoStrutturaEsterna).strutture.FirstOrDefault();
                if (Ufficio != null)
                {
                    string EmailPersona = "";
                    string PecPersona = "";
                    string nome = "";
                    string matricola = "";
                    var IdentificativoPersonaEsterna = DestinatariCCU[i];//.Split('_').LastOrDefault();
                    var DatiPersonaEsterna = StaticGetCustomData<PersoneEsterne>(documentoDaProtocollare.CriterioRicercaPersonaEsterna, IdentificativoPersonaEsterna).persone.FirstOrDefault();
                    if (DatiPersonaEsterna != null)
                    {
                        if (DatiPersonaEsterna.recapito != null && DatiPersonaEsterna.recapito.email != null && !string.IsNullOrEmpty(DatiPersonaEsterna.recapito.email.addr))
                            EmailPersona = DatiPersonaEsterna.recapito.email.addr;

                        if (DatiPersonaEsterna.recapito != null && DatiPersonaEsterna.recapito.email_certificata != null && !string.IsNullOrEmpty(DatiPersonaEsterna.recapito.email_certificata.addr))
                            PecPersona = DatiPersonaEsterna.recapito.email_certificata.addr;
                        nome = DatiPersonaEsterna.cognome + " " + DatiPersonaEsterna.nome;
                        matricola = DatiPersonaEsterna.matricola;
                    }

                    riferimentiEsterni.Add(GeneraElementoRifEsterno(
                        nomeCod: Ufficio.cod_uff,
                        nomePreserve: Ufficio.nome,
                        referenteNominativo: nome,
                        referenteCod: matricola,
                        indirizzoEmail: EmailPersona,
                        indirizzoEmailCert: PecPersona,
                        copiaConoscenza: true
                        )); ;
                }
            }
            catch (Exception ex) { }
        }

        //riferimenti interni

        //mittenti
        var IdentificativoPersonaInterna = documentoDaProtocollare.PersonaInterna; //cognome.nome
//        var IdentificativoStrutturaInterna = documentoDaProtocollare.StrutturaInterna;//.Split('_');// nome_coduff
        if (!String.IsNullOrEmpty(IdentificativoPersonaInterna))
        {    
            var DatiPersonaInterna = StaticGetCustomData<PersoneInterne>(documentoDaProtocollare.CriterioRicercaPersonaInterna, IdentificativoPersonaInterna).persone.FirstOrDefault();
            if  (DatiPersonaInterna != null)
            {
                
                var DatiUfficioInterno = StaticGetCustomData<StruttureInterne>(CustomDataEnum.StruttureInterneDaCodice, DatiPersonaInterna.cod_uff).strutture.FirstOrDefault();

                riferimentiInterni.Add(GeneraElementoRifInterno(
                    diritto: "RPA",
                    codPersona: DatiPersonaInterna.matricola,
                    codUfficio: DatiPersonaInterna.cod_uff,
                    nomePersona: DatiPersonaInterna.cognome + " " + DatiPersonaInterna.nome,
                    nomeUfficio: DatiUfficioInterno.nome.Value
                    ));
            }
        };

        //generazione della stringa xml doc
        string generatedXML = GenerateXML(
            draft: draft,
            minuta: minuta,
            tipo: tipoDocTitulus,
            oggettoValue: documentoDaProtocollare.Oggetto,
            tipologiaCod: (tipologiaIsNeeded) ? documentoDaProtocollare.Tipologia : null,
            mezzoTrasmissioneCod: (mezzoIsNeeded) ? documentoDaProtocollare.MezzoTrasmissione : null,
            repertorioValue: documentoDaProtocollare.NumeroRepertorio,
            repertorioCod: documentoDaProtocollare.Repertorio,
            noteValue: (noteIsNeeded) ? documentoDaProtocollare.Note : null,
            voceIndiceValue: (voceIndiceIsNeeded) ? voceIndiceValue : null,
            classifCod: (classifIsNeeded) ? classif.cod : null,
            classifValue: (classifIsNeeded) ? classif.Value : null,
            rifInterniList: riferimentiInterni,
            rifEsterniList: riferimentiEsterni,
            //allegatoValue: "0 - nessun allegato",
            riferimentiValue: "CNIPA - Protocollo Informatico"

        );
        // ** COMMENTARE SOLO PER TEST
        // ** RIPROSTINARE PRIMA DI COMPILARE
        // ** 
        var res = client.saveDocument(generatedXML, documentoDaProtocollare.Attachments.ToArray(), null);
        //var res = "";
        return GetProtocolFromResponse(res);
    }

    public ProtocolData SaveInternalProtocol(InternalDocument documentoDaProtocollare, bool draft)
    {
        //preparazione riferimento interni ed esterni
        var riferimentiInterni = new List<XElement>();
        var riferimentiEsterni = new List<XElement>();
        XElement minuta = null;

        bool voceIndiceIsNeeded = false;
        bool classifIsNeeded = false;
        bool tipologiaIsNeeded = true;
        bool mezzoIsNeeded = true;
        bool noteIsNeeded = true;


        var voceIndiceValue = documentoDaProtocollare.VoceIndice;

        indice_titolarioCompilazione_automaticaClassif classif = null;
        string tipoDocTitulus = "";


        tipoDocTitulus = "interno";

        //minuta

        // gia sopra  var voceIndiceValue = ProtocolImpl.GetExtraInfoProperty("voceIndiceValue", documentoDaProtocollare.ExtraProtocolInfo);
        //var classifX = StaticGetCustomData<IndiceTitolari>(CustomDataEnum.IndiceTitolario, voceIndiceValue).Titolari.FirstOrDefault().compilazione_automatica.classif;

        var titolari = StaticGetCustomData<IndiceTitolari>(CustomDataEnum.IndiceTitolario, voceIndiceValue); //.Titolari?.FirstOrDefault().compilazione_automatica.classif;
        if (titolari.Titolari.Count > 0)
            classif = titolari.Titolari[0].compilazione_automatica.classif;


        var IdentificativoMittente = documentoDaProtocollare.PersonaInterna; //cognome.nome

        if (string.IsNullOrEmpty(IdentificativoMittente))
            throw new Exception("Non è stato indicato nessun mittente.");

        var DatiMittenteX = StaticGetCustomData<PersoneInterne>(documentoDaProtocollare.CriterioRicercaPersonaInterna, IdentificativoMittente).persone.FirstOrDefault();
        if (DatiMittenteX == null)
        {
            IdentificativoMittente = uid;
            DatiMittenteX = StaticGetCustomData<PersoneInterne>(CustomDataEnum.PersoneInterneDaLogin, IdentificativoMittente).persone.FirstOrDefault();
            if (DatiMittenteX == null)
            {
                throw new Exception("Il mittente indicato non è stato trovato: (" + IdentificativoMittente + ")");
            }
        }

        var UfficioMittenteX = StaticGetCustomData<StruttureInterne>(CustomDataEnum.StruttureInterneDaCodice, DatiMittenteX.cod_uff).strutture.FirstOrDefault();
        if (UfficioMittenteX == null)
            throw new Exception("La struttura del mittente indicato non è stata trovata: ("+DatiMittenteX.cod_uff+")");

        minuta = GeneraElementoMinuta(classif.cod, classif.Value, DatiMittenteX.cognome + " " + DatiMittenteX.nome, UfficioMittenteX.nome.Value , DatiMittenteX.matricola, DatiMittenteX.cod_uff);
        //riferimenti interni

        //RPA
        //destinatari 
        var DestinatariX = documentoDaProtocollare.PersoneEsterne;
        //var UfficiDestinatariX = documentoDaProtocollare.StruttureEsterne;
        if (DestinatariX.Count < 1 && documentoDaProtocollare.StruttureInterne.Count < 1)
            throw new Exception("Non è stato indicato nessun destinatario.");

        var dirittoInterno = "RPA";

        foreach (var destinatario in DestinatariX)
        {
            var IdentificativoPersonaInterna = destinatario;
            var DatiPersonaDestinataria = StaticGetCustomData<PersoneInterne>(documentoDaProtocollare.CriterioRicercaPersonaInterna, IdentificativoPersonaInterna).persone.FirstOrDefault();
            if (DatiPersonaDestinataria == null)
            {
                IdentificativoPersonaInterna = uid;
                DatiPersonaDestinataria = StaticGetCustomData<PersoneInterne>(CustomDataEnum.PersoneInterneDaLogin, IdentificativoPersonaInterna).persone.FirstOrDefault();
                if (DatiPersonaDestinataria == null)
                {
                    throw new Exception("Il destinatario indicato non è stato trovato: (" + destinatario + ")");
                }
            }
            var DatiUfficioDestinatario = StaticGetCustomData<StruttureInterne>(CustomDataEnum.StruttureInterneDaCodice, DatiPersonaDestinataria.cod_uff).strutture.FirstOrDefault().nome.Value;
            if (DatiUfficioDestinatario == null)
                throw new Exception("La struttura del destinatario indicato non è stata trovata: ("+DatiPersonaDestinataria.cod_uff+")");

            riferimentiInterni.Add(GeneraElementoRifInterno(diritto: dirittoInterno,
                                        codPersona: DatiPersonaDestinataria.matricola,
                                        codUfficio: DatiPersonaDestinataria.cod_uff,
                                        nomePersona: DatiPersonaDestinataria.cognome + " " + DatiPersonaDestinataria.nome,
                                        nomeUfficio: DatiUfficioDestinatario
                                        ));
            dirittoInterno = "CC"; //dopo aver dato al primo destinatario il diritto RPA
        }

        //destinatari in copia conoscenza

        var DestinatariCCX = documentoDaProtocollare.PersoneEsterneCC;
       // var UfficiDestinatariCCX = documentoDaProtocollare.StruttureEsterneCC;

        foreach (var destinatario in DestinatariCCX)
        {
            try
            {
                var IdentificativoPersonaInterna = destinatario;
                var DatiPersonaInterna = StaticGetCustomData<PersoneInterne>(documentoDaProtocollare.CriterioRicercaPersonaInterna, IdentificativoPersonaInterna).persone.FirstOrDefault();
                if (DatiPersonaInterna != null)
                {
                    var NomeUfficio = StaticGetCustomData<StruttureInterne>(CustomDataEnum.StruttureInterneDaCodice, DatiPersonaInterna.cod_uff).strutture.FirstOrDefault().nome.Value;
                    if (NomeUfficio != null)
                    {
                        riferimentiInterni.Add(GeneraElementoRifInterno(diritto: "CC",
                                                    codPersona: DatiPersonaInterna.matricola,
                                                    codUfficio: DatiPersonaInterna.cod_uff,
                                                    nomePersona: DatiPersonaInterna.cognome + " " + DatiPersonaInterna.nome,
                                                    nomeUfficio: NomeUfficio
                                                    ));
                    }
                }
            }
            catch (Exception ex) { }
        }

        var UfficiDestinatariU = documentoDaProtocollare.StruttureInterne;
        for (int i = 0; i < UfficiDestinatariU.Count; i++)
        {
            try
            {
                var IdentificativoStrutturaInterna = UfficiDestinatariU[i];
                var DatiUfficioInterno = StaticGetCustomData<StruttureInterne>(documentoDaProtocollare.CriterioRicercaStrutturaInterna, IdentificativoStrutturaInterna).strutture.FirstOrDefault();
                //var Ufficio = UfficiDestinatariU[i].Split('_'); //nome_cod
                if (DatiUfficioInterno != null)
                {
                    string EmailPersona = "";
                    string PecPersona = "";
                    string nome = "";
                    string matricola = "";
                    var IdentificativoPersonaInterna = DatiUfficioInterno.cod_responsabile; //  DestinatariU.Count() > i ? DestinatariU[i] : "";//.Split('_').LastOrDefault();
                    if (!string.IsNullOrEmpty(IdentificativoPersonaInterna))
                    {
                        var DatiPersonaInterna = StaticGetCustomData<PersoneInterne>(CustomDataEnum.PersoneInterneDaMatricola, IdentificativoPersonaInterna).persone.FirstOrDefault();
                        if (DatiPersonaInterna != null)
                        {
                            nome = DatiPersonaInterna.cognome + " " + DatiPersonaInterna.nome;
                            matricola = DatiPersonaInterna.matricola;
                        }
                    }
                    riferimentiInterni.Add(GeneraElementoRifInterno(
                            diritto: dirittoInterno,
                            codPersona: matricola,
                            codUfficio: DatiUfficioInterno.cod_uff,
                            nomePersona: nome,
                            nomeUfficio: DatiUfficioInterno.nome.Value.ToString()
                            ));
                    dirittoInterno = "CC"; //dopo aver dato al primo destinatario il diritto RPA
                }
            }
            catch (Exception ex) { }
        }

        //destinatari in copia conoscenza

        var UfficiDestinatariCCU = documentoDaProtocollare.StruttureInterneCC;
        for (int i = 0; i < UfficiDestinatariCCU.Count; i++)
        {
            try
            {
                var IdentificativoStrutturaInterna = UfficiDestinatariCCU[i];
                var DatiUfficioInterno = StaticGetCustomData<StruttureInterne>(documentoDaProtocollare.CriterioRicercaStrutturaInterna, IdentificativoStrutturaInterna).strutture.FirstOrDefault();
                //var Ufficio = UfficiDestinatariU[i].Split('_'); //nome_cod
                if (DatiUfficioInterno != null)
                {
                    string EmailPersona = "";
                    string PecPersona = "";
                    string nome = "";
                    string matricola = "";
                    var IdentificativoPersonaInterna = DatiUfficioInterno.cod_responsabile; 
                    if (!string.IsNullOrEmpty(IdentificativoPersonaInterna))
                    {
                        var DatiPersonaInterna = StaticGetCustomData<PersoneInterne>(CustomDataEnum.PersoneInterneDaMatricola, IdentificativoPersonaInterna).persone.FirstOrDefault();
                        if (DatiPersonaInterna != null)
                        {
                            nome = DatiPersonaInterna.cognome + " " + DatiPersonaInterna.nome;
                            matricola = DatiPersonaInterna.matricola;
                        }
                    }
                    riferimentiInterni.Add(GeneraElementoRifInterno(
                            diritto: "CC",
                            codPersona: matricola,
                            codUfficio: DatiUfficioInterno.cod_uff,
                            nomePersona: nome,
                            nomeUfficio: DatiUfficioInterno.nome.Value.ToString()
                            ));
                }
            }
            catch (Exception ex) { }
        }

        //RPAM
        //mittente
        var nome_cognomeX = documentoDaProtocollare.PersonaInterna; //cognome.nome
                                                                                     // già presente sopra var strutturaMittenteX = ProtocolImpl.GetExtraInfoProperty("strutturaMittente", documentoDaProtocollare.ExtraProtocolInfo).Split('_');// nome_coduff
                                                                                      // già presente sopra var personaInternaX = ProtocolImpl.StaticGetCustomData<PersoneInterne>("SoggettiInterni", nome_cognomeX).persone.FirstOrDefault();

        if (string.IsNullOrEmpty(nome_cognomeX))
            throw new Exception("Non è stato indicato nessun mittente.");


        riferimentiInterni.Add(GeneraElementoRifInterno(
            diritto: "RPAM",
            codPersona: DatiMittenteX.matricola,
            codUfficio: DatiMittenteX.cod_uff,
            nomePersona: DatiMittenteX.cognome + " " + DatiMittenteX.nome,
            nomeUfficio: UfficioMittenteX.nome.Value
            ));




        //generazione della stringa xml doc
        string generatedXML = GenerateXML(
            draft: draft,
            minuta: minuta,
            tipo: tipoDocTitulus,
            oggettoValue: documentoDaProtocollare.Oggetto,
            tipologiaCod: (tipologiaIsNeeded) ? documentoDaProtocollare.Tipologia : null,
            mezzoTrasmissioneCod: (mezzoIsNeeded) ? documentoDaProtocollare.MezzoTrasmissione : null,
            repertorioValue: documentoDaProtocollare.NumeroRepertorio,
            repertorioCod: documentoDaProtocollare.Repertorio,
            noteValue: (noteIsNeeded) ? documentoDaProtocollare.Note : null,
            voceIndiceValue: (voceIndiceIsNeeded) ? voceIndiceValue : null,
            classifCod: (classifIsNeeded) ? classif.cod : null,
            classifValue: (classifIsNeeded) ? classif.Value : null,
            rifInterniList: riferimentiInterni,
            rifEsterniList: riferimentiEsterni
        );
        //preparazione attachments
        // ** COMMENTARE SOLO PER TEST
        // ** RIPROSTINARE PRIMA DI COMPILARE
        // ** 
        var res = client.saveDocument(generatedXML, documentoDaProtocollare.Attachments.ToArray(), null);
        //var res = "";
        return GetProtocolFromResponse(res);
    }



    private ProtocolData GetProtocolFromResponse(string res)
    {
        ProtocolData DatiProtocollo = new();

        XDocument xmlDoc = XDocument.Parse(res);
        XElement url = xmlDoc.Root.Element("url");
        XElement docElement = xmlDoc.Root.Element("Document");
        XElement doc = docElement.Element("doc");
        XElement rep = doc.Element("repertorio");

        if (doc != null)
        {
            string dataProt = doc.Attribute("data_prot")?.Value ?? "";
            string numProtEsteso = doc.Attribute("num_prot")?.Value ?? "";
            string idTitulus = doc.Attribute("_id")?.Value ?? "";
            string repertorio = "";
            if (rep != null)
                repertorio = rep.Attribute("numero")?.Value;

            DateTime DataProtocollo;
            DateTime.TryParseExact(dataProt, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DataProtocollo);
            DatiProtocollo.DataProtocollo = DataProtocollo;
            DatiProtocollo.IdInterno = idTitulus;
            var link =  this.URL + url.Value.Substring(url.Value.IndexOf("/", 8));
            DatiProtocollo.URL = link;
            DatiProtocollo.Oggetto = (doc.Element("oggetto")?.Value) ?? "";
            DatiProtocollo.Anno = doc.Attribute("anno")?.Value ?? "";
            DatiProtocollo.Tipo = doc.Attribute("tipo")?.Value ?? "";
            DatiProtocollo.physdoc= doc.Attribute("physdoc")?.Value ?? "";
            //Controllo se il doc è stato PROTOCOLLATO
            if (!String.IsNullOrEmpty(numProtEsteso))
            {
                var datiProtocollo = numProtEsteso.Split('-'); // anno-registro-numeroProtocollo
                DatiProtocollo.Protocollo = datiProtocollo[2].TrimStart('0');
                DatiProtocollo.Registro = datiProtocollo[1];
                DatiProtocollo.NumeroProtocollo = Convert.ToInt32(datiProtocollo[2].TrimStart('0'));
                DatiProtocollo.ProtocolloEsterno = numProtEsteso + "_" + idTitulus;
                DatiProtocollo.DataProtocolloEsterno = DataProtocollo;
            }

            //Controllo se al doc è stato aggiunto un NUM REPETORIO
            if (!String.IsNullOrEmpty(repertorio))
                DatiProtocollo.Repertorio = repertorio;

            //var voceDiIndice = ParseVoceIndiceElement(doc.Element("voce_indice"));
            //var classifElement = (doc.Element("classif") != null) ? doc.Element("classif") : doc.Element("minuta").Element("classif");
            //var classifRes = ParseClassifElement(classifElement);

            //DatiProtocollo.TitoloTitolario = (classifRes != null) ? classifRes.cod.Split('/').FirstOrDefault() : "";
            //DatiProtocollo.ClasseTitolario = (classifRes != null) ? classifRes.cod.Split('/').LastOrDefault() : "";
            //DatiProtocollo.DescrizioneTitolario = (classifRes != null) ? classifRes.Value.Split('-').LastOrDefault() : "";
        }
        return DatiProtocollo;
    }


    private T StaticGetCustomData<T>(CustomDataEnum tipo, object parameter = null)
    {
        object data = null;
        string query = "";
        string xmlResponse = "";
        string jsonDataSoggetti = "";
        switch (tipo)
        {
            // recupera le persone interne tramite nome e cognome
            case CustomDataEnum.PersoneInterneDaCognome:
                var soggettiInterni = new PersoneInterne();
                parameter = parameter.ToString().Replace(".", " ");
                query = "[/persona_interna/#cgnm]=\"*" + parameter.ToString() + "*\"";
                query = (string.IsNullOrEmpty(parameter.ToString())) ? "([UD,/xw/@UdType/]=\"persona_interna\")" : query;
                xmlResponse = AclSearch(query, null);
                soggettiInterni.persone = ExtractFormXmlListNodesOf<persona_interna>(xmlResponse, "persona_interna");
                jsonDataSoggetti = JsonConvert.SerializeObject(soggettiInterni);
                return JsonConvert.DeserializeObject<T>(jsonDataSoggetti);

            case CustomDataEnum.PersoneInterneDaLogin:
                var loginInterne = new PersoneInterne();
                query = "[/persona_interna/login/@name]=\"" + parameter.ToString() + "\"";
                query = (string.IsNullOrEmpty(parameter.ToString())) ? "([UD,/xw/@UdType/]=\"persona_interna\")" : query;
                xmlResponse = AclSearch(query, null);
                loginInterne.persone = ExtractFormXmlListNodesOf<persona_interna>(xmlResponse, "persona_interna");
                jsonDataSoggetti = JsonConvert.SerializeObject(loginInterne);
                return JsonConvert.DeserializeObject<T>(jsonDataSoggetti);

            // recupera le persone interne tramite matricola
            case CustomDataEnum.PersoneInterneDaMatricola:
                var personeInterne = new PersoneInterne();
                query = "[/persona_interna/@matricola]=\"" + parameter.ToString() + "\"";
                query = (string.IsNullOrEmpty(parameter.ToString())) ? "([UD,/xw/@UdType/]=\"persona_interna\")" : query;
                xmlResponse = AclSearch(query, null);
                personeInterne.persone = ExtractFormXmlListNodesOf<persona_interna>(xmlResponse, "persona_interna");
                jsonDataSoggetti = JsonConvert.SerializeObject(personeInterne);
                return JsonConvert.DeserializeObject<T>(jsonDataSoggetti);

            // recupera le strutture interne tramite codice ufficio
            case CustomDataEnum.StruttureInterneDaCodice:
                var struttureInterne = new StruttureInterne();
                query = "[/struttura_interna/@cod_uff]=\"" + parameter.ToString() + "\"";
                query = (string.IsNullOrEmpty(parameter.ToString())) ? "([UD,/xw/@UdType/]=\"struttura_interna\")" : query;
                xmlResponse = AclSearch(query, null);
                struttureInterne.strutture = ExtractFormXmlListNodesOf<struttura_interna>(xmlResponse, "struttura_interna");
                jsonDataSoggetti = JsonConvert.SerializeObject(struttureInterne);
                return JsonConvert.DeserializeObject<T>(jsonDataSoggetti);

            // recupera le strutture interne tramite codice ufficio
            case CustomDataEnum.StruttureInterneDaNome:
                var struttureInterne2 = new StruttureInterne();
                query = "[/struttura_interna/nome]=\"" + parameter.ToString() + "\"";
                query = (string.IsNullOrEmpty(parameter.ToString())) ? "([UD,/xw/@UdType/]=\"struttura_interna\")" : query;
                xmlResponse = AclSearch(query, null);
                struttureInterne2.strutture = ExtractFormXmlListNodesOf<struttura_interna>(xmlResponse, "struttura_interna");
                jsonDataSoggetti = JsonConvert.SerializeObject(struttureInterne2);
                return JsonConvert.DeserializeObject<T>(jsonDataSoggetti);

            // recupera una persona esterne tramite matricola
            case CustomDataEnum.PersonaEsternaDaMatricola:
                var personaEsterna = new persona_esterna();
                query = "[/persona_esterna/@matricola]=\"" + parameter.ToString() + "\"";
                //query = (string.IsNullOrEmpty(parameter.ToString())) ? "([UD,/xw/@UdType/]=\"persona_esterna\")" : query;
                xmlResponse = AclSearch(query, null);
                personaEsterna = ExtractFormXmlListNodesOf<persona_esterna>(xmlResponse, "persona_esterna").FirstOrDefault();
                jsonDataSoggetti = JsonConvert.SerializeObject(personaEsterna);
                return JsonConvert.DeserializeObject<T>(jsonDataSoggetti);

            // recupera una persona esterne tramite matricola
            case CustomDataEnum.PersonaEsternaDaCodiceFiscale:
                var personaEsternaCF = new persona_esterna();
                query = "[/persona_esterna/@codice_fiscale]=\"" + parameter.ToString() + "\"";
                //query = (string.IsNullOrEmpty(parameter.ToString())) ? "([UD,/xw/@UdType/]=\"persona_esterna\")" : query;
                xmlResponse = AclSearch(query, null);
                personaEsternaCF = ExtractFormXmlListNodesOf<persona_esterna>(xmlResponse, "persona_esterna").FirstOrDefault();
                jsonDataSoggetti = JsonConvert.SerializeObject(personaEsternaCF);
                return JsonConvert.DeserializeObject<T>(jsonDataSoggetti);


            // recupera le persone esterne tramite cognome e nome
            case CustomDataEnum.PersoneEsterneDaCognome:
                var soggettiTitulus = new PersoneEsterne();
                parameter = parameter.ToString().Replace(".", " ");
                query = "[/persona_esterna/#cgnm]=\"*" + parameter.ToString() + "*\"";
                query = (string.IsNullOrEmpty(parameter.ToString())) ? "([UD,/xw/@UdType/]=\"persona_esterna\")" : query;
                xmlResponse = AclSearch(query, null);
                soggettiTitulus.persone = ExtractFormXmlListNodesOf<persona_esterna>(xmlResponse, "persona_esterna");
                jsonDataSoggetti = JsonConvert.SerializeObject(soggettiTitulus);
                return JsonConvert.DeserializeObject<T>(jsonDataSoggetti);

            // recupera le strutture esterne tramite codice ufficio
            case CustomDataEnum.StruttureEsterneDaNome:
                var struttureEsterne = new StruttureEsterne();
                query = "[/struttura_esterna/nome]=\"*" + parameter.ToString() + "*\"";
                query = (string.IsNullOrEmpty(parameter.ToString())) ? "([UD,/xw/@UdType/]=\"struttura_esterna\")" : query;
                xmlResponse = AclSearch(query, null);
                struttureEsterne.strutture = ExtractFormXmlListNodesOf<struttura_esterna>(xmlResponse, "struttura_esterna");
                jsonDataSoggetti = JsonConvert.SerializeObject(struttureEsterne);
                return JsonConvert.DeserializeObject<T>(jsonDataSoggetti);

            case CustomDataEnum.StruttureEsterneDaCodiceFiscale:
                var struttureEsterneCF = new StruttureEsterne();
                query = "[/struttura_esterna/@codice_fiscale]=\"" + parameter.ToString() + "\"";
                query = (string.IsNullOrEmpty(parameter.ToString())) ? "([UD,/xw/@UdType/]=\"struttura_esterna\")" : query;
                xmlResponse = AclSearch(query, null);
                struttureEsterneCF.strutture = ExtractFormXmlListNodesOf<struttura_esterna>(xmlResponse, "struttura_esterna");
                jsonDataSoggetti = JsonConvert.SerializeObject(struttureEsterneCF);
                return JsonConvert.DeserializeObject<T>(jsonDataSoggetti);

            case CustomDataEnum.StruttureEsterneDaCodice:
                var struttureEsterneCod = new StruttureEsterne();
                query = "[/struttura_esterna/@cod_uff]=\"" + parameter.ToString() + "\"";
                query = (string.IsNullOrEmpty(parameter.ToString())) ? "([UD,/xw/@UdType/]=\"struttura_esterna\")" : query;
                xmlResponse = AclSearch(query, null);
                struttureEsterneCod.strutture = ExtractFormXmlListNodesOf<struttura_esterna>(xmlResponse, "struttura_esterna");
                jsonDataSoggetti = JsonConvert.SerializeObject(struttureEsterneCod);
                return JsonConvert.DeserializeObject<T>(jsonDataSoggetti);

            case CustomDataEnum.IndiceTitolario:
                var IndiceTitolario = new IndiceTitolari();
                query = "[/indice_titolario/@voce]=\"" + parameter.ToString() + "\"";
                query = (string.IsNullOrEmpty(parameter.ToString())) ? "([UD,/xw/@UdType/]=\"indice_titolario\")" : query;
                xmlResponse = AclSearch(query, null);
                IndiceTitolario.Titolari = ExtractFormXmlListNodesOf<indice_titolario>(xmlResponse, "indice_titolario");
                jsonDataSoggetti = JsonConvert.SerializeObject(IndiceTitolario);
                return JsonConvert.DeserializeObject<T>(jsonDataSoggetti);


            case CustomDataEnum.Repertori:
                var listaRepertori = new List<Repertorio>();
                listaRepertori.Add(new Repertorio() { Codice = "ALBO", Descrizione = "Albo ufficiale di AOUP", Tipologia = "I" });
                listaRepertori.Add(new Repertorio() { Codice = "CMZ", Descrizione = "Comunicazione", Tipologia = "V" });
                listaRepertori.Add(new Repertorio() { Codice = "CONTR", Descrizione = "Contratti", Tipologia = "V" });
                listaRepertori.Add(new Repertorio() { Codice = "DDG", Descrizione = "Delibere Direttore Generale", Tipologia = "U" });
                listaRepertori.Add(new Repertorio() { Codice = "DxCdA", Descrizione = "Deliberazioni", Tipologia = "X" });
                listaRepertori.Add(new Repertorio() { Codice = "DURC", Descrizione = "DURC", Tipologia = "I" });
                listaRepertori.Add(new Repertorio() { Codice = "PRP", Descrizione = "Proposta", Tipologia = "X" });
                listaRepertori.Add(new Repertorio() { Codice = "RdV", Descrizione = "Rapporto di versamento", Tipologia = "I" });
                listaRepertori.Add(new Repertorio() { Codice = "MandatiPRE", Descrizione = "Ordinativi informatici pregressi - Mandati", Tipologia = "V" });
                listaRepertori.Add(new Repertorio() { Codice = "GDCP", Descrizione = "Giornali di cassa pregressi", Tipologia = "V" });
                listaRepertori.Add(new Repertorio() { Codice = "RIPG", Descrizione = "Registro informatico giornaliero di protocollo", Tipologia = "V" });
                var jsonDataLista = JsonConvert.SerializeObject(listaRepertori);
                return JsonConvert.DeserializeObject<T>(jsonDataLista);
        }
        return (T)(object)data;
    }

    private List<T> ExtractFormXmlListNodesOf<T>(string xmlResponse, string tagName)
    {
        List<T> items = new List<T>();
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlResponse);
            XmlNodeList TNodes = xmlDoc.GetElementsByTagName(tagName);
            foreach (XmlNode node in TNodes)
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    T item = (T)serializer.Deserialize(new XmlNodeReader(node));
                    items.Add(item);
                }
                catch (Exception) { }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Errore nell'estrazione delle {tagName} dall'XML: {ex.Message}");
        }

        return items;
    }

    //private string Search(string query, string orderby)
    //{
    //    return client.search(query, orderby, null);
    //}
    private string AclSearch(string query, string orderby, int OrderBy = 20)
    {
        var queryParams = new global::TitulusIntegration.TitulusACL.QueryParams() { titlePageSize = OrderBy };
        return ACL.search(query, orderby, queryParams);
    }


    private XElement GeneraElementoRifEsterno(
        string dataProt = "",
        string numeroProt = "",
        string nomeCod = "",
        string nomePreserve = "",
        string referenteCod = "",
        string referenteNominativo = "",
        string indirizzoEmail = "",
        string indirizzoEmailCert = "",
        string indirizzoFax = "",
        string indirizzoTel = "",
        string indirizzoPreserve = "",
        bool copiaConoscenza = false)
    {
        XNamespace xml = "http://www.w3.org/XML/1998/namespace";
        XElement rifEsterno = new XElement("rif_esterno");
        if (copiaConoscenza)
        {
            rifEsterno.Add(new XAttribute("copia_conoscenza", "si"));
        }
        if (!string.IsNullOrEmpty(dataProt))
        {
            rifEsterno.Add(new XAttribute("data_prot", dataProt));
        }
        if (!string.IsNullOrEmpty(numeroProt))
        {
            rifEsterno.Add(new XAttribute("n_prot", numeroProt));
        }
        if (!string.IsNullOrEmpty(nomeCod) && !string.IsNullOrEmpty(nomePreserve))
        {
            rifEsterno.Add(new XElement("nome",
                new XAttribute("cod", nomeCod),
                new XAttribute(xml + "space", "preserve"),
                nomePreserve
            ));
        }
        if (!string.IsNullOrEmpty(referenteCod) && !string.IsNullOrEmpty(referenteNominativo))
        {
            rifEsterno.Add(new XElement("referente",
                new XAttribute("cod", referenteCod),
                new XAttribute("nominativo", referenteNominativo)
            ));
        }

        if (!string.IsNullOrEmpty(indirizzoEmail) || !string.IsNullOrEmpty(indirizzoEmailCert) ||
            !string.IsNullOrEmpty(indirizzoFax) || !string.IsNullOrEmpty(indirizzoTel) ||
            !string.IsNullOrEmpty(indirizzoPreserve))
        {
            XElement indirizzoElement = new XElement("indirizzo");
            if (!string.IsNullOrEmpty(indirizzoEmail))
            {
                indirizzoElement.Add(new XAttribute("email", indirizzoEmail));
            }
            if (!string.IsNullOrEmpty(indirizzoEmailCert))
            {
                indirizzoElement.Add(new XAttribute("email_certificata", indirizzoEmailCert));
            }
            if (!string.IsNullOrEmpty(indirizzoFax))
            {
                indirizzoElement.Add(new XAttribute("fax", indirizzoFax));
            }
            if (!string.IsNullOrEmpty(indirizzoTel))
            {
                indirizzoElement.Add(new XAttribute("tel", indirizzoTel));
            }
            if (!string.IsNullOrEmpty(indirizzoPreserve))
            {
                indirizzoElement.Add(new XAttribute(xml + "space", "preserve"));
                indirizzoElement.Value = indirizzoPreserve;
            }
            rifEsterno.Add(indirizzoElement);
        }
        return rifEsterno;
    }

    private XElement GeneraElementoRifInterno(
        string diritto,
        string codPersona,
        string codUfficio,
        string nomePersona,
        string nomeUfficio)
    {
        XElement rifInterno = new XElement("rif_interno",
            new XAttribute("diritto", diritto),
            new XAttribute("cod_uff", codUfficio),
            new XAttribute("nome_uff", nomeUfficio),
            new XAttribute("cod_persona", codPersona),
            new XAttribute("nome_persona", nomePersona)
        );

        return rifInterno;
    }

    private XElement GeneraElementoMinuta(string classifCod,
        string classifValue,
        string nomePersona,
        string nomeUff,
        string codPersona,
        string codUff)
    {
        XNamespace xml = "http://www.w3.org/XML/1998/namespace";
        XElement minuta = new XElement("minuta",
                    new XAttribute("scarto", "99"),
                    new XElement("classif",
                        new XAttribute("cod", classifCod),
                        new XAttribute(xml + "space", "preserve"),
                        classifValue),
                    new XElement("mittente",
                        new XAttribute("nome_persona", nomePersona),
                        new XAttribute("nome_uff", nomeUff),
                        new XAttribute("cod_persona", codPersona),
                        new XAttribute("cod_uff", codUff)));

        return minuta;
    }


    private string GenerateXML(
        bool draft,
        string tipo = "",
        string repertorioCod = "",
        string repertorioValue = "",
        string oggettoValue = "",
        XElement minuta = null,
        string classifCod = "",
        string classifValue = "",
        string tipologiaCod = "",
        string mezzoTrasmissioneCosto = "",
        string mezzoTrasmissioneValuta = "",
        string mezzoTrasmissioneCod = "",
        string noteValue = "",
        string riferimentiValue = "",
        string xlinkHref = "",
        string xlinkValue = "",
        string allegatoValue = "",
        string voceIndiceValue = "",
        //string nomeRifEsterno = "",
        //string codRifEsterno = "", 
        //string indirizzoRifEsterno = "",
        //string codReferente = "",
        //string nominativoReferente = "", 
        //string copiaConoscenza = "", 
        //string codRifEsternoCC = "",
        //string nominativoReferenteCC = "", 
        List<XElement> rifInterniList = null,
        List<XElement> rifEsterniList = null)
    {
        XNamespace xml = "http://www.w3.org/XML/1998/namespace";
        XElement doc = new XElement("doc",
            new XAttribute("tipo", tipo),
            new XAttribute("bozza", draft ? "si" : "no")
        );

        if (!string.IsNullOrEmpty(repertorioCod) && !string.IsNullOrEmpty(repertorioValue))
        {
            doc.Add(new XElement("repertorio",
                new XAttribute("cod", repertorioCod),
                repertorioValue
            ));
        }
        if (minuta != null)
        {
            doc.Add(minuta);
        }
        if (!string.IsNullOrEmpty(oggettoValue))
        {
            doc.Add(new XElement("oggetto", oggettoValue));
        }
        if (!string.IsNullOrEmpty(classifCod) && !string.IsNullOrEmpty(classifValue))
        {
            doc.Add(new XElement("classif",
                        new XAttribute("cod", classifCod),
                        new XAttribute(xml + "space", "preserve"),
                        classifValue));
        }
        if (!string.IsNullOrEmpty(tipologiaCod))
        {
            doc.Add(new XElement("tipologia",
                new XAttribute("cod", tipologiaCod)
            ));
        }
        if (!string.IsNullOrEmpty(mezzoTrasmissioneCod))
        {
            doc.Add(new XElement("mezzo_trasmissione",
                new XAttribute("costo", mezzoTrasmissioneCosto),
                new XAttribute("valuta", mezzoTrasmissioneValuta),
                new XAttribute("cod", mezzoTrasmissioneCod)
            ));
        }
        if (!string.IsNullOrEmpty(noteValue))
        {
            doc.Add(new XElement("note", noteValue));
        }
        if (!string.IsNullOrEmpty(riferimentiValue))
        {
            doc.Add(new XElement("riferimenti",
                new XAttribute(xml + "space", "preserve"),
                riferimentiValue
            ));
        }
        if (!string.IsNullOrEmpty(xlinkHref) && !string.IsNullOrEmpty(xlinkValue))
        {
            doc.Add(new XElement("xlink",
                new XAttribute("href", xlinkHref),
                new XAttribute(xml + "space", "preserve"),
                xlinkValue
            ));
        }
        if (!string.IsNullOrEmpty(allegatoValue))
        {
            doc.Add(new XElement("allegato", allegatoValue));
        }
        if (!string.IsNullOrEmpty(voceIndiceValue))
        {
            doc.Add(new XElement("voce_indice",
                new XAttribute(xml + "space", "preserve"),
                voceIndiceValue
            ));
        }
        if (tipo.Equals("arrivo"))
        {

            if (rifInterniList != null && rifInterniList.Count > 0)
            {
                XElement rifInterni = new XElement("rif_interni");
                foreach (XElement i in rifInterniList)
                {
                    rifInterni.Add(i);
                }
                doc.Add(rifInterni);
            }
            if (rifEsterniList != null && rifEsterniList.Count > 0)
            {
                XElement rifEsterni = new XElement("rif_esterni");
                foreach (XElement i in rifEsterniList)
                {
                    rifEsterni.Add(i);
                }
                doc.Add(rifEsterni);
            }
        }
        else
        {
            if (rifEsterniList != null && rifEsterniList.Count > 0)
            {
                XElement rifEsterni = new XElement("rif_esterni");
                foreach (XElement i in rifEsterniList)
                {
                    rifEsterni.Add(i);
                }
                doc.Add(rifEsterni);
            }
            if (rifInterniList != null && rifInterniList.Count > 0)
            {
                XElement rifInterni = new XElement("rif_interni");
                foreach (XElement i in rifInterniList)
                {
                    rifInterni.Add(i);
                }
                doc.Add(rifInterni);
            }
        }
        XDocument xmlDocument = new XDocument(new XDeclaration("1.0", "iso-8859-1" /*"utf -8"*/, "yes"), doc);
        return xmlDocument.ToString();
    }

}
