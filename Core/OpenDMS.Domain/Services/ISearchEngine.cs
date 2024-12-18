using OpenDMS.Domain.Models;

namespace OpenDMS.Domain.Services;
public interface ISearchEngine
{
    Task<Dictionary<int, float>> Search(string searchString, int PageSize, UserProfile user);
    Task<string> GetContent(int documentId, UserProfile user);
    //Task<bool> AddDocument(Documento d, byte[] img);
    Task Add(int documentId);
    Task Remove(int documentId);

    Task RebuildIndex();
}

