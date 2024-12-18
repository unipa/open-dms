using DigitalSignService.Interfaces;
using OpenDMS.Core.DTOs;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.PdfManager;
using System.Text.Json;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace DigitalSignService.Services
{
    public class DMSWrapper : IDMSWrapper
    {
        public IAppSettingService AppSetting { get; }

        public DMSWrapper(IAppSettingService appSetting)
        {
            AppSetting = appSetting;
        }


        public async Task<string> GetSettings(string Host, string Setting)
        {
            HttpClient client = new HttpClient();
            string content = "?clientSecret=dss-" + (await AppSetting.GetSecret(Host)) + "&Setting=" + Setting;
            var Response = await client.GetAsync(Host + "/internalapi/client/GetSetting" + content);
            return await Response.Content.ReadAsStringAsync();

        }
        public async Task SetSettings(string Host, string Setting, string Value)
        {
            HttpClient client = new HttpClient();
            string content = "?clientSecret=dss-" + (await AppSetting.GetSecret(Host)) + "&Setting=" + Setting + "&Value=" + Value;
            var Response = await client.GetAsync(Host + "/internalapi/client/SetSetting" + content);
        }

        public async Task<DocumentInfo> GetDocument(string Host, int documentId)
        {
            var secret = await AppSetting.GetSecret(Host);
            string content = "?clientSecret=dss-" + (secret) + "&documentId=" + documentId;
            HttpClient client = new HttpClient();
            var Response = await client.GetAsync(Host + "/internalapi/client/Get" + content);
            var r = await Response.Content.ReadAsStringAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<DocumentInfo>(r);
        }


        public async Task<List<LookupTable>> GetFileSignature(string Host, FileProperty document)
        {
            List<LookupTable> _Signatures = new();
            try
            {
                var data = await GetFile(Host, document.Id, document.ImageId);
                using (var M = new MemoryStream(data))
                {
                    PDFFile pdf = new PDFFile(M);
                    foreach (var field in pdf.BlankSignatures())
                    {
                        var L = new LookupTable() { };
                        L.Id = field.Name.ToString();
                        L.Description = field.Name.ToString();
                        L.Annotation = field.Page.ToString();
                        _Signatures.Add(L);
                    }

                }
            }
            catch (Exception ex)
            {
            };
            return _Signatures;
        }

        public async Task<byte[]> GetFile(string Host, int documentId, int imageId)
        {
            HttpClient client = new HttpClient();
            string content = "?clientSecret=dss-" + (await AppSetting.GetSecret(Host)) + "&documentId=" + documentId + "&imageId=" + imageId;
            var Response = await client.GetAsync(Host + "/internalapi/client/GetContent" + content);
            return await Response.Content.ReadAsByteArrayAsync();
        }


        public async Task<List<FileProperty>> GetFiles(string Host, string Documents, bool postback)
        {
            List<FileProperty> _FileInfo = new List<FileProperty>();
            List<int> DocList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(Documents);
            foreach (var id in DocList)
            {
                try
                {
                    string r = "";
                    if (!postback)
                    {
                        var doc = await GetDocument(Host, id);

                        var fp = new FileProperty();
                        fp.Id = doc.Id;
                        fp.ImageId = doc.Image.Id;
                        fp.FileName = doc.Image.FileName;
                        fp.Name = doc.Description;
                        fp.DocType = doc.DocumentType.Name;
                        fp.Nr = doc.DocumentNumberFormattedValue;
                        fp.Date = doc.DocumentDate?.Date.ToString("dd/MM/yyyy") ?? "";
                        fp.Status = "";
                        _FileInfo.Add(fp);
                    }
                }
                catch (Exception ex)
                {
                };
            }
            return _FileInfo;
        }


        public async Task<string> SignFile(string Host, int documentId, int imageId, string fileName, byte[] file)
        {
            FileContent c = new FileContent();
            c.FileData = Convert.ToBase64String(file);
            c.DataIsInBase64 = true;
            c.FileName = fileName;
            HttpClient client = new HttpClient();
            var content = "?clientSecret=dss-" + (await AppSetting.GetSecret(Host)) + "&documentId=" + documentId + "&imageId=" + imageId;
            var Response = await client.PostAsync(Host + "/internalapi/client/Sign" + content, new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(c), System.Text.Encoding.UTF8, "application/json"));
            //data = await Response.Content.ReadAsByteArrayAsync();
            return await Response.Content.ReadAsStringAsync();

        }
        public async Task<int> AddFile(string Host, int documentId, string fileName, byte[] file)
        {
            FileContent c = new FileContent();
            c.FileData = Convert.ToBase64String(file);
            c.DataIsInBase64 = true;
            c.FileName = fileName;
            HttpClient client = new HttpClient();
            var content = "?clientSecret=dss-" + (await AppSetting.GetSecret(Host)) + "&documentId=" + documentId;
            var Response = await client.PostAsync(Host + "/internalapi/client/Upload" + content, new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(c), System.Text.Encoding.UTF8, "application/json"));
            //data = await Response.Content.ReadAsByteArrayAsync();
            return 1;// int.Parse(await Response.Content.ReadAsStringAsync());

        }
        public async Task<int> CheckIn(string Host, int documentId, string fileName, byte[] file)
        {
            FileContent c = new FileContent();
            c.FileData = Convert.ToBase64String(file);
            c.DataIsInBase64 = true;
            c.FileName = fileName;
            HttpClient client = new HttpClient();
            var content = "?clientSecret=dss-" + (await AppSetting.GetSecret(Host)) + "&documentId=" + documentId;
            var Response = await client.PostAsync(Host + "/internalapi/client/CheckIn" + content, new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(c), System.Text.Encoding.UTF8, "application/json"));
            DocumentImage data = JsonSerializer.Deserialize<DocumentImage>( await Response.Content.ReadAsByteArrayAsync());
            return data.Id;// int.Parse(await Response.Content.ReadAsStringAsync());

        }
        public async Task<byte[]> CheckOut(string Host, int imageId)
        {
            HttpClient client = new HttpClient();
            var content = "?clientSecret=dss-" + (await AppSetting.GetSecret(Host)) + "&imageId=" + imageId;
            var Response = await client.GetAsync(Host + "/internalapi/client/CheckOut" + content);
            return await Response.Content.ReadAsByteArrayAsync();
        }
    }

}