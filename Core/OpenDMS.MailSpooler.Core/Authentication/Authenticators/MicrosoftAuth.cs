using MailKit;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.MailSpooler.Core;
using OpenDMS.MailSpooler.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Web;


namespace Authenticator
{
    public class MicrosoftAuth : IAuthenticator
    {
        private String _ClientId = "";
        private String _ClientSecret = "";
        private String _TenantId = "";
        private String _RedirectUri = "";
        private String _Scope = "openid offline_access https://outlook.office.com/IMAP.AccessAsUser.All https://outlook.office.com/SMTP.Send";
        //private String _Scope = "openid";
        private readonly IConfiguration _configuration;
        public MicrosoftAuth(IConfiguration configuration)
        {
            _configuration = configuration;
            _RedirectUri = _configuration.GetSection("OAUTH:Redirect_Uri").Value ?? "";
        }

        public AuthenticationType AuthType => AuthenticationType.Microsoft_OAuth;
        //public void SetConfig(OAUTHConfig conf)
        //{
        //    _ClientId = conf.ClientId;
        //    _ClientSecret = conf.ClientSecret;
        //    _TenantId = conf.TenantId;
        //    _RedirectUri = conf.BaseURI ?? "";

        //    _RedirectUri += _RedirectUri.EndsWith("/") ? "" : "/";
        //    _RedirectUri += "OAUTH.aspx";
        //}
        public OAUTHToken AcquireIntercativeToken(string mailAddress)
        {
            throw new NotImplementedException();
        }
        public OAUTHToken AcquireSilentToken(string mailAddress)
        {
            var app = ConfidentialClientApplicationBuilder
                  .Create(_ClientId)
                  .WithTenantId(_TenantId)
                  .WithClientSecret(_ClientSecret)
                  .Build();

            var scopes = new string[] {
                    "offline_access",
                    "https://graph.microsoft.com/.default"
                };

            var authToken = app.AcquireTokenForClient(scopes)
            .ExecuteAsync().Result;

            return new OAUTHToken() { Token = authToken.AccessToken, RefreshToken = "", eMail = mailAddress, OauthType = AuthenticationType.Microsoft_OAuth };
        }


        public void Authenticate(IMailService server,  Mailbox mailbox, string _redirectURI)// OAUTHCredential credential)
        {
            _ClientId = mailbox.MailServer.ClientID;
            _ClientSecret = mailbox.MailServer.ClientSecret;
            _TenantId = mailbox.MailServer.TenantID;
            if (String.IsNullOrEmpty(_redirectURI))
                _RedirectUri = _redirectURI ?? "";
            _RedirectUri += _RedirectUri.EndsWith("/") ? "" : "/";
            _RedirectUri += "OAUTH.aspx";
            if (!String.IsNullOrEmpty(mailbox.TokenId))
            {

                SaslMechanism oauth2;

                if (server.AuthenticationMechanisms.Contains("XOAUTH2"))
                    oauth2 = new SaslMechanismOAuth2(mailbox.MailAddress, mailbox.TokenId);
                else
                    oauth2 = new SaslMechanismOAuthBearer(mailbox.MailAddress, mailbox.TokenId);

                server.Authenticate(oauth2);
            }
            else
            {
                //log.WarningFormat("Oauth Token vuoto per l'indirizzo: {0}", credential.eMail);
                throw new Exception(String.Format("OAUTH Token vuoto per l'indirizzo: {0}", mailbox.MailAddress));
            }

        }
        public string GetOAUTH_URL(OAUTHState Stato, Mailbox mailbox)
        {
            var _uribuilder_authorize = new UriBuilder(String.Format("https://login.microsoftonline.com/{0}/oauth2/v2.0/authorize", mailbox.MailServer.TenantID));

            var query = HttpUtility.ParseQueryString(_uribuilder_authorize.Query);

            if (!String.IsNullOrEmpty(mailbox.MailServer.ClientID) && mailbox.MailServer.ClientID.Contains("decrypt"))
            {
                var base64Bytes_client = System.Convert.FromBase64String(mailbox.MailServer.ClientID.Replace("decrypt://", ""));
                mailbox.MailServer.ClientID = System.Text.Encoding.UTF8.GetString(base64Bytes_client);
            }

            if (!String.IsNullOrEmpty(mailbox.MailServer.ClientSecret) && mailbox.MailServer.ClientSecret.Contains("decrypt"))
            {
                var base64Bytes_secret = System.Convert.FromBase64String(mailbox.MailServer.ClientSecret.Replace("decrypt://", ""));
                mailbox.MailServer.ClientSecret = System.Text.Encoding.UTF8.GetString(base64Bytes_secret);
            }
            query["client_id"] = mailbox.MailServer.ClientID;
            query["client_secret"] = mailbox.MailServer.ClientSecret;
            query["response_type"] = "code";
            query["redirect_uri"] = _RedirectUri; //credential.RedirectURI;
            query["scope"] = _Scope;
            //query["grant_type"] = "authorization_code";
            query["response_mode"] = "form_post";
            query["nonce"] = "abcde";
            query["state"] = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(System.Text.Json.JsonSerializer.Serialize(Stato)));

            _uribuilder_authorize.Query = query.ToString();

