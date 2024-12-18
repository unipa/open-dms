using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using OpenDMS.PdfManager;
using System.Text.Json;
using Web.Controllers.Documents;

namespace Web.Pages
{

    public class NewItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string IconColor { get; set; }
        public string URL { get; set; }
        public string Tooltip { get; set; }
        public string ImageData { get; set; }
        public bool Starred { get; set; }
        public bool IsFolder { get; set; }
    }
    public class ItemGroup
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Icon { get; set; }
        public string IconColor { get; set; }
        public List<NewItem> Items { get; set; }
    }

    [Authorize]
    public class NewModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IDocumentService documentService;
        private readonly ISearchService searchService;
        private readonly IDocumentTypeService documentTypeService;
        private readonly IUserService userService;
        private readonly ILookupTableService tableService;
        private readonly IACLService aclService;
        private readonly ILoggedUserProfile userContext;
        private readonly IUISettingsRepository uISettingsRepository;
        private readonly IConfiguration _config;

        public NewModel(ILogger<IndexModel> logger,
            IDocumentService documentService,
            ISearchService searchService,
            IDocumentTypeService documentTypeService,
            IUserService userService,
            ILookupTableService tableService,
            IACLService aclService,
            ILoggedUserProfile userContext,
            IUISettingsRepository uISettingsRepository,
            IConfiguration config)
        {
            _logger = logger;
            this.documentService = documentService;
            this.searchService = searchService;
            this.documentTypeService = documentTypeService;
            this.userService = userService;
            this.tableService = tableService;
            this.aclService = aclService;
            this.userContext = userContext;
            this.uISettingsRepository = uISettingsRepository;
            _config = config;
        }

        public string? ErrorMessage { get; set; } = "";
        public int FolderId { get; set; } = 0;

        public bool CanAddGenericDocument { get; set; } = false;

        //public List<LookupTable> Categorie { get; set; } = new List<LookupTable>();
        public List<ItemGroup> Tipologie { get; set; } = new List<ItemGroup>();
        public List<NewItem> InEvidenza { get; set; } = new List<NewItem>();

        //public List<Document> Processi { get; set; } = new List<Document>();


        public async Task<IActionResult> OnGetTogglePreferred(string DocumentTypeId, bool Preferred)
        {
            var u = userContext.Get();
            var starred = await uISettingsRepository.Get(User.Identity.Name, "DocumentTypes.Preferred");
            var tipi = string.IsNullOrEmpty(starred) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(starred);

            if (tipi == null) tipi = new List<string>();
            bool exist = tipi.IndexOf(DocumentTypeId) > -1;
            if (exist) tipi.Remove(DocumentTypeId);
            if (Preferred) tipi.Insert(0, DocumentTypeId);
            var tipiString = JsonSerializer.Serialize(tipi);
            await uISettingsRepository.Set(User.Identity.Name, "DocumentTypes.Preferred", tipiString);
            return new OkResult();
        }




        public async Task OnGet(int FolderId = 0)
        {
            var u = userContext.Get();
            Dictionary<string, ItemGroup> gruppi = new Dictionary<string, ItemGroup>();
            InEvidenza = new List<NewItem>();
            var starredTypes = await uISettingsRepository.Get(u.userId, "DocumentTypes.Preferred");
            var tipi = string.IsNullOrEmpty(starredTypes) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(starredTypes);
            if (tipi == null) tipi = new List<string>();

            try
            {
                var CanCreateRootFolder = await aclService.GetAuthorization("", u, PermissionType.Profile_CanCreateRootFolder) == AuthorizationType.Granted;
                //CanSendMail = (await userService.GetAllContactDigitalAddress(u.userId)).Count() > 0 && (await aclService.GetAuthorization("", u, PermissionType.Profile_CanSendMail) == AuthorizationType.Granted);
                //var CanCreateDocument = Tipologie.Where(t => t.ContentType != ContentType.Folder).Count() > 0;
                //CanAddAttachment = false;

                CanAddGenericDocument = (await aclService.GetAuthorization("", u, PermissionType.Profile_CanCreateGenericDocument) == AuthorizationType.Granted);
                if (FolderId > 0)
                {
                    var doc = await documentService.Get(FolderId);
                    var CanAddContent = ((await documentService.GetPermission(FolderId, u, PermissionType.CanAddContent)).Authorization == AuthorizationType.Granted) && doc.DocumentStatus != DocumentStatus.Stored && doc.DocumentStatus != DocumentStatus.Deleted;
                    CanCreateRootFolder = doc.ContentType == ContentType.Folder && CanAddContent;

                    if (CanAddGenericDocument)
                        CanAddGenericDocument = CanAddContent;

                    if (CanCreateRootFolder)
                    {
                        var url = "/NewDocument?DocumentType=&ContentType=" + ((int)ContentType.Folder).ToString() + "&FolderId=" + FolderId;
                        var imageData = ""; // "/images/previews/" + PreviewController.GetThumbnail("", ContentType.Folder, 0, OpenDMS.Domain.Enumerators.JobStatus.NotNeeded);

                        InEvidenza.Add(new()
                        {
                            Id = "",
                            Name = Constant.Labels.SubFolder,
                            Icon = "fa fa-folder",
                            IconColor = "gold",
                            Tooltip = "",
                            URL = url,
                            ImageData = imageData,
                            IsFolder = true,
                            Starred = false
                        });
                    }
                } else
                if (CanCreateRootFolder)
                {
                    var url = "/NewDocument?DocumentType=&ContentType=" + ((int)ContentType.Folder).ToString() + "&FolderId=" + FolderId;
                    var imageData = ""; // "/images/previews/" + PreviewController.GetThumbnail("", ContentType.Folder, 0, OpenDMS.Domain.Enumerators.JobStatus.NotNeeded);

                    InEvidenza.Add(new()
                    {
                        Id = "",
                        Name = Constant.Labels.SharedFolder,
                        Icon = "fa fa-folder",
                        IconColor = "lime",
                        Tooltip = "",
                        URL = url,
                        ImageData = imageData,
                        IsFolder = true,
                        Starred = false
                    });
                }
                foreach (var tipo in (await documentTypeService.GetByPermission(u, PermissionType.CanCreate, "", ""))
                      .Where(t => t.ContentType != ContentType.Folder
                                  && !t.Internal
                                  //&& (FolderId> 0 || t.Direction != 0)
                                  ).OrderBy(o => o.Description))
                {
                    ItemGroup gruppo = new();
                    if (gruppi.ContainsKey(tipo.CategoryId))
                        gruppo = gruppi[tipo.CategoryId];
                    else
                    {
                        Tipologie.Add(gruppo);
                        var cat = await tableService.GetById("$CATEGORIES$", tipo.CategoryId);
                        gruppo.Id = tipo.CategoryId;
                        gruppo.Name = cat.Description;
                        gruppo.Icon = tipo.Icon;
                        gruppo.IconColor = tipo.IconColor;
                        gruppo.Items = new();
                        gruppi.Add(tipo.CategoryId, gruppo);
                    }
                    var url = (!string.IsNullOrEmpty(tipo.CreationFormKey) ? "/NewForm" : "/NewDocument") + "?DocumentType=" + tipo.Id + "&ContentType=" + tipo.ContentType + "&FolderId=" + FolderId;
                   
                    var imageData = ""; // await GetImage(tipo); 
                    var starred = (tipi.IndexOf(tipo.Id) > -1);
                    if (string.IsNullOrEmpty(tipo.Icon))
                        tipo.Icon = tipo.ContentType == ContentType.Form
                            ? "fa fa-table"
                            : "fa fa-file-pdf-o";
                    if (string.IsNullOrEmpty(tipo.IconColor))
                        tipo.IconColor = tipo.ContentType == ContentType.Form
                            ? "var(--primary-bg-01)" //#0077dd"
                            : "crimson";

                    var item = new NewItem()
                    {
                        Id = tipo.Id,
                        Name = tipo.Name,
                        Icon = tipo.Icon,
                        IconColor = tipo.IconColor,
                        Tooltip = tipo.Description,
                        URL = url,
                        ImageData = imageData,
                        IsFolder = tipo.ContentType == ContentType.Folder,
                        Starred = starred
                    };
                    gruppo.Items.Add(item);
                    if (starred) InEvidenza.Add(item);
                }

                //if (CanCreateGenericDocument)
                //{
                //    InEvidenza.Add(new DocumentType() { Id = "", Name = "Documento da file", Icon = "fa fa-image", IconColor = "crimson", ContentType = ContentType.Document });
                //}



                //foreach (var id in tipi)
                //{
                //    var tipo = await documentTypeService.GetById(id);
                //    if (string.IsNullOrEmpty(tipo.Icon))
                //        tipo.Icon = tipo.ContentType == ContentType.Form
                //            ? "fa fa-table"
                //            : "fa fa-file-pdf-o";
                //    if (string.IsNullOrEmpty(tipo.IconColor))
                //        tipo.IconColor = tipo.ContentType == ContentType.Form
                //            ? "var(--primary-bg-01)" //#0077dd"
                //            : "crimson";
                //    tipo.ToBePreserved = true;
                //    var t = Tipologie.FirstOrDefault(t => t.Id == id);
                //    if (t != null) t.ToBePreserved = true;
                //    InEvidenza.Add(tipo);
                //}

                //foreach (var F in Tipologie.Select(t => t.CategoryId).Distinct())
                //{
                //    var L = await tableService.GetById("$CATEGORIES$", F);
                //    Categorie.Add(L);
                //}

                this.FolderId = FolderId;

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message.ToString();
            }
        }
    }
}