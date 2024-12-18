using FirmeRemoteLib.Interfaces;
using FirmeRemoteLib.Models;
using iTextSharp.text.pdf;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.WsTrust;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Reflection.PortableExecutable;
using System.Xml;
using System.Xml.Serialization;

namespace FirmeRemoteLib.Implementation
{
	public class InfocertUnipaSign : IRemoteSign
	{
		private readonly ILogger<InfocertUnipaSign> _logger;
		private readonly IConfiguration _configuration;

		private Uri _baseuri;
		private String RCONTEXT;
		private String ACONTEXT;
		private String LANGUAGE = "it";
		private Queue<FileToSign> _elencoFile;

		public InfocertUnipaSign(IConfiguration config, ILogger<InfocertUnipaSign> logger)
		{
			this._logger = logger;
			this._configuration = config;

			_elencoFile = new Queue<FileToSign>();

			_baseuri = new Uri("https://<infocert url>");
			ACONTEXT = "auto";
			RCONTEXT = "remote";

			if (!String.IsNullOrEmpty(config["BaseURL"]))
				_baseuri = new Uri(config["BaseURL"]);
			if (!String.IsNullOrEmpty(config["ACONTEXT"]))
				ACONTEXT = config["ACONTEXT"];
			if (!String.IsNullOrEmpty(config["RCONTEXT"]))
				RCONTEXT = config["RCONTEXT"];

		}

		public async Task<bool> SendOTP(string alias)
		{
			UriBuilder _builder = new UriBuilder(_baseuri);
			_builder.Path = RCONTEXT + "/request-otp/" + alias + "/" + LANGUAGE;

			using (HttpClient client = new HttpClient())
			{
				var resp = await client.GetAsync(_builder.Uri);

				if (!resp.IsSuccessStatusCode)
				{
					var result = await resp.Content.ReadAsStringAsync();
					_logger.LogError(result);

					XmlSerializer ser = new XmlSerializer(typeof(InfocertResponse));
					using (var sr = new StringReader(result))
					{
						InfocertResponse re = (InfocertResponse)ser.Deserialize(sr);
						throw new Exception(re.ToString());
					}
				}
				else
				{
					_logger.LogDebug("OTP Inviato");
				}
			}
			return true;
		}

		public void ClearDocumentQueue()
		{
			this._elencoFile.Clear();
		}

		public void EnqueueDocument(string filename, byte[] fileToSign)
		{
			this._elencoFile.Enqueue(new FileToSign() { FileName = filename, FileContent = fileToSign });
		}

		#region Firma Cades

		public async Task<byte[]> SignCades(string alias, string pin, string otp, String filename, byte[] filebyteToSign)
		{
			//ClearDocumentQueue();
			//EnqueueDocument(filename, filebyteToSign);
			//return await MultiSignCades(alias, pin, otp);
			throw new NotImplementedException();
		}

		public async Task<(byte[] Result, string FileName)> MultiSignCades(string alias, string pin, string otp)
		{
			if (String.IsNullOrEmpty(pin))
				throw new ArgumentException("PIN non fornito");

			if (_elencoFile.Count <= 0)
				throw new ArgumentException("File non forniti");

			UriBuilder _builder = new UriBuilder(_baseuri);
			_builder.Path = RCONTEXT + "/sign/cades/" + alias;

			byte[] Result = new byte[0];
			string FileName = "";
			using (HttpClient client = new HttpClient())
			{
				MultipartFormDataContent content = new MultipartFormDataContent();
				content.Add(new StringContent(pin), "pin");
				content.Add(new StringContent(otp), "otp");

				int counter = 0;
				foreach (var fl in _elencoFile)
				{

                    if (fl.FileName.EndsWith(".SignJson"))
                        continue;

                    content.Add(new ByteArrayContent(fl.FileContent), "contentToSign-" + counter, fl.FileName);
					counter++;
				}

				var resp = await client.PostAsync(_builder.Uri, content);

				if (resp.IsSuccessStatusCode)
				{
					using (MemoryStream mem = new MemoryStream())
					{
						resp.Content.ReadAsStream().CopyTo(mem);
						FileName = resp.Content.Headers.ContentDisposition.FileName;
						mem.Position = 0;
						Result = mem.ToArray();
					}
				}
				else
				{

					var res = await resp.Content.ReadAsStringAsync();
					_logger.LogError(res);

					XmlSerializer ser = new XmlSerializer(typeof(InfocertResponse));
					using (var sr = new StringReader(res))
					{
						InfocertResponse re = (InfocertResponse)ser.Deserialize(sr);
						throw new Exception(re.ToString());
					}
				}
			}

			return (Result, FileName);
		}

