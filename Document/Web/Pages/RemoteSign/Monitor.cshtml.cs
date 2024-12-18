using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;
using Web.DTOs;

namespace Web.Pages.RemoteSign
{
    [IgnoreAntiforgeryToken]
    public class MonitorModel : PageModel
    {
        private readonly ILogger<MonitorModel> logger;
        private readonly IConfiguration _config;
        private readonly IUserService _userService;
        private readonly IDocumentService _documentService;
        private readonly ILoggedUserProfile _userContext;
        private readonly UserProfile userProfile;



        public MonitorModel(ILogger<MonitorModel> logger, IConfiguration config, IUserService userService, ILoggedUserProfile userContext, IDocumentService documentService)
        {
            this.logger = logger;
            _config = config;
            _userService = userService;
            _userContext = userContext;
            _documentService = documentService;
            userProfile = _userContext.Get();
        }

        public string ErrorMessage { get; set; } = "";
        public string Provider { get; set; } = "";

        private List<int> Images = new();

        public List<int> DocumentIDList { get; set; } = new();
        public List<FileProperty> Files { get; set; } = new();
        public string SignRoom { get; set; }
        public SignRoom_DTO SignRoomInfo { get; set; } = new SignRoom_DTO();

        public class SignInfo
        {
            public SignRoom_DTO SignRoom { get; set; } = new SignRoom_DTO();
            public List<FileProperty> Files { get; set; } = new();
            public string ErrorMessage { get; set; } = "";
        }

        public async Task OnGetAsync(string SignRoom)
        {
            this.SignRoom = SignRoom;
            try
            {
                ErrorMessage = await CheckProvider();
                if (String.IsNullOrEmpty(ErrorMessage)) ErrorMessage = await CheckCredentials();

                if (!String.IsNullOrEmpty(ErrorMessage))
                {
                    logger.LogWarning("RemoteSign/Monitor:" + ErrorMessage);
                }
                await CreateFileList();
            }
            catch (Exception ex) {
                logger.LogError(ex, "RemoteSign/Monitor");
                ErrorMessage = ex.Message;
            }
        }

        public async Task<JsonResult> OnPostFilesAsync(string SignRoom)
        {
            this.SignRoom = SignRoom;
            var s = new SignInfo();
            try
            {
                s.ErrorMessage = await CheckProvider();
                if (String.IsNullOrEmpty(s.ErrorMessage)) s.ErrorMessage = await CheckCredentials();

                if (String.IsNullOrEmpty(s.ErrorMessage))
                {
                    SignRoomInfo = await ApiGetSignRoom();
                    if (SignRoomInfo != null)
                    {
                        s.ErrorMessage = await CheckDocuments();
                        await CreateFileList();
                    }
                }
                if (!String.IsNullOrEmpty(s.ErrorMessage))
                {
                    logger.LogWarning("RemoteSign/GetFiles:" + s.ErrorMessage);
                }
                s.SignRoom = SignRoomInfo;
                s.Files = Files;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "RemoteSign/GetFiles");
                s.ErrorMessage = ex.Message;
            }
            return new JsonResult ( s);
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
                Images = await _documentService.GetImagesBySignatureSession(SignRoomInfo.UserName, SignRoomInfo.SignRoom);
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
        
        private async Task CreateFileList()
        {
            var su = await _userService.GetUserProfile(SpecialUser.SystemUser);
            var u = User.Identity.Name;
            Files = new List<FileProperty>();
            string esito = "";
            for (int i = 0; i< DocumentIDList.Count; i++)
            {
                var document = DocumentIDList[i];
                var imageId = Images[i];
                var fp = new FileProperty();
                fp.Id = document;
                fp.Excluded = false;
                fp.Motivation = "";
                try
                {
                    var doc = await _documentService.Load(document,su);
                    var image = await _documentService.GetContentInfo(imageId);

                    fp.Name = doc.Description;
                    fp.Owner = (await _userService.GetById(doc.Owner)).Contact.FullName;
                    fp.ImageId = imageId;
                    fp.DocType = doc.DocumentType.Name;
                    fp.Nr = doc.DocumentNumberLookupValue;
                    fp.Date = doc.DocumentDate?.Date.ToString("dd/MM/yyyy") ?? "";
                    fp.StatusCode = image.SignatureStatus;
                    fp.SignFields = null;
                    fp.SignRoom = image.SignatureSession;
                    fp.SignUser = image.SignatureUser;
                    if (image.SignatureStatus == JobStatus.Completed)
                    {
                        fp.Motivation = "Firma Eseguita";
                    }
                    else
                    if (image.SignatureStatus == JobStatus.Failed || image.SignatureStatus == JobStatus.Aborted || image.SignatureStatus == JobStatus.Ignored)
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
  
   


    }

}
