using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;
using OpenDMS.PdfManager;
using System.Net.Http.Headers;
using Web.Utilities;

namespace Web.Pages
{
    [IgnoreAntiforgeryToken]
    public class AddRemoteSignModel : PageModel
    {

        private readonly ILogger<AddRemoteSignModel> logger;
        private readonly IConfiguration _config;
        private readonly IUserService _userService;
        private readonly IDocumentService _documentService;
        private readonly ILoggedUserProfile _userContext;
        private readonly UserProfile userProfile;


        public AddRemoteSignModel(ILogger<AddRemoteSignModel> logger, IConfiguration config, IUserService userService, ILoggedUserProfile userContext, IDocumentService documentService)
        {
            this.logger = logger;
            _config = config;
            _userService = userService;
            _userContext = userContext;
            _documentService = documentService;
            userProfile = _userContext.Get();
        }

        public string Documents { get; set; }
        public string ErrorMessage { get; set; } = "";
        public string WarningMessage { get; set; } = "";

        public string Provider { get; set; } = "";
        //public int docId { get; set; }
        public SignTypes SignType { get; set; }
        public bool IsFirmaMassiva { get; set; } = false;

        public Dictionary<int, List<FieldPosition>> SignFields = new();
        public List<FieldPosition> CommonFields = new();

        public List<int> DocumentIDList { get; set; } = new();

        public List<FileProperty> Files { get; set; } = new();
        //public List<FieldPosition> SignFields { get; set; } = new();
        public string signField { get; set; }
        public bool fromActivity { get; set; }


        public async Task<IActionResult> OnGetSignWithField(string documents, string signField, bool fromActivity=false) //richiesta diretta 
        {
            return await OnGetAsync(documents, SignTypes.Pades, signField, fromActivity);
        }