		#endregion

		#region Firma Pades

		public async Task<byte[]> SignPades(string alias, string pin, string otp, string filename, byte[] filebyteToSign, FirmaPades firma)
		{
			ClearDocumentQueue();
			EnqueueDocument(filename, filebyteToSign);
			return (await MultiSignPades(alias, pin, otp, firma)).Result;
		}

		public async Task<(byte[] Result, string FileName)> MultiSignPades(string alias, string pin, string otp, FirmaPades firma)
		{
			if (String.IsNullOrEmpty(pin))
				throw new ArgumentException("PIN non fornito");

			if (_elencoFile.Count <= 0)
				throw new ArgumentException("File non forniti");

			UriBuilder _builder = new UriBuilder(_baseuri);
			_builder.Path = RCONTEXT + "/sign/pades/" + alias;

			byte[] result = new byte[0];
			string FileName = "";
			using (HttpClient client = new HttpClient())
			{
				MultipartFormDataContent content = new MultipartFormDataContent("boundary");
				content.Add(new StringContent(pin), "pin");
				content.Add(new StringContent(otp), "otp");


				int counter = 0;
                foreach (var fl in _elencoFile)
                {
                    if (fl.FileName.EndsWith(".SignJson"))
                        continue;

                    if (counter == 0)
                    {
                        //Ottengo le coordinate del campo di firma  
                        //var frm = GetCampoFirmaByFieldName(firma, fl.FileContent);
                        //firma = frm.Item1;
                        //fl.FileContent = frm.Item2;
                    }

                    content.Add(new ByteArrayContent(fl.FileContent), "contentToSign-" + counter, fl.FileName);
                    counter++;
                }

                content.Add(new StringContent(firma.Pagina.ToString()), "box_signature_page");

                if (!String.IsNullOrEmpty(firma.SignField ?? ""))
                    content.Add(new StringContent((firma.SignField ?? "").ToString()), "box_signature_fieldname");
                else
                {
                    content.Add(new StringContent(firma.StartX.ToString()), "box_signature_llx");
                    content.Add(new StringContent(firma.StartY.ToString()), "box_signature_lly");
                    content.Add(new StringContent(firma.EndX.ToString()), "box_signature_urx");
                    content.Add(new StringContent(firma.EndY.ToString()), "box_signature_ury");
                }


                //content.Add(new StringContent((firma.MotivoFirma ?? "")), "box_signature_reason");
                //content.Add(new StringContent((firma.LabelMotivoFirma ?? "")), "box_signature_lbl_reason");
                //content.Add(new StringContent((firma.LabelFirma ?? "")), "box_signature_lbl_signedby");
                //content.Add(new StringContent((firma.LabelData ?? "")), "box_signature_lbl_date");

                var resp = await client.PostAsync(_builder.Uri, content);

				if (resp.IsSuccessStatusCode)
				{
					using (MemoryStream mem = new MemoryStream())
					{
						resp.Content.ReadAsStream().CopyTo(mem);
						FileName = resp.Content.Headers.ContentDisposition.FileName;
						mem.Position = 0;
						result = mem.ToArray();
					}
				}
				else
				{

					var res = await resp.Content.ReadAsStringAsync();
					_logger.LogError(res);

					XmlSerializer ser = new XmlSerializer(typeof(InfocertResponse));
					using (var sr = new StringReader(res))
					{
						InfocertResponse re = (InfocertResponse)ser.Deserialize(sr);
						throw new Exception(re.ToString());
					}

				}

			}

			return (result, FileName);
		}

        #endregion

        private (FirmaPades, byte[]) GetCampoFirmaByFieldName(FirmaPades fp, byte[] fileContent)
        {
            var Pdf = new PdfReader(fileContent);
            var fld = Pdf.AcroFields.GetFieldPositions(fp.SignField);

            if (fld.Count > 0)
            {
                fp.StartX = 1 * (int)(fld[0].position.Left);
                fp.StartY = 1 * (int)(fld[0].position.Bottom);
                fp.EndX = 1 * (int)(fld[0].position.Right);
                fp.EndY = 1 * (int)(fld[0].position.Top);
            }
            byte[] _content = new byte[0];

            using (var mem = new MemoryStream())
            {
                using (PdfStamper stamper = new PdfStamper(Pdf, mem))
                {
                    Pdf.AcroFields.RemoveField(fp.SignField, fp.Pagina);
                }
                _content = mem.ToArray();
            }

            return (fp, _content);
        }

        private class FileToSign
		{
			public String? FileName { get; set; }
			public Byte[]? FileContent { get; set; }
		}

	}
}
