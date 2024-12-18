using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace OpenDMS.Core.DataTypes
{
    public class RestAPIFieldType : IDataTypeManager
    {
        private readonly ILookupTableService lookupTable;

        public string DataTypeValue => "$ra";
        public string DataTypeName => "Servizio REST";
        public string Icon => "icoRESTDataType";
        public string ControlType => "lookup";
        public bool IsInternal => false;
        public bool IsSearchable => false;
        public bool IsBlob => false;
        public bool IsCalculated => false;
        public bool IsPerson => false;
        public string AdminWebComponent => "AdminRESTInputField";
        public string WebComponent => "RESTInputField";
        public FieldType[] AvailableFields =>  new [] {
            new FieldType { ControlType="lookup", Customized=false, DataType = DataTypeValue, Description = "Servizio REST", Id = DataTypeValue, Name = "Servizio REST", Title = "Servizio REST" }
        };


        public RestAPIFieldType(ILookupTableService lookupTable)
        {
            this.lookupTable = lookupTable;
        }

        public string Deserialize(string Value, bool Cifrato = false)
        {
            return Value;
        }

        public async Task<FieldTypeValue> Calculate(FieldType M, string Value, Document document)
        {
            throw new NotImplementedException();
        }


        public async Task<FieldTypeValue> Lookup(FieldType M, string Value)
        {
            RESTProperties props = Newtonsoft.Json.JsonConvert.DeserializeObject<RESTProperties>(M.CustomProperties);
            FieldTypeValue FV = new FieldTypeValue() { Value = Value, LookupValue = Value, FormattedValue = Value, IsValid = false, FieldTypeId = M };
            if (!string.IsNullOrEmpty( props.Cache))
            {
                var l = await lookupTable.GetById(props.Cache, Value);
                if (l != null) {
                    FV.LookupValue = l.Description;
                    FV.FormattedValue = l.Id;
                    FV.Value = l.Id;
                    FV.IsValid = true;
                    FV.FieldTypeId = M;
                }
            }
            return FV;
        }

        public class JWTHeader
        {
            public string Alg { get; set; } = "HS256";
            public string Typ { get; set; } = "JWT";
        }
        public class JWTPayload
        {
            public Dictionary<string, string> Claims { get; set; } = new();
        }

        public string Base64UrlEncode(string somestring)
        {
            var bytes = Encoding.UTF8.GetBytes(somestring);
            return Base64UrlEncode(bytes);
        }
        public string Base64UrlEncode(byte[] bytes)
        {
            var base64 = System.Convert.ToBase64String(bytes);
            var base64url = base64.TrimEnd('=').Replace('+', '-').Replace('/', '_');
            return base64url;
        }
        public string GenerateSignature (string base64Header, string base64Payload, string secretKey)
        {
            var cipherText = $"{base64Header}.{base64Payload}";
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(cipherText));
            return Base64UrlEncode(hash);
        }

        public async Task<List<FieldTypeValue>> Search(FieldType M, string SearchValue, int MaxResults = 8)
        {
            List<FieldTypeValue> Items = new List<FieldTypeValue>();
            RESTProperties props = Newtonsoft.Json.JsonConvert.DeserializeObject<RESTProperties>(M.CustomProperties);

            var parameters = SearchValue.Split(',');

            var url = props.LookupURL.Replace("{maxresults}", MaxResults.ToString());
            for (int i = 0; i < parameters.Length; i++)
            {
                url = url.Replace($"{{{i}}}", parameters[i]);
            }
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(props.Timeout);
                if (props.AuthMethod == "BEARER")
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", props.Token);
                }
                if (props.AuthMethod == "BASIC")
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(props.UserName + ":" + props.Password)));
                }
                if (props.AuthMethod == "JWT")
                {
                    var h = new JWTHeader() { };
                    var p = new JWTPayload() { };
                    //if (!String.IsNullOrEmpty(props.UserName)) p.Claims.Add(props.UserName, props.Password);
                    p.Claims.Add("iat", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString());
                    if (!String.IsNullOrEmpty(props.sub)) p.Claims.Add("sub", props.sub);
                    if (!String.IsNullOrEmpty(props.exp)) p.Claims.Add("exp", props.exp);
                    if (!String.IsNullOrEmpty(props.iss)) p.Claims.Add("iss", props.iss);
                    if (!String.IsNullOrEmpty(props.aud)) p.Claims.Add("aud", props.aud);

                    foreach (var k in p.Claims.Keys)
                    {
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            p.Claims[k]= p.Claims[k].Replace($"{{{i}}}", parameters[i]);
                        }
                    }

                    var header = Base64UrlEncode(JsonConvert.SerializeObject(h, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() }, Formatting = Formatting.Indented }));
                    var pyload = Base64UrlEncode(JsonConvert.SerializeObject(p.Claims, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() }, Formatting = Formatting.Indented }));

                    var signature = GenerateSignature(header, pyload, props.Token);
                    var token = $"{header}.{pyload}.{signature}";
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue (token);
                }
                HttpRequestMessage req = new HttpRequestMessage();
                req.RequestUri = new Uri(url);
                switch (props.LookupMethod.ToLower())
                {
                    case "post":
                        req.Method = HttpMethod.Post;
                        break;
                    case "put":
                        req.Method = HttpMethod.Put;
                        break;
                    case "patch":
                        req.Method = HttpMethod.Patch;
                        break;
                    case "delete":
                        req.Method = HttpMethod.Delete;
                        break;
                    case "head":
                        req.Method = HttpMethod.Head;
                        break;
                    default:
                        req.Method = HttpMethod.Get;
                        break;
                }

                var response = await httpClient.SendAsync(req);
                if (response != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var jsonstring = await response.Content.ReadAsStringAsync();
                        var json = JArray.Parse (jsonstring);
                        foreach (var token in json.Children<JObject>())
                        {
                            StringBuilder text = new StringBuilder();
                            FieldTypeValue FV = new FieldTypeValue();
                            foreach (var f in props.Decodifica.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                            {
                                // se il cmapo inizia per apice o doppio apice, lo considero come una costante 
                                if (f.StartsWith("\"") || f.StartsWith("'"))
                                {
                                    text.Append(f.Substring(1,f.Length-2));
                                }
                                else
                                {
                                    text.Append(token[f].ToString());
                                }
                            }
                            FV.LookupValue = text.ToString();
                            FV.FormattedValue = token[props.Codice].ToString();
                            FV.Value = FV.FormattedValue;
                            FV.IsValid = true;
                            FV.FieldTypeId = M;
                            FV.Fields = new List<Domain.Entities.Settings.LookupTable>();
                            foreach (var t in token.Properties())
                            {
                                FV.Fields.Add(new Domain.Entities.Settings.LookupTable() { Id = t.Name, Description = t.Value.ToString() });
                            }
                            if (!String.IsNullOrEmpty(props.Cache))
                            {
                                var l = await lookupTable.GetById(props.Cache, FV.Value, false);
                                if (l == null)
                                {
                                    l = new Domain.Entities.Settings.LookupTable() { TableId = props.Cache, Id = FV.Value, Description = FV.LookupValue };
                                    await lookupTable.Insert(l);
                                } else
                                {
                                    l.Description = FV.LookupValue;
                                    await lookupTable.Update(l);
                                }
                            }
                            Items.Add(FV);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FieldTypeValue FV = new FieldTypeValue();
                FV.LookupValue = ex.Message;
                FV.FormattedValue = SearchValue;
                FV.Value = FV.FormattedValue;
                FV.IsValid = false;
                FV.FieldTypeId = M;
                Items.Add(FV);
            }
            return Items;
        }

        public string Serialize(string Value, bool Cifrato = false)
        {
            return Value;
        }

        public string Validate(FieldType M, string Value)
        {
            return "";
        }

        class RESTProperties
        {
            public string LookupURL { get; set; }
            public string LookupMethod { get; set; } = "GET";
            public string AuthMethod { get; set; } = "";

            public string Cache { get; set; } = "";
            public string Token { get; set; } = "";
            public string UserName { get; set; } = "";
            public string Password { get; set; } = "";
            public string Codice { get; set; }
            public string Decodifica { get; set; }
            //public string AltriCampi { get; set; }
            public string iss { get; set; }
            public string aud { get; set; }
            public string sub { get; set; }
            public string exp { get; set; }

            public int Timeout { get; set; } = 60;
        }

    }
}
