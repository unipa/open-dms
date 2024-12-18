using Core.DigitalSignature.Model;
using Core.DigitalSignature.Pkcs11;
using DigitalSignService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;

namespace DesktopService.Pages
{

    public class TestSignModel : PageModel
    {
        private readonly ILogger<TestSignModel> _logger;
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

        public string Token { get; set; }




        public TestSignModel(ILogger<TestSignModel> logger,
            IAppSettingService appSettingService,
            IDMSWrapper wrapper)
        {
            _logger = logger;
            this.appSettingService = appSettingService;
            this.wrapper = wrapper;
        }

        private void SetProperties()
        {
            Vendors = new Dictionary<string, string>(DigitalSignature.Token.Vendors.List);
            Vendors.Add("", "Nessuno");
        }

        public async Task<IActionResult> OnGet(string documents, string host)
        {
            SignType = 0;// signType;
            Documents = documents;
            SetProperties();
            return Page();
        }

        public async Task<IActionResult> OnGetTokens(string host, string newVendor)
        {
            List<TokenItem> lista = new();
            try
            {
                var T = new SignatureDevice(newVendor);
                if (!String.IsNullOrEmpty(newVendor))
                {
                    lista = T.GetTokens();
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
                return new JsonResult(L);
            }
            catch (Exception ex) { new JsonResult(new CertificateItem() { Name = ex.Message, Id= "" }); }
            return null;
        }
        





   
    }
}