using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;

namespace Web.Pages
{
    [Authorize]
    public class PermissionsModel : PageModel
    {
        private readonly IDocumentService documentService;
        private readonly ILoggedUserProfile userContext;
        public bool CanView { get; set; }
        public bool CanViewContent { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanAddContent { get; set; }
        public bool CanRemoveContent { get; set; }
        public bool CanShare { get; set; }

        public string url { get; set; }

        public class PermissionModel
        {
            [BindProperty]
            public string Documents { get; set; }

            [BindProperty]
            public string Profiles { get; set; }

            [BindProperty]
            public int ViewDocument { get; set; } = 0;
            [BindProperty]
            public int ViewContent { get; set; } = 0;
            [BindProperty]
            public int EditDocument { get; set; } = 0;
            [BindProperty]
            public int DeleteDocument { get; set; } = 0;
            [BindProperty]
            public int AddContent { get; set; } = 0;
            [BindProperty]
            public int RemoveContent { get; set; } = 0;
            [BindProperty]
            public int ShareDocument { get; set; } = 0;

        }

        public PermissionModel Data { get; set; } = new PermissionModel();

        public PermissionsModel(IDocumentService documentService,
            ILoggedUserProfile userContext)
        {
            this.documentService = documentService;
            this.userContext = userContext;
        }

        public async Task OnGet(string id, string? Profile)
        {
            Data.Documents = id;
            Data.Profiles = Profile;
            //url = "https://dms-test.elmisoftware.com/internalapi/ui/";
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            bool isAdmin = u.userId == SpecialUser.SystemUser || u.Roles.Select(s => s.Id).Contains("admin");
            CanView = true;
            CanViewContent = true;
            CanEdit = true;
            CanAddContent = true;
            CanRemoveContent = true;
            CanShare = true;
            CanDelete = true;
            var docs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(Data.Documents);
            foreach (var DocumentId in docs)
            {
                if (DocumentId > 0)
                {
                    var PList = await documentService.GetPermissions(DocumentId, u);
                    var P = PList.FirstOrDefault(p => p.PermissionId == PermissionType.CanView);
                    CanView &= ((P != null) && (P.Authorization == OpenDMS.Domain.Enumerators.AuthorizationType.Granted)) || isAdmin;
                    P = PList.FirstOrDefault(p => p.PermissionId == PermissionType.CanViewContent);
                    CanViewContent &= ((P != null) && (P.Authorization == OpenDMS.Domain.Enumerators.AuthorizationType.Granted)) || isAdmin;
                    P = PList.FirstOrDefault(p => p.PermissionId == PermissionType.CanEdit);
                    CanEdit &= ((P != null) && (P.Authorization == OpenDMS.Domain.Enumerators.AuthorizationType.Granted)) || isAdmin;
                    P = PList.FirstOrDefault(p => p.PermissionId == PermissionType.CanAddContent);
                    CanAddContent &= ((P != null) && (P.Authorization == OpenDMS.Domain.Enumerators.AuthorizationType.Granted)) || isAdmin;
                    P = PList.FirstOrDefault(p => p.PermissionId == PermissionType.CanRemoveContent);
                    CanRemoveContent &= ((P != null) && (P.Authorization == OpenDMS.Domain.Enumerators.AuthorizationType.Granted)) || isAdmin;
                    P = PList.FirstOrDefault(p => p.PermissionId == PermissionType.CanDelete);
                    CanShare &= ((P != null) && (P.Authorization == OpenDMS.Domain.Enumerators.AuthorizationType.Granted)) || isAdmin;
                    P = PList.FirstOrDefault(p => p.PermissionId == PermissionType.CanShare);
                    CanDelete &= ((P != null) && (P.Authorization == OpenDMS.Domain.Enumerators.AuthorizationType.Granted)) || isAdmin;
                }
            }
            if (string.IsNullOrEmpty(Profile))
            {
                Data.ViewDocument = 1;
                Data.ViewContent = 1;
                Data.ShareDocument = 1;
            }
            else
            {
                var ProfileId = Profile.Substring(1);
                var PType = (ProfileType)(int.Parse(Profile.Substring(0, 1)));

                var docs2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(Data.Documents);
                foreach (var DocumentId in docs2)
                {
                    if (DocumentId > 0)
                    {
                        var PList2 = await documentService.GetProfilePermissions(DocumentId, PType, ProfileId);

                        var P2 = PList2.Permissions.FirstOrDefault(p => p.PermissionId == PermissionType.CanView);
                        if (P2 != null) Data.ViewDocument = (int)P2.Authorization;
                        
                        P2 = PList2.Permissions.FirstOrDefault(p => p.PermissionId == PermissionType.CanViewContent);
                        if (P2 != null) Data.ViewContent = (int)P2.Authorization;
                        
                        P2 = PList2.Permissions.FirstOrDefault(p => p.PermissionId == PermissionType.CanEdit);
                        if (P2 != null) Data.EditDocument = (int)P2.Authorization;
                        
                        P2 = PList2.Permissions.FirstOrDefault(p => p.PermissionId == PermissionType.CanAddContent);
                        if (P2 != null) Data.AddContent = (int)P2.Authorization;
                        
                        P2 = PList2.Permissions.FirstOrDefault(p => p.PermissionId == PermissionType.CanRemoveContent);
                        if (P2 != null) Data.RemoveContent = (int)P2.Authorization;
                        
                        P2 = PList2.Permissions.FirstOrDefault(p => p.PermissionId == PermissionType.CanShare);
                        if (P2 != null) Data.ShareDocument = (int)P2.Authorization;

                        P2 = PList2.Permissions.FirstOrDefault(p => p.PermissionId == PermissionType.CanDelete);
                        if (P2 != null) Data.DeleteDocument = (int)P2.Authorization;
                    }
                }
            }
        }

        public async Task OnGetSave(PermissionModel NewData)
        {

                var u = userContext.Get();
                //u = UserProfile.SystemUser();
                bool isAdmin = u.userId == SpecialUser.SystemUser || u.Roles.Select(s => s.Id).Contains("admin");
                string error = "";
                if (string.IsNullOrEmpty(NewData.Profiles))
                    error = "Non è stato indicato nessun profilo da autorizzare";
                else
                {

                    var docs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(NewData.Documents);

                    foreach (var p in NewData.Profiles.Split(","))
                    {
                        var ProfileId = p.Substring(1);
                        var PType = (ProfileType)(int.Parse(p.Substring(0, 1)));
                        try
                        {
                            await documentService.SetPermissions(docs, u, ProfileId, PType, new() {
                            {  PermissionType.CanView, (AuthorizationType)NewData.ViewDocument },
                            {  PermissionType.CanViewContent, (AuthorizationType)NewData.ViewContent },
                            {  PermissionType.CanEdit, (AuthorizationType)NewData.EditDocument },
                            {  PermissionType.CanShare, (AuthorizationType)NewData.ShareDocument },
                            {  PermissionType.CanAddContent, (AuthorizationType)NewData.AddContent },
                            {  PermissionType.CanRemoveContent, (AuthorizationType)NewData.RemoveContent },
                            {  PermissionType.CanDelete, (AuthorizationType)NewData.DeleteDocument }
                            });
                        }
                        catch (Exception ex)
                        {
                            error += p + ": " + ex.Message + "<br/>";
                        }
                    }
                }
                //error = "I seguenti profili hanno ha già un permesso sul documento";
//                return new JsonResult(error);
        }

    }
}