using Microsoft.AspNetCore.Authentication;
using OpenDMS.MaximoSR.API.Utility.Interfacce;
using System.Net;
using System.Net.Http.Headers;

namespace OpenDMS.MaximoSR.API.Utility
{
    public class HttpCallHandler
    {
        // HttpCallHandler : Creato da Vito Puleio
        public static async Task<HttpResponseMessage> PostAsyncCall(string URL, StringContent content, string? token, List<Tuple<string, string>>? headers)
        {
            //test in locale da noi
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            /* .... */
            HttpClient client = new HttpClient(clientHandler);
            if (token != null)
                client.DefaultRequestHeaders.Add("Authorization", token);

            if (headers != null)
            {
                foreach (Tuple<string, string> pair in headers)
                {
                    client.DefaultRequestHeaders.Add(pair.Item1, pair.Item2);
                }
            }

            HttpResponseMessage result = await client.PostAsync(URL, content);
            return result;
        }

        public static async Task<HttpResponseMessage> PostAsyncCallUrlEncoded(string URL, Dictionary<string, string> parameters, List<Tuple<string, string>>? headers)
        {
            //test in locale da noi
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient client = new HttpClient(clientHandler);

            if (headers != null)
            {
                foreach (Tuple<string, string> pair in headers)
                {
                    client.DefaultRequestHeaders.Add(pair.Item1, pair.Item2);
                }
            }

            var content = new FormUrlEncodedContent(parameters);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            HttpResponseMessage result = await client.PostAsync(URL, content);
            return result;
        }

        public static async Task<string> PostStringAsyncCall(string URL, StringContent content, string? token, List<Tuple<string, string>>? headers)
        {
            var result = await PostAsyncCall(URL, content, token, headers);
            return result.Content.ReadAsStringAsync().Result;
        }

        public static async Task<HttpResponseMessage> PutAsyncCall(string URL, StringContent content, List<Tuple<string, string>>? headers)
        {
            //test in locale da noi
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            /* .... */
            HttpClient client = new HttpClient(clientHandler);

            if (headers != null)
            {
                foreach (Tuple<string, string> pair in headers)
                {
                    client.DefaultRequestHeaders.Add(pair.Item1, pair.Item2);
                }
            }

            HttpResponseMessage result = await client.PutAsync(URL, content);
            return result;
        }

        public static async Task<string> GetStringAsync(string URL, string? token)
        {
            //test in locale da noi
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            /* .... */
            HttpClient client = new HttpClient(clientHandler);
            if (token != null)
                client.DefaultRequestHeaders.Add("Authorization", token);
            string result = await client.GetStringAsync(URL);
            return result;
        }

        public static async Task<HttpResponseMessage> GetAsyncCall(string URL, string? token, List<Tuple<string, string>>? headers)
        {
            //test in locale da noi
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            /* .... */
            HttpClient client = new HttpClient(clientHandler);
            if (token != null)
                client.DefaultRequestHeaders.Add("Authorization", token);

            if (headers != null)
            {
                foreach (Tuple<string, string> pair in headers)
                {
                    client.DefaultRequestHeaders.Add(pair.Item1, pair.Item2);
                }
            }

            HttpResponseMessage result = await client.GetAsync(URL);
            return result;
        }

