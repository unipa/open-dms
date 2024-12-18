using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RemoteSignInfocert.Controllers;
using System.Text;

namespace RemoteSignInfocert.Pages
{
    public class DeletedModel : PageModel
    {
        private readonly ILogger<DeletedModel> _logger;
        private readonly IConfiguration _config;
        private readonly SignController _signController;

        public DeletedModel(SignController signController, ILogger<DeletedModel> logger, IConfiguration config)
        {
            _signController = signController;
            _logger = logger;
            _config = config;
        }

        public string signRoom { get; set; }
        public string responsePhrase { get; set; }
        public async void OnGet(string signRoom,string username, string? esito = null)
        {
            try
            {
                //mando la richiesta di annullamento sessione di firma a dms(web)
                using (HttpClient client = new HttpClient())
                {
                    string url = _config["AbortUrl"] ?? "";
                    if (!url.Contains("/RemoteSignHandler/AbortSignatureSession"))
                    {
                        if (!url.EndsWith("/"))
                            url += "/";
                        url += "RemoteSignHandler/AbortSignatureSession";
                    }
                    //"/RemoteSignHandler/ReceiveAckSignedFile",
                    //"AbortUrl": "https://localhost:7001/RemoteSignHandler/AbortSignatureSession"

                    HttpResponseMessage response = await client.PostAsync(url+"/"+signRoom+"/"+ username, null);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        _logger.LogInformation("Esito annullamento sessione di firma su app DMS(Web): " + responseContent);
                    }
                    else
                        _logger.LogError("Esito annullamento sessione di firma su app DMS(Web): " + response.StatusCode);
                }
                this.responsePhrase = esito;
                this.signRoom = signRoom;
                _signController.ClearSignRoom(signRoom,(esito ?? "Sessione di firma annullata"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
