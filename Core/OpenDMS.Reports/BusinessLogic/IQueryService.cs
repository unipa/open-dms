
using OpenDMS.Core.DTOs;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;

namespace OpenDMS.Reports.BusinessLogic;

public interface IQueryService
{
    void AddTable(string prefix, string table);
    void AddRelation(string fromPrefix, string toPrefix, string rules);

    string AddColumn(string fieldName, AggregateType aggregateType);
    



    bool ResolveView(string viewId);
    Task<bool> GetRowActionLinks(string viewId, RowId rowKeys, UserProfile acl);

    Task<SearchResultPage> GetPage(ViewProperties view, SearchRequest request, UserProfile acl);
    Task<int> Count(List<SearchFilter> Filters, UserProfile acl);

    Task<List<int>> Find(List<SearchFilter> filters, UserProfile acl, int maxResults, List<SortingColumn> OrderBy = null);

    Task<SearchResult> Get(ViewProperties view, SearchRequest request, UserProfile acl);
    Task<bool> ChangeRowState(RowId RowKeys, RowState NewState, UserProfile acl);

    Task<ViewProperties> GetDefaultViewProperties(string viewId);

    Task<bool> GetCellActionLinks(ViewColumn Column, RowId rowKeys, UserProfile acl);

    ViewColumn CreateColumn(string Key, bool visible = true);


}