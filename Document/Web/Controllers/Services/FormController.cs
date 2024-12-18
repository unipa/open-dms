using Microsoft.AspNetCore.Mvc;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Core.Interfaces;
using OpenDMS.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using OpenDMS.Domain.Repositories;
using System.Text;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Models;
using OpenDMS.Core.Managers;
using java.nio.channels;
using HtmlToOpenXml.IO;

namespace Web.Controllers.Services;

[Authorize]
[ApiController]
[Route("internalapi/[controller]")]
public class FormController : Controller
{
    private readonly ILogger logger;
    private readonly ILoggedUserProfile userContext;
    private readonly IDocumentService documentService;
    private readonly IUISettingsRepository uISettingsRepository;
    private readonly IViewServiceResolver viewMapper;
    private readonly IViewManager viewManager;

    public FormController(ILogger<FormController> logger,
        ILoggedUserProfile userContext,
        IDocumentService documentService,
        IUISettingsRepository uISettingsRepository,
        IViewServiceResolver viewMapper,
        IViewManager viewManager)
    {
        this.logger = logger;
        this.userContext = userContext;
        this.documentService = documentService;
        this.uISettingsRepository = uISettingsRepository;
        this.viewMapper = viewMapper;
        this.viewManager = viewManager;
    }


  
    public class FormRequest
    {
        public int MaxResults { get; set; } = 25;
        public Boolean Personal { get; set; } = false;
        public List<SortingColumn> OrderBy { get; set; } = new List<SortingColumn>();

        public List<SearchFilter> Filters { get; set; } = new List<SearchFilter>();

    }

    public class FormResponse
    {
        public DocumentInfo Document { get; set; }
        public string Data { get; set; }
    }

    [HttpPost("Documents")]
    public async Task<ActionResult<List<FormResponse>>> GetDocuments(FormRequest filters)
    {
        var u = userContext.Get();
        List<FormResponse> DocList = new List<FormResponse>();
        var service = await viewMapper.GetSearchService("doc.all");
        if (filters.Personal)
            filters.Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Owner, Operator = OperatorType.EqualTo, Values = { u.userId } });
        if (filters.MaxResults <= 0)
            filters.MaxResults = 8;

       var IdLists = await service.Find(filters.Filters, u, filters.MaxResults, filters.OrderBy);
        foreach (var idList in IdLists)
        {
            var image = (await documentService.Images(idList, u))
                .FirstOrDefault(i=>i.FileName.ToLower().EndsWith(".formhtml")
                                || i.FileName.ToLower().EndsWith(".formjs")
                                || i.FileName.ToLower().EndsWith(".formio")
                                );
            if (image != null)
            {
                var doc = await documentService.Load(idList, u);
                var data = System.Text.Encoding.UTF8.GetString(await documentService.GetContent(image.ImageId));
                if (image.FileName.ToLower().EndsWith(".formhtml"))
                {
                    // Test: x<ab xx a>
                    //       0123456789
                    // i = 1
                    // j = 7
                    // json = (1+4, 7 - 1 - 4) = xx
                    //var jsonString = Encoding.Default.GetString(await documentService.GetContent(Document.Image.Id));
                    var jsonString = data;
                    var StartTag = "<!-- FORM-DATA-BEGIN ";
                    var EndTag = " FORM-DATA-END -->";
                    var i = jsonString.IndexOf(StartTag);
                    var j = jsonString.IndexOf(EndTag);
                    if (i >= 0 && j > 0)
                    {
                        string json = jsonString.Substring(i + StartTag.Length, j - i - StartTag.Length);
                        data = json;
                        break;
                    }
                }

                FormResponse FR = new()
                {
                    Document = doc,
                    Data = data
                };
                DocList.Add(FR);
            }
        }
        return DocList;
    }



}
