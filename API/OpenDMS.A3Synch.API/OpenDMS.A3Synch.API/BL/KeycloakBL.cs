using A3Synch.Interfacce;
using A3Synch.Models;
using System.Text.Json;
using OpenDMS.A3Synch.API.Utility;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using java.awt.dnd;

namespace A3Synch.BL
{
    public class KeycloakBL : IKeycloakBL
    {
        
        private readonly IContactsDAO _contactsdao;
        private readonly IUtils _utils;
        private readonly IRolesDAO _rolesdao;
        private readonly IUsersBL _usersdao;
        private readonly IConfiguration _config;

        private IUserGroupRolesDAO _usergrouproles;
        private readonly ILogger<KeycloakBL> _logger;


        public KeycloakBL(IUtils utils, IConfiguration config, IRolesDAO rolesdao, ILogger<KeycloakBL> logger, IUsersBL usersdao, IUserGroupRolesDAO usergrouproles)
        {
            //_contactsdao = contactsdao;
            _utils = utils;
            _config = config;
            _rolesdao = rolesdao;
            _logger = logger;
            _usersdao = usersdao;
            _usergrouproles = usergrouproles;
        }

        public async Task<(string AccessToken, string RefreshToken)> GetTokens()
        {
            try
            {
                var url = $"{_config["Keycloak:auth-server-url"]}/realms/{_config["Keycloak:realm"]}/protocol/openid-connect/token";
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                HttpClient client = new HttpClient(clientHandler);
                var requestBody = new Dictionary<string, string>
                                {
                                    { "grant_type", "client_credentials" },
                                    { "client_id", _config["Keycloak:resource"] },
                                    { "client_secret", _config["Keycloak:credentials:secret"] },
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
                var refreshToken = tokenResponse.GetProperty("refresh_token").GetString();

                return (accessToken, refreshToken);
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

        public async Task<List<Roles>> GetRolesFromKC()
        {
            var token = (await GetTokens()).AccessToken; /*await Admin.Utility.KeycloakTokenHandler.IsLoginStillValidAsync(_config);*/
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient client = new HttpClient(clientHandler);
            client.Timeout = TimeSpan.FromMinutes(45);
            var url = _config["Keycloak:auth-server-url"] + "admin/realms/"+_config["Keycloak:realm"]+"/roles";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var roles_dynamic = JsonSerializer.Deserialize<List<RolesKC>>(responseBody);
                var roles = new List<Roles>();

                foreach(var role in roles_dynamic)
                {
                    Roles new_role = new Roles();
                    new_role.Id = role.id;
                    new_role.RoleName = role.name;
                    new_role.CreationDate = _utils.GetData();
                    new_role.LastUpdate = _utils.GetData();
                    roles.Add(new_role);
                }

                return roles;
            }
            catch (Exception ex)
            {
                _logger.LogError("Errore in KeycloakBL, errore: " + ex.Message);
                throw new Exception("Errore in KeycloakBL, errore: " + ex.Message);
            }
        }

        public async Task PostRoleInKc(Roles roles)
        {
            var token = (await GetTokens()).AccessToken; 
            var requestUrl = _config["Keycloak:auth-server-url"]+"/admin/realms/" + _config["Keycloak:realm"] + "/roles?max=-1";
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient client = new HttpClient(clientHandler);
            client.Timeout = TimeSpan.FromMinutes(45);
            var requestData = new JsonObject
                {
                    { "name", roles.Id },
                    { "description", $"${{{roles.RoleName}}}" },
                    { "composite", true },
                    { "clientRole", false },
                    { "containerId", _config["Keycloak:realm"] }
                };
            var jsonBody = requestData.ToString();
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync(requestUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    throw new Exception($"Errore nell'inserimento dei ruoli su keycloak, status code: {response.StatusCode}");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Errore in KeycloakBL, errore: " + ex.Message);
                }
            }
        }

        public async Task AddCompositeRoleKc(string roleName)
        {
            var token = (await GetTokens()).AccessToken;
            var requestUrl = _config["Keycloak:auth-server-url"] + "/admin/realms/" + _config["Keycloak:realm"] + "/roles/" + roleName + "/composites";
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient client = new HttpClient(clientHandler);
            client.Timeout = TimeSpan.FromMinutes(45);
            var requestData = new JsonArray
            {
                new JsonObject
                {
                    { "id", "9c3c948b-c0fd-4e09-9300-f0c268595ba9" },
                    { "name", "view-groups" },
                }
            };

            var jsonBody = requestData.ToString();
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync(requestUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    throw new Exception($"Errore nell'inserimento dei ruoli su keycloak, status code: {response.StatusCode}");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Errore in KeycloakBL, errore: " + ex.Message);
                }
            }
        }

    

        public async Task<List<Members>> GetUsersFromKC(int totalUsers)
        {
            var token = (await GetTokens()).AccessToken;
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient client = new HttpClient(clientHandler);
            client.Timeout = TimeSpan.FromMinutes(45);
            var url = _config["Keycloak:auth-server-url"] + "admin/realms/" + _config["Keycloak:realm"] + "/users?max=1000";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                List<Members> members = new List<Members>();

                if (totalUsers <= 1000)
                {
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    var responseBody = await response.Content.ReadAsStringAsync();
                    var users_kc = JsonSerializer.Deserialize<List<UsersKC>>(responseBody);

                    foreach (var user in users_kc)
                    {
                        Members m = new();
                        m.username = user.username;
                        m.nome = user.username;
                        m.cognome = "";
                        members.Add(m);
                    }

                }
                else
                {
                    int totalPages = (totalUsers / 1000) + 1;
                    for (int i = 0; i < totalPages; i++)
                    {
                        var offset = i * 1000;
                        var response = await client.GetAsync($"{url}&first={offset}");
                        Console.WriteLine($"{url}&first={offset}");
                        response.EnsureSuccessStatusCode();

                        var responseBody = await response.Content.ReadAsStringAsync();
                        var users_kc = JsonSerializer.Deserialize<List<UsersKC>>(responseBody);

                        foreach (var user in users_kc)
                        {
                            Members m = new();
                            m.username = user.username;
                            m.nome = user.username;
                            m.cognome = "";
                            members.Add(m);
                        }
                        await _usersdao.SynchUsersInDb(members,true);

                    }
                }

                return members;
            }
            catch (Exception ex)
            {
                _logger.LogError("Errore in KeycloakBL, errore: " + ex.Message);
                throw new Exception("Errore in KeycloakBL, errore: " + ex.Message);
            }
        }

        public async Task<int> GetUsersCountFromKC()
		{
			var token = (await GetTokens()).AccessToken; 
			HttpClientHandler clientHandler = new HttpClientHandler();
			clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
			HttpClient client = new HttpClient(clientHandler);
			client.Timeout = TimeSpan.FromMinutes(45);
			var url = _config["Keycloak:auth-server-url"] + "admin/realms/" + _config["Keycloak:realm"] + "/users/count";

			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			try
			{
				var response = await client.GetAsync(url);
				response.EnsureSuccessStatusCode();

				var responseBody = await response.Content.ReadAsStringAsync();
				var users_count_kc = int.Parse(responseBody);
				return users_count_kc;
			}
			catch (Exception ex)
			{
				_logger.LogError("Errore in KeycloakBL, errore: " + ex.Message);
				throw new Exception("Errore in KeycloakBL, errore: " + ex.Message);
			}
		}

