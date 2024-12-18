using MailKit;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.MailSpooler.Core;
using OpenDMS.MailSpooler.Core.Interfaces;
using System.Net.Http.Headers;
using System.Web;

namespace Authenticator
{
    public class GoogleAuth : IAuthenticator
    {
        //private String _ClientId = "";
        //private String _ClientSecret = "";
        private String _RedirectUri = "";
        private String _Scope = "openid https://mail.google.com/ https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile";
        private  readonly IConfiguration _configuration;

        public AuthenticationType AuthType => AuthenticationType.Google_OAuth;
        public GoogleAuth(IConfiguration configuration)
        {
            _configuration = configuration;
            _RedirectUri = _configuration.GetSection("OAUTH:Redirect_Uri").Value ?? "";
        }
        public void Authenticate(IMailService server, Mailbox mailbox, string _redirectURI = "")
        {
            //_ClientId = credential.ClientId;
            //_ClientSecret = credential.ClientSecret;

            //var clientSecrets = new ClientSecrets
            //{
            //    ClientId = credential.ClientId,
            //    ClientSecret = credential.ClientSecret
            //};

            //var codeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            //{
            //    DataStore = new FileDataStore("CredentialCacheFolder", false),
            //    Scopes = new[] { "https://mail.google.com/" },
            //    ClientSecrets = clientSecrets,
               
            //});

            //// Note: For a web app, you'll want to use AuthorizationCodeWebApp instead.
            //var codeReceiver = new LocalServerCodeReceiver();
            //var authCode = new AuthorizationCodeInstalledApp(codeFlow, codeReceiver);

            //var credential_1 =  authCode.AuthorizeAsync(credential.eMail, CancellationToken.None).Result;

            //if (credential_1.Token.IsExpired(SystemClock.Default))
            //    _ = credential_1.RefreshTokenAsync(CancellationToken.None).Result;

            SaslMechanism oauth2;

            if (server.AuthenticationMechanisms.Contains("OAUTHBEARER"))
                oauth2 = new SaslMechanismOAuthBearer(mailbox.MailAddress, mailbox.TokenId);
            else
                oauth2 = new SaslMechanismOAuth2(mailbox.MailAddress, mailbox.TokenId);

             server.Authenticate(oauth2);

        }
        public OAUTHToken RefreshToken(Mailbox mailbox)
        {

            var refreshToken = mailbox.RefreshToken;
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("refresh_token", refreshToken),
                new KeyValuePair<string, string>("client_id", mailbox.MailServer.ClientID),
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("client_secret", mailbox.MailServer.ClientSecret)
             });

            HttpClient client = new HttpClient();
            var _uribuilder_token = new UriBuilder(String.Format("https://oauth2.googleapis.com/token"));
            var res = client.PostAsync(_uribuilder_token.Uri.ToString(), content).Result;
            var access_token = res.Content.ReadAsStringAsync().Result;
            OAUTHToken result = new OAUTHToken();
            if (res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var token = System.Text.Json.JsonSerializer.Deserialize<TokenAuth>(access_token);
                //JwtSecurityTokenHandler hand = new JwtSecurityTokenHandler();
                //var tokenS = hand.ReadJwtToken(token.access_token);
                //if (tokenS.Payload.ContainsKey("upn"))
                  //  result.eMail = tokenS.Payload["upn"].ToString();

                result.Token = token.access_token;
                result.RefreshToken = mailbox.RefreshToken;
                result.OauthType = AuthenticationType.Google_OAuth;
                result.UserName = mailbox.UserId;
            }
            else
            {
                //string error = "";
                //var error = System.Text.Json.JsonSerializer.Deserialize<ErrorReceived>(access_token);
                throw new Exception($"Errore chiamata oauth ---> {res.Content.ToString()} ");
            }

