using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;
using OpenDMS.PdfManager;
using SixLabors.ImageSharp;
using System.Net.Http.Headers;
using Web.DTOs;
using Web.Utilities;

namespace Web.Pages.RemoteSign
{
    [IgnoreAntiforgeryToken]
    public class OTPModel : PageModel
    {
        private readonly ILogger<OTPModel> logger;
        private readonly IConfiguration _config;
        private readonly IUserService _userService;
        private readonly IDocumentService _documentService;
        private readonly ILoggedUserProfile _userContext;
        private readonly UserProfile userProfile;



        public OTPModel(ILogger<OTPModel> logger, IConfiguration config, IUserService userService, ILoggedUserProfile userContext, IDocumentService documentService)
        {
            this.logger = logger;
            _config = config;
            _userService = userService;
            _userContext = userContext;
            _documentService = documentService;
            userProfile = _userContext.Get();
        }

        private string Documents { get; set; }
        public string ErrorMessage { get; set; } = "";
        public string Provider { get; set; } = "";

        public string pin { get; set; } = "";
        public string otp { get; set; } = "";


        public Dictionary<int, List<FieldPosition>> SignFields = new();
        public List<FieldPosition> CommonFields = new();

        public List<int> DocumentIDList { get; set; } = new();
        public List<FileProperty> Files { get; set; } = new();
        public string SignRoom { get; set; }
        public SignRoom_DTO SignRoomInfo { get; set; } = new SignRoom_DTO();
        public bool WaitingForOTP { get; set; }


        public async Task<IActionResult> OnGetAsync(string SignRoom)
        {
            this.SignRoom = SignRoom;
            try
            {
                ErrorMessage = await CheckProvider();
                if (String.IsNullOrEmpty(ErrorMessage)) ErrorMessage = await CheckCredentials();

                if (String.IsNullOrEmpty(ErrorMessage))
                {
                    SignRoomInfo = await ApiGetSignRoom();
                    if (SignRoomInfo != null)
                    {
                        if (SignRoomInfo.Status != SignRoomStatus.Created &&
                            SignRoomInfo.Status != SignRoomStatus.FileUploaded &&
                            SignRoomInfo.Status != SignRoomStatus.ReadyToSign)
                        {
                            return  Redirect($"/RemoteSign/Monitor/?SignRoom={SignRoom}");
                        }
                        else
                        {
                            ErrorMessage = await CheckDocuments();
                            await CreateFileList();
                        }
                    }
                }
                if (!String.IsNullOrEmpty(ErrorMessage))
                {
                    logger.LogWarning("RemoteSign/OTP:" + ErrorMessage);
                }
            } catch (Exception ex) {
                logger.LogError(ex,"RemoteSign/OTP");
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
                var Images = await _documentService.GetImagesBySignatureSession(SignRoomInfo.UserName, SignRoomInfo.SignRoom);
                if (Images != null)
                {
                    foreach (var Image in Images)
                    {
                        var docs = await _documentService.GetDocumentsFromContentId(Image, userProfile);
                        DocumentIDList.AddRange(docs);
                    }
                }
                else return "Nessuna Immagine trovata";
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
  
  




        public async Task<IActionResult> OnPostConfirm(string SignRoom, string otp, string pin)
        {
            this.SignRoom = SignRoom;
 
            try
            {

                ErrorMessage = await CheckProvider();
                if (String.IsNullOrEmpty(ErrorMessage)) ErrorMessage = await CheckCredentials();


                if (String.IsNullOrEmpty(ErrorMessage))
                {
                    SignRoomInfo = await ApiGetSignRoom();
                    if (SignRoomInfo != null)
                    {
                        if (SignRoomInfo.Status != SignRoomStatus.Created &&
                            SignRoomInfo.Status != SignRoomStatus.FileUploaded &&
                            SignRoomInfo.Status != SignRoomStatus.ReadyToSign)
                        {
                            return Redirect($"/RemoteSign/Monitor/?SignRoom={SignRoom}");
                            //RedirectToPage("Monitor", new { SignRoom });
                        }
                        else
                        {
                            ErrorMessage = await CheckDocuments();
                            await CreateFileList();
                            if (String.IsNullOrEmpty(ErrorMessage)) ErrorMessage = await ApiStartSign(pin,otp);
                            return Redirect($"/RemoteSign/Monitor/?SignRoom={SignRoom}");

                        }
                    }
                }
                if (!String.IsNullOrEmpty(ErrorMessage))
                {
                    logger.LogWarning("RemoteSign/OTP/Post:" + ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "RemoteSign/OTP/Post");
                ErrorMessage = ex.Message;
            }
            return Page();
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

        private async Task<SignRoom_DTO> ApiGetSignRoom()
        {
            SignRoom_DTO result = null;
            string url = _config["ExternalPages:RemoteSign:" + Provider] + "/Sign/GetSignRoom/" + SignRoom;
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
                        result = Newtonsoft.Json.JsonConvert.DeserializeObject<SignRoom_DTO>(responseContent);
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
        private async Task<string> ApiStartSign(string PIN, string OTP)
        {
            var url = _config["ExternalPages:RemoteSign:" + Provider] + "/Sign/StartSign/" + SignRoom+"/"+ userProfile.userId + "/" + OTP +"/" + PIN;
            HttpClientHandler handler = new HttpClientHandler();
            // Ignora la verifica del certificato SSL
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            using (HttpClient httpClient = new HttpClient(handler))
            {
                HttpResponseMessage response = await httpClient.PostAsync(url, null);
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Errore in creazione SignRoom: {response.StatusCode} - {response.ReasonPhrase} - URL: {url}");
            }
            return "";
        }
 
        private async Task CreateFileList()
        {
            var su = await _userService.GetUserProfile(SpecialUser.SystemUser);
            var u = User.Identity.Name;
            Files = new List<FileProperty>();
            string esito = "";
            foreach (var document in DocumentIDList)
            {
                var fp = new FileProperty();
                fp.Id = document;
                fp.Excluded = false;
                fp.Motivation = "";
                try
                {
                    var doc = await _documentService.Load(document,su);

                    fp.Name = doc.Description;
                    fp.Owner = (await _userService.GetById(doc.Owner)).Contact.FullName;
                    fp.DocType = doc.DocumentType.Name;
                    fp.ImageId = doc.Image == null ? 0 : doc.Image.Id;
                    fp.Nr = doc.DocumentNumberLookupValue;
                    fp.Date = doc.DocumentDate?.Date.ToString("dd/MM/yyyy") ?? "";
                    fp.StatusCode = doc.Image.SignatureStatus;
                    fp.SignFields = null;
                    fp.SignRoom = doc.Image.SignatureSession;
                    fp.SignUser = doc.Image.SignatureUser;

                    if (doc.Image.SignatureStatus == JobStatus.Completed)
                    {
                        fp.Motivation = "Firma Già Eseguita";
                    }
                    else
                    if (doc.Image.SignatureStatus == JobStatus.Failed || doc.Image.SignatureStatus == JobStatus.Aborted || doc.Image.SignatureStatus == JobStatus.Ignored)
                    {
                        fp.Motivation = "Firma Annullata";
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
                    return await _documentService.GetBlankSignFields(doc);
            }
            catch { 
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
