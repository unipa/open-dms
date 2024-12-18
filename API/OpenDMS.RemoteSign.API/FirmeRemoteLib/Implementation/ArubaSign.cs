using ArubaService;
using FirmeRemoteLib.Interfaces;
using FirmeRemoteLib.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace FirmeRemoteLib.Implementation
{
    public class ArubaSign : IRemoteSign
    {
        private readonly ILogger<ArubaSign> _logger;
        private readonly IConfiguration _configuration;
        private ArubaService.ArubaSignServiceClient arubaSignServiceClient;
        private Queue<FileToSign> _elencoFile;

        public ArubaSign(IConfiguration config, ILogger<ArubaSign> logger)
        {
            //String url = "https://arss.demo.firma-automatica.it/ArubaSignService/ArubaSignService?wsdl";
            //String url = "https://prdapp-firmaarss.vsofia.local:8080/ArubaSignService/ArubaSignService?wsdl";
            String url = "http://172.20.1.112:8080/ArubaSignService/ArubaSignService?wsdl";

            arubaSignServiceClient = new ArubaService.ArubaSignServiceClient();
            arubaSignServiceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(new Uri(url));

            this._elencoFile = new Queue<FileToSign>();
            this._logger = logger;
            this._configuration = config;
        }

        public void ClearDocumentQueue()
        {
            this._elencoFile.Clear();
        }

        public void EnqueueDocument(string filename, byte[] fileToSign)
        {
            this._elencoFile.Enqueue(new FileToSign() { FileName = filename, FileContent = fileToSign });
        }

        public async Task<byte[]> MultiSignCades(string alias, string pin, string otp)
        {
            if (String.IsNullOrEmpty(pin))
                throw new ArgumentException("PIN non fornito");

            if (_elencoFile.Count <=0)
                throw new ArgumentException("File non forniti");

            if (String.IsNullOrEmpty(otp))
                throw new ArgumentException("OTP non fornito");

            ArubaService.auth auth = new ArubaService.auth();

            auth.user = alias;
            auth.userPWD = pin;
            auth.typeOtpAuth = "demoprod";
            auth.otpPwd = otp;
            auth.typeHSM = "COSIGN";

            var cert = await GetSignCert(auth);

            var session_id = await GetSignRoom(auth);

            List<signRequestV2> elencoReq = new List<signRequestV2>();

            foreach (var fl in _elencoFile)
            {
                signRequestV2 req = new signRequestV2();
                req.session_id = session_id;
                req.certID = cert.id;
                req.binaryinput = fl.FileContent;
                req.dstName = fl.FileName;
                req.identity= auth;
                req.transport = typeTransport.BYNARYNET;
                req.requiredmark = false;
                elencoReq.Add(req);
            }

            var signedResult = await arubaSignServiceClient.pkcs7signV2_multipleAsync(auth, elencoReq.ToArray(), false);
            if (signedResult.@return.status.StartsWith("KO"))
            {
                _logger.LogError("ERROR: " + signedResult.@return.description);
                throw new Exception(signedResult.@return.description);
            }
            else
            {

                var tmp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                Directory.CreateDirectory(tmp);

                if (signedResult.@return.return_signs.Length == elencoReq.Count)
                {
                    for (int i = 0; i < signedResult.@return.return_signs.Length; i++)
                    {
                        signedResult.@return.return_signs[i].dstPath = elencoReq[i].dstName + ".p7m";
                        var flFirmato = Path.Combine(tmp, elencoReq[i].dstName + ".p7m");
                        System.IO.File.WriteAllBytes(flFirmato, signedResult.@return.return_signs[i].binaryoutput);
                    }
                }

                var destzip = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".zip");
                ZipFile.CreateFromDirectory(tmp, destzip, CompressionLevel.Fastest, false);
                var result = System.IO.File.ReadAllBytes(destzip);
                Directory.Delete(tmp, true);
                System.IO.File.Delete(destzip);

                return result;
            }

        }

        public async Task<byte[]> MultiSignPades(string alias, string pin, string otp, FirmaPades firma)
        {
            if (String.IsNullOrEmpty(pin))
                throw new ArgumentException("PIN non fornito");

            if (_elencoFile.Count <=0)
                throw new ArgumentException("File non forniti");

            if (String.IsNullOrEmpty(otp))
                throw new ArgumentException("OTP non fornito");

            ArubaService.auth auth = new ArubaService.auth();

            auth.user = alias;
            auth.userPWD = pin;
            auth.typeOtpAuth = "demoprod";
            auth.otpPwd = otp;
            auth.typeHSM = "COSIGN";

            var cert = await GetSignCert(auth);

            var session_id = await GetSignRoom(auth);

            List<signRequestV2> elencoReq = new List<signRequestV2>();

            foreach (var fl in _elencoFile)
            {
                signRequestV2 req = new signRequestV2();
                req.session_id = session_id;
                req.certID = cert.id;
                req.binaryinput = fl.FileContent;
                req.dstName = fl.FileName;
                req.identity= auth;
                req.transport = typeTransport.BYNARYNET;
                req.requiredmark = false;
                elencoReq.Add(req);
            }

            pdfSignApparence _apparence = new pdfSignApparence();
            _apparence.leftx = firma.StartX;
            _apparence.rightx = firma.EndX;
            _apparence.lefty = firma.StartY;
            _apparence.righty = firma.EndY;
            _apparence.page = firma.Pagina;
            _apparence.testo = "Firmato da " + GetNomeFirmatario(cert);
            _apparence.bShowDateTime = true;
            _apparence.bScaleFont = false;


            var signedResult = await arubaSignServiceClient.pdfsignatureV2_multipleAsync(auth, elencoReq.ToArray(), _apparence, firma.SignField, pdfProfile.BASIC, null);
            if (signedResult.@return.status.StartsWith("KO"))
            {
                _logger.LogError("ERROR: " + signedResult.@return.description);
                throw new Exception(signedResult.@return.description);
            }
            else
            {

                var tmp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                Directory.CreateDirectory(tmp);

                if (signedResult.@return.return_signs.Length == elencoReq.Count)
                {
                    for (int i = 0; i < signedResult.@return.return_signs.Length; i++)
                    {
                        signedResult.@return.return_signs[i].dstPath = elencoReq[i].dstName;
                        var flFirmato = Path.Combine(tmp, elencoReq[i].dstName);
                        System.IO.File.WriteAllBytes(flFirmato, signedResult.@return.return_signs[i].binaryoutput);
                    }
                }

                var destzip = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".zip");
                ZipFile.CreateFromDirectory(tmp, destzip, CompressionLevel.Fastest, false);
                var result = System.IO.File.ReadAllBytes(destzip);
                Directory.Delete(tmp, true);
                System.IO.File.Delete(destzip);

                return result;
            }
        }

        public async Task<bool> SendOTP(string alias)
        {
            throw new NotImplementedException();
        }

        public async Task<byte[]> SignCades(string alias, string pin, string otp, string filename, byte[] filebyteToSign)
        {
            ArubaService.auth auth = new ArubaService.auth();

            auth.user = alias;
            auth.userPWD = pin;
            auth.typeOtpAuth = "demoprod";
            auth.otpPwd = otp;
            auth.typeHSM = "COSIGN";
            
            var cert = await GetSignCert(auth);

            var session_id = await GetSignRoom(auth);

            signRequestV2 req = new signRequestV2();
            req.session_id = session_id;
            req.certID = cert.id;
            req.binaryinput = filebyteToSign;
            req.identity= auth;
            req.transport = typeTransport.BYNARYNET;
            req.requiredmark = false;

            var signedResult = await arubaSignServiceClient.pkcs7signV2Async(req, false, false);
            if (signedResult.@return.status.StartsWith("KO"))
            {
                _logger.LogError("ERROR: " + signedResult.@return.description);
                throw new Exception(signedResult.@return.description);
            }
            else
                return signedResult.@return.binaryoutput;

        }

        public async Task<byte[]> SignPades(string alias, string pin, string otp, string filename, byte[] filebyteToSign, FirmaPades firma)
        {
            ArubaService.auth auth = new ArubaService.auth();

            auth.user = alias;
            auth.userPWD = pin;
            auth.typeOtpAuth = "demoprod";
            auth.otpPwd = otp;
            auth.typeHSM = "COSIGN";

            var cert = await GetSignCert(auth);

            var session_id = await GetSignRoom(auth);

            signRequestV2 req = new signRequestV2();
            req.session_id = session_id;
            req.certID = cert.id;
            req.binaryinput = filebyteToSign;
            req.identity= auth;
            req.transport = typeTransport.BYNARYNET;
            req.requiredmark = false;

            pdfSignApparence _apparence = new pdfSignApparence();
            _apparence.leftx = firma.StartX;
            _apparence.rightx = firma.EndX;
            _apparence.lefty = firma.StartY;
            _apparence.righty = firma.EndY;
            _apparence.page = firma.Pagina;
            _apparence.testo = "Firmato da " + GetNomeFirmatario(cert);
            _apparence.bShowDateTime = true;
            _apparence.bScaleFont = false;

            var signedResult = await arubaSignServiceClient.pdfsignatureV2Async(req, _apparence, firma.SignField, pdfProfile.BASIC, "", null);

            if (signedResult.@return.status.StartsWith("KO"))
            {
                _logger.LogError("ERROR: " + signedResult.@return.description);
                throw new Exception(signedResult.@return.description);
            }
            else
                return signedResult.@return.binaryoutput;

        }

        private String GetNomeFirmatario(cert _cert)
        {
            String nome = "";
            try
            {
                X509Certificate2 cert = new X509Certificate2(_cert.value);
                Regex cn = new Regex("CN=([^,]+)", RegexOptions.IgnoreCase);
                if (cn.IsMatch(cert.Subject))
                    nome = cn.Match(cert.Subject).Groups[1].Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return nome;
        }

        private async Task<cert> GetSignCert(ArubaService.auth auth)
        {
            var returnValue = new cert();

            var result = await arubaSignServiceClient.listCertAsync(auth);

            if (result.@return.status.StartsWith("KO"))
            {
                _logger.LogError("ERROR: " + result.@return.description);
                throw new Exception(result.@return.description);
            }
            else
                returnValue = result.@return.app1.FirstOrDefault() ?? result.@return.app2.FirstOrDefault();

            return returnValue ?? new cert();
        }

        private async Task<String> GetSignRoom(ArubaService.auth auth)
        {
            var result = await arubaSignServiceClient.opensessionAsync(auth);
            if (result.@return.StartsWith("KO"))
            {
                _logger.LogError("ERROR: " + result.@return);
                throw new Exception(result.@return);
            }
            else
                return result.@return;
        }

        Task<(byte[] Result, string FileName)> IRemoteSign.MultiSignCades(string alias, string pin, string otp)
        {
            throw new NotImplementedException();
        }

		Task<(byte[] Result, string FileName)> IRemoteSign.MultiSignPades(string alias, string pin, string otp, FirmaPades firma)
		{
			throw new NotImplementedException();
		}

		private class FileToSign
        {
            public String? FileName { get; set; }
            public Byte[]? FileContent { get; set; }
        }

    }
}
