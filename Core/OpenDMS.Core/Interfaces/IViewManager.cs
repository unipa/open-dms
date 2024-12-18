using OpenDMS.Core.DTOs;
using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Core.Interfaces;

public interface IViewManager
{
    Task<ViewProperties> Get(string viewId, string userId);
    Task<ViewProperties> Reset(string viewId, string userId);
    Task<bool> ChangeVisibility(string viewId, string userId, string columnId, bool show);
    Task<bool> ChangeWidth(string viewId, string userId, string columnId, string width);
    Task<bool> ChangeSorting(string viewId, string userId, string columnId, SortingType sortingType);
    Task<bool> ChangeAggregation(string viewId, string userId, string columnId, AggregateType aggregateType);
    Task<bool> MoveColumn(string viewId, string userId, string fromColumnId, string toColumnId);
    Task<bool> GroupBy(string viewId, string userId, string columnId);
    Task<bool> Save(ViewProperties viewProperties, string userId);
}