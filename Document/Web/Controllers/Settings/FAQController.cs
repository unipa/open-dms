using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Core.Filters;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using org.apache.cxf.jaxrs.provider;
using Web.Pages.Folders;

namespace Web.Controllers.Settings;

[Authorize]
[ApiController]
[Route("internalapi/[controller]")]
public class FAQController : ControllerBase
{


    public class FAQItem : LookupTable
    {
        public List<LookupTable> Documents { get; set; } = new();
    }


    private readonly ILookupTableService lookupTableRepo;
    private readonly IDocumentService documentService;
    private readonly ILoggedUserProfile loggedUserProfile;
    private readonly ISearchService searchRepository;

    public FAQController(
        ILookupTableService lookupTableRepo,
        IDocumentService documentService,
        ILoggedUserProfile loggedUserProfile,
        ISearchService searchRepository )
    {
        this.lookupTableRepo = lookupTableRepo;
        this.documentService = documentService;
        this.loggedUserProfile = loggedUserProfile;
        this.searchRepository = searchRepository;
    }

    /// <summary>
    /// Restituisce i dati relativi a tutte le LookupTable associate ad un TableId. 
    /// </summary>
    /// <returns>Restituisce una lista di oggetti LookupTable</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<FAQItem>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet()]
    public async Task<ActionResult<List<FAQItem>>> GetAll()
    {
        try
        {
            var u =  loggedUserProfile.Get();
            List<FAQItem> result = new List<FAQItem>();
            foreach (var r in await lookupTableRepo.GetAll("FAQ"))
            {
                List<LookupTable> results = new List<LookupTable>();

                var Filters = new List<SearchFilter>();
                var su = UserProfile.SystemUser();
                Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.ContentType, Operator = OperatorType.EqualTo, Values = new() { ((int)ContentType.Document).ToString() } });
                Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Status, Operator = OperatorType.EqualTo, Values = new() { ((int)DocumentStatus.Active).ToString() } });
                Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.DocumentType, Operator = OperatorType.EqualTo, Values = new() { "FAQ" } });
                Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Field + "argomento", Operator = OperatorType.EqualTo, Values = new() { r.Id } });

                foreach (var d in await searchRepository.Find(Filters, u, 5, new List<SortingColumn>() { new SortingColumn() { ColumnId = DocumentColumn.Description, Descending = false } }))
                {
                    try
                    {
                        var doc = await documentService.Get(d);
                        LookupTable F = new();
                        F.Id = doc.Id.ToString();
                        F.Description = doc.Description;
                        var content = await documentService.GetContent(doc.ImageId.Value);
                        F.Annotation = System.Text.Encoding.Default.GetString(content);
                        F.TableId = r.Id; // doc.CreationDate.ToString("dd/MM/yyyy HH:mm");
                        results.Add(F);
                    }
                    catch { };
                }
                if (results.Count > 0)
                {
                    var f = new FAQItem() { Id = r.Id, Description = r.Description, Annotation = r.Annotation, Documents = results };
                    result.Add(f);
                }
            }
            return result.Count == 0 ? Ok(new List<LookupTable>()) : Ok(result);
        }
        catch (Exception ex) {
            return BadRequest(ex.Message); 
        }
    }


    /// <summary>
    /// Restituisce i dati relativi ad una LookupTable tramite Id e TableId. 
    /// </summary>
    /// <param name="GroupId"> Id della tabella </param>
    /// <returns>Restituisce un oggetto LookupTable</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(FAQItem))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{GroupId}")]
    public async Task<ActionResult<FAQItem>> GetById(string? GroupId)
    {
        try
        {
            var faq = (await lookupTableRepo.GetById("FAQ", GroupId.ToUpper()));
            FAQItem result = new FAQItem()
            {
                Id = faq.Id,
                Annotation = faq.Annotation,
                Description = faq.Description,
                TableId = faq.TableId
            };

            var u = loggedUserProfile.Get();
            List<LookupTable> results = new List<LookupTable>();

            var Filters = new List<SearchFilter>();
            var su = UserProfile.SystemUser();
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.ContentType, Operator = OperatorType.EqualTo, Values = new() { ((int)ContentType.Document).ToString() } });
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Status, Operator = OperatorType.EqualTo, Values = new() { ((int)DocumentStatus.Active).ToString() } });
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.DocumentType, Operator = OperatorType.EqualTo, Values = new() { "FAQ" } });

            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Field+ "argomento", Operator = OperatorType.EqualTo, Values = new() { GroupId } });

            foreach (var d in await searchRepository.Find(Filters, u, 200, new List<SortingColumn>() { new SortingColumn() { ColumnId = DocumentColumn.Description, Descending = false } }))
            {
                try
                {
                    var doc = await documentService.Get(d);
                    LookupTable F = new();
                    F.Id = doc.Id.ToString();
                    F.Description = doc.Description;
                    var content = await documentService.GetContent(doc.ImageId.Value);

                    F.Annotation = System.Text.Encoding.Default.GetString(content);
                    F.TableId = GroupId;// doc.CreationDate.ToString("dd/MM/yyyy HH:mm");
                    results.Add(F);
                }
                catch { };
            }
            result.Documents = results;
            return Ok(result);
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }


    /// <summary>
    /// Restituisce i dati relativi a tutte le LookupTable associate ad un TableId. 
    /// </summary>
    /// <returns>Restituisce una lista di oggetti LookupTable</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<FAQItem>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("/search/{Text}")]
    public async Task<ActionResult<List<FAQItem>>> Search(string Text)
    {
        try
        {
            var u = loggedUserProfile.Get();
            List<FAQItem> result = new List<FAQItem>();
            foreach (var r in (await lookupTableRepo.GetAll("FAQ")).Select(r => (FAQItem)r).ToList())
            {
                List<LookupTable> results = new List<LookupTable>();

                var Filters = new List<SearchFilter>();
                var su = UserProfile.SystemUser();
                Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.ContentType, Operator = OperatorType.EqualTo, Values = new() { ((int)ContentType.Document).ToString() } });
                Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Status, Operator = OperatorType.EqualTo, Values = new() { ((int)DocumentStatus.Active).ToString() } });
                Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.DocumentType, Operator = OperatorType.EqualTo, Values = new() { "FAQ" } });
                Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Field + "argomento", Operator = OperatorType.EqualTo, Values = new() { r.Id } });

                foreach (var d in await searchRepository.Find(Filters, u, 5, new List<SortingColumn>() { new SortingColumn() { ColumnId = DocumentColumn.Description, Descending = false } }))
                {
                    try
                    {
                        var doc = await documentService.Get(d);
                        LookupTable F = new();
                        F.Id = doc.Id.ToString();
                        F.Description = doc.Description;
                        var content = await documentService.GetContent(doc.ImageId.Value);

                        F.Annotation = System.Text.Encoding.Default.GetString(content);
                        F.TableId = r.Id; // doc.CreationDate.ToString("dd/MM/yyyy HH:mm");
                        results.Add(F);
                    }
                    catch { };
                }
                r.Documents = results;
                if (r.Documents.Count > 0)
                    result.Add(r);
            }
            return result.Count == 0 ? Ok(new List<LookupTable>()) : Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


}