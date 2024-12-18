using A3Synch.Interfacce;
using A3Synch.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenDMS.A3Synch.API.Utility;
using org.json;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace A3Synch.Utility
{
    public class Utils : IUtils
    {
        private readonly IOrganizationNodesDAO _organizationnodesDAO;
        private readonly IUserGroupsDAO _usergroupsDAO;
        private readonly ILogger _logger;
        private IConfiguration _config;

        public const string A3Synch = "A3Synch";

        public Utils(ILogger<Utils> logger, IOrganizationNodesDAO organizationnodesDAO, IUserGroupsDAO usergroupsDAO, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _organizationnodesDAO = organizationnodesDAO;
            _usergroupsDAO = usergroupsDAO;
        }

        //Restituisce il TimeStamp generato per effettuare la chiamata API
        public string GetApiTimestamp()
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var timestamp = (int)(DateTime.UtcNow - epoch).TotalSeconds;

            return timestamp.ToString();
        }
        //Restituisce l'Api Nonce generato per effettuare la chiamata API
        public string GetApiNonce()
        {

            byte[] randomBytes = new byte[64];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return Convert.ToBase64String(randomBytes);
        }
        //Restituisce il Token generato tramite combinazione dei parametri precedenti per effettuare la chiamata API
        public string GetApiToken(string apiTimestamp, string apiNonce)
        {
            string apiKeySecret = (string)_config.GetValue(typeof(string), "API_KEY_SECRET");

            string dataToHash = apiTimestamp + apiNonce;
            byte[] keyBytes = Encoding.UTF8.GetBytes(apiKeySecret);
            byte[] dataBytes = Encoding.UTF8.GetBytes(dataToHash);

            using var hmacSha512 = new HMACSHA512(keyBytes);
            byte[] hashedData = hmacSha512.ComputeHash(dataBytes);

            // Converte l'array di byte in una stringa Base64
            string apiToken = Convert.ToBase64String(hashedData);
            return apiToken;
        }
        public DateTime GetData()
        {
            DateTime currentDateTime = DateTime.Now;
            string formattedDateTime = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            return DateTime.Parse(formattedDateTime);
        }
        public bool CheckClosed(string date)
        {
            bool closed = false;
            DateTime inputDate;

            // Prova a convertire la stringa in un oggetto DateTime
            if (DateTime.TryParseExact(date, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out inputDate))
            {
                // Confronto la data di input con la data e ora corrente
                if (inputDate < DateTime.Now)
                {
                    closed = true;
                }
            }
            else
            {
                //Lancio l'eccezione se la data non è valida
                throw new ArgumentException("La data inserita non è valida.");
            }

            return closed;
        }

        public int ISOnumericDate(DateTime? date)
        {
            if (date == null) return 0;
            if (date == DateTime.MinValue) return 0;

            DateTime notNullDate = (DateTime)date;
            int numeric_date = int.Parse(notNullDate.ToString("yyyyMMdd"));
            return numeric_date;
        }

        public DateTime ISOnumericToDate(string isoDate)
        {
            if (isoDate.Length < 8)
            {
                isoDate = isoDate.PadLeft(8, '0');
            }

            // Converte la stringa nel formato YYYYMMDD in un oggetto DateTime
            if (DateTime.TryParseExact(isoDate, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
            {
                return parsedDate;
            }
            throw new ArgumentException($"Invalid ISO date format: {isoDate}");
        }

        public bool IsJSON(string jsonString)
        {
            try
            {
                JToken.Parse(jsonString);
                return true;
            }
            catch (JsonReaderException)
            {
                return false;
            }
        }


  

        //GetOrganizationPage Filtrata
        public async Task<StrutturaInput> GetOrganizationPage(string url)
        {
            string apiKeySecret = (String)_config.GetValue(typeof(String), "API_KEY_SECRET");
            string apiKeyId = (String)_config.GetValue(typeof(String), "API_KEY_ID");
            var httpClient = new HttpClient();
            var apiTimestamp = GetApiTimestamp();
            var apiNonce = GetApiNonce();
            string apiToken = GetApiToken(apiTimestamp, apiNonce);

            //Crea un nuovo HttpRequestMessage
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
            };

            //Aggiungi le intestazioni personalizzate
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("a3-api-key-id", apiKeyId);
            request.Headers.Add("a3-api-timestamp", apiTimestamp);
            request.Headers.Add("a3-api-nonce", apiNonce);
            request.Headers.Add("a3-api-token", apiToken);


            //Aggiungi l'intestazione 'Content - Type' alle intestazioni del contenuto
            request.Content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();

            await WriteJsonInFile(url, jsonString);

            var struttureInput = System.Text.Json.JsonSerializer.Deserialize<StrutturaInput>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            //struttureInput.results.units.data.ToList();
            return struttureInput;
        }

        //Ritorna tutte le struttur edi A3
        public async Task<List<Struttura>> GetAllOrganizationNodes()
        {
            string url = (String)_config.GetValue(typeof(String), "A3_API_URL") + "/units?page=1";
            StrutturaInput input = await GetOrganizationPage(url);
            int last_page = input.results.units.last_page;
            int count  = input.results.units.data.Count * last_page;
            List<Struttura> risultati = new List<Struttura>(count);
            risultati.AddRange(input.results.units.data);
            for (int i = 1; i < last_page; i++)
            {
                url = input.results.units.next_page_url;
                input = await GetOrganizationPage(url);
                _logger.LogInformation($"Ho preso {input.results.units.data.Count} strutture nella pagina {i}\n");
                risultati.AddRange(input.results.units.data);
            }
            return risultati;
        }



   
        public async Task WriteJsonInFile(string url, string jsonString)
        {
            string filePath = _config.GetValue<string>("FileSettings:FilePath");

            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File path non configurato.");

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string separator = $"\n====== {timestamp} ======";
                string content = $"{separator}\nURL: {url}\n{jsonString}\n";

                await File.AppendAllTextAsync(filePath, content);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Errore durante la scrittura del file: {ex.Message}");
            }
        }

        public async Task ClearFile()
        {
            string filePath = _config.GetValue<string>("FileSettings:FilePath");

            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File path non configurato.");


            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            await File.WriteAllTextAsync(filePath, string.Empty);

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _logger.LogInformation($"Contenuto del file cancellato con successo il {timestamp}");
        }

        public int MaxRightBound(List<OrganizationNodes> list)
        {
            int maxRightBound = list.MaxBy(o => o.RightBound)?.RightBound ?? 0;
            return maxRightBound;
        }

        public int ParentLeftBound(List<OrganizationNodes> list, string id_struttura)
        {
            OrganizationNodes parentNode = list.FirstOrDefault(s => s.Id == id_struttura);
            int parentLeftBound = parentNode?.LeftBound ?? 0;
            return parentLeftBound;
        }


        public async Task<List<Members>> GetMembersInStructure(string UserGroupId, string ExternalId)
        {
            string apiKeyId = (String)_config.GetValue(typeof(String), "API_KEY_ID");
            string url = (String)_config.GetValue(typeof(String), "A3_API_URL") + "/units/" + ExternalId + "/members";
            var httpClient = new HttpClient();
            var apiTimestamp = GetApiTimestamp();
            var apiNonce = GetApiNonce();
            string apiToken = GetApiToken(apiTimestamp, apiNonce);

            //Crea un nuovo HttpRequestMessage
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
            };

            //Aggiungi le intestazioni personalizzate
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("a3-api-key-id", apiKeyId);
            request.Headers.Add("a3-api-timestamp", apiTimestamp);
            request.Headers.Add("a3-api-nonce", apiNonce);
            request.Headers.Add("a3-api-token", apiToken);


            //Aggiungi l'intestazione 'Content - Type' alle intestazioni del contenuto
            request.Content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return new List<Members>();
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            // Verifica se la stringa è vuota
            if (IsJSON(jsonString) == false)
            {
                jsonString = "{\"results\":{\"members\":[]}}";
            }

            await WriteJsonInFile(url, jsonString);

            JToken? json = JToken.Parse(jsonString);
            JArray? membersArray = (JArray)json["results"]["members"];
            List<Members> members = membersArray.ToObject<List<Members>>();

            //string UserGroupId = _usergroupsDAO.GetUserGroupsId(Id);

            // Aggiungi il valore UserGroupsId a tutti gli elementi della lista
            foreach (var member in members)
            {
                member.UserGroupId = UserGroupId;
                member.inizio_validita = TimeZoneInfo.ConvertTime(member.inizio_validita, TimeZoneInfo.FindSystemTimeZoneById("Europe/Rome"));
                member.fine_validita = TimeZoneInfo.ConvertTime(member.fine_validita, TimeZoneInfo.FindSystemTimeZoneById("Europe/Rome"));
            }
            return members;
        }

    

        public async Task<List<Members>> GetAllMembersInStructures()
        {
            List<Members> all_members = new List<Members>();
            List<UserGroups> userGroups = await _usergroupsDAO.GetAllUserGroups();
            //PER DEBUG
            //int memberCount = 0;
            foreach (var userGroup in userGroups)
            {
                List<Members> members = await GetMembersInStructure(userGroup.Id, userGroup.ExternalId);
                all_members.AddRange(members);
                SharedVariables.elaborated_member_list++;
     
            }

            return all_members;
        }

        public async Task SendErrorNotification(string errorMessage)
        {
            string subject = "ERRORE DURANTE LA SINCRONIZZAZIONE CON A3";
            string body = $"Si è verificato un errore durante la sincronizzazione:\n{errorMessage}";

            string smtpServer = _config.GetValue<string>("EmailSettings:SmtpServer");
            int smtpPort = _config.GetValue<int>("EmailSettings:SmtpPort");
            string smtpUser = _config.GetValue<string>("EmailSettings:SmtpUser");
            string smtpPassword = _config.GetValue<string>("EmailSettings:SmtpPassword");
            string fromEmail = _config.GetValue<string>("EmailSettings:FromEmail");
            string toEmail = _config.GetValue<string>("EmailSettings:ToEmail");
            bool enableSSL = _config.GetValue<bool>("EmailSettings:EnableSSL");

            var mailMessage = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body
            };

            using (var smtpClient = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPassword),
                EnableSsl = enableSSL
            })
            {
                try
                {
                    await smtpClient.SendMailAsync(mailMessage);
                    _logger.LogInformation("Email di notifica errore inviata con successo.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Errore durante l'invio della notifica email: {ex.Message}");
                }
            }
        }


  
    }
}