        public async Task<IActionResult> OnGetAsync(string documents, SignTypes type = SignTypes.Pades, string signField = "", bool fromActivity = false)
        {
            this.Documents = documents;
            this.SignType = type;
            this.signField = signField;
            this.fromActivity = fromActivity;

            try
            {

                ErrorMessage = await CheckProvider();
                if (String.IsNullOrEmpty(ErrorMessage)) ErrorMessage = await CheckCredentials();
                if (String.IsNullOrEmpty(ErrorMessage)) ErrorMessage = await CheckDocuments();
                if (String.IsNullOrEmpty(ErrorMessage)) WarningMessage = await CheckSignField(true);
                if (String.IsNullOrEmpty(ErrorMessage)) ErrorMessage = CheckCommonSignFields();

                if (!String.IsNullOrEmpty(signField) && string.IsNullOrEmpty(ErrorMessage))
                {
                    return await OnPostConfirm(this.Documents, SignTypes.Pades, signField);
                }
                else
                {
                    await CreateFileList();
                }
                if (!String.IsNullOrEmpty(ErrorMessage))
                {
                    logger.LogWarning("RemoteSign:" + ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "RemoteSign");
                ErrorMessage = ex.Message;
            }
            return Page();
        }


        private async Task<string> CheckProvider()
        {
            Provider = await _userService.GetAttribute(userProfile.userId, UserAttributes.CONST_REMOTESIGNATURE_SERVICE);
            if (String.IsNullOrEmpty(Provider)) return "Il Servizio di Firma Digitale Remota non è configurato o non è attivo. Per verificare la tua configurazione vai sulla tua area personale nel menu <b>Firme</b>.";
            return "";
        }
        private async Task<string> CheckCredentials()
        {
            //controllo sulle informazioni dell'user su firme remote
            if (!(await ApiCheckUserInfo())) return ("Le tue credenziali di firma digitale remota non sono corrette o non sono state configurate. Per verificare la tua configurazione vai sulla tua area personale nel menu <b>Firme</b>.");
            return "";
        }
        private async Task<string> CheckDocuments()
        {
            try
            {
                DocumentIDList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(Documents);
                if (DocumentIDList == null || DocumentIDList.Count == 0)
                    return "Non è stato trovato nessun documento da firmare.";
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private async Task<string> CheckSignField(bool checkEmpty = true)
        {
            SignFields = await GetAllSignatureFieldsAsync();
            if (!String.IsNullOrEmpty(signField))
            {
                if (!SignFields.Any(f => f.Value.Any(f => f.Name == signField)))
                {
                    signField = "";
                }
                    //return "Il campo firma non è presente in nessun documento";
            }
            else
            {
                if (SignType == SignTypes.Pades && !checkEmpty)
                    return "Non è stato indicato un campo di firma";
            }
            return "";
        }
        private string CheckCommonSignFields()
        {
            bool FirstFound = false;
            List<FieldPosition> result = new();
            foreach (var doc in SignFields)
            {
                // Ignoro i documenti senza campi di firma
                if (doc.Value.Count > 0)
                {
                    if (FirstFound)
                    {
                        result = result.Where(a => doc.Value.Any(b => string.Compare(a.Name, b.Name, true) == 0)).ToList();
                        // Se la lista si svuota non ha senso continuare
                        if (result.Count == 0) break;
                    }
                    else
                    {
                        result.AddRange(doc.Value);
                        FirstFound = true;
                    }
                }

            }
            CommonFields = result;
            if (result.Count == 0) return "Non è stato trovato alcun campo di firma in comune";
            return "";
        }






        public async Task<IActionResult> OnPostConfirm(string documents, SignTypes type = SignTypes.Pades, string signField = "", bool fromActivity=false)
        {
            this.Documents = documents;
            this.SignType = type;
            this.signField = signField;
            this.fromActivity = fromActivity;

            try
            {

                ErrorMessage = await CheckProvider();
                if (String.IsNullOrEmpty(ErrorMessage)) ErrorMessage = await CheckCredentials();
                if (String.IsNullOrEmpty(ErrorMessage)) ErrorMessage = await CheckDocuments();
                if (String.IsNullOrEmpty(ErrorMessage)) WarningMessage = await CheckSignField();
                if (String.IsNullOrEmpty(ErrorMessage)) ErrorMessage = CheckCommonSignFields();

                await CreateFileList();

                if (String.IsNullOrEmpty(ErrorMessage) && string.IsNullOrEmpty(WarningMessage) && !String.IsNullOrEmpty(signField))
                {
                    var SignRoom = await ApiCreateSignRoom();
                    if (!String.IsNullOrEmpty(SignRoom))
                    {
                        await UploadFiles(SignRoom);
                        return Redirect($"/RemoteSign/OTP/?SignRoom={SignRoom}");
                    }
                    else
                        ErrorMessage = "Non è stato possibile creare una Sessione di Firma";
                }
                if (!String.IsNullOrEmpty(ErrorMessage))
                {
                    logger.LogWarning("RemoteSign/Post:" + ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "RemoteSign/Post");
                ErrorMessage = ex.Message;
            }
            return Page();

        }






        private async Task<string> UploadFiles(string signRoom)
        {
            foreach (var f in Files.Where(f => !f.Excluded))
            {
                //Carico il file nella signRoom e aggiorno lo status della sessione di firma; in caso di errore cancello la signroom
                await ApiUploadFile(signRoom, f);
            }
            //}
            return _config["ExternalPages:RemoteSign:" + Provider] + $"/?signRoom={signRoom}&UserName={userProfile.userId}&Wait=true";
        }
        private async Task<bool> ApiCheckUserInfo()
        {
            bool result = false;
            string url = _config["ExternalPages:RemoteSign:" + Provider] + "/Sign/CheckUserInfo/" + userProfile.userId;
            HttpClientHandler handler = new HttpClientHandler();
            // Ignora la verifica del certificato SSL
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            try
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        bool.TryParse(responseContent, out result);
                    }
                    else
                        throw new Exception(response.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Si è verificato il seguente errore: {ex.Message}.<br/>Per verificare la configurazione vai sulla tua area personale alla voce <b>Firme</b>");
            }

            return result;
        }
        private async Task ApiDeleteSignRoom(string signRoom)
        {
            var url = _config["ExternalPages:RemoteSign:" + Provider] + "/Sign/ClearSignRoom?SignRoom=" + signRoom;
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            using (HttpClient httpClient = new HttpClient(handler))
            {
                HttpResponseMessage response = await httpClient.DeleteAsync(url);
                if (response.IsSuccessStatusCode)
                    signRoom = await response.Content.ReadAsStringAsync();
                else
                    throw new Exception($"Si è verificato un errore di comunicazione durante il richiamo del servizio \"{url}\": {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
        private async Task<string> ApiCreateSignRoom()
        {
            string SignRoom = "";
            var url = _config["ExternalPages:RemoteSign:" + Provider] + "/Sign/CreateSignRoom/" + userProfile.userId + "/" + this.SignType;
            HttpClientHandler handler = new HttpClientHandler();
            // Ignora la verifica del certificato SSL
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            using (HttpClient httpClient = new HttpClient(handler))
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    SignRoom = await response.Content.ReadAsStringAsync();
                }
                else
                    throw new Exception($"Errore in creazione SignRoom: {response.StatusCode} - {response.ReasonPhrase} - URL: {url}");
            }
            return SignRoom;
        }
        private async Task ApiUploadFile(string signRoom, FileProperty f)
        {
            try
            {
                var url = _config["ExternalPages:RemoteSign:" + Provider];
                //aggiungo il campo di firma selezionato in caso di firma pades
                if (this.SignType == SignTypes.Pades && !string.IsNullOrEmpty(this.signField))
                    url += "/Sign/UploadFileWithSignField/" + signRoom + "/" + signField;
                else
                    url += "/Sign/UploadFile/" + signRoom;

                var imageInfo = await _documentService.GetContentInfo(f.ImageId);
                var immagine = await _documentService.GetContent(f.ImageId);

                using var fileContent = new ByteArrayContent(immagine);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "file", // Nome del campo nella richiesta
                    FileName = $"{f.ImageId}.{ControllerUtility.GetAllExtensions(imageInfo.FileName)}",
                    Size = immagine.LongLength,
                };

                HttpClientHandler handler = new HttpClientHandler();
                // Ignora la verifica del certificato SSL
                handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                using (HttpClient httpClient = new HttpClient(handler))
                {
                    // Esegui la richiesta POST all'API con il file come corpo della richiesta.
                    var response = await httpClient.PostAsync(url, new MultipartFormDataContent
                        {
                            { fileContent }
                        });
                    if (!response.IsSuccessStatusCode)
                        throw new Exception($"Si è verificato il seguente errore durante l'upload del documento: {response.StatusCode} - {response.ReasonPhrase}");
                }

                //aggiorno lo status della sessione di firma; in caso di errore cancello la signroom
                if ((await _documentService.UpdateSignatureStatus(f.ImageId, JobStatus.Queued, userProfile, signRoom)) <= 0)
                    throw new Exception("Si è verificato un errore di aggiornamento dello stato di firma del documento");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"RemoteSign/UploadFile: {f.Id}/{f.ImageId} - {f.Name}");
            }
        }
        private async Task CreateFileList()
        {
            var su = await _userService.GetUserProfile(SpecialUser.SystemUser);
            var u = User.Identity.Name;
            Files = new List<FileProperty>();
            string esito = "";
            foreach (var document in SignFields)
            {
                var fp = new FileProperty();
                fp.Id = document.Key;
                fp.Excluded = false;
                fp.Motivation = "";
                try
                {
                    var doc = await _documentService.Load(document.Key, su);
                    fp.ImageId = doc.Image == null ? 0 : doc.Image.Id;

                    fp.Name = doc.Description;
                    fp.Owner = (await _userService.GetById(doc.Owner)).Contact.FullName;
                    fp.DocType = doc.DocumentType.Name;
                    fp.Nr = doc.DocumentNumberLookupValue;
                    fp.Date = doc.DocumentDate?.Date.ToString("dd/MM/yyyy") ?? "";
                    fp.StatusCode = doc.Image == null ? 0 : doc.Image.SignatureStatus;
                    fp.SignFields = document.Value.Select(s => s.Name).ToArray();
                    fp.SignRoom = doc.Image == null ? "" : doc.Image.SignatureSession;
                    fp.SignUser = doc.Image == null ? "" : doc.Image.SignatureUser;

                    if (doc.Image == null)
                    {
                        fp.Excluded = true;
                        fp.Motivation = "Il documento non ha un file associato";
                    }
                    else
                    if (document.Value.Count == 0 && SignType == SignTypes.Pades)
                    {
                        fp.Excluded = true;
                        fp.Motivation = "Nessun campo di firma";
                    }
                    else
                    if (!doc.Image.FileName.ToLower().EndsWith(".pdf") && SignType == SignTypes.Pades)
                    {
                        fp.Excluded = true;
                        fp.Motivation = "Il documento non è un PDF";
                    }
                    else
                    //if (doc.Image.SignatureStatus == JobStatus.Running)
                    //{
                    //    fp.Excluded = true;
                    //    fp.Motivation = "Già in firma";

                    //} else
                    if (!String.IsNullOrEmpty(doc.Image.CheckOutUser))
                    {
                        fp.Excluded = true;
                        fp.Motivation = "Checkout in corso";

                    }
                    if (doc.Image != null
                        && !String.IsNullOrEmpty(doc.Image.SignatureSession)
                        && doc.Image.SignatureStatus != JobStatus.Running
                        && doc.Image.SignatureStatus != JobStatus.Completed
                        && string.Compare(doc.Image.SignatureUser, u, true) == 0
                        )
                    {
                        fp.SignRoom = "";
                        fp.SignUser = "";
                        await ApiDeleteSignRoom(doc.Image.SignatureSession);
                    }
                }
                catch (Exception ex)
                {
                    fp.Motivation = ex.Message;
                    fp.Excluded = true;
                    logger.LogError(ex, $"RemoteSign/CreateFileList: {fp.Id}");

                };
                Files.Add(fp);
            }
        }
        private async Task<List<FieldPosition>> GetSignFields(int docId)
        {
            try
            {

                var doc = await _documentService.Get(docId);
                if (doc != null)
                {
                    List<FieldPosition> fields = new();
                    var blankfields = await _documentService.GetBlankSignFields(doc);
                    for (int i = 0; i < blankfields.Count; i++)
                    {
                        var f = blankfields[i];

                        var desc = f.Description.ToLower();

                        //if (desc.StartsWith("wf:"))
                        //{
                        //    desc = desc.Substring(3);
                        //    if (!fromActivity)
                        //    {
                        //        // se non provengo da attività ignoro il campo
                        //        desc = "";
                        //    }
                        //}
                        //else
                        //{
                        //    if (fromActivity)
                        //    {
                        //        // se provengo da attività ignoro il campo legato a workflow
                        //        desc = "";
                        //    }
                        //}
                        if (desc.StartsWith("p7m:"))
                        {
                            if (SignType != SignTypes.Cades)
                                desc = "";
                            else
                                desc = desc.Substring(4);
                        }
                        if (desc.StartsWith("pdf:"))
                        {
                            if (SignType != SignTypes.Pades)
                                desc = "";
                            else
                                desc = desc.Substring(4);
                        }

                        if (desc.StartsWith("2"))
                        {
                            var ruolo = desc.Substring(1).ToLower();
                            // tolgo l'indicatore di selezione multipla delle firme
                            var all = ruolo.IndexOf(":");
                            if (all >= 0)
                                ruolo = ruolo.Substring(0, all);
                            // Verifica il ruolo
                            if (userProfile.Roles.Any(r => r.Id.ToLower() == ruolo))
                            {
                                f.Description = "Ruolo " + userProfile.Roles.FirstOrDefault(r => r.Id.ToLower() == ruolo)?.Description;
                            }
                            else
                            if (userProfile.GlobalRoles.Any(r => r.Id.ToLower() == ruolo))
                            {
                                f.Description = "Ruolo " + userProfile.GlobalRoles.FirstOrDefault(r => r.Id.ToLower() == ruolo)?.Description;
                            }
                            else
                            {
                                desc = "";
                            }
                        }
                        if (desc.StartsWith("0"))
                        {
                            var utente = desc.Substring(1).ToLower();
                            // tolgo l'indicatore di selezione multipla delle firme
                            var me = utente == "user";
                            if (me)
                            {
                                f.Description = "Utente " + userProfile.UserInfo.Contact.FullName;
                            }
                            else
                            {
                                desc = "";
                            }
                        }
                        if (!String.IsNullOrEmpty(desc))
                        {
                            f.Description += " - Pagina " + f.Page;
                            fields.Add(f);
                        }
                    }
                    return fields;
                }
            }
            catch
            {
            }
            return new List<FieldPosition>();
        }

        private async Task<Dictionary<int, List<FieldPosition>>> GetAllSignatureFieldsAsync()
        {
            var result = new Dictionary<int, List<FieldPosition>>();
            for (int i = 0; i < DocumentIDList.Count; i++) //il for perché devo scartare il primo (vedi i=1)
            {
                var currentDocumentFields = await GetSignFields(DocumentIDList[i]);
                if (currentDocumentFields != null && currentDocumentFields.Count > 0)
                {
                    result.Add(DocumentIDList[i], currentDocumentFields);
                }
                else
                    result.Add(DocumentIDList[i], new List<FieldPosition>());
            }
            return result;
        }

        public enum SignTypes
        {
            Cades = 0,
            Pades = 1,
            Xades = 2
        }




    }

}