        public static async Task<T?> GetAsync<T>(string URL, string? token = null, Dictionary<string, string>? headers = null) where T : class
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient client = new HttpClient(clientHandler);
            if (token != null) client.DefaultRequestHeaders.Add("Authorization", token);
            if (headers != null)
            {
                foreach (var pair in headers)
                    client.DefaultRequestHeaders.Add(pair.Key, pair.Value);
            }
            T? result = null;
            HttpResponseMessage response = await client.GetAsync(URL);
            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                string jsonResult = await response.Content.ReadAsStringAsync();
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonResult);
            }
            return result;
        }


        public async Task<string> GetStringAsyncCall(string URL, string? token, List<Tuple<string, string>>? headers)
        {
            var result = await GetAsyncCall(URL, token, headers);
            return result.Content.ReadAsStringAsync().Result;
        }

        public static async Task<HttpResponseMessage> DeleteAsyncCall(string URL, string? token)
        {
            //test in locale da noi
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            /* .... */
            HttpClient client = new HttpClient(clientHandler);
            if (token != null)
                client.DefaultRequestHeaders.Add("Authorization", token);
            HttpResponseMessage result = await client.DeleteAsync(URL);
            return result;
        }
    }

    /// <summary>
    /// HttpCallHandlerV2 : Esteso da Bosco Gianluca dalla prima versione di Vito Puleio.
    /// Classe arricchiti con altri metodi ma che parsano l'oggetto direttamente,
    /// I metodi di questa classe prendono il token autonomamente in caso in cui il token in ingresso è nullo
    /// lanciando un eccezione in caso di StatusCode diverso da 200.
    /// </summary>
    public class HttpCallHandlerV2 : IHttpCallHandlerV2
    {
        private readonly IKeycloakTokenHandler tokenHandler;
        private readonly IHttpContextAccessor httpContextAccessor;
        public HttpCallHandlerV2(IKeycloakTokenHandler tokenHandler = null, IHttpContextAccessor httpContextAccessor = null)
        {
            this.tokenHandler = tokenHandler;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetStringAsyncCall(string URL, string? token, List<Tuple<string, string>>? headers)
        {
            string SecondaryToken = "";
            if (token == null)
                SecondaryToken = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            //test in locale da noi
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            /* .... */
            HttpClient client = new HttpClient(clientHandler);

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + (token ?? SecondaryToken));

            if (headers != null)
            {
                foreach (Tuple<string, string> pair in headers)
                {
                    client.DefaultRequestHeaders.Add(pair.Item1, pair.Item2);
                }
            }

            HttpResponseMessage httpResult = await client.GetAsync(URL);

            var result = string.Empty;

            if (httpResult != null && httpResult.StatusCode == HttpStatusCode.OK)
            {
                result = httpResult.Content.ReadAsStringAsync().Result;
                return result;
            }
            else
                throw new HttpCallException(httpResult?.StatusCode, httpResult?.ReasonPhrase);
        }

        public async Task<string> PostStringAsyncCall(string URL, StringContent content, string? token, List<Tuple<string, string>>? headers)
        {
            string SecondaryToken = "";
            if (token == null)
                SecondaryToken = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            //test in locale da noi
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            /* .... */
            HttpClient client = new HttpClient(clientHandler);

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + (token ?? SecondaryToken));

            if (headers != null)
            {
                foreach (Tuple<string, string> pair in headers)
                {
                    client.DefaultRequestHeaders.Add(pair.Item1, pair.Item2);
                }
            }

            HttpResponseMessage httpResult = await client.PostAsync(URL, content);

            var result = string.Empty;

            if (httpResult != null && httpResult.StatusCode == HttpStatusCode.OK)
            {
                result = httpResult.Content.ReadAsStringAsync().Result;
                return result;
            }
            else
                throw new HttpCallException(httpResult?.StatusCode, httpResult?.ReasonPhrase);

        }

        public async Task<T> GetAsyncCall<T>(string URL, string? token, List<Tuple<string, string>>? headers)
        {
            string SecondaryToken = "";
            if (token == null)
                SecondaryToken = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            //test in locale da noi
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            /* .... */
            HttpClient client = new HttpClient(clientHandler);

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + (token ?? SecondaryToken));

            if (headers != null)
            {
                foreach (Tuple<string, string> pair in headers)
                {
                    client.DefaultRequestHeaders.Add(pair.Item1, pair.Item2);
                }
            }

            HttpResponseMessage httpResult = await client.GetAsync(URL);

            var result = default(T);

            if (httpResult != null && httpResult.StatusCode == HttpStatusCode.OK)
            {
                string jsonResult = httpResult.Content.ReadAsStringAsync().Result;
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonResult);
                return result;
            }
            else
                throw new HttpCallException(httpResult?.StatusCode, httpResult?.ReasonPhrase);
        }

        public async Task<T> PostAsyncCall<T>(string URL, StringContent content, string? token, List<Tuple<string, string>>? headers)
        {
            string SecondaryToken = "";
            if (token == null)
                SecondaryToken = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            //test in locale da noi
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            /* .... */
            HttpClient client = new HttpClient(clientHandler);

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + (token ?? SecondaryToken));

            if (headers != null)
            {
                foreach (Tuple<string, string> pair in headers)
                {
                    client.DefaultRequestHeaders.Add(pair.Item1, pair.Item2);
                }
            }

            HttpResponseMessage httpResult = await client.PostAsync(URL, content);

            var result = default(T);

            if (httpResult != null && httpResult.StatusCode == HttpStatusCode.OK)
            {
                string jsonResult = httpResult.Content.ReadAsStringAsync().Result;
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonResult);
                return result;
            }
            else
                throw new HttpCallException(httpResult?.StatusCode, httpResult?.ReasonPhrase);

        }

        public async Task<T> PutAsyncCall<T>(string URL, StringContent content, string? token, List<Tuple<string, string>>? headers)
        {
            string SecondaryToken = "";
            if (token == null)
                SecondaryToken = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            //test in locale da noi
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            /* .... */
            HttpClient client = new HttpClient(clientHandler);

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + (token ?? SecondaryToken));

            if (headers != null)
            {
                foreach (Tuple<string, string> pair in headers)
                {
                    client.DefaultRequestHeaders.Add(pair.Item1, pair.Item2);
                }
            }

            HttpResponseMessage httpResult = await client.PutAsync(URL, content);

            var result = default(T);

            if (httpResult != null && httpResult.StatusCode == HttpStatusCode.OK)
            {
                string jsonResult = httpResult.Content.ReadAsStringAsync().Result;
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonResult);
                return result;
            }
            else
                throw new HttpCallException(httpResult?.StatusCode, httpResult?.ReasonPhrase);
        }

        public async Task<T> DeleteAsyncCall<T>(string URL, StringContent content, string? token)
        {
            string SecondaryToken = "";
            if (token == null)
                SecondaryToken = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            //test in locale da noi
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            /* .... */
            HttpClient client = new HttpClient(clientHandler);

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + (token ?? SecondaryToken));

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(URL),
                Content = content
            };

            HttpResponseMessage httpResult = await client.SendAsync(request);

            var result = default(T);

            if (httpResult != null && httpResult.StatusCode == HttpStatusCode.OK)
            {
                string jsonResult = httpResult.Content.ReadAsStringAsync().Result;
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonResult);

                return result;
            }
            else
                throw new HttpCallException(httpResult?.StatusCode, httpResult?.ReasonPhrase);
        }

    }

  

    /// <summary>
    /// HttpCallHandlerV3 : Esteso da Bosco Gianluca dalla prima versione di Vito Puleio.
    /// Classe arricchiti con altri metodi ma che parsano l'oggetto direttamente,
    /// lanciando un eccezione in caso di StatusCode diverso da 200.
    /// </summary>
    public class HttpCallHandlerV3
    {
        public static async Task<string> GetStringAsyncCall(string URL, string? token, List<Tuple<string, string>>? headers)
        {
            //test in locale da noi
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            /* .... */
            HttpClient client = new HttpClient(clientHandler);
            if (token != null)
                client.DefaultRequestHeaders.Add("Authorization", token);

            if (headers != null)
            {
                foreach (Tuple<string, string> pair in headers)
                {
                    client.DefaultRequestHeaders.Add(pair.Item1, pair.Item2);
                }
            }

            HttpResponseMessage httpResult = await client.GetAsync(URL);

            var result = string.Empty;

            if (httpResult != null && httpResult.StatusCode == HttpStatusCode.OK)
            {
                result = httpResult.Content.ReadAsStringAsync().Result;
                return result;
            }
            else
                throw new HttpCallException(httpResult?.StatusCode, httpResult?.ReasonPhrase);
        }

        public static async Task<string> PostStringAsyncCall(string URL, StringContent content, string? token, List<Tuple<string, string>>? headers)
        {
            //test in locale da noi
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            /* .... */
            HttpClient client = new HttpClient(clientHandler);
            if (token != null)
                client.DefaultRequestHeaders.Add("Authorization", token);

            if (headers != null)
            {
                foreach (Tuple<string, string> pair in headers)
                {
                    client.DefaultRequestHeaders.Add(pair.Item1, pair.Item2);
                }
            }

            HttpResponseMessage httpResult = await client.PostAsync(URL, content);

            var result = string.Empty;

            if (httpResult != null && httpResult.StatusCode == HttpStatusCode.OK)
            {
                result = httpResult.Content.ReadAsStringAsync().Result;
                return result;
            }
            else
                throw new HttpCallException(httpResult?.StatusCode, httpResult?.ReasonPhrase);

        }


        public static async Task<T> GetAsyncCall<T>(string URL, string? token, List<Tuple<string, string>>? headers)
        {
            //test in locale da noi
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            /* .... */
            HttpClient client = new HttpClient(clientHandler);
            if (token != null)
                client.DefaultRequestHeaders.Add("Authorization", token);

            if (headers != null)
            {
                foreach (Tuple<string, string> pair in headers)
                {
                    client.DefaultRequestHeaders.Add(pair.Item1, pair.Item2);
                }
            }

            HttpResponseMessage httpResult = await client.GetAsync(URL);

            var result = default(T);

            if (httpResult != null && httpResult.StatusCode == HttpStatusCode.OK)
            {
                string jsonResult = httpResult.Content.ReadAsStringAsync().Result;
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonResult);
                return result;
            }
            else
                throw new HttpCallException(httpResult?.StatusCode, httpResult?.ReasonPhrase);
        }

        public static async Task<T> PostAsyncCall<T>(string URL, StringContent content, string? token, List<Tuple<string, string>>? headers)
        {
            //test in locale da noi
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            /* .... */
            HttpClient client = new HttpClient(clientHandler);
            if (token != null)
                client.DefaultRequestHeaders.Add("Authorization", token);

            if (headers != null)
            {
                foreach (Tuple<string, string> pair in headers)
                {
                    client.DefaultRequestHeaders.Add(pair.Item1, pair.Item2);
                }
            }

            HttpResponseMessage httpResult = await client.PostAsync(URL, content);

            var result = default(T);

            if (httpResult != null && httpResult.StatusCode == HttpStatusCode.OK)
            {
                string jsonResult = httpResult.Content.ReadAsStringAsync().Result;
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonResult);
                return result;
            }
            else
                throw new HttpCallException(httpResult?.StatusCode, httpResult?.ReasonPhrase);

        }

        public static async Task<T> PutAsyncCall<T>(string URL, StringContent content, string? token, List<Tuple<string, string>>? headers)
        {
            //test in locale da noi
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            /* .... */
            HttpClient client = new HttpClient(clientHandler);

            if (token != null)
                client.DefaultRequestHeaders.Add("Authorization", token);

            if (headers != null)
            {
                foreach (Tuple<string, string> pair in headers)
                {
                    client.DefaultRequestHeaders.Add(pair.Item1, pair.Item2);
                }
            }

            HttpResponseMessage httpResult = await client.PutAsync(URL, content);

            var result = default(T);

            if (httpResult != null && httpResult.StatusCode == HttpStatusCode.OK)
            {
                string jsonResult = httpResult.Content.ReadAsStringAsync().Result;
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonResult);
                return result;
            }
            else
                throw new HttpCallException(httpResult?.StatusCode, httpResult?.ReasonPhrase);
        }

        public static async Task<T> DeleteAsyncCall<T>(string URL, StringContent content, string? token)
        {
            //test in locale da noi
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            /* .... */
            HttpClient client = new HttpClient(clientHandler);
            if (token != null)
                client.DefaultRequestHeaders.Add("Authorization", token);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(URL),
                Content = content
            };

            HttpResponseMessage httpResult = await client.SendAsync(request);

            var result = default(T);

            if (httpResult != null && httpResult.StatusCode == HttpStatusCode.OK)
            {
                string jsonResult = httpResult.Content.ReadAsStringAsync().Result;
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonResult);

                return result;
            }
            else
                throw new HttpCallException(httpResult?.StatusCode, httpResult?.ReasonPhrase);
        }

    }

    public class HttpCallException : Exception
    {

        public HttpCallException()
            : base()
        {

        }

        public HttpCallException(HttpStatusCode? code, string? statusMessage)
        {
            StatusCode = code;
            if (code == HttpStatusCode.Unauthorized)
                StatusMessage = "Accesso non autorizzato";
            else
                StatusMessage = statusMessage;
        }

        public HttpStatusCode? StatusCode { get; set; }
        public string? StatusMessage { get; set; }

    }

}
