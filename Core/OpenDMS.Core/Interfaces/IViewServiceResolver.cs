

namespace OpenDMS.Core.Interfaces;

public interface IViewServiceResolver
{
    Task<ISearchService> GetSearchService(string viewId);
}