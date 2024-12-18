using OpenDMS.Core.DTOs;
using OpenDMS.Domain.Models;

namespace OpenDMS.Core.Interfaces;
public interface IDocumentParser
{
    //    public Task<string> FileParse(string Text);
    Task Validate(string Text, UserProfile user);
    Task<List<ParseResult>> Parse(string Text, UserProfile user, bool SuppressEvents = false);

    bool IsSupported(string Text);
}


public class ParseResult
{
    public bool Success { get; set; }
    public Exception Exceptionn { get; set; }
    public DocumentInfo Document { get; set; }
}