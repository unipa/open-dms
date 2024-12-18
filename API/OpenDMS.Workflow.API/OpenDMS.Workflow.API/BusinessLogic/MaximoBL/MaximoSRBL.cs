using OpenDMS.Workflow.API.Utility;
using OpenDMS.Workflow.API.BusinessLogic.MaximoBL.Interfacce;
using OpenDMS.Workflow.API.DTOs.Maximo;
using System.Text.Json;
using System.Text;

namespace OpenDMS.Workflow.API.BusinessLogic.MaximoBL
{
    public class MaximoSRBL : IMaximoSRBL
    {
        private readonly IConfiguration _config;
        public MaximoSRBL(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> GetToken()
        {
            try
            {
                var url = $"{_config["Keycloak_maximo:auth-server-url"]}/realms/{_config["Keycloak:realm"]}/protocol/openid-connect/token";
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                HttpClient client = new HttpClient(clientHandler);
                var requestBody = new Dictionary<string, string>
                                {
                                    { "grant_type", "password" },
                                    { "client_id", _config["Keycloak:resource"] },
                                    { "client_secret", _config["Keycloak:credentials:secret"] },
                                    { "username", _config["Keycloak:credentials:username"] },
                                    { "password", _config["Keycloak:credentials:password"] },
                                };

                var requestContent = new FormUrlEncodedContent(requestBody);

                var response = await client.PostAsync(url, requestContent);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Failed to retrieve tokens. Status code: {response.StatusCode}. Response: {responseContent}");
                }

                var tokenResponse = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(responseContent);

                var accessToken = tokenResponse.GetProperty("access_token").GetString();

                return accessToken;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    Console.WriteLine("InnerException: " + ex.InnerException.Message);
                }
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> PostSrToMaximo(SR sr)
        {
            string token = await GetToken();
            string url = _config["Maximo:INSERT_SR_API_URL"];
            var content = new Dictionary<string, string>
                            {
                                { "ASSETNUM", sr.Assetnum },
                                { "ASSETORGID", sr.Assetorgid },
                                { "ASSETSITEID", sr.Assetsiteid },
                                { "DESCRIPTION", sr.Description },
                                { "REPORTEDPRIORITY", sr.Reportedpriority },
                                { "WORKTYPE", sr.Worktype },
                                { "STUDENTE", sr.Studente },
                                { "ORIGINE", sr.Origine },
                                { "IDDMS", sr.Iddms }
                             };
            List<Tuple<string, string>> headers = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("Authorization", "Bearer "+token),
                new Tuple<string, string>("Accept", "*/*"),
            };

            var response = await HttpCallHandler.PostAsyncCallUrlEncoded(url, content, headers);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    byte[] responseBytes = await response.Content.ReadAsByteArrayAsync();

                    // Converte il flusso di byte in una stringa utilizzando l'encoding appropriato (es. UTF-8)
                    string responseString = Encoding.UTF8.GetString(responseBytes);
                    return  responseString;
                }
                else
                {
                    throw new Exception("Rsponse="+response.StatusCode+ " - "+response.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Errore durante la chiamata API a Maximo: " + ex.Message);
            }
        }

    }
}
