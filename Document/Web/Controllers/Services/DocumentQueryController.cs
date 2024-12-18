using Microsoft.AspNetCore.Mvc;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Core.Interfaces;
using OpenDMS.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Models;

namespace Web.Controllers.Services;

[Authorize]
[ApiController]
[Route("internalapi/[controller]")]
public class DocumentQueryController : Controller
{
    private readonly ILogger logger;
    private readonly ILoggedUserProfile userContext;

    public IDocumentService documentService { get; }
    public IViewServiceResolver viewMapper { get; }

    public DocumentQueryController(ILogger<SearchController> logger,
        ILoggedUserProfile userContext,
        IDocumentService documentService,
        IViewServiceResolver viewMapper)
    {
        this.documentService = documentService;
        this.viewMapper = viewMapper;
        logger = logger;
        this.userContext = userContext;
    }


    public class QueryRequest
    {
        public int MaxResults { get; set; } = 25;
        public List<SortingColumn> OrderBy { get; set; } = new List<SortingColumn>();
        public List<SearchFilter> Filters { get; set; } = new List<SearchFilter>();

    }



    [HttpPost("find")]
    public async Task<List<DocumentInfo>> Find(QueryRequest request)
    {
        var u = userContext.Get();
        string userId = u.userId;
        var service = await viewMapper.GetSearchService("doc.all");
        var idlist = await service.Find(request.Filters, u, request.MaxResults, request.OrderBy);
        List<DocumentInfo> list = new List<DocumentInfo>();
        foreach (var id in idlist)
        {
            list.Add(await documentService.Load(id, u));
        }
        return list;
    }


}
