
using Microsoft.Extensions.Logging;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Models;
using System.DirectoryServices.ActiveDirectory;
using System.Security.Principal;

namespace OpenDMS.Core.Parser;

/// <summary>
/// {
///     documents: [{
///         id: 0,
///         description: "",
///         ...
///     }]
/// 
/// </summary>
public class JsonParser : IDocumentParser
{
    private readonly ILogger<JsonParser> logger;
    private readonly IDocumentService documentService;

    public JsonParser(ILogger<JsonParser> logger,
        IDocumentService documentService)
    {
        this.logger = logger;
        this.documentService = documentService;
    }

    public bool IsSupported(string Text)
    {
        return (Text.Length> 20) && Text.Substring(0,20).Trim().Remove('\r').Remove('\n').Remove(' ').ToLower().StartsWith("{documents:[");
    }

    public class OperationType
    {
        public string Id { get; set; }
        public string Document { get; set; }
    }
  

    public async Task<List<ParseResult>> Parse(string Text, UserProfile user, bool SuppressEvents = false)
    {
        List<ParseResult> Result = new List<ParseResult>();
        ParseResult R = new ParseResult();
        R.Success = true;
        try
        {
            Result.Add(R);
            OperationType[] operations = Newtonsoft.Json.JsonConvert.DeserializeObject<OperationType[]>(Text);
            foreach (var op in operations)
            {
                ParseResult R1 = new ParseResult();
                Result.Add(R1);  
                try
                {
                    //TODO: parsare op.Document

                    CreateOrUpdateDocument doc = Newtonsoft.Json.JsonConvert.DeserializeObject<CreateOrUpdateDocument>(op.Document);
                    int Id = 0;
                    int.TryParse(op.Id, out Id);
                    var documentInfo = Id <= 0 ? await documentService.CreateAndRead(doc, user) : await documentService.Update(Id, doc, user);
                    R1.Success = true;
                    R1.Document = documentInfo;
                }
                catch (Exception ex)
                {
                    R1.Exceptionn = ex;
                }
            }
        } catch (Exception ex)
        {
            R.Exceptionn = ex;
        }
        R.Success = !Result.Any(r=>!r.Success);
        R.Document = Result.Select(r=>r.Document).FirstOrDefault(r => r != null);
        return Result;
    }

    public async Task Validate(string Text, UserProfile user)
    {
        return;
    }
}
