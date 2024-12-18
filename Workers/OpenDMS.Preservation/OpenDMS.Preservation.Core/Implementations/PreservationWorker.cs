using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Core.Utilities;
using OpenDMS.Domain.DigitalSignature;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Infrastructure;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using OpenDMS.Preservation.Core.Interfaces;
using OpenDMS.Preservation.Core.Models;
using OpenDMS.Preservation.Core.Models.API_Responses;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using static OpenDMS.Preservation.Core.Models.LegalDocIndex;

namespace OpenDMS.Preservation.Core.Implementations
{
    public class PreservationWorker : IPreservationWorker
    {

        private readonly ILogger _logger;
        private readonly IDocumentRepository _docRepo;
        private readonly IDocumentService _docService;
        private readonly IConfiguration _config;
        private readonly IVirtualFileSystem _fs;
        private readonly IVirtualFileSystemProvider _fp;
        private readonly ILoggedUserProfile _pf;
        private readonly IUserService _userService;
        private readonly IUserTaskService _taskService;

        public PreservationWorker(IConfiguration configuration,
            ILogger<PreservationWorker> logger,
            IDocumentRepository docRepo,
            IVirtualFileSystem virtualFileSystem,
            IVirtualFileSystemProvider virtualFileSystemProvider,
            IDocumentService docService,
            ILoggedUserProfile pf,
            IUserService userService,
            IUserTaskService taskService)
        {
            _config = configuration;
            _logger = logger;
            _docRepo = docRepo;
            _fs = virtualFileSystem;
            _fp = virtualFileSystemProvider;
            _docService = docService;
            _pf = pf;
            _userService = userService;
            _taskService = taskService;
        }
        public async Task<LoginResponse> Login(string utentiResponsabili)
        {
            LoginResponse lgResp = null;
            try
            {
                String Uid = _config.GetSection("CS:Auth:Username").Value;
                String Pwd = _config.GetSection("CS:Auth:Password").Value;
                String Endpoint = _config.GetSection("CS:Auth:Url").Value;


                var client = new RestClient(Endpoint);
                var request = new RestRequest("session", Method.Post);
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("application/x-www-form-urlencoded", $"userid={Uid}&password={Pwd}", ParameterType.RequestBody);
                //request.AddParameter("application/x-www-form-urlencoded", $"userid={Uid}&password={Pwd}&idBucket={idBucket}&policyId={policyId}", ParameterType.RequestBody);
                var response = client.Execute(request);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    _logger.LogCritical(response.Content);
                    await GenerateErrorTask(
                     "Errore conservazione Infocert",
                     response.Content,
                     utentiResponsabili
                    );

                    return null;
                }
                lgResp = JsonConvert.DeserializeObject<LoginResponse>(response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError("Preservation Login EXCEPTION: " + ex.Message);
                await GenerateErrorTask(
                     "Errore conservazione Infocert",
                     "Preservation Login EXCEPTION: " + ex.Message,
                     utentiResponsabili
                    );
            }
            return lgResp;
        }
        public async Task<bool> Logout(string SessionID, string utentiResponsabili)
        {
            bool resp = false;
            try
            {
                String Endpoint = _config.GetSection("CS:Auth:Url").Value;
                var client = new RestClient(Endpoint);
                var request = new RestRequest("session", Method.Delete);
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("ldsessionid", SessionID);
                //request.AddParameter("application/x-www-form-urlencoded", $"ldsessionid={SessionID}", ParameterType.RequestBody);
                var response = client.Execute(request);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    _logger.LogCritical(response.Content);
                    resp = false;
                }
                resp = true;

            }
            catch (Exception ex)
            {
                _logger.LogError("Preservation Logout EXCEPTION: " + ex.Message);
                await GenerateErrorTask(
                      "Errore conservazione Infocert",
                      "Preservation Logout EXCEPTION: " + ex.Message,
                      utentiResponsabili
                      );
            }
            return resp;
        }
        public async Task<bool> Preservation(string SessionID, string pdvCode, List<int> Docs, TypeConf typeConf, bool fromWS = false)
        {
            bool isSuccess = true;
            try
            {
                String Endpoint = _config.GetSection("CS:Auth:Url").Value;
                String idBucket = _config.GetSection("CS:Auth:Bucket").Value;
                Dictionary<int, string> errors = new Dictionary<int, string>();
                foreach (var idDoc in Docs)
                {
                    try
                    {
                        var doc = await _docRepo.GetById(idDoc);
                        if (doc == null || doc.ImageId == null)
                        {
                            _logger.LogError($"Impossibile recuperare documento o immagine per iddoc {idDoc}");
                            errors.Add(idDoc, "Impossibile recuperare documento o immagine");
                            continue;
                        }
                        var image = await _docRepo.GetImage(Convert.ToInt32(doc.ImageId));
                        image.PreservationStatus = Domain.Enumerators.JobStatus.Running;
                        if (await _docRepo.UpdateImage(image) <= 0)
                        {
                            _logger.LogError($"Impossibile aggiornare stato immagine doc {idDoc}");
                            errors.Add(idDoc, $"Impossibile aggiornare stato immagine doc");
                            continue;
                        }
                        var fileDict = await xmlToDataMapping(typeConf.Metadati, doc, image, typeConf.MimeType, typeConf.TypeName);
                        if (fileDict != null)
                        {
                            HttpClient httpClient = new HttpClient();
                            MultipartFormDataContent form = new MultipartFormDataContent();
                            form.Headers.Add("ldsessionid", SessionID);

                            foreach (var item in fileDict)
                            {
                                var fileBytes = await _fs.ReadAllBytes(item.Value);
                                form.Add(new ByteArrayContent(fileBytes, 0, fileBytes.Length), item.Key, item.Value);
                            }
                            var fileManager = await _fp.InstanceOf(image.FileManager);
                            var bytes = await fileManager.ReadAllBytes(image.FileName);
                            form.Add(new ByteArrayContent(bytes, 0, bytes.Length), "DATAFILE", Path.GetFileName(image.FileName) + ".pdf");

                            HttpResponseMessage response = await httpClient.PostAsync($"{Endpoint}/{idBucket}/document", form);
                            var IDC = await response.Content.ReadAsByteArrayAsync();
                            if (response.StatusCode != HttpStatusCode.Created)
                            {
                                _logger.LogWarning($"Errore conservazione documento {doc.Id} per via dell'errore {IDC}");
                                errors.Add(doc.Id, $"Errore conservazione documento {doc.Id} per via dell'errore {IDC}");
                                await updateDocPreserveStatus(idDoc, Domain.Enumerators.JobStatus.Failed);
                                isSuccess = false;
                                continue;
                            }
                            else
                            {
                                if (!(await createIDCDoc(doc, IDC, pdvCode)))
                                {
                                    isSuccess = false;
                                    _logger.LogWarning($"Conservazione doc {doc.Id} su pacchetto {pdvCode} andata a buon fine ma si è verificato un errore nella creazione del documento IDC");
                                    errors.Add(doc.Id, $"Conservazione doc {doc.Id} su pacchetto {pdvCode} andata a buon fine ma si è verificato un errore nella creazione del documento IDC");
                                }

                                if (!(await updateDocPreserveStatus(idDoc, Domain.Enumerators.JobStatus.Completed, pdvCode)))
                                {
                                    isSuccess = false;
                                    _logger.LogWarning($"Conservazione doc {doc.Id} su pacchetto {pdvCode} andata a buon fine ma si è verificato un errore nel salvataggio del db");
                                    errors.Add(doc.Id, $"Conservazione doc {doc.Id} su pacchetto {pdvCode} andata a buon fine ma si è verificato un errore nel salvataggio del db");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Errore durante processo di conservazione del doc {idDoc}: {ex.Message}");
                        errors.Add(idDoc, ex.Message);
                        await updateDocPreserveStatus(idDoc, Domain.Enumerators.JobStatus.Failed);
                    }
                }
                //CREAZIONE TASK IN CASO DI ERRORI
                if (errors.Count > 0)
                {
                    await GenerateErrorTask(
                    $"Errore conservazione per tipologia {typeConf.DocType}",
                    $"Per la conservazione della tipologia {typeConf.DocType} Sono stato riscontrati i seguenti errori: ",
                     typeConf.UserResp,
                     typeConf.DocType,
                     errors);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                if (fromWS)
                    throw new Exception(ex.Message);
            }
            return isSuccess;
        }

        #region METODI INTERNI
        public async Task<Dictionary<string, string>> xmlToDataMapping(Dictionary<string, string> metadati, Document doc, DocumentImage image, string mimeType, string typeLabel)
        {
            Dictionary<string, string> fileMap = new Dictionary<string, string>();
            //FILE METADATI
            legaldocIndex legalDocIndex = new legaldocIndex();
            legalDocIndex.documentClass = doc.DocumentTypeId.ToString();
            legalDocIndex.label = typeLabel;
            List<legaldocIndexField> legalDocIndexFieldArray = new List<legaldocIndexField>();
            doc.Fields = await _docRepo.GetFields(doc.Id);
            foreach (var metadatiDoc in metadati)
            {
                var parsing_value = metadatiDoc.Value.Parse(doc, "doc");
                //SE SI TRATTA DEI METADATI DEL DOC
                if (parsing_value == metadatiDoc.Value && parsing_value.Contains("Fields"))
                {
                    string pattern = @"\[(\d+)\]";
                    Match match = Regex.Match(parsing_value, pattern);

                    if (match.Success)
                    {
                        string extractedNumber = match.Groups[1].Value;
                        parsing_value = doc.Fields.ElementAt(Convert.ToInt32(extractedNumber)).Value;
                    }
                }

                if (metadatiDoc.Key.EndsWith("_dt"))
                    parsing_value = Convert.ToDateTime(parsing_value).ToString("dd-MM-yyyy HH:mm:ss");

                legalDocIndexFieldArray.Add(new legaldocIndexField() { name = metadatiDoc.Key.Replace("--", "__"), Value = parsing_value });

            }
            var indexFileName = "index_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Guid.NewGuid().ToString().Substring(0, 5) + ".xml";
            legalDocIndex.field = legalDocIndexFieldArray.ToArray();


            //SCRITTURA FILE VIRTUALE
            XmlSerializer serializer = new XmlSerializer(typeof(legaldocIndex));
            var encoding = Encoding.GetEncoding("ISO-8859-1");
            MemoryStream memStream = new MemoryStream();
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = false,
                Encoding = Encoding.Latin1,
                NamespaceHandling = NamespaceHandling.Default
            };
            XmlWriter xmlWriter = XmlWriter.Create(memStream, xmlWriterSettings);
            serializer.Serialize(xmlWriter, legalDocIndex);
            if (await _fs.WriteFromStream(indexFileName, memStream))
                _logger.LogDebug("Creato file index " + indexFileName);
            else
            {
                _logger.LogError("Impossibile creare file Virtual " + indexFileName);
                throw new Exception("Impossibile creare file Virtual " + indexFileName);
            }

            //PARAMETRI
            parameters parameters_file = new parameters();
            parameters_file.policy_id = _config.GetSection("CS:Auth:Policy").Value;

            //INDEX FILE
            parameters_file.index_file = new parametersIndex_file();
            parameters_file.index_file.index_name = indexFileName;
            parameters_file.index_file.index_hash = MessageDigest.HashString(MessageDigest.HashType.SHA256, await _fs.ReadAllBytes(indexFileName));
            parameters_file.index_file.index_mimetype = "text/xml;1.0";

            //DATA FILE
            parameters_file.data_file = new parametersData_file();
            parameters_file.data_file.data_name = Path.GetFileName(image.FileName);
            var fileManager = await _fp.InstanceOf(image.FileManager);
            var bytes = await fileManager.ReadAllBytes(image.FileName);
            parameters_file.data_file.data_hash = MessageDigest.HashString(MessageDigest.HashType.SHA256, bytes);
            parameters_file.data_file.data_mimetype = mimeType;

            //DA VERIFICARE PERCORSO
            parameters_file.path = "/elmi_doctest/" + DateTime.Now.Year.ToString();
            parameters_file.encrypted_by_owner = "N";

            //SCRITTURA FILE VIRTUALE
            var paramsFileName = "params_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Guid.NewGuid().ToString().Substring(0, 5) + ".xml";
            XmlSerializer paramsSerializer = new XmlSerializer(typeof(parameters));
            MemoryStream paramsMemStream = new MemoryStream();
            paramsSerializer.Serialize(paramsMemStream, parameters_file);
            if (await _fs.WriteFromStream(paramsFileName, paramsMemStream))
                _logger.LogDebug("Creato file Params " + paramsFileName);
            else
            {
                _logger.LogError("Impossibile creare Params file Virtual " + paramsFileName);
                throw new Exception("Impossibile creare file Virtual " + paramsFileName);
            }
            fileMap.Add("PARAMFILE", paramsFileName);
            fileMap.Add("INDEXFILE", indexFileName);

            return fileMap;
        }

        public async Task<bool> updateDocPreserveStatus(int idDoc, Domain.Enumerators.JobStatus status, string pdv = "")
        {
            var docInfo = await _docRepo.GetById(idDoc);
            var image = await _docService.GetContentInfo(Convert.ToInt32(docInfo.ImageId));
            //IN PROD DA ELIMINARE LA SUBSTRING
            image.PreservationPDV = !String.IsNullOrEmpty(pdv) ? pdv.Substring(0, 15) : "";
            image.PreservationDate = status == Domain.Enumerators.JobStatus.Completed ? DateTime.Now : null;
            image.PreservationStatus = status;
            return await _docRepo.UpdateImage(image) > -1;
        }

        public async Task<bool> createIDCDoc(Document doc, byte[] idcByteArray, string pdvCode)
        {
            bool createdSuccess = false;
            CreateOrUpdateDocument doc_idc = new CreateOrUpdateDocument();
            doc_idc.DocumentTypeId = "$IDC$";
            doc_idc.DocumentDate = DateTime.UtcNow;
            doc_idc.Description = "IDC del Documento - " + doc.Description;
            doc_idc.Status = Domain.Enumerators.DocumentStatus.Stored;
            doc_idc.FieldList.Add(new AddOrUpdateDocumentField() { FieldTypeId = "1", FieldName = "PDV", FieldIndex = 0, Value = pdvCode });
            doc_idc.FieldList.Add(new AddOrUpdateDocumentField() { FieldTypeId = "1", FieldName = "IdDoc", FieldIndex = 1, Value = doc.Id.ToString() });

            var userProfile = await _userService.GetUserProfile("admin");
            var docInfo = await _docService.CreateAndRead(doc_idc, _pf.Get() == null ? userProfile : _pf.Get());




            if (docInfo != null && docInfo.Id > 0)
            {
                FileContent fileContent = new FileContent();
                fileContent.FileData = Convert.ToBase64String(idcByteArray);
                fileContent.DataIsInBase64 = true;
                fileContent.FileName = "IDC_" + doc.Id + ".p7m.tsd";

                //INSERIMENTO COLLEGAMENTO ALLEGATI
                try
                {
                    await _docService.AddLink(docInfo.Id, doc.Id, _pf.Get() == null ? userProfile : _pf.Get(), true);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Impossibile creare collegamento tra documento $IDC$ {docInfo.Id} e documento iniziale {doc.Id}");
                }

                var newImage = await _docService.AddContent(docInfo.Id, _pf.Get() == null ? userProfile : _pf.Get(), fileContent);
                if (newImage != null)
                    createdSuccess = true;
                else
                {
                    _logger.LogError($"Impossibile creare immagine documento $IDC$ {docInfo.Id}");
                    throw new Exception($"Impossibile creare immagine documento $IDC$ {docInfo.Id}");
                }
            }
            else
            {
                _logger.LogError($"Impossibile creare documento $IDC$ per doc {doc.Id}");
                throw new Exception($"Impossibile creare documento $IDC$ per doc {doc.Id}");
            }
            return createdSuccess;
        }
        public void Test()
        {
            try
            {
                bool createdSuccess = false;
                CreateOrUpdateDocument doc_idc = new CreateOrUpdateDocument();
                doc_idc.DocumentTypeId = "$IDC$";
                doc_idc.DocumentDate = DateTime.UtcNow;
                doc_idc.Description = "IDC del Documento "; //+ doc.Id;
                //doc_idc.Status = Domain.Enumerators.DocumentStatus.Stored;
                var userProfile = _userService.GetUserProfile("admin").Result;
                var docInfo = _docService.Create(doc_idc, _pf.Get() == null ? userProfile : _pf.Get()).Result;
                string test = "";
            }
            catch (Exception ex)
            {

                string s = ex.ToString();
            }

            //if (docInfo != null && docInfo.Id > 0)
            //{
            //    FileContent fileContent = new FileContent();
            //    fileContent.FileData = Convert.ToBase64String(idcByteArray);
            //    fileContent.DataIsInBase64 = true;
            //    fileContent.FileName = "IDC_" + doc.Id + ".xml";

            //    var newImage = await _docService.AddContent(docInfo.Id, _pf.Get() == null ? userProfile : _pf.Get(), fileContent);
            //    if (newImage != null)
            //        createdSuccess = true;
            //    else
            //    {
            //        _logger.LogError($"Impossibile creare immagine documento $IDC$ {docInfo.Id}");
            //        throw new Exception($"Impossibile creare immagine documento $IDC$ {docInfo.Id}");
            //    }
            //}
            //else
            //{
            //    _logger.LogError($"Impossibile creare documento $IDC$ per doc {doc.Id}");
            //    throw new Exception($"Impossibile creare documento $IDC$ per doc {doc.Id}");
            //}
            //return createdSuccess;
        }


        public async Task<bool> GenerateErrorTask(
            string title,
            string desc,
            string notifyToList,
            string docType = "",
            Dictionary<int, string> errorDict = null)
        {

            bool isSuccess = false;
            try
            {
                CreateNewTask task = new CreateNewTask();
                task.Description = errorDict != null && errorDict.Count > 0 ? BuildErrorString(docType, desc, errorDict) : desc;
                task.Title = title;
                task.NotifyTo = notifyToList.Split(",").ToList();
                task.TaskType = Domain.Enumerators.TaskType.Error;
                var userProfile = await _userService.GetUserProfile("admin");
                var taskInfo = await _taskService.CreateTask(task, _pf.Get() == null ? userProfile : _pf.Get());
                isSuccess = taskInfo != null;
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
            }
            return isSuccess;
        }
        public string BuildErrorString(string tipoDoc, string mess, Dictionary<int, string> errors)
        {
            mess = $"Per la conservazione della tipologia {tipoDoc} Sono stato riscontrati i seguenti errori: ";
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(mess);
            var errorList = errors.Select(kvp => $"{kvp.Key}: {kvp.Value}");
            string errorString = string.Join(", ", errorList);
            stringBuilder.Append(errorString);
            return stringBuilder.ToString();
        }
        #endregion
    }
}
