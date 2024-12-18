using OpenDMS.Domain.Entities.V2;

namespace OpenDMS.Domain.Repositories;

public interface IPostItRepository
{
    public Task<PostIt> GetById(int id);
    public Task<IEnumerable<PostIt>> GetByDocument(int documentId, int pageIndex, int PageSize);
    public Task<int> CountByDocument(int documentId);
    public Task Add(PostIt postIt);
    public Task Delete(int id);
}