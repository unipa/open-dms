using jdk.nashorn.@internal.objects.annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using System.Text.Json;
using System.Web.Helpers;

namespace Web.Pages
{
    [Authorize]
    public class ChooseTypeModel : PageModel
    {
        private readonly ILogger<ChooseTypeModel> _logger;
        private readonly IDocumentTypeService doctypeService;
        private readonly ILookupTableService tableService;
        private readonly ILoggedUserProfile userContext;
        private readonly ISearchService searchService;
        private readonly IDocumentService documentService;
        private readonly IUISettingsRepository uISettingsRepository;
        private readonly IConfiguration _config;

        public ChooseTypeModel(ILogger<ChooseTypeModel> logger,
            IDocumentTypeService doctypeService,
            ILookupTableService tableService,
            ILoggedUserProfile userContext,
            ISearchService searchService,
            IDocumentService documentService,
            IUISettingsRepository uISettingsRepository,
            IConfiguration config)
        {
            _logger = logger;
            this.doctypeService = doctypeService;
            this.tableService = tableService;
            this.userContext = userContext;
            this.searchService = searchService;
            this.documentService = documentService;
            this.uISettingsRepository = uISettingsRepository;
            _config = config;
        }
        public int FolderId { get; set; } = 0;

        public string? ErrorMessage { get; set; } = "";
        public LookupTable? Categoria { get; set; } = new() { Description = "Tutte" };
        public List<DocumentType> Tipologie { get; set; } = new List<DocumentType>();



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

        public async Task OnGet(string? IdCategoria, int FolderId=0, string? DocId = null)
        {
            var starred = await uISettingsRepository.Get(User.Identity.Name, "DocumentTypes.Preferred");
            var tipi = string.IsNullOrEmpty(starred) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(starred);
            if (tipi == null) tipi = new List<string>();

            var u = userContext.Get();
            try
            {
                this.FolderId = FolderId;
                Tipologie = (await doctypeService.GetByPermission(u, PermissionType.CanCreate, "", "")).Where(t => !t.Internal).ToList();
                if (IdCategoria != null)
                {
                    Tipologie = Tipologie.Where(dt => dt.CategoryId.Equals(IdCategoria)).ToList();
                    Categoria = await tableService.GetById("$CATEGORIES$", IdCategoria);
                }
                foreach (var T in  Tipologie) {
                    bool exist = tipi.IndexOf(T.Id) > -1;
                    //TODO: mettere un DTO
                    T.ToBePreserved = exist;
                    if (string.IsNullOrEmpty(T.Icon))
                        T.Icon = T.ContentType == ContentType.Form
                            ? "fa fa-table"
                            : "fa fa-file-pdf-o";
                    if (string.IsNullOrEmpty(T.IconColor))
                        T.IconColor = T.ContentType == ContentType.Form
                            ? "var(--primary-bg-01)"
                            : "crimson";

                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message.ToString();
            }
        }

    }
}
