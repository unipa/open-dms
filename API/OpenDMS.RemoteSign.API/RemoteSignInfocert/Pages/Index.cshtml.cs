using FirmeRemoteLib.Interfaces;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RemoteSign.BL;
using RemoteSignInfocert.Controllers;
using RemoteSignInfocert.Interfaces;
using RemoteSignInfocert.Models;
using RemoteSignInfocert.Models.VM;
using RemoteSignInfocert.Utils;
using System.ComponentModel;
using System.IO.Compression;

namespace RemoteSignInfocert.Pages
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogger<SignController> _logger2;
        private const string SIGN_ROOMS = "SignRooms";
        private string _signRoomBasePath = "";
        private readonly IWebHostEnvironment _hostEnv;
        private readonly IUserDAO _dao;
        private readonly ISignRoomDAO _signRoomDao;
        private readonly IConfiguration _config;
        private readonly SignController _signController;
        private readonly IRemoteSign _remoteSign;
        private readonly Sign _sign;

        public List<FileListVM> ElencoFile { get; set; } = new List<FileListVM>();
        public string? UserName { get; set; } = "";
        public string? Alias { get; set; } = "";
        public bool? WaitOTP { get; set; } = false;
        public string ErrorMessage { get; set; } = "";
        public string? SignRoom { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment hostenv, IRemoteSign remoteSign, IUserDAO dao, IConfiguration config, ISignRoomDAO signRoomDao, ILogger<SignController> logger2, SignController signController, Sign sign)
        {
            _logger = logger;
            _hostEnv = hostenv;
            _remoteSign = remoteSign;
            _dao = dao;
            _signRoomBasePath = Path.Combine(_hostEnv.ContentRootPath, SIGN_ROOMS);
            _config = config;
            if (Directory.Exists(_signRoomBasePath))
                Directory.CreateDirectory(_signRoomBasePath);

            ElencoFile = new List<FileListVM>();
            _signRoomDao = signRoomDao;
            _logger2 = logger2;
            _signController = signController;
            _sign = sign;
        }

        public IActionResult OnGet(string SignRoom, string UserName, bool Wait)
        {
            _logger.LogInformation("Index -> OnGet: " + SignRoom + ";" + UserName + ";" + Wait);
            try
            {
                if (string.IsNullOrEmpty(UserName))
                    throw new Exception("Utente non indicato!");

                if (!string.IsNullOrEmpty(TempData["ErrorMessage"] as string))
                    ErrorMessage = TempData["ErrorMessage"] as string;

                var ut = _dao.GetDatiUtenteFromUserName(UserName);
                if (string.IsNullOrEmpty(ut.Alias))
                    throw new Exception("Alias Utente non indicato!");
                if (string.IsNullOrEmpty(ut.NomeCompleto))
                    throw new Exception("Nome Utente non indicato!");

                if (!string.IsNullOrEmpty(SignRoom))
                {

                    var signRoomPath = Path.Combine(_signRoomBasePath, SignRoom);
                    var files = _signRoomDao.GetSignRoomFiles(signRoomPath, this.UserName).ToArray();//Directory.GetFiles(signRoomPath);
                    ElencoFile = new List<FileListVM>();
                    if (Directory.Exists(signRoomPath))
                    {
                        //foreach (var fileInfo in files)
                        //{
                        //    var isErasable = false;
                        //    ElencoFile.Add(new FileListVM(
                        //        /*non viene passato nell uploadFile*/"",
                        //        fileInfo.Descrizione/*Descrizione*/,
                        //        Path.GetExtension(Utility.RemoveDocIdPattern(fileInfo.TempFileName)).Replace(".", "")/*tipo(estensione)*/,
                        //        (decimal.Round((Decimal)fileInfo.Size / 1024 / 1024, 2)).ToString(),
                        //        ""/*non viene passato nell uploadFile*/,
                        //        isErasable
                        //    ));
                        //}
                    }
                    else
                        throw new Exception("Directory signRoom non trovata!");

                    var sr = _signRoomDao.GetSignRoom(SignRoom ?? "", UserName);
                    if (sr == null)
                        throw new Exception("SignRoom non trovata!");
                    if (sr.Status == null) _signRoomDao.UpdateSignRoomStatus(SignRoom, SignRoomStatus.Created);

                    if (sr.Status != SignRoomStatus.ReadyToSign && sr.Status != SignRoomStatus.FileUploaded && sr.Status != SignRoomStatus.Signing)
                        return RedirectToPage("Loading", new { SignRoom });
                    if (!string.IsNullOrEmpty(sr.StatusComment) && sr.Status != SignRoomStatus.Signing)
                    {
                        ErrorMessage = sr.StatusComment;
                        _signRoomDao.UpdateSignRoomStatus(SignRoom, SignRoomStatus.ReadyToSign, "");
                    }

                    sr.UserName = UserName;
                    sr.NumeroFile = files.Length;
                    sr.Status = SignRoomStatus.ReadyToSign;
                    _signRoomDao.AddOrUpdateSignRoom(sr);
                }

                if (Wait) _remoteSign.SendOTP(ut.Alias);
                WaitOTP = Wait;
                Alias = ut.Alias;
                this.UserName = ut.UserName;
                this.SignRoom = SignRoom;

            }
            catch (Exception ex)
            {
                _logger.LogError("Index -> OnGet: " + ex.Message);
                ErrorMessage = ex.Message;
            }
            return Page();
        }

        public async Task<IActionResult> OnPost(string UserName, string SignRoom, string pin, string otp)
        {
            _logger.LogInformation("Index -> OnPostSign: " + UserName + ";" + SignRoom + ";" + pin + ";" + otp);
            try
            {
                var sr = _signRoomDao.GetSignRoom(SignRoom, UserName);
                if (sr.Status != SignRoomStatus.ReadyToSign || sr.Status != SignRoomStatus.FileUploaded)
                    RedirectToPage("Loading", new { signRoom = SignRoom });

                var ut = _dao.GetDatiUtenteFromUserName(UserName);

                if (string.IsNullOrEmpty(UserName))
                    throw new Exception("Username non trovato");

                if (string.IsNullOrEmpty(SignRoom))
                    throw new Exception("SignRoom non trovata");

                if (String.IsNullOrEmpty(pin))
                    throw new ArgumentException("PIN non fornito");

                if (String.IsNullOrEmpty(otp))
                    throw new ArgumentException("OTP non fornito");


                var signRoomPath = Path.Combine(_signRoomBasePath, SignRoom);
                if (Directory.Exists(signRoomPath))
                {
                    var files = Directory.GetFiles(signRoomPath);
                    if (files.Length == 0)
                    {
                        _signController.ClearSignRoom(SignRoom, "Sessione di firma annullata per mancanza di file da firmare.");
                        throw new Exception("Non ci sono file da firmare la sessione di firma sarà annullata.");
                    }

                    BackgroundJob.Enqueue(() => _sign.SignFile(SignRoom, pin, otp, ut, signRoomPath, UserName));
                }
                else
                    throw new Exception("Directory signRoom non trovata!");
                _signRoomDao.UpdateSignRoomStatus(SignRoom, SignRoomStatus.Signing); //aggiorno lo status in fase di firma

                return RedirectToPage("Loading", new { signRoom = SignRoom });
            }
            catch (Exception ex)
            {
                _logger.LogError("Index -> OnPostSign: " + ex.Message);
                ErrorMessage = ex.Message;
                TempData["ErrorMessage"] = ErrorMessage;
                return RedirectToPage("Index", new { SignRoom, UserName, Wait = false });
            }
        }

        //public void SignFile(string SignRoom, string pin, string otp, UserModel ut, string signRoomPath, string UserName)
        //{
        //    try
        //    {
        //        var sr = _signRoomDao.GetSignRoom(SignRoom);

        //        var elencoFP = FileManagerUtils.GetAllFileParameter(signRoomPath);
        //        var elencoF2Sign = FileManagerUtils.GetAllFileToSign(signRoomPath);

        //        foreach (var fl in elencoF2Sign)
        //            _remoteSign.EnqueueDocument(Path.GetFileName(fl), System.IO.File.ReadAllBytes(fl));
        //        (byte[] Result, string FileName) fileFirmati = new();

        //        switch (sr.SignType)
        //        {
        //            case SignTypes.Cades:
        //                fileFirmati = _remoteSign.MultiSignCades(ut.Alias, pin, otp).Result;
        //                break;
        //            case SignTypes.Pades:
        //                var fileParameter = elencoFP.FirstOrDefault(p => p.TempFileName == Path.GetFileName(elencoF2Sign.First()));
        //                if (fileParameter == null || fileParameter.CampoFirma == null)
        //                    throw new Exception("Parametri di campo firma non passati.");

        //                fileFirmati = _remoteSign.MultiSignPades(ut.Alias, pin, otp, fileParameter.CampoFirma).Result;

        //                break;
        //            case SignTypes.Xades:
        //                throw new NotImplementedException("Tipo di firma non disponibile.");
        //            default:
        //                throw new Exception("Tipo di firma non pervenuto.");
        //        }

        //        _signRoomDao.UpdateSignRoomStatus(SignRoom, SignRoomStatus.Signed); //aggiorno lo status firma ottenuta

        //        //salvataggio del file firmato
        //        fileFirmati.FileName = fileFirmati.FileName.Replace("\"", "");
        //        //int DotIndex = fileFirmati.FileName.IndexOf('.');
        //        //int Lenght = fileFirmati.FileName.Length - DotIndex;
        //        //var ext = (DotIndex >= 0) ? fileFirmati.FileName.Substring(DotIndex, Lenght) : string.Empty;
        //        var ext = GetFirstExtension(fileFirmati.FileName);

        //        var FileName = Path.Combine(_signRoomBasePath, SignRoom, SignRoom + ext);

        //        System.IO.File.WriteAllBytes(FileName, fileFirmati.Result); // salvo il file dentro la cartella della signroom

        //        string retUrl = _config["ReturnUrl"] ?? "";
        //        if (!retUrl.Contains("/RemoteSignHandler/ReceiveAckSignedFile"))
        //        {
        //            if (!retUrl.EndsWith("/"))
        //                retUrl += "/";
        //            retUrl += "RemoteSignHandler/ReceiveAckSignedFile";
        //        }
        //        //"/RemoteSignHandler/ReceiveAckSignedFile",
        //        //"AbortUrl": "https://localhost:7001/RemoteSignHandler/AbortSignatureSession"


        //        string url = retUrl + "/" + SignRoom + "/" + UserName;

        //        using (HttpClient httpClient = new HttpClient())
        //        {
        //            HttpResponseMessage response = httpClient.GetAsync(url).Result;

        //            if (response.IsSuccessStatusCode)
        //            {
        //                string esito = response.Content.ReadAsStringAsync().Result;
        //                _signRoomDao.ConfirmDelivery(SignRoom, esito);
        //                _signRoomDao.UpdateSignRoomStatus(SignRoom, SignRoomStatus.Completed);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Index -> SignFile: " + ex.Message);
        //        if (_signRoomDao.GetSignRoom(SignRoom).Status == SignRoomStatus.Signing) //caso di firma fallita 
        //        {
        //            var Message = ex.Message;

        //            var index = Message.IndexOf("ERROR-SIGNATURE -");

        //            Message = Message.Substring(index + 17);

        //            _signRoomDao.UpdateSignRoomStatus(SignRoom, SignRoomStatus.ReadyToSign, "Firma fallita. Errore: " + Message); //lo rimetto a ReadyToSign per fargli ritentare la firma
        //        }
        //        else
        //            _signController.ClearSignRoom(SignRoom, "Processo di firma fallito. Errore: " + ex.Message);
        //    }
        //}

        public async Task<IActionResult> OnPostSendOTP(string SignRoom, string UserName)
        {
            try
            {
                var ut = _dao.GetDatiUtenteFromUserName(UserName);
                await _remoteSign.SendOTP(ut.Alias);
                return RedirectToPage("Index", new { SignRoom, UserName, Wait = true });
            }
            catch (Exception ex)
            {
                _logger.LogError("Index -> OnPostSendOTP: " + ex.Message);
                ErrorMessage = ex.Message;
                TempData["ErrorMessage"] = ErrorMessage;
                return RedirectToPage("Index", new { SignRoom, UserName, Wait = false });
            }
        }

        private string GetFirstExtension(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("Il nome del file non può essere vuoto o nullo.", nameof(fileName));
            }

            // Dividi il nome del file in base al carattere '.'
            string[] parts = fileName.Split('.');

            // Se ci sono almeno due parti (nome del file e almeno un'estensione)
            if (parts.Length >= 2)
            {
                // Prendi la seconda parte come estensione
                string firstExtension = "." + parts[1];
                return firstExtension;
            }
            else
                throw new Exception("Non ci sono estensioni in questo fileName.");
        }

        private bool CanUsePades(List<string> extensions)
        {
            return extensions.All(extension => extension.Equals(".pdf"));
        }

    }
}