
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Infrastructure;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;

namespace Web.Pages
{
    public class AddSignModel : PageModel
    {
        private readonly ILoggedUserProfile userContext;
        private readonly IUserService userService;
        private readonly IDocumentService documentService;
        private readonly IAppSettingsRepository appSettings;
        private readonly IUserSettingsRepository userSettings;

        public string Documents { get; set; }
        public bool HasHandWrittenSign { get; set; }
        public bool HasVisto { get; set; }
        public bool HasRemoteSign { get; set; }
        public bool HasOTPSign { get; set; }
        public bool HasFEASign { get; set; }
        public bool HasPADESSign { get; set; }
        public string DigitalSignVendor { get; set; }
        public string signField { get; set; }

        public int UserPort { get; set; }

        public AddSignModel(ILoggedUserProfile userContext,
            IUserService userService,
            IDocumentService documentService,
            IAppSettingsRepository appSettings,
            IUserSettingsRepository userSettings)
        {
            this.userContext = userContext;
            this.userService = userService;
            this.documentService = documentService;
            this.appSettings = appSettings;
            this.userSettings = userSettings;
        }

        public async Task OnGet(string documents, string? signField = "")
        {
            var u = userContext.Get();
            var user = await userService.GetById(u.userId);
            var Vendor = await userSettings.Get(user.ContactId, OpenDMS.Domain.Constants.UserAttributes.CONST_DIGITALSIGNATURE_VENDOR);
            int.TryParse( await userSettings.Get(user.ContactId, OpenDMS.Domain.Constants.UserAttributes.CONST_DIGITALSIGNATURE_PORT), out int port);
            DigitalSignVendor = string.IsNullOrEmpty(Vendor) ? "Token non configurato" : Vendor;
            var remoteSignurl = await userSettings.Get(user.ContactId, OpenDMS.Domain.Constants.UserAttributes.CONST_REMOTESIGNATURE_SERVICE);
            var otpserviceurl = await userSettings.Get(user.ContactId, OpenDMS.Domain.Constants.UserAttributes.CONST_OTPSIGNATURE_SERVICE);
            var feaserviceurl = await userSettings.Get(user.ContactId, OpenDMS.Domain.Constants.UserAttributes.CONST_FEASIGNATURE_VENDOR);
            HasHandWrittenSign = false;
            HasVisto = false;
            this.signField = signField;

            List<int> docs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(documents);
            bool IsPdf = true;
            foreach (var docId in docs)
            {
                try
                {
                    var doc = await documentService.Load(docId, u);
                    if (doc.Image != null)
                    {
                        var fname = doc.Image.FileName;
                        if (Path.GetExtension(fname).ToLower() != ".pdf" && doc.Image.PreviewStatus == JobStatus.Completed)
                        {
                            fname += ".pdf";
                        }
                        IsPdf = Path.GetExtension(fname).ToLower() == ".pdf";
                        if (!IsPdf) break;
                        //HasHandWrittenSign = (userService.GetUserStamp(u.userId, ProfileType.User, "Sign") != null && (HasPADESSign || (doc.Image.PreviewStatus == OpenDMS.Domain.Enumerators.JobStatus.Completed || doc.Image.PreviewStatus == OpenDMS.Domain.Enumerators.JobStatus.NotNeeded)));
                        //HasVisto = (userService.GetUserStamp(u.userId, ProfileType.User, "Visto") != null && (HasPADESSign || (doc.Image.PreviewStatus == OpenDMS.Domain.Enumerators.JobStatus.Completed || doc.Image.PreviewStatus == OpenDMS.Domain.Enumerators.JobStatus.NotNeeded)));
                    }
                } catch (Exception ex){
                }
            }
            HasPADESSign = IsPdf;
            UserPort = port > 0 ? port : 5000;
            Documents = documents;
            HasRemoteSign =  !string.IsNullOrEmpty(remoteSignurl);
            HasOTPSign= !string.IsNullOrEmpty(otpserviceurl);
            HasFEASign = !string.IsNullOrEmpty(feaserviceurl);
        }
    }
}
