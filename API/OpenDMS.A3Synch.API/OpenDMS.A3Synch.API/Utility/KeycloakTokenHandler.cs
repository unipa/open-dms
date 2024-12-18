using Newtonsoft.Json.Linq;
using OpenDMS.A3Synch.API.Utility;
using System.IdentityModel.Tokens.Jwt;

namespace Admin.Utility
{
    public class KeycloakTokenHandler
    {
        public KeycloakTokenHandler()
        {
        }

        /// <summary>
        /// Utilizzando un refresh token e le informazioni contenute nell'appsetting (config) chiama il servizio Keycloak per eseguire il 
        /// refresh dell'access_token e del "refresh token" stesso, settandoli automaticamente nell'HttpContext.
        /// </summary>
        /// <param name="refreshToken"> Refresh Token (in corso di validità)</param>
        /// <param name="config"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task RefreshTokenAsync(string refreshToken, IConfiguration config)
        {
            if (IsTokenExpired(refreshToken))
                throw new Exception("Error refresh token is expired.");

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient httpClient = new HttpClient(clientHandler);

            var endpoint = (String)config.GetValue(typeof(String), "Keycloak:auth-server-url");
            var realm = (String)config.GetValue(typeof(String), "Keycloak:realm");
            var client_id = (String)config.GetValue(typeof(String), "Keycloak:resource");
            var client_secret = (String)config.GetValue(typeof(String), "Keycloak:credentials:secret");

            var request = new HttpRequestMessage(HttpMethod.Post, endpoint + "/realms/" + realm + "/protocol/openid-connect/token");

            var parameters = new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", refreshToken },
                { "client_id", client_id },
                { "client_secret", client_secret }
            };

            request.Content = new FormUrlEncodedContent(parameters);
            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                // Handle error response
                throw new Exception("Error refreshing token.");
            }

            var tokenResponse = JObject.Parse(await response.Content.ReadAsStringAsync());
            var new_accessToken = tokenResponse.Value<string>("access_token");
            var new_refreshToken = tokenResponse.Value<string>("refresh_token");

            SharedVariables.kc_refresh_token = new_refreshToken;
            SharedVariables.kc_access_token = new_accessToken;
        }
        
        /// <summary>
        /// Esegue il controllo sulla scadenza dei token.
        /// Se l'access_token è valido ritorna true. 
        /// Se è scaduto prova a refreshare se non riesce esegue il logout e riporta al login.
        /// </summary>
        /// <returns></returns>
        public static async Task<string?> IsLoginStillValidAsync(IConfiguration config)
        {
            string access_token = SharedVariables.kc_access_token;
            if (String.IsNullOrEmpty(access_token)) throw new Exception("Empty Token.");

            //Se il token non è scaduto ritorno true
            if (!IsTokenExpired(access_token)) return access_token;

            var refresh_token = SharedVariables.kc_refresh_token;

            try
            {
                await RefreshTokenAsync(refresh_token, config);
                return SharedVariables.kc_access_token;
            }
            catch (Exception ex)
            {
                throw new Exception("Sessione scaduta, errore: " + ex.Message);
            }

        }
        
    

        /// <summary>
        /// Controlla la scandeza del token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns> True se è scaduto; False se è ancora valido</returns>
        private static bool IsTokenExpired(string token)
        {
            //leggo la scadenza
            var handler = new JwtSecurityTokenHandler();
            var DecodedToken = handler.ReadJwtToken(token);
            var TokenExpiration = DecodedToken.Claims.First(claim => claim.Type == "exp").Value;
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var Expiration = origin.AddSeconds(Convert.ToDouble(TokenExpiration));

            //ottengo il datetime in questo momento
            var now = DateTime.UtcNow;

            //controllo
            return (Expiration < now) ? true : false;
        }

    }
}
