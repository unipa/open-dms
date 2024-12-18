using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using System.Text.Json;
using Web.Model;

namespace Web.Pages
{
    [Authorize]
    public class ShareModel : PageModel
    {
        private readonly ILogger<ShareModel> _logger;
        private readonly IACLService aCLService;
        private readonly IDocumentService documentService;
        private readonly ILoggedUserProfile userProfile;
        private readonly IConfiguration _config;

        public bool CanAssignToAll { get; set; } = false;

        public ShareModel(ILogger<ShareModel> logger, 
            IACLService aCLService,
            IDocumentService documentService,
            ILoggedUserProfile userProfile,
            IConfiguration config)
        {
            _logger = logger;
            this.aCLService = aCLService;
            this.documentService = documentService;
            this.userProfile = userProfile;
            _config = config;
        }

        public SharePanelViewModel ShareInfo { get; set; }

        public async Task OnGetAsync(string id, string Message="")
        {
            ShareInfo = new SharePanelViewModel();

            try
            {
                ShareInfo.DocId = id;
                ShareInfo.Message = Message;
                ShareInfo.To = "";
                ShareInfo.Cc = "";
                var u = userProfile.Get();
                ShareInfo.Action = ((int)ActionRequestType.None).ToString();
                var docs = JsonSerializer.Deserialize<int[]>(id);
                var doc = await documentService.Get(docs[0]);
                int n = docs.Count();
                ShareInfo.Selected = n.ToString() + " document" + (n > 1 ? "i" : "o") ;
                CanAssignToAll = (await aCLService.GetAuthorization("", u, "Task.CanAssignToAll")) == AuthorizationType.Granted;
                ShareInfo.CompanyId =  doc.CompanyId;
                var valori = Enum.GetValues(typeof(ActionRequestType));
                var Stringhe = Enum.GetNames(typeof(ActionRequestType));

                ShareInfo.Actions = new List<SelectListItem>();
                ShareInfo.Actions.Add(new SelectListItem {  Value = ((int)ActionRequestType.None).ToString(), Selected = true, Text = "Presa Visione della comunicazione" });
                ShareInfo.Actions.Add(new SelectListItem { Value = ((int)ActionRequestType.Generic).ToString(), Text = "Attivita Generica" });
                ShareInfo.Actions.Add(new SelectListItem { Value = ((int)ActionRequestType.Approve_Document).ToString(), Text= "Approvazione / Autorizzazione" });
                if (docs.Length > 0)
                {
                    //ShareInfo.Actions.Add(new FieldTypeValue { Value = ((int)ActionRequestType.View_Document).ToString(), LookupValue = "Presa Visione degli allegati" });

                    //ShareInfo.Actions.Add(new FieldTypeValue { Value = ((int)ActionRequestType.Check_Document).ToString(), LookupValue = "Apposiizone Visto" });
                    //ShareInfo.Actions.Add(new FieldTypeValue { Value = ((int)ActionRequestType.Sign_Document).ToString(), LookupValue = "Firma Autografa" });
                    ShareInfo.Actions.Add(new SelectListItem { Value = ((int)ActionRequestType.DigitalSign_Document).ToString(), Text = "Firma Digitale degli allegati" });
                }
                //ShareInfo.Actions.Add(new FieldTypeValue { Value = ((int)ActionRequestType.Folder_Document).ToString(), LookupValue = "Fascicolazione" });
            }
            catch (Exception ex) { ShareInfo.ErrorMessage = ex.Message.ToString(); }

        }
    }
}
