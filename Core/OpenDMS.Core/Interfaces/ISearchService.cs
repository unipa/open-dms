using OpenDMS.Core.DTOs;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;

namespace OpenDMS.Core.Interfaces;

public interface ISearchService
{
    bool ResolveView(string viewId);
    //Task<bool> GetRowActionLinks(string viewId, RowId rowKeys, UserProfile acl);

    Task<SearchResultPage> GetPage(ViewProperties view, SearchRequest request, UserProfile acl);
    Task<int> Count(List<SearchFilter> Filters, UserProfile acl);

    Task<List<int>> Find(List<SearchFilter> filters, UserProfile acl, int maxResults, List<SortingColumn> OrderBy = null);

    Task<SearchResult> Get(ViewProperties view, SearchRequest request, UserProfile acl);
    Task<bool> ChangeRowState(RowId RowKeys, RowState NewState, UserProfile acl);

    Task<ViewProperties> GetDefaultViewProperties(string ViewId, UserProfile acl);

    //Task<bool> GetCellActionLinks(ViewColumn Column, RowId rowKeys, UserProfile acl);

//    ViewColumn CreateColumn(string Key, bool visible = true);


}