using FirmeRemoteLib.Interfaces;
using Microsoft.AspNetCore.Mvc;
using RemoteSignInfocert.Controllers;
using RemoteSignInfocert.DAO;
using RemoteSignInfocert.Interfaces;
using RemoteSignInfocert.Models;
using RemoteSignInfocert.Utils;
using System.ComponentModel;
using System.IO.Compression;
using System.Net.Http.Headers;
using static System.Net.WebRequestMethods;

namespace RemoteSign.BL
{
    public class SignService
    {
        private readonly IRemoteSign _remoteSign;
        private readonly ILogger<SignService> _logger;
        private const string SIGN_ROOMS = "SignRooms";
        private string _signRoomBasePath = "";
        private readonly ISignRoomDAO _signRoomDao;
        private readonly IUserDAO _userDao;
        private readonly IWebHostEnvironment _hostEnv;
        private readonly IConfiguration _config;


        public SignService(
            ISignRoomDAO signRoomDao,
            IUserDAO userDao,
            IRemoteSign remoteSign, 
            ILogger<SignService> logger, 
            IWebHostEnvironment hostEnv, 
            IConfiguration config)
        {
            _hostEnv = hostEnv;
            _signRoomBasePath = Path.Combine(_hostEnv.ContentRootPath, SIGN_ROOMS);
            if (!Directory.Exists(_signRoomBasePath))
                Directory.CreateDirectory(_signRoomBasePath);
            _signRoomDao = signRoomDao;
            _userDao = userDao;
            _remoteSign = remoteSign;
            _logger = logger;
            _config = config;
        }

        public string CreateSignRoom(string UserName, SignTypes signType)
        {
            string _guid = Guid.NewGuid().ToString();
            var dirRoom = Path.Combine(_signRoomBasePath, _guid);
            if (!Directory.Exists(dirRoom))
            {
                Directory.CreateDirectory(dirRoom);
                _signRoomDao.AddOrUpdateSignRoom(new SignRoomModel() { SignRoom = _guid, Status = SignRoomStatus.Created, UserName = UserName, SignType = signType });
            }
            return _guid;
        }


