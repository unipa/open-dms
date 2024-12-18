using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenDMS.TitulusIntegration.API.BL.Interfacce;
using OpenDMS.TitulusIntegration.API.Models;
using OpenDMS.TitulusIntegration.API.Models.LoadDocument;
using OpenDMS.TitulusIntegration.API.Models.SearchDocument;
using OpenDMS.TitulusIntegration.API.Utility;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;
using File = OpenDMS.TitulusIntegration.API.Models.LoadDocument.File;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace OpenDMS.TitulusIntegration.API.BL
{
    public class TitulusBL : ITitulusBL
    {
        private readonly IConfiguration _config;
        public TitulusBL(IConfiguration config)
        {
            _config = config;
        }


        //Metodi utili
        public string GetCorrectHost(string url)
        {
            Uri uri = new Uri(url);
            string host = uri.Host;
            string textAfterHost = url.Substring(url.IndexOf(host) + host.Length);
            string url_whit_correct_host = _config["TITULUS_HOST"] + textAfterHost;
            return url_whit_correct_host;
        }

        public string RemoveNameSpace(string xml,string prefix)
        {
            xml = xml.Replace("xmlns:"+ prefix, "xmlns"); // Rimuove il prefisso xw dal namespace
            string xml_whitout_namespace = xml.Replace(prefix+":", "");

            return xml_whitout_namespace;
        }

        public string XmlToJson(string xml,Type xmlType)
        {
            string json = "";
            try
            {
                XmlSerializer serializer = new XmlSerializer(xmlType);
                using (var reader = new System.IO.StringReader(xml)) 
                {
                    var response= new Object();
                    if (xmlType == typeof(Response))
                    {
                        response = (Response)serializer.Deserialize(reader);
                        var typizedResponse = (Response)response;
                        if (typizedResponse.Url != null)
                        {
                            typizedResponse.Url = GetCorrectHost(typizedResponse.Url);
                        }
                    }
                    if (xmlType == typeof(ResponseSearch))
                    {
                        response = (ResponseSearch)serializer.Deserialize(reader);
                    }
                    if (xmlType == typeof(ResponseNewDocument))
                    {
                        response = (ResponseNewDocument)serializer.Deserialize(reader);
                    }


                    // Access other properties as needed
                    json = JsonConvert.SerializeObject(response);
                }
                return json;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore nella conversione da XML a JSON, errore: " + ex.Message);
            }
        }

        public string EncodeNumProt(string input)
        {
            string result = "";
            string secondPart = "";
            string thirdPart = "";

            if (input.Contains("/"))
            {
                string[] parts = input.Split('/');
                secondPart = parts[1];
                thirdPart = parts[0].PadLeft(7, '0');

                if (parts.Length != 2 || (secondPart.StartsWith("UNPACLE-") && thirdPart.Length == 7))
                {
                    throw new ArgumentException("Input non valido. Il formato richiesto è X/Y oppure ANNO-UNPACLE-YYYYYYY.");
                }
                result = $"{secondPart}-UNPACLE-{thirdPart}";
            }
            else if(input.Contains("-"))
            {
                string[] parts = input.Split('-');
                string firstPart = parts[0];
                secondPart = parts[1];
                thirdPart = parts[2];
                if ( firstPart.Length == 4 && secondPart.StartsWith("UNPACLE") && thirdPart.Length == 7)
                {
                    result = input; // Restituisce l'input senza modifiche
                }
            }
            else
            {
                throw new Exception("Il numero di protocollo non è nel formato corretto! (ID/ANNO oppure ANNO-CodAoo-ID-7-CARATTERI)");
            }

            return result;
        }

        //Metodi per chiamare i WebService titulus

        public string GetFolderMetadada(long id)
        {
            string url = _config["Services:titulus4_services_url"];
            string username = _config["Services:username"];
            string password = _config["Services:password"];

            try
            {
                string xml = @"<soapenv:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:ws='http://ws.titulus.kion.it'>
                               <soapenv:Header/>
                               <soapenv:Body>
                                  <ws:getFolder soapenv:encodingStyle='http://schemas.xmlsoap.org/soap/encoding/'>
                                     <id xsi:type='soapenc:string' xmlns:soapenc='http://schemas.xmlsoap.org/soap/encoding/'>" + id + @"</id>
                                  </ws:getFolder>
                               </soapenv:Body>
                            </soapenv:Envelope>"; 
                string response = Soaphandler.SoapCall(xml,url,username,password);
                return (response);
            }
            catch (Exception ex)
            {
                throw new Exception("Errore nella chiamata soap, errore: " + ex.Message);
            }
        }

        public string Search(string query, string? orderby)
        {
            string url = _config["Services:titulus4_services_url"];
            string username = _config["Services:username"];
            string password = _config["Services:password"];

            try
            {
                string xml = @"<soapenv:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:ws='http://ws.titulus.kion.it'>
                                   <soapenv:Header/>
                                   <soapenv:Body>
                                      <ws:search soapenv:encodingStyle='http://schemas.xmlsoap.org/soap/encoding/'>
                                         <query xsi:type ='soapenc:string' xmlns:soapenc='http://schemas.xmlsoap.org/soap/encoding/'>" + query +@"</query>
                                         <orderby xsi:type='soapenc:string' xmlns:soapenc='http://schemas.xmlsoap.org/soap/encoding/'>"+ orderby +@"</orderby>
                                         <params xsi:type='tit:QueryParams' xmlns:tit='http://www.kion.it/titulus'>
                                            <fields xsi:type ='tit1:ArrayOf_soapenc_string' soapenc:arrayType='soapenc:string[]' xmlns:tit1='"+url+@"' xmlns:soapenc='http://schemas.xmlsoap.org/soap/encoding/'/>
                                         </params>
                                      </ws:search>
                                   </soapenv:Body>
                                </soapenv:Envelope>";

                string response = Soaphandler.SoapCall(xml, url, username, password);

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(response);
                XmlNode responseXML = xmldoc.GetElementsByTagName("ns1:searchResponse")[0];

                if (responseXML != null)
                {
                    return (responseXML.InnerText);
                }
                else
                {
                    throw new Exception("Non esiste alcun risultato per questa query");
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception("Errore nella chiamata soap, errore: " + ex.Message);
            }
        }

        public string LoadDocument(long physdoc)
        {
            string url = _config["Services:titulus4_services_url"];
            string username = _config["Services:username"];
            string password = _config["Services:password"];

            try
            {
                string xml = @"<soapenv:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ws=""http://ws.titulus.kion.it"">
                                   <soapenv:Header/>
                                   <soapenv:Body>
                                      <ws:loadDocument soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                                         <id xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">"+physdoc+ @"</id>
                                         <lock xsi:type=""xsd:boolean"">false</lock>
                                      </ws:loadDocument>
                                   </soapenv:Body>
                                </soapenv:Envelope>";

                string response = Soaphandler.SoapCall(xml, url, username, password);

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(response);
                XmlNode loadDocument = xmldoc.GetElementsByTagName("loadDocumentReturn")[0];

                string xml_whitout_ns = RemoveNameSpace(loadDocument.InnerText, "xw");
                string jsonString = XmlToJson(xml_whitout_ns, typeof(Response));

                return (jsonString);
            }
            catch (Exception ex)
            {
                throw new Exception("Errore nella chiamata soap, errore: " + ex.Message);
            }
        }

        public string GetFileBase64(string fileID)
        {
            string url = _config["Services:titulus4_services_url"];
            string username = _config["Services:username"];
            string password = _config["Services:password"];

            try
            {
                string xml = @"<soapenv:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ws=""http://ws.titulus.kion.it"">
                                   <soapenv:Header/>
                                   <soapenv:Body>
                                      <ws:getAttachment soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                                         <fileId xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">" + fileID + @"</fileId>
                                      </ws:getAttachment>
                                   </soapenv:Body>
                                </soapenv:Envelope>";


                string response = Soaphandler.SoapCall(xml, url, username, password);

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(response);
                XmlNode loadDocument = xmldoc.GetElementsByTagName("content")[0];

                return (loadDocument.InnerText);
            }
            catch (Exception ex)
            {
                throw new Exception("Errore nella chiamata soap, errore: " + ex.Message);
            }
        }

        public string GetDocumentUrl(long physdoc)
        {

            string url = _config["Services:titulus4_services_url"];
            string username = _config["Services:username"];
            string password = _config["Services:password"];

            try
            {
                string xml = @"<soapenv:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ws=""http://ws.titulus.kion.it"">
                                   <soapenv:Header/>
                                   <soapenv:Body>
                                      <ws:getDocumentURL soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                                         <id xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">"+physdoc+@"</id>
                                      </ws:getDocumentURL>
                                   </soapenv:Body>
                                </soapenv:Envelope>";

                string response = Soaphandler.SoapCall(xml, url, username, password);

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(response);
                XmlNode documentUrlXML = xmldoc.GetElementsByTagName("getDocumentURLReturn")[0];

                string documentUrl = GetCorrectHost(documentUrlXML.InnerText);

                return (documentUrl);
            }
            catch (Exception ex)
            {
                throw new Exception("Errore nella chiamata soap, errore: " + ex.Message);
            }
        }

        public string GetDocumentFromProtocol(string numero_protocollo)
        {
            try
            {
                string encoded_num_protocol = EncodeNumProt(numero_protocollo);
                string searchResponse = Search("[docnumprot]=" + encoded_num_protocol, null);

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(searchResponse);
                XmlNode searchedDocument = xmldoc.GetElementsByTagName("Response")[0];
                string xml_whitout_ns = RemoveNameSpace(searchedDocument.OuterXml, "xw");
                string jsonString = XmlToJson(xml_whitout_ns, typeof(ResponseSearch));

                JsonDocument jsonDocument = JsonDocument.Parse(jsonString);
                JsonElement root = jsonDocument.RootElement;
                int pageCount = root.GetProperty("pageCount").GetInt32();

                if(pageCount == 0)
                {
                    throw new Exception("Il file cercato non esiste!");
                }

                JsonDocument document = JsonDocument.Parse(jsonString);
                JsonElement docElement = document.RootElement.GetProperty("Doc");

                // Deserializza direttamente l'oggetto "Doc"
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                DocSearch jsonObject = JsonSerializer.Deserialize<DocSearch>(docElement.ToString(), options);

                if (jsonObject.Files != null)
                {
                    jsonObject = GetFiles(jsonString);
                }

                jsonObject.titulusDocumentURL = GetDocumentUrl(long.Parse(jsonObject.Physdoc));

                string updatedJsonString = JsonSerializer.Serialize(jsonObject);
                jsonDocument.Dispose();
                return (updatedJsonString);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DocSearch GetFiles(string jsonString)
        {

            JsonDocument document = JsonDocument.Parse(jsonString);
            JsonElement docElement = document.RootElement.GetProperty("Doc");

            // Deserializza direttamente l'oggetto "Doc"
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            DocSearch jsonObject = JsonSerializer.Deserialize<DocSearch>(docElement.ToString(), options);

            /*Chiamo LoadDocument per integrare informazioni aggiuntive sul documento*/
            string loadedDocument = LoadDocument(long.Parse(jsonObject.Physdoc));
            dynamic jsonLoadedObject = JsonConvert.DeserializeObject(loadedDocument);
            JArray jsonArray = jsonLoadedObject.Document.Doc.Files.file;
            List<File> filesListLoaded = jsonArray.ToObject<List<File>>();
            /*Fine chiamata a LoadDocument*/

            List<FileSearch> filesList = jsonObject.Files.file.ToList();
            foreach (FileSearch file in filesList)
            {
                // Converte il Base64 in array di byte
                byte[] fileBytes = Convert.FromBase64String(GetFileBase64(file.Name));
                string fileName = file.Title;
                file.Principale = filesListLoaded.Find(item => item.Name == file.Name).Principale;
                // Percorso temporaneo per salvare il file
                string filePath = Path.Combine(_config["TitulusFilePath"], fileName);
                // Salva il file sul disco
                System.IO.File.WriteAllBytes(filePath, fileBytes);
            }

            return jsonObject;
        }

        public string CreateNewDocument(NewDocument doc,bool draft)
        {
            string url = _config["Services:titulus4_services_url"];
            string username = _config["Services:username"];
            string password = _config["Services:password"];
            string bozza = "no";
            if (draft)
            {
                bozza = "si";
            }

            try
            {
                string header = @"<soapenv:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ws=""http://ws.titulus.kion.it"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">
                                    <soapenv:Header/>
                                    <soapenv:Body>
                                    <ws:saveDocument soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                                     <document xsi:type=""soapenc:string"">";

                string open_cdata = @"<![CDATA[<?xml version=""1.0"" encoding=""UTF-8""?>";
                string doc_tag = "<doc tipo='"+doc.Tipo+"' bozza='"+bozza+"' data_prot='"+doc.data_prot+"'>";
                string prot_differito = "<prot_differito data_arrivo='" + doc.data_arrivo + "'>" + doc.commento_prot_differito + "</prot_differito>";
                string oggetto = "<oggetto>" + doc.Oggetto + "</oggetto>";
                string tipologia = "<tipologia cod='" + doc.tipologia_cod +"' />";
                string mezzo_trasmissione = "<mezzo_trasmissione costo='"+ doc.mezzo_trasmissione_costo+"' valuta='"+ doc.mezzo_trasmissione_valuta+"' cod='"+ doc.mezzo_trasmissione_cod +"'></mezzo_trasmissione>";
                string note = "<note>'" + doc.note + "'</note>";
                string riferimenti = "<riferimenti xml:space ='preserve'>'"+doc.riferimenti_innertext+"'</riferimenti>";
                //string allegato = "<allegato> 0 - nessun allegato </allegato>";
                string voce_indice = "<voce_indice xml:space = 'preserve'>" + doc.voce_indice_innertext + "</voce_indice>";
                string rif_esterni =
                                   @"<rif_esterni >
                                        <rif_esterno data_prot = """ + doc.rif_esterno_dataprot + @""" n_prot = """ + doc.rif_esterno_numprot + @""" >
                                            <nome cod = """ + doc.rif_esterno_nome_cod + @""" xml:space = ""preserve"" > " + doc.rif_esterno_nome_innertext + @" </nome >
                                            <referente cod = """ + doc.rif_esterno_referente_cod + @""" nominativo = """ + doc.rif_esterno_referente_nominativo + @""" />
                                            <indirizzo email = """ + doc.rif_esterno_indirizzo_email + @"""
                                                        email_certificata = """ + doc.rif_esterno_indirizzo_email_certificata + @"""
                                                        fax = """ + doc.rif_esterno_fax + @"""
                                                        tel = """ + doc.rif_esterno_tel + @"""
                                                        xml:space = ""preserve"" >
                                                " + doc.rif_esterno_indirizzo_innertext + @"
                                            </indirizzo>
                                        </rif_esterno>
                                    </rif_esterni>";

                string rif_interni =
                                    @"<rif_interni>
                                        <rif_interno diritto = """ + doc.rif_interno_diritto_rpa + @""" cod_persona = """ + doc.rif_interno_codice_persona + @""" cod_uff = """ + doc.rif_interno_codice_ufficio + @""" nome_persona = """ + doc.rif_interno_nome_persona + @""" nome_uff = """ + doc.rif_interno_nome_ufficio + @""" />
                                    </rif_interni>";

                string close_cdata = 
                                    @"</doc>]]>
                                   </document> ";

                string footer = @"
                                     <params xsi:type=""tit:SaveParams"" xmlns:tit=""http://www.kion.it/titulus"">
                                        <pdfConversion xsi:type=""xsd:boolean"">false</pdfConversion>
                                        <sendEMail xsi:type=""xsd:boolean"">false</sendEMail>
                                     </params>
                                  </ws:saveDocument>
                               </soapenv:Body>
                            </soapenv:Envelope>";

                if(doc.Tipo == "partenza")
                {
                    prot_differito = "";
                }
                else if(doc.Tipo == "arrivo" && doc.data_arrivo == null)
                {
                    prot_differito = "";
                }

                string xml = header + open_cdata + doc_tag + prot_differito + oggetto + tipologia + mezzo_trasmissione + note + riferimenti  + voce_indice + rif_esterni + rif_interni + close_cdata;

                if (doc.Attachments != null && doc.Attachments.Count > 0)
                {
                    xml += "<attachmentBeans xsi:type=\"tit:ArrayOf_tns1_AttachmentBean\" soapenc:arrayType=\"tit1:AttachmentBean[]\" xmlns:tit=\""+url+ "\" xmlns:tit1=\"http://www.kion.it/titulus\">";

                    int counter = 0;
                    foreach (var attachment in doc.Attachments)
                    {
                        xml += $@"
                                <multiRef id=""id{counter}"" soapenc:root=""0"" soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"" xsi:type=""ns2:AttachmentBean"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"" xmlns:ns2=""http://www.kion.it/titulus"">
                                    <content xsi:type=""soapenc:base64Binary"">{attachment.Content}</content>
                                    <description xsi:type=""soapenc:string"" xsi:nil=""true"">{attachment.Description}</description>
                                    <id xsi:type=""soapenc:string"">{attachment.Id}</id>
                                    <fileName xsi:type=""soapenc:string"">{attachment.FileName}</fileName>
                                    <mimeType xsi:type=""soapenc:string"">{attachment.MimeType}</mimeType>
                                </multiRef>";
                        counter++;
                    }
                    xml += "</attachmentBeans>";
                    xml += footer;
                }

                string response = Soaphandler.SoapCall(xml, url, username, password);

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(response);
                XmlNode loadDocument = xmldoc.GetElementsByTagName("saveDocumentReturn")[0];

                string xml_whitout_ns = RemoveNameSpace(loadDocument.InnerText, "xw");
                string jsonString = XmlToJson(xml_whitout_ns, typeof(ResponseNewDocument));

                return (jsonString);
            }
            catch (Exception ex)
            {
                throw new Exception("Errore nella chiamata soap, errore: " + ex.Message);
            }
        }


        /// <summary>
        /// Crea un protocollo in ingresso
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="draft"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string CreateInboundDocument(NewDocument doc, bool draft)
        {
            string url = _config["Services:titulus4_services_url"];
            string username = _config["Services:username"];
            string password = _config["Services:password"];
            string bozza = "no";
            if (draft)
            {
                bozza = "si";
            }

            try
            {
                string header = @"<soapenv:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ws=""http://ws.titulus.kion.it"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">
                                    <soapenv:Header/>
                                    <soapenv:Body>
                                    <ws:saveDocument soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                                     <document xsi:type=""soapenc:string"">";

                string open_cdata = @"<![CDATA[<?xml version=""1.0"" encoding=""UTF-8""?>";
                string doc_tag = "<doc tipo='" + doc.Tipo + "' bozza='" + bozza + "' data_prot='" + doc.data_prot + "'>";
                string prot_differito = "<prot_differito data_arrivo='" + doc.data_arrivo + "'>" + doc.commento_prot_differito + "</prot_differito>";
                string oggetto = "<oggetto>" + doc.Oggetto + "</oggetto>";
                string tipologia = "<tipologia cod='" + doc.tipologia_cod + "' />";
                string mezzo_trasmissione = "<mezzo_trasmissione costo='" + doc.mezzo_trasmissione_costo + "' valuta='" + doc.mezzo_trasmissione_valuta + "' cod='" + doc.mezzo_trasmissione_cod + "'></mezzo_trasmissione>";
                string note = "<note>'" + doc.note + "'</note>";
                string riferimenti = "<riferimenti xml:space ='preserve'>'" + doc.riferimenti_innertext + "'</riferimenti>";
                //string allegato = "<allegato> 0 - nessun allegato </allegato>";
                string voce_indice = "<voce_indice xml:space = 'preserve'>" + doc.voce_indice_innertext + "</voce_indice>";
                string rif_esterni =
                                   @"<rif_esterni >
                                        <rif_esterno data_prot = """ + doc.rif_esterno_dataprot + @""" n_prot = """ + doc.rif_esterno_numprot + @""" >
                                            <nome cod = """ + doc.rif_esterno_nome_cod + @""" xml:space = ""preserve"" > " + doc.rif_esterno_nome_innertext + @" </nome >
                                            <referente cod = """ + doc.rif_esterno_referente_cod + @""" nominativo = """ + doc.rif_esterno_referente_nominativo + @""" />
                                            <indirizzo email = """ + doc.rif_esterno_indirizzo_email + @"""
                                                        email_certificata = """ + doc.rif_esterno_indirizzo_email_certificata + @"""
                                                        fax = """ + doc.rif_esterno_fax + @"""
                                                        tel = """ + doc.rif_esterno_tel + @"""
                                                        xml:space = ""preserve"" >
                                                " + doc.rif_esterno_indirizzo_innertext + @"
                                            </indirizzo>
                                        </rif_esterno>
                                    </rif_esterni>";

                string rif_interni =
                                    @"<rif_interni>
                                        <rif_interno diritto = """ + doc.rif_interno_diritto_rpa + @""" cod_persona = """ + doc.rif_interno_codice_persona + @""" cod_uff = """ + doc.rif_interno_codice_ufficio + @""" nome_persona = """ + doc.rif_interno_nome_persona + @""" nome_uff = """ + doc.rif_interno_nome_ufficio + @""" />
                                    </rif_interni>";

                string close_cdata =
                                    @"</doc>]]>
                                   </document> ";

                string footer = @"
                                     <params xsi:type=""tit:SaveParams"" xmlns:tit=""http://www.kion.it/titulus"">
                                        <pdfConversion xsi:type=""xsd:boolean"">false</pdfConversion>
                                        <sendEMail xsi:type=""xsd:boolean"">false</sendEMail>
                                     </params>
                                  </ws:saveDocument>
                               </soapenv:Body>
                            </soapenv:Envelope>";

                if (doc.Tipo == "partenza")
                {
                    prot_differito = "";
                }
                else if (doc.Tipo == "arrivo" && doc.data_arrivo == null)
                {
                    prot_differito = "";
                }

                string xml = header + open_cdata + doc_tag + prot_differito + oggetto + tipologia + mezzo_trasmissione + note + riferimenti + voce_indice + rif_esterni + rif_interni + close_cdata;

                if (doc.Attachments != null && doc.Attachments.Count > 0)
                {
                    xml += "<attachmentBeans xsi:type=\"tit:ArrayOf_tns1_AttachmentBean\" soapenc:arrayType=\"tit1:AttachmentBean[]\" xmlns:tit=\""+url+"\" xmlns:tit1=\"http://www.kion.it/titulus\">";

                    int counter = 0;
                    foreach (var attachment in doc.Attachments)
                    {
                        xml += $@"
                                <multiRef id=""id{counter}"" soapenc:root=""0"" soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"" xsi:type=""ns2:AttachmentBean"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"" xmlns:ns2=""http://www.kion.it/titulus"">
                                    <content xsi:type=""soapenc:base64Binary"">{attachment.Content}</content>
                                    <description xsi:type=""soapenc:string"" xsi:nil=""true"">{attachment.Description}</description>
                                    <id xsi:type=""soapenc:string"">{attachment.Id}</id>
                                    <fileName xsi:type=""soapenc:string"">{attachment.FileName}</fileName>
                                    <mimeType xsi:type=""soapenc:string"">{attachment.MimeType}</mimeType>
                                </multiRef>";
                        counter++;
                    }
                    xml += "</attachmentBeans>";
                    xml += footer;
                }

                string response = Soaphandler.SoapCall(xml, url, username, password);

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(response);
                XmlNode loadDocument = xmldoc.GetElementsByTagName("saveDocumentReturn")[0];

                string xml_whitout_ns = RemoveNameSpace(loadDocument.InnerText, "xw");
                string jsonString = XmlToJson(xml_whitout_ns, typeof(ResponseNewDocument));

                return (jsonString);
            }
            catch (Exception ex)
            {
                throw new Exception("Errore nella chiamata soap, errore: " + ex.Message);
            }
        }


    }
}