            return result;
        }
        //public OAUTHToken GetToken(string BD, string UserName, string mailAddress)
        //{
        //    OAUTHToken result = new OAUTHToken();
        //    result.UserName = UserName;
        //    result.eMail = mailAddress;
        //    //result.BD = BD;
        //    //result.Token = OAUTHUtils.Get_OAUTH_TOKEN(BD, UserName, mailAddress, MailType.eMail);
        //    //result.RefreshToken = OAUTHUtils.Get_OAUTH_REFRESHTOKEN(BD, UserName, mailAddress, MailType.eMail);
        //    //result.OauthType = OAUTHUtils.Get_OAUTH_TYPE(BD, UserName, mailAddress, MailType.eMail);
        //    return result;
        //}
        public string GetOAUTH_URL(OAUTHState stato, Mailbox mailbox)
        {
            var _uribuilder_authorize = new UriBuilder("https://accounts.google.com/o/oauth2/v2/auth");
            var query = HttpUtility.ParseQueryString(_uribuilder_authorize.Query);
            query["client_id"] = mailbox.MailServer.ClientID;
            //query["client_secret"] = credential.ClientSecret;
            query["response_type"] = "code";
            query["redirect_uri"] = _RedirectUri; // credential.RedirectURI;
            query["access_type"] = "offline";
            query["prompt"] = "consent";
            query["include_granted_scopes"] = "true";
            query["scope"] = _Scope;
            query["state"] = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(System.Text.Json.JsonSerializer.Serialize(stato)));

            _uribuilder_authorize.Query = query.ToString();

            return _uribuilder_authorize.Uri.ToString();
        }
        public String getEmail(string token)
        {
            var _uribuilder_authorize = new UriBuilder("https://openidconnect.googleapis.com/v1/userinfo");
            //var query = HttpUtility.ParseQueryString(_uribuilder_authorize.Query);
            //query["scope"] = "profile email";
            //_uribuilder_authorize.Query = query.ToString();
            var url = _uribuilder_authorize.Uri.ToString();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var res = client.GetAsync(url).Result;
            var userinfojson = res.Content.ReadAsStringAsync().Result;
            var _userinfo = Newtonsoft.Json.JsonConvert.DeserializeObject<UserInfo>(userinfojson);
            return _userinfo.email;
        }
        public bool EmptyToken(OAUTHToken token)
        {
            OAUTHUtils.Set_OAUTH_TOKEN(token.UserName, token.eMail, MailType.Mail, token.Token);
            OAUTHUtils.Set_OAUTH_REFRESHTOKEN(token.UserName, token.eMail, MailType.Mail, token.RefreshToken);
            OAUTHUtils.Set_OAUTH_TYPE(token.UserName, token.eMail, MailType.Mail, token.OauthType);
            return true;
        }
        OAUTHToken IAuthenticator.GetToken(string BD, string UserName, string mailAddress)
        {
            OAUTHToken result = new OAUTHToken();
            result.UserName = UserName;
            result.eMail = mailAddress;
            // result.BD = BD;
            result.Token = OAUTHUtils.Get_OAUTH_TOKEN(BD, UserName, mailAddress, MailType.Mail);
            result.RefreshToken = OAUTHUtils.Get_OAUTH_REFRESHTOKEN(BD, UserName, mailAddress, MailType.Mail);
            result.OauthType = OAUTHUtils.Get_OAUTH_TYPE(BD, UserName, mailAddress, MailType.Mail);
            return result;
        }
        public bool SetToken(OAUTHToken token)
        {
            OAUTHUtils.Set_OAUTH_TOKEN(token.UserName, token.eMail, MailType.Mail, token.Token);
            OAUTHUtils.Set_OAUTH_REFRESHTOKEN(token.UserName, token.eMail, MailType.Mail, token.RefreshToken);
            OAUTHUtils.Set_OAUTH_TYPE(token.UserName, token.eMail, MailType.Mail, token.OauthType);
            return true;
        }
        public OAUTHToken AcquireIntercativeToken(string MailAddress)
        {
            throw new NotImplementedException();
        }
        public OAUTHToken AcquireSilentToken(string MailAddress)
        {
            throw new NotImplementedException();
        }
        public OAUTHToken AcquireCode(string code, Mailbox mailbox)
        {
            OAUTHToken result = new OAUTHToken();

            var content = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("code", code),
                    new KeyValuePair<string, string>("redirect_uri", _RedirectUri ),//credential.RedirectURI),
                    new KeyValuePair<string, string>("client_id", mailbox.MailServer.ClientID),
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("client_secret", mailbox.MailServer.ClientSecret)
                });

            HttpClient client = new HttpClient();
            var _uribuilder_token = new UriBuilder("https://oauth2.googleapis.com/token");
            var res = client.PostAsync(_uribuilder_token.Uri.ToString(), content).Result;

            var access_token = res.Content.ReadAsStringAsync().Result;
            var token = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenAuth>(access_token);

            result.Token = token.access_token;
            result.RefreshToken = token.refresh_token;
            result.OauthType = AuthenticationType.Google_OAuth;
            result.eMail = getEmail(token.access_token);

            return result;
        }
        internal class TokenAuth
        {
            public String user_name { get; set; }
            public String token_type { get; set; }
            public String scope { get; set; }
            public int expires_in { get; set; }
            public int ext_expires_in { get; set; }
            public String access_token { get; set; }
            public String refresh_token { get; set; }
            public String id_token { get; set; }
        }
        internal class UserInfo
        {
            public string sub { get; set; }
            public string name { get; set; }
            public string given_name { get; set; }
            public string family_name { get; set; }
            public string picture { get; set; }
            public string email { get; set; }
            public string email_verified { get; set; }
            public string locale { get; set; }
        }
    }
}