        public async Task CheckForExpiredSignRoom()
        {
            try
            {
                //chiudo e cancello la cartella per tutte le signroom ancora da consegnare da più di 24h
                var signRoomsToDelivery = _signRoomDao.GetSignRoomsToDelivery();
                foreach (var signRoom in signRoomsToDelivery)
                {
                    var Now = DateTime.UtcNow;
                    var delta = Now - signRoom.CreationDate;
                    if (delta.TotalDays > 1)
                        ClearSignRoom(signRoom.SignRoom, "Sessione cancellata per scadenza.");
                }

                //cancello la cartella per tutte le signroom vecchie più di 7 giorni
                var signRooms = _signRoomDao.GetAllSignRooms();
                foreach (var signRoom in signRoomsToDelivery)
                {
                    var Now = DateTime.UtcNow;
                    var delta = Now - signRoom.CreationDate;
                    if (delta.TotalDays > 7)
                    {
                        try
                        {
                            DeleteSignroomDirectory(signRoom.SignRoom);
                        }
                        catch (Exception ex) { }

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }

        public async Task CheckForDelivery()
        {
            try
            {

                var signRoomsToDelivery = _signRoomDao.GetSignRoomsToDelivery();
                foreach (var signRoom in signRoomsToDelivery)
                {
                    if (string.IsNullOrEmpty(signRoom.UserName))
                    {
                        _signRoomDao.ConfirmDelivery(signRoom.SignRoom, "Username non pervenuto");
                        continue;
                    }
                    await CallReceiver(signRoom);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }


        private async Task CallReceiver(SignRoomModel SignRoom)
        {
            string retUrl = SignRoom.ReturnURL ?? "";
            if (!retUrl.Contains("/RemoteSignHandler/ReceiveAckSignedFile"))
            {
                if (!retUrl.EndsWith("/"))
                    retUrl += "/";
                retUrl += "RemoteSignHandler/ReceiveAckSignedFile";
            }

            string url = retUrl + "/" + SignRoom.SignRoom + "/" + SignRoom.UserName;

            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string esito = await response.Content.ReadAsStringAsync();
                    ClearSignRoom(SignRoom.SignRoom, "Sessione cancellata per scadenza.");
                    _signRoomDao.ConfirmDelivery(SignRoom.SignRoom, esito);
                }
            }
        }

        public async Task<bool> AddFile (string SignRoom, IFormFile file, string labelCampoFirma = "")
        {
            try
            {
                var signRoomPath = Path.Combine(_signRoomBasePath, SignRoom);

                var sr = _signRoomDao.GetSignRoom(SignRoom);
                if (!Directory.Exists(signRoomPath) || sr == null)
                    throw new Exception($"Sessione di Firma '{SignRoom}' non trovata!");

                if (file == null || file.Length == 0)
                    throw new Exception($"File vuoto su sessione di Firma '{SignRoom}'");

                var MaxFileSizeMB = 200/*MB*/ * 1024 * 1024; //conversione in Bytes
                if (file.Length > MaxFileSizeMB)
                    throw new Exception($"La grandezza del file ({file.Length}) eccede il limite massimo ({MaxFileSizeMB}) per la sessione di firma '{SignRoom}'");

                var contentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                var fileParameter = new FileParameter();
                fileParameter.OriginalFileName = file.FileName;
                var flnamejson = Path.Combine(signRoomPath, fileParameter.OriginalFileName + FileManagerUtils.FILE_PARAMETER_EXT);
                if (!string.IsNullOrEmpty(labelCampoFirma))
                    fileParameter.CampoFirma = new FirmeRemoteLib.Models.FirmaPades() { SignField = labelCampoFirma };
                FileManagerUtils.WriteFileParameter(flnamejson, fileParameter);

                var flname = Path.Combine(signRoomPath, file.FileName);
                using (var fileStream = new FileStream(flname, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                sr.NumeroFile = (sr.NumeroFile == null) ? 1 : sr.NumeroFile + 1;
                sr.Status = SignRoomStatus.FileUploaded;
                _signRoomDao.AddOrUpdateSignRoom(sr);
                return true;
            }
            catch (Exception ex)
            {
                ClearSignRoom(SignRoom, "Upload file fallito. Errore: " + ex.Message);
                throw;
            }
        }

        public bool ClearSignRoom(string SignRoom, string? esito = "")
        {
                var dirname = Path.Combine(_signRoomBasePath, SignRoom);
                if (!Directory.Exists(dirname)) return true;
                foreach (var fl in Directory.GetFiles(dirname))
                    System.IO.File.Delete(fl);
                Directory.Delete(dirname);

                _signRoomDao.UpdateSignRoomStatus(SignRoom, SignRoomStatus.Aborted, esito);
                _signRoomDao.ConfirmDelivery(SignRoom, esito);

                return true;
        }

        public bool DeleteSignroomDirectory(string SignRoom)
        {
            var dirname = Path.Combine(_signRoomBasePath, SignRoom);
            if (!Directory.Exists(dirname))
                return true;

            foreach (var fl in Directory.GetFiles(dirname))
                System.IO.File.Delete(fl);
            Directory.Delete(dirname);

            return true;
        }

        public bool DeleteSingleFile(string SignRoom, string FileName)
        {
            try
            {
                var dirname = Path.Combine(_signRoomBasePath, SignRoom);
                if (!Directory.Exists(dirname))
                    throw new Exception("Sign room non trovata!");
                var fullfilename = Path.Combine(dirname, FileName);
                if (System.IO.File.Exists(fullfilename))
                    System.IO.File.Delete(fullfilename);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public bool CheckDelivery(string SignRoom)
        {
            return _signRoomDao.GetSignRoom(SignRoom).Delivered;
        }


        public bool CheckUserInfo(string username)
        {
            return (_userDao.GetDatiUtenteFromUserName(username) != null) ? true : false;
        }


        public bool CheckSignRoom(string SignRoom, string UserName)
        {
            //se esiste l'entità
            var sr = _signRoomDao.GetSignRoom(SignRoom, UserName);
            if (sr == null)
                return false;

            //se esiste la cartella della signroom
            var dirname = Path.Combine(_signRoomBasePath, SignRoom);
            if (!Directory.Exists(dirname))
                return false;

            if (sr.Status == SignRoomStatus.Created)
                return false;

            return true;
        }


        public string CheckDeliveryResult(string SignRoom)
        {

            if (_signRoomDao.GetSignRoom(SignRoom) == null)
                return "La sessione di firma non trovata.";
            if (!_signRoomDao.GetSignRoom(SignRoom).Delivered)
                return "La sessione di firma non ha avuto ancora esito.";

            return _signRoomDao.GetSignRoom(SignRoom).DeliveryResult;
        }

        public string CheckStatus(string SignRoom)
        {
            var sr = _signRoomDao.GetSignRoom(SignRoom);
            var result = SignRoomModel.GetDescription(sr.Status);

            return result;
        }

        public SignRoomModel GetSignRoom(string SignRoom)
        {
            return _signRoomDao.GetSignRoom(SignRoom);
        }

        public bool CheckSigned(string SignRoom)
        {
            var dirname = Path.Combine(_signRoomBasePath, SignRoom);

            if (!Directory.Exists(dirname))
                return false;

            return Directory.GetFiles(dirname).Any(p =>
            Path.GetFileNameWithoutExtension(p.Replace(".p7m", "")).Equals(SignRoom, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<string> SendOTP(string SignRoom, string UserName)
        {
            try
            {
                var sr = _signRoomDao.GetSignRoom(SignRoom, UserName);
                if (sr == null)
                    return ("Nessuna Sessione trovata con "+SignRoom);
                if (sr.Status != SignRoomStatus.FileUploaded && sr.Status != SignRoomStatus.ReadyToSign)
                    return ("Nessun File Caricato");

                var ut = _userDao.GetDatiUtenteFromUserName(UserName);
                await _remoteSign.SendOTP(ut.Alias);
                _signRoomDao.UpdateSignRoomStatus(SignRoom, SignRoomStatus.ReadyToSign); //aggiorno lo status in fase di firma
                return "";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"SendOTP({SignRoom},{UserName})");
                return ex.Message;
            }
        }


        public byte[] GetSignedFile(string SignRoom)
        {
            var dirname = Path.Combine(_signRoomBasePath, SignRoom);
            if (CheckSigned(SignRoom))
            {
                var flname = Directory.GetFiles(dirname).FirstOrDefault(f => Path.GetFileNameWithoutExtension(f).Equals(SignRoom, StringComparison.InvariantCultureIgnoreCase));
                if (System.IO.File.Exists(flname))
                {
                    return  System.IO.File.ReadAllBytes(flname);
                }
                else
                    throw new Exception($"File '{flname}' non trovato");
            }
            throw new Exception($"Sessione di firma '{SignRoom}' non trovata");
        }

        public string GetSignedFileName(string SignRoom)
        {
            var dirname = Path.Combine(_signRoomBasePath, SignRoom);
            return Directory.GetFiles(dirname).FirstOrDefault(f => Path.GetFileNameWithoutExtension(f).Equals(SignRoom, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task SignFile(string SignRoom, string pin, string otp)
        {
            try
            {
                if (string.IsNullOrEmpty(SignRoom))
                    throw new Exception("SignRoom non trovata");
                if (String.IsNullOrEmpty(pin))
                    throw new ArgumentException("PIN non fornito");
                if (String.IsNullOrEmpty(otp))
                    throw new ArgumentException("OTP non fornito");


                var sr = _signRoomDao.GetSignRoom(SignRoom);
                var ut = _userDao.GetDatiUtenteFromUserName (sr.UserName);
                _signRoomDao.UpdateSignRoomStatus(SignRoom, SignRoomStatus.Signing); //aggiorno lo status in fase di firma

                var signRoomPath = Path.Combine(_signRoomBasePath, SignRoom);
                var elencoFP = FileManagerUtils.GetAllFileParameter(signRoomPath);
                var elencoF2Sign = FileManagerUtils.GetAllFileToSign(signRoomPath);

                foreach (var fl in elencoF2Sign)
                    _remoteSign.EnqueueDocument(Path.GetFileName(fl), System.IO.File.ReadAllBytes(fl));

                (byte[] Result, string FileName) fileFirmati = new();

                switch (sr.SignType)
                {
                    case SignTypes.Cades:
                        fileFirmati = await _remoteSign.MultiSignCades(ut.Alias, pin, otp);
                        break;
                    case SignTypes.Pades:
                        var fileParameter = elencoFP.FirstOrDefault(p => p.OriginalFileName == Path.GetFileName(elencoF2Sign.First()));
                        if (fileParameter == null)
                            throw new Exception("Parametri di firma non trovati.");
                        if (fileParameter.CampoFirma == null)
                            throw new Exception("Campo firma non trovato");

                        fileFirmati = await _remoteSign.MultiSignPades(ut.Alias, pin, otp, fileParameter.CampoFirma);

                        break;
                    case SignTypes.Xades:
                        throw new NotImplementedException("Tipo di firma non disponibile.");
                    default:
                        throw new Exception("Tipo di firma non pervenuto.");
                }

                _signRoomDao.UpdateSignRoomStatus(SignRoom, SignRoomStatus.Signed); //aggiorno lo status firma ottenuta

                //salvataggio del file firmato
                fileFirmati.FileName = fileFirmati.FileName.Replace("\"", "");
                var ext = GetFirstExtension(fileFirmati.FileName);

                var FileName = Path.Combine(signRoomPath, SignRoom + ext);

                System.IO.File.WriteAllBytes(FileName, fileFirmati.Result); // salvo il file dentro la cartella della signroom

                string retUrl = _config["ReturnUrl"] ?? "";
                if (!retUrl.Contains("/RemoteSignHandler/ReceiveAckSignedFile"))
                { 
                    if (!retUrl.EndsWith("/"))
                        retUrl += "/";
                    retUrl += "RemoteSignHandler/ReceiveAckSignedFile";
                }
                //"/RemoteSignHandler/ReceiveAckSignedFile",
                //"AbortUrl": "https://localhost:7001/RemoteSignHandler/AbortSignatureSession"


                string url = retUrl + "/" + SignRoom + "/" + sr.UserName;
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage response = httpClient.GetAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string esito = response.Content.ReadAsStringAsync().Result;
                        _signRoomDao.ConfirmDelivery(SignRoom, esito);
                        _signRoomDao.UpdateSignRoomStatus(SignRoom, SignRoomStatus.Completed);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sign");
                _signRoomDao.UpdateSignRoomStatus(SignRoom, SignRoomStatus.Failed, "Firma fallita. Errore: " + ex.Message); 
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

        private List<(byte[] Data, string FileName)> ExtractFilesFromZip(byte[] zipData)
        {
            List<(byte[] Data, string FileName)> extractedFiles = new List<(byte[] Data, string FileName)>();

            string tempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempFolder);

            var zipDestination = Path.Combine(tempFolder, Guid.NewGuid().ToString() + ".zip");

            System.IO.File.WriteAllBytes(zipDestination, zipData);

            using (var inputStream = new MemoryStream(zipData))
            using (var archive = new ZipArchive(inputStream, ZipArchiveMode.Read))
            {
                foreach (var entry in archive.Entries)
                {
                    using (var entryStream = entry.Open())
                    using (var memoryStream = new MemoryStream())
                    {
                        entryStream.CopyTo(memoryStream);
                        extractedFiles.Add((memoryStream.ToArray(), entry.Name));
                    }
                }
            }

            return extractedFiles;
        }


    }
}