		public async Task PostUserInKc(Users user)
        {
            var token = (await GetTokens()).AccessToken; 
            var requestUrl = _config["Keycloak:auth-server-url"] + "/admin/realms/" + _config["Keycloak:realm"] + "/users";
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient client = new HttpClient(clientHandler);
            client.Timeout = TimeSpan.FromMinutes(45);

            var requestData = new JsonObject
                {
                    { "username", user.Id },
                    { "enabled", true },
                    { "totp", false },
                    { "emailVerified", true },
                    { "notBefore", 0 },
                };

            var jsonBody = requestData.ToString();
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync(requestUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    throw new Exception($"Errore nell'inserimento deglu utenti su keycloak, status code: {response.StatusCode}");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Errore in KeycloakBL, errore: " + ex.Message);
                }
            }
        }

        public async Task AdduserRoleInKc(string userKcId, Roles role)
        {
            var token = (await GetTokens()).AccessToken; 
            var requestUrl = _config["Keycloak:auth-server-url"] + "/admin/realms/" + _config["Keycloak:realm"] + "/users/" + userKcId + "/role-mappings/realm";
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient client = new HttpClient(clientHandler);
            client.Timeout = TimeSpan.FromMinutes(45);
            var requestData = new JsonArray
            {
                new JsonObject
                {
                    { "id", role?.Id },
                    { "name", role?.RoleName },
                }
            };

            var jsonBody = requestData.ToString();
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync(requestUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    throw new Exception($"Errore nell'assegnazione di un ruolo ad un utente su keycloak, status code: {response.StatusCode}");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Errore in KeycloakBL, errore: " + ex.Message);
                }
            }
        }

        public async Task<string> GetKcId(string username)
        {
            var token = (await GetTokens()).AccessToken;  
            string apiUrl = _config["Keycloak:auth-server-url"] + "admin/realms/"+ _config["Keycloak:realm"] + "/users?username="+ username;
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient client = new HttpClient(clientHandler);

            // Imposta l'header di autenticazione
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Effettua la chiamata GET all'API
            HttpResponseMessage response = await client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                // Leggi il contenuto JSON della risposta
                string jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserializza il JSON e ottieni il valore della chiave "id"
                JsonDocument jsonDocument = JsonDocument.Parse(jsonResponse);
                JsonElement firstElement = jsonDocument.RootElement.EnumerateArray().FirstOrDefault();
                if (firstElement.TryGetProperty("id", out JsonElement idElement) && idElement.ValueKind == JsonValueKind.String)
                {
                    string userId = idElement.GetString();
                    return userId;
                }
                else
                {
                    throw new Exception($"La chiave id non è presente nel JSON");
                }
            }
            else
            {
                throw new Exception($"Errore nella chiamata API. Codice di stato: {response.StatusCode}");
            }
        }

        public async Task AddIdpLinkInKc(string userKcId, string username)
        {
            var token = (await GetTokens()).AccessToken;  
            var requestUrl = _config["Keycloak:auth-server-url"] + "admin/realms/" + _config["Keycloak:realm"] + "/users/" + userKcId + "/federated-identity/" + _config["Keycloak:idp_alias"];
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient client = new HttpClient(clientHandler);
            client.Timeout = TimeSpan.FromMinutes(45);

            var requestData = new Dictionary<string, string>
                                {
                                    { "userId", username },
                                    { "userName", username },
                                };

            var jsonBody = System.Text.Json.JsonSerializer.Serialize(requestData);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync(requestUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    throw new Exception($"Errore nell'assegnazione dell'IdP ad un utente su keycloak, status code: {response.StatusCode}");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Errore in KeycloakBL, errore: " + ex.Message);
                }
            }
        }

        public async Task<int> SynchAllInKC()
        {
            try
            {
                var token = (await GetTokens()).AccessToken; 
                List<Roles> db_roles = await _rolesdao.GetAllRoles();
                List<Roles> kc_roles = await GetRolesFromKC();
                List<Roles> diffRoles = db_roles.Where(dbRole => !kc_roles.Any(kcRole => kcRole.RoleName == dbRole.Id)).ToList();
                if (diffRoles.Count == 0)
                {
                    SharedVariables.kc_roles_total = -1;
                }
                else
                {
                    SharedVariables.kc_roles_total = diffRoles.Count;
                }
                foreach (var role in diffRoles)
                {
                    await PostRoleInKc(role);
                    await AddCompositeRoleKc(role.Id);
                    SharedVariables.kc_roles_elaborated++;
                }

                List<Users> db_users = await _usersdao.GetAllUsers();
				//PER DEBUG SOLO I PRIMI 1000 UTENTI
				//b_users = db_users.OrderBy(user => user.Id).Take(20).ToList();

				int users_count_kc = await GetUsersCountFromKC();
                //PER DEBUG
                //users_count_kc = 999;
                SharedVariables.total_keycloak_users_counter = users_count_kc;
                List<Members> kc_users = await GetUsersFromKC(users_count_kc);
                List<Users> diffUsers = db_users.Where(dbUser => !kc_users.Any(kcUser => kcUser.username == dbUser.Id)).ToList();

                if (diffUsers.Count == 0)
                {
                    SharedVariables.kc_users_total = -1;
                }
                else
                {
                    SharedVariables.kc_users_total = diffUsers.Count;
                }
                foreach (var user in diffUsers)
                {
                    await PostUserInKc(user);
                    SharedVariables.kc_users_elaborated++;
                }
                kc_users = await GetUsersFromKC(users_count_kc);
                SharedVariables.total_addedRole_counter = db_users.Count;
                foreach (var user in db_users)
                {
                    List<UserGroupRoles> userGroupRoles = await _usergrouproles.GetUserGroupsRoles(user.Id);
                    string KC_id = await GetKcId(user.Id);
                    int i = 0;
                    foreach (var role in userGroupRoles)
                    {
                        i++;
                        Roles matched_role = kc_roles.FirstOrDefault(r => r.RoleName == role.RoleId);
						await AdduserRoleInKc(KC_id, matched_role);

                        string externalRole = _config["EXTERNAL_USER_ROLE"];

                        List<string> externalRoles = GetList(externalRole);

                        foreach(string listRole in externalRoles)
                        {
                            Roles ruolo = new Roles()
                            {
                                Id = listRole,
                                RoleName = listRole,
                                CreationDate = DateTime.Now,
                                LastUpdate = DateTime.Now
                            };
							await AdduserRoleInKc(KC_id, matched_role);
                        }

                        await AdduserRoleInKc(KC_id, matched_role);
                        
                        if (i == userGroupRoles.Count)
                        {
                            SharedVariables.elaborated_addedRole_counter++;
                            i = 0;
                        }

                    }
                }

                SharedVariables.total_addedIdp_counter = db_users.Count;
                foreach (var user in db_users)
                {
                    string KC_id = await GetKcId(user.Id);
                    await AddIdpLinkInKc(KC_id, user.Id);
                    SharedVariables.elaborated_addedIdp_counter++;
                }

                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore in KeycloakBL: " + ex.Message);
            } 

        }

        public List<string> GetList(string externalRoleString)
        {
            List<string> externalRoleList = new List<string>();
            // Verificare se la stringa esiste e non è vuota
            if (!string.IsNullOrEmpty(externalRoleString))
            {
                // Dividi la stringa in una lista di stringhe utilizzando un delimitatore (ad esempio, ",")
                externalRoleList = externalRoleString.Split(',').ToList();

                foreach (var role in externalRoleList)
                {
                    externalRoleList.Add(role.Trim()); 
                }
            }
            return externalRoleList;
        }

        public void ResetStatus()
        {
            SharedVariables.kc_roles_elaborated = 0;
            SharedVariables.kc_roles_total = 0;
            SharedVariables.kc_users_total = 0;
            SharedVariables.kc_users_elaborated = 0;
            SharedVariables.elaborated_member_list = 0;
            SharedVariables.total_addedRole_counter = 0;
            SharedVariables.elaborated_addedRole_counter = 0;
            SharedVariables.total_addedIdp_counter = 0;
            SharedVariables.elaborated_addedIdp_counter = 0;
            SharedVariables.total_keycloak_users_counter = 0;
            SharedVariables.elaborated_keycloak_users_counter= 0;

        }


    }
}
