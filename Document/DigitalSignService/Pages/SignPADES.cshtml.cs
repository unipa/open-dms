using Core.DigitalSignature.Model;
using Core.DigitalSignature.Pkcs11;
using DigitalSignService.Interfaces;
using DigitalSignService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.DTOs;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.PdfManager;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace DesktopService.Pages
{

    public class SignPADESModel : PageModel
    {
        private readonly ILogger<SignModel> _logger;
        private readonly IAppSettingService appSettingService;
        private readonly IDMSWrapper wrapper;

        [BindProperty]
        public string Vendor { get; set; }
        public Dictionary<string, string> Vendors { get; set; } = new Dictionary<string, string>();


        [BindProperty]
        public string Certificate{ get; set; }
        public List<X509Certificate2> Certificates { get; set; } = new List<X509Certificate2>();

        [BindProperty]
        public string Documents { get; set; }

        [BindProperty]
        public string PIN { get; set; }

        [BindProperty]
        public int SignType { get; set; }

        [BindProperty]
        public string Host { get; set; }
        public string Token { get; set; }
        public string Reason { get; set; }
        public string SignField{ get; set; }

        //        public static FileProperty _FileInfo { get; set; } = new();
        //        public static List<LookupTable> _Signatures { get; set; } = new();
        public FileProperty FileInfo { get; set; } = new();
  
        public List<LookupTable> Signatures { get; set; } = new();


        public SignPADESModel(ILogger<SignModel> logger,
            IAppSettingService appSetting,
            IDMSWrapper wrapper)
        {
            _logger = logger;
            this.appSettingService = appSetting;
            this.wrapper = wrapper;
        }

        private void SetProperties()
        {
            Vendors = new Dictionary<string, string>(DigitalSignature.Token.Vendors.List);
            Vendors.Add("", "Nessuno");
        }


        public async Task<IActionResult> OnGet(string documents, string host, string? signField)
        {
            SignType = 0;// signType;
            Documents = documents;
            Host = host;
            if (Host == "") RedirectToPage("Error");
            SignField = signField;
            var ClientSecret = await appSettingService.GetSecret(host);
            if (!string.IsNullOrEmpty(ClientSecret))
            {
                try
                {
                    Vendor = await wrapper.GetSettings(Host, "DigitalSignature.TokenService.Vendor");
                }
                catch (Exception)
                {
                    ClientSecret = await appSettingService.GetSecret(host, true);
                    if (string.IsNullOrEmpty(ClientSecret))
                    {
                        return RedirectToPage("/SignIn", new { host = Host, returnUrl = Request.Path + Request.QueryString });
                    }
                    Vendor = await wrapper.GetSettings(Host, "DigitalSignature.TokenService.Vendor");
                }
                Certificate = await wrapper.GetSettings(Host, "DigitalSignature.TokenService.Certificate");
                Token = await wrapper.GetSettings(Host, "DigitalSignature.TokenService.Token");
                PIN = await wrapper.GetSettings(Host, "DigitalSignature.TokenService.PIN");

                SetProperties();
                var files = await wrapper.GetFiles(Host, documents, false);
                if (files.Count > 0)
                {
                    FileInfo = files[0];
                    Signatures = await wrapper.GetFileSignature(Host, FileInfo);

                }
                return Page();
            } else
                return RedirectToPage("/SignIn" , new { host = Host, returnUrl = Request.Path + Request.QueryString });
        }
        public async Task<IActionResult> OnGetTokens(string host, string newVendor)
        {
            Host = host;
            if (Host == "") RedirectToPage("Error");
            List<TokenItem> lista = new();
            try
            {
                var T = new SignatureDevice(newVendor);
                if (!String.IsNullOrEmpty(newVendor))
                {
                    lista = T.GetTokens();
                    var ClientSecret = await appSettingService.GetSecret(host);
                    if (!string.IsNullOrEmpty(ClientSecret))
                    {
                        await wrapper.SetSettings(Host, "DigitalSignature.TokenService.Vendor", newVendor);
                    }
                }
            }
            catch (Exception ex) { lista.Add(new TokenItem() { Label = ex.Message, Serial = "" }); }
            return new JsonResult(lista);
        }

        public async Task<IActionResult> OnGetCertificate(string host, string newVendor, string tokenSerial)
        {
            try
            {
                var T = new SignatureDevice(newVendor);
                var L = T.GetCertificates(T.GetTokenBySerial(tokenSerial).Slot);
                Host = host;
                if (Host == "") RedirectToPage("Error");
                var ClientSecret = await appSettingService.GetSecret(host);
                if (!string.IsNullOrEmpty(ClientSecret))
                {
                    await wrapper.SetSettings(Host, "DigitalSignature.TokenService.Vendor", newVendor);
                }
                return new JsonResult(L);
            }
            catch (Exception ex) { new JsonResult(new CertificateItem() { Name = ex.Message, Id = "" }); }
            return null;
        }



        public async Task<IActionResult> OnGetSignFileAsync([Required] string host, [Required] string vendor, [Required] string tokenSerial, [Required] string certificateSerial,[Required] string reason, [Required] string pin, [Required] int file, string? signField)
        {
            Host = host;
            if (Host == "") RedirectToPage("Error");
            var ClientSecret = await appSettingService.GetSecret(host);
            if (!string.IsNullOrEmpty(ClientSecret))
            {
                await wrapper.SetSettings(host, "DigitalSignature.TokenService.Vendor", vendor);
                await wrapper.SetSettings(host, "DigitalSignature.TokenService.Token", tokenSerial);
                await wrapper.SetSettings(host, "DigitalSignature.TokenService.Certificate", certificateSerial);
                await wrapper.SetSettings(host, "DigitalSignature.TokenService.PIN", pin);
                var f = await wrapper.GetFiles(Host, "[" + file + "]", false);

                var data = await wrapper.GetFile(host, file, f[0].ImageId);
                Vendor = vendor;
                Certificate = certificateSerial;

                var D = new SignatureDevice(Vendor);
                using (MemoryStream signed = new MemoryStream(D.SignPDF(data, reason, tokenSerial, certificateSerial, pin, signField)))
                {
                    //signed.Position = 0;
                    //FileContent c = new FileContent();
                    //c.FileData = Convert.ToBase64String(signed.ToArray());
                    //c.DataIsInBase64 = true;
                    //c.FileName = f[0].FileName;
                    var storedFile = await wrapper.SignFile(host, file, f[0].ImageId, f[0].FileName, signed.ToArray());

                    return new JsonResult(storedFile);
                }
            }
            return new JsonResult("");
        }

  
    }
}