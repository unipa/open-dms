using OpenDMS.MaximoSR.API.BL.Interfacce;
using OpenDMS.MaximoSR.API.Models;
using OpenDMS.MaximoSR.API.Utility;
using OpenDMS.MaximoSR.API.Utility.Interfacce;
using System.Text.Json;

namespace OpenDMS.MaximoSR.API.BL
{
    public class MaximoSRBL : IMaximoSRBL
    {
        private readonly IConfiguration _config;
        private readonly IZeebeHandler _zeebe;
        public MaximoSRBL(IConfiguration config, IZeebeHandler zeebe)
        {
            _config = config;
            _zeebe = zeebe;
        }

        public async Task<string> GetToken()
        {
            try
            {
                var url = $"{_config["Keycloak:auth-server-url"]}/realms/{_config["Keycloak:realm"]}/protocol/openid-connect/token";
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


        public async Task<List<ASSET>> GetAssetListFromMaximo()
        {
            try
            {
                string token = await GetToken();
                string url = _config["Endpoint:ASSET_API_URL"];
                List<Tuple<string, string>> headers = new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("Authorization", "Bearer " + token),
                    new Tuple<string, string>("Accept", "*/*"),
                };
                var response = await HttpCallHandler.GetAsyncCall(url, null, headers);
                string json = await response.Content.ReadAsStringAsync();

                // Deserializzazione del JSON
                QueryMXASSET_DMSResponse mxasset_list = JsonSerializer.Deserialize<QueryMXASSET_DMSResponse>(json);
                List<ASSET> assets = mxasset_list.MXASSET_DMSSetWrapper.MXASSET_DMSSet.ASSET;

                return assets;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore in MaximoSRBL nel metodo GetAssetListFromMaximo, errore: " + ex.Message);
            }

        }

        public void StartBpmnProcess(SR sr)
        {
            try
            {
                _zeebe.Main(sr);
                return;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
