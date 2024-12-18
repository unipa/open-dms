using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json.Linq;
using OpenDMS.MaximoSR.API.Utility.Interfacce;
using System.IdentityModel.Tokens.Jwt;

namespace OpenDMS.MaximoSR.API.Utility
{
    public class KeycloakTokenHandler : IKeycloakTokenHandler
    {
        private readonly IConfiguration config;
        private readonly IHttpContextAccessor httpContextAccessor;
        public KeycloakTokenHandler(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Utilizzando un refresh token e le informazioni contenute nell'appsetting (config) chiama il servizio Keycloak per eseguire il 
        /// refresh dell'access_token e del "refresh token" stesso, settandoli automaticamente nell'HttpContext.
        /// </summary>
        /// <param name="refreshToken"> Refresh Token (in corso di validità)</param>
        /// <param name="config"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> RefreshTokenAsync(string refreshToken)
        {

            if (IsTokenExpired(refreshToken))
                throw new Exception("Error refresh token is expired.");

            var httpClient = new HttpClient();

            var endpoint = (string)config.GetValue(typeof(string), "Keycloak:auth-server-url");
            var realm = (string)config.GetValue(typeof(string), "Keycloak:realm");
            var client_id = (string)config.GetValue(typeof(string), "Keycloak:resource");
            var client_secret = (string)config.GetValue(typeof(string), "Keycloak:credentials:secret");

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

            //var token = await httpContextAccessor.HttpContext.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme, OpenIdConnectParameterNames.AccessToken);

            var tokens = new List<AuthenticationToken>
            {
                new AuthenticationToken { Name = OpenIdConnectParameterNames.AccessToken, Value = new_accessToken },
                new AuthenticationToken { Name = OpenIdConnectParameterNames.RefreshToken, Value = new_refreshToken }
            };
            try
            {
                var info = await httpContextAccessor.HttpContext.AuthenticateAsync("Cookies");
                info.Properties.StoreTokens(tokens);

                await httpContextAccessor.HttpContext.SignInAsync(info.Principal, info.Properties);
            }
            catch (Exception ex) { }

            return new_accessToken;
        }

        /// <summary>
        /// Esegue il controllo sulla scadenza dei token.
        /// Se l'access_token è valido ritorna true. 
        /// Se è scaduto prova a refreshare se non riesce esegue il logout e riporta al login.
        /// </summary>
        /// <returns></returns>
        public async Task<string?> IsLoginStillValidAsync()
        {
            var access_token = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            if (string.IsNullOrEmpty(access_token)) throw new Exception("Empty Token.");

            //Se il token non è scaduto ritorno true
            if (!IsTokenExpired(access_token)) return access_token;

            var refresh_token = await httpContextAccessor.HttpContext.GetTokenAsync("refresh_token");

            try
            {
                await RefreshTokenAsync(refresh_token);
                return await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            }
            catch (Exception ex)
            {
                throw new Exception("Sessione scaduta, sarai reidirizzato alla pagina di login.");
            }

        }

        ///// <summary>     ORIGINALE
        ///// Esegue il controllo sulla scadenza dei token.
        ///// Se l'access_token è valido ritorna true. 
        ///// Se è scaduto prova a refreshare se non riesce esegue il logout e riporta al login.
        ///// </summary>
        ///// <returns></returns>
        //public async Task<string?> IsLoginStillValidAsync()
        //{
        //    var access_token = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");

        //    //Se il token non è scaduto ritorno true
        //    if (!IsTokenExpired(access_token)) return access_token;

        //    var refresh_token = await httpContextAccessor.HttpContext.GetTokenAsync("refresh_token");

        //    try
        //    {
        //        await RefreshTokenAsync(refresh_token);
        //        return await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
        //    }
        //    catch (Exception ex)
        //    {
        //        await httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //        await httpContextAccessor.HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme); //, new AuthenticationProperties { RedirectUri = "/Home/Logout-Complete" });
        //        throw new Exception("Sessione scaduta, sarai reidirizzato alla pagina di login.");
        //    }

        //}

        /// <summary>
        /// Controlla la scandeza del token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns> True se è scaduto; False se è ancora valido</returns>
        private bool IsTokenExpired(string token)
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
            return Expiration < now ? true : false;
        }

    }
}
