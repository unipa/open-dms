using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Models;

namespace Web.Pages.Folders
{
    [Authorize]
    public class ExpiredModel : PageModel
    {
        private readonly IDocumentTypeService documentTypeService;


        [BindProperty]
        public string SearchText { get; set; }
        [BindProperty]
        public DateTime dc1 { get; set; }
        [BindProperty]
        public DateTime dc2 { get; set; }

        public DocumentType DocumentType { get; set; }

        public SearchRequest FileFilters { get; set; } = null;

        public ExpiredModel(
            IDocumentTypeService documentTypeService)
        {
            this.documentTypeService = documentTypeService;
        }

        public async Task OnGet(string Id)
        {
            var doctype = await documentTypeService.GetById(Id);
            DocumentType = doctype;

            var freetext = ""; // Request.Form["Search"];
            dc1 = DateTime.UtcNow;
            dc2 = DateTime.UtcNow;
            var viewId = "Doc.ExpirationDate";
            FileFilters = new SearchRequest();
            FileFilters.PageSize = 25;
            FileFilters.PageIndex = 0;
            FileFilters.ViewId = viewId;
            FileFilters.Filters = new List<SearchFilter>();
            if (!String.IsNullOrEmpty(freetext))
                FileFilters.Filters.Add(new SearchFilter() { ColumnName = "FreeText", Operator = OpenDMS.Domain.Enumerators.OperatorType.Contains, Values = new() { freetext } });
            FileFilters.Filters.Add(new SearchFilter() { ColumnName = "ExpirationDate", Operator = OpenDMS.Domain.Enumerators.OperatorType.In, Values = new() { dc1.ToString("yyyyMMdd"), dc2.ToString("yyyyMMdd") } });

            //            var Status = Request.Form["Status"].ToString();
            //            if (!String.IsNullOrEmpty(Status))
            //                FileFilters.Filters.Add(new SearchFilter() { ColumnName = "Status", Operator = OpenDMS.Domain.Enumerators.OperatorType.In, Values = new() { Status } });
        }

        public async Task OnPost()
        {
            var freetext = SearchText;
            var viewId = "Doc.Expiration";
            FileFilters = new SearchRequest();
            FileFilters.PageSize = 25;
            FileFilters.PageIndex = 0;
            FileFilters.ViewId = viewId;
            FileFilters.Filters = new List<SearchFilter>();
            if (!String.IsNullOrEmpty(freetext))
                FileFilters.Filters.Add(new SearchFilter() { ColumnName = "FreeText", Operator = OpenDMS.Domain.Enumerators.OperatorType.Contains, Values = new() { freetext } });
            FileFilters.Filters.Add(new SearchFilter() { ColumnName = "ExpirationDate", Operator = OpenDMS.Domain.Enumerators.OperatorType.In, Values = new() { dc1.ToString("yyyyMMdd"), dc2.ToString("yyyyMMdd") } });

            //            var Status = Request.Form["Status"].ToString();
            //            if (!String.IsNullOrEmpty(Status))
            //                FileFilters.Filters.Add(new SearchFilter() { ColumnName = "Status", Operator = OpenDMS.Domain.Enumerators.OperatorType.In, Values = new() { Status } });
        }

    }
}
