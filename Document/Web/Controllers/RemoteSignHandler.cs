using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;
using System.IO.Compression;

namespace Web.Controllers;


/// <summary>
/// Controller per esporre l'endpoint di ricezione del file firmato da parte delle app di firma digitale remota
/// </summary>
[ApiController]
[Route("[controller]/[action]")]
public class RemoteSignHandlerController : ControllerBase
{

    private readonly IDocumentService _docService;
    private readonly IUserService _userService;
    private readonly IConfiguration _config;
    private UserProfile userProfile { get; set; } = UserProfile.SystemUser();



    private class DownloadResponse
    {
        public bool IsZip { get; set; } = false;
        public bool IsPades { get; set; } = false;
        public string FileName { get; set; } = "";
        public string Provider { get; set; } = "";
        public string SignRoom { get; set; } = "";
        public byte[]? Data { get; set; } = null;

    }


    public RemoteSignHandlerController(
        IDocumentService docService,
        IConfiguration config,
        IUserService userService)
    {
        this._docService = docService;
        _userService = userService;
        _config = config;

 

    }

    // Richiamato dal servizio di firma per restituire tutti i documenti di una sessione di firma
    [HttpGet("{SignatureSession}/{UserName}")]
    public async Task<ActionResult<string>> ReceiveAckSignedFile(string SignatureSession, string UserName)
    {
        try
        {
            //recupero del provider
            var Provider = await _userService.GetAttribute(UserName, UserAttributes.CONST_REMOTESIGNATURE_SERVICE);
            if (String.IsNullOrEmpty(Provider)) throw new Exception($"Provider di firma non è stato trovato. Impossibile gestire la sessione '{SignatureSession}' per l'utente '{UserName}'");

            //Recupero l'Id dei file inviati alla SignRoom
            var SignatureSessionImagesId = await _docService.GetImagesBySignatureSession(UserName, SignatureSession);
            if (SignatureSessionImagesId.Count <= 0) return $"Nessun documento trovato per la sessione '{SignatureSession}'";


            //eseguo il download del file firmato
            var downloadResponse = await ApiDownloadSignedFile(SignatureSession, Provider);

            // gestisco i file firmati scaricati
            string esito = "";
            if (downloadResponse.Data != null)
            {
                if (downloadResponse.IsZip)
                    esito = await GestisciZip(downloadResponse, SignatureSessionImagesId);
                else
                    esito = await AddSignedContentFromSignedFile(downloadResponse, SignatureSessionImagesId[0]);
            }
            else
            {
                throw new Exception($"Nessun file scaricato per la sessione '{SignatureSession}'");
            }
            return esito;
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }


    // Invia un nuovo OTP
    [HttpPost("{SignatureSession}/{UserName}")]
    public async Task<IActionResult> SendOTP([FromRoute] string SignatureSession, [FromRoute] string UserName)
    {
        //recupero del provider
        var Provider = await _userService.GetAttribute(UserName, UserAttributes.CONST_REMOTESIGNATURE_SERVICE);
        if (String.IsNullOrEmpty(Provider)) throw new Exception($"Provider di firma non è stato trovato. Impossibile gestire la sessione '{SignatureSession}' per l'utente '{UserName}'");

        string url = _config["ExternalPages:RemoteSign:" + Provider] + "/Sign/SendOTP/" + SignatureSession+"/"+UserName;
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(url, null);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(response.ReasonPhrase);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
        return Ok();
    }


    [HttpPost("{SignatureSession}/{UserName}")]
    public async Task<ActionResult<string>> AbortSignatureSession(string SignatureSession, string UserName)
    {
        try
        {
           var  esito = "";
            var user = await _userService.GetUserProfile(UserName);
            var signatureSessionImagesId = await _docService.GetImagesBySignatureSession(UserName, SignatureSession);
            if (signatureSessionImagesId.Count <= 0) esito =  $"Nessun documento trovato per la sessione '{SignatureSession}'";

            foreach (var imgId in signatureSessionImagesId)
                await _docService.UpdateSignatureStatus(imgId, JobStatus.Aborted, user, SignatureSession);

            return Ok(esito);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }


  

    #region metodi composti

    private async Task<string> GestisciZip(DownloadResponse response, List<int> SignatureImages)
    {
        var UnsavedFiles = 0;
        var FilesInError = new List<int>();
        var ImagesFound = false;

        var SignedFiles = ExtractFilesFromZip(response.Data!);

        //carico le nuove versioni firmate sui file corrispondenti
        foreach (var content in SignedFiles)
        {
            var imageId = GetIdFromFileName(content.FileName);
            if (imageId > 0)
            {
                DownloadResponse singleFile = new DownloadResponse()
                {
                    Data = content.Data,
                    FileName = response.FileName,
                    Provider = response.Provider,
                    SignRoom = response.SignRoom,
                    IsPades = response.IsPades
                };

                ImagesFound = true;
                try
                {
                    await AddSignedContentFromSignedFile(singleFile, imageId);
                }
                catch (Exception ex)
                {
                    UnsavedFiles +=1 ;
                    FilesInError.Add(imageId);
                }
            };

        }

        if (!ImagesFound)
            return ($"I file firmati ricevuti non corrispondono agli ID dei file inviati alla sessione '{response.SignRoom}'");
        if (UnsavedFiles > 0)
        {
            var esito = "I seguenti ID di file non sono stati aggiornati:";
            foreach (var DocEsito in FilesInError)
                esito += $" {DocEsito}";
            return esito;
        }
        return "";
    }

    #endregion

    #region metodi api
    private async Task<DownloadResponse> ApiDownloadSignedFile(string SignatureSession, string Provider)
    {
        DownloadResponse result = new();
        result.SignRoom = SignatureSession;
        result.Provider = Provider;

        string url = _config["ExternalPages:RemoteSign:" + Provider] + "/Sign/DownloadSignedFile/" + SignatureSession;
        using var httpClient = new HttpClient();
        var response = await httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            result.Data = await response.Content.ReadAsByteArrayAsync();
            //controllo se il DownloadedFile è un zip tramite un header personalizzato passato dalla api /Sign/DownloadSignedFile
            IEnumerable<string>? values = new List<string>();
            result.IsZip = response.Headers.TryGetValues("IsZip", out values);
            result.IsPades = response.Headers.TryGetValues("IsPades", out values);
        }
        else
            throw new Exception($"Errore durante il download del file della sessione '{SignatureSession}': {response.StatusCode} - {response.ToString()}");

        return result;
    }



    //private async Task<bool> ApiClearSignRoom(string esito,string SignatureSession, string Provider)
    //{
    //    string urlClearRoom = $"{_config["ExternalPages:RemoteSign:" + Provider]}/Sign/ClearSignRoom?SignRoom={SignatureSession}&esito={esito}";
    //    using (HttpClient httpClient2 = new HttpClient())
    //    {
    //        var CrResponse = await httpClient2.DeleteAsync(urlClearRoom);

    //        if (CrResponse.IsSuccessStatusCode)
    //            return true;
    //        else
    //            return false;
    //        //throw new Exception($"Errore durante la chiusura della sessione di firma");
    //    }
    //}

    //private async Task<bool> RemoveBlankSignField(int docId, UserProfile userProfile, string FieldName)
    //{
    //    var docInfo = await _docService.Load(docId, userProfile);

    //    return await _docService.RemoveBlankSignField(docInfo, userProfile, FieldName);
    //}

    #endregion

    #region metodi utili

    /// <summary>
    /// Aggiunge il documento firmato al doc. Nota il docId è contenuto nel filename 
    /// (inserito in fase di UploadFile alla richiesta di firma vedi AddRemoteSign:OnPost)
    /// </summary>
    /// <param name="data"> Byte array del file .p7m </param>
    /// <param name="fileName">nome del file che contiene il docId </param>
    /// <returns></returns>
    private async Task<string> AddSignedContentFromSignedFile(DownloadResponse response, int imageId)
    {
        var docIdList = await _docService.GetDocumentsFromContentId(imageId, userProfile);
        var imageInfo = (await _docService.GetContentInfo(imageId));

        var DataBase64 = Convert.ToBase64String(response.Data);

        // Se il file non è PADES aggiungo l'estensione P7M al nome file originale
        var ext = response.IsPades ? "" : ".p7m";
        var NewFileName = Path.GetFileName(imageInfo.FileName) + ext;

        var fileContent = new FileContent()
        {
            DataIsInBase64 = true,
            FileData = DataBase64,
            FileName = NewFileName,
            LinkToContent = false
        };
        var u = await _userService.GetUserProfile(imageInfo.SignatureUser);
        try
        {
            foreach (var docId in docIdList)
            {
                var image = await _docService.AddContent(docId, userProfile, fileContent, false);
            }
            await _docService.UpdateSignatureStatus(imageId, JobStatus.Completed, u, response.SignRoom);
        }
        catch (Exception ex) {
            await _docService.UpdateSignatureStatus(imageId, JobStatus.Aborted, u, response.SignRoom);
            throw;
        }

        return "";

    }

    /// <summary>
    /// Controllo quali immagini si trovano nello stato di running (esito: TRUE)
    /// e aspettano un file firmato
    /// </summary>
    /// <param name="images"></param>
    /// <returns>elenco di immagini che aspettano un file firmato</returns>
    //private async Task<List<int>> CheckImageListAreInStatusRunningAndSetSignType(List<int> images)
    //{
    //    //        var status = ""; 
    //    var extensions = new List<int>() { };
    //    foreach (var imageId in images)
    //    {
    //        var imageInfo = (await _docService.GetContentInfo(imageId));
    //        if (imageInfo.SignatureStatus == JobStatus.Running)
    //            extensions.Add(imageId);
    //        //                status.Concat($" {imageInfo.SignatureStatus} ; ");
    //        //            extensions.Add(ControllerUtility.GetFirstExtension(Path.GetFileName(imageInfo.FileName)));
    //    }

    //    return extensions;
    //}

    private List<(byte[] Data, string FileName)> ExtractFilesFromZip(byte[] zipData)
    {
        List<(byte[] Data, string FileName)> extractedFiles = new List<(byte[] Data, string FileName)>();

        //string tempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        //Directory.CreateDirectory(tempFolder);
        //var zipDestination = Path.Combine(tempFolder, Guid.NewGuid().ToString() + ".zip");
        //System.IO.File.WriteAllBytes(zipDestination, zipData);

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


    private int GetIdFromFileName(string fileName)
    {
        var sid = Path.GetFileNameWithoutExtension(fileName);
        int id = 0;
        int.TryParse(sid, out id);
        return id;
    }

    #endregion
}