            return _uribuilder_authorize.Uri.ToString();
        }
        public OAUTHToken RefreshToken(Mailbox mailbox)
        {
            var refreshToken = mailbox.RefreshToken;
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("refresh_token", refreshToken),
                new KeyValuePair<string, string>("scope", _Scope),
                new KeyValuePair<string, string>("client_id", mailbox.MailServer.ClientID),
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("client_secret", mailbox.MailServer.ClientSecret)
             });

            HttpClient client = new HttpClient();
            var _uribuilder_token = new UriBuilder(String.Format("https://login.microsoftonline.com/{0}/oauth2/v2.0/token", mailbox.MailServer.TenantID));
            var res = client.PostAsync(_uribuilder_token.Uri.ToString(), content).Result;
            var access_token = res.Content.ReadAsStringAsync().Result;
            OAUTHToken result = new OAUTHToken();
            if (res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var token = System.Text.Json.JsonSerializer.Deserialize<TokenAuth>(access_token);
                JwtSecurityTokenHandler hand = new JwtSecurityTokenHandler();
                var tokenS = hand.ReadJwtToken(token.access_token);
                if (tokenS.Payload.ContainsKey("upn"))
                    result.eMail = tokenS.Payload["upn"].ToString();

                result.Token = token.access_token;
                result.RefreshToken = token.refresh_token;
                result.OauthType = AuthenticationType.Microsoft_OAuth;
                result.UserName = mailbox.UserId;
            }
            else
            {
                var error = System.Text.Json.JsonSerializer.Deserialize<ErrorReceived>(access_token);
                throw new Exception(error.error_description);
            }

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
        internal class ErrorReceived
        {
            public String error { get; set; }
            public String error_description { get; set; }
            public String[] error_codes { get; set; }
            public DateTime timestamp { get; set; }
            public String trace_id { get; set; }
            public String correlation_id { get; set; }
        }
        public OAUTHToken GetToken(string BD, string UserName, string mailAddress)
        {
            OAUTHToken result = new OAUTHToken();
            result.UserName = UserName;
            result.eMail = mailAddress;
            //result.BD = BD;
            result.Token = OAUTHUtils.Get_OAUTH_TOKEN(BD, UserName, mailAddress, MailType.Mail);
            result.RefreshToken = OAUTHUtils.Get_OAUTH_REFRESHTOKEN(BD, UserName, mailAddress, MailType.Mail);
            result.OauthType = OAUTHUtils.Get_OAUTH_TYPE(BD, UserName, mailAddress, MailType.Mail);
            return result;
        }
        public Boolean SetToken(OAUTHToken token)
        {
            OAUTHUtils.Set_OAUTH_TOKEN(token.UserName, token.eMail, MailType.Mail, token.Token);
            OAUTHUtils.Set_OAUTH_REFRESHTOKEN(token.UserName, token.eMail, MailType.Mail, token.RefreshToken);
            OAUTHUtils.Set_OAUTH_TYPE(token.UserName, token.eMail, MailType.Mail, token.OauthType);
            return true;
        }
        public Boolean EmptyToken(OAUTHToken token)
        {
            OAUTHUtils.Set_OAUTH_TOKEN(token.UserName, token.eMail, MailType.Mail, "");
            OAUTHUtils.Set_OAUTH_REFRESHTOKEN(token.UserName, token.eMail, MailType.Mail, "");
            OAUTHUtils.Set_OAUTH_TYPE(token.UserName, token.eMail, MailType.Mail, token.OauthType);
            return true;
        }
        public OAUTHToken AcquireCode(string code, Mailbox mailbox)
       {
            OAUTHToken result = new OAUTHToken();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("redirect_uri", _RedirectUri),
                new KeyValuePair<string, string>("scope", _Scope),
                new KeyValuePair<string, string>("client_id", mailbox.MailServer.ClientID),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("client_secret", mailbox.MailServer.ClientSecret)
             });

            HttpClient client = new HttpClient();
            var _uribuilder_token = new UriBuilder(String.Format("https://login.microsoftonline.com/{0}/oauth2/v2.0/token", mailbox.MailServer.TenantID));
            var res = client.PostAsync(_uribuilder_token.Uri.ToString(), content).Result;
            var access_token = res.Content.ReadAsStringAsync().Result;
            if (res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var token = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenAuth>(access_token);
                result.Token = token.access_token;
                result.RefreshToken = token.refresh_token;
                result.OauthType = AuthenticationType.Microsoft_OAuth;

                JwtSecurityTokenHandler hand = new JwtSecurityTokenHandler();
                var tokenS = hand.ReadJwtToken(token.access_token);
                if (tokenS.Payload.ContainsKey("upn"))
                    result.eMail = tokenS.Payload["upn"].ToString();
            }
            else
            {
                var error = Newtonsoft.Json.JsonConvert.DeserializeObject<ErrorReceived>(access_token);
                throw new Exception(error.error_description);
            }
            return result;
        }
        private OAUTHToken RefreshToken(String refreshToken)
        {

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("refresh_token", refreshToken),
                new KeyValuePair<string, string>("scope", _Scope),
                new KeyValuePair<string, string>("client_id", _ClientId),
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("client_secret", _ClientSecret)
             });

            HttpClient client = new HttpClient();
            var _uribuilder_token = new UriBuilder(String.Format("https://login.microsoftonline.com/{0}/oauth2/v2.0/token", _TenantId));
            var res = client.PostAsync(_uribuilder_token.Uri.ToString(), content).Result;
            var access_token = res.Content.ReadAsStringAsync().Result;
            OAUTHToken result = new OAUTHToken();
            if (res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var token = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenAuth>(access_token);
                JwtSecurityTokenHandler hand = new JwtSecurityTokenHandler();
                var tokenS = hand.ReadJwtToken(token.access_token);
                if (tokenS.Payload.ContainsKey("upn"))
                    result.eMail = tokenS.Payload["upn"].ToString();

                result.Token = token.access_token;
                result.RefreshToken = token.refresh_token;
                result.OauthType = AuthenticationType.Microsoft_OAuth;
            }
            else
            {
                var error = Newtonsoft.Json.JsonConvert.DeserializeObject<ErrorReceived>(access_token);
                throw new Exception(error.error_description);
            }

            return result;
        }

    }
}
