using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;

namespace OpenDMS.Core.Managers;

public class ViewManager : IViewManager
{
    private readonly IUserService userService;
    private readonly IUISettingsRepository uISettings;
    private readonly IViewServiceResolver serviceResolver;

    /// <summary>
    /// Ricve in input:
    /// - Contesto dell'utente loggato
    /// - Contesto di database del tenant corrente
    /// </summary>
    public ViewManager(IViewServiceResolver serviceResolve, IUserService userService, IUISettingsRepository uISettings)
    {
        this.userService = userService;
        this.uISettings = uISettings;
        this.serviceResolver = serviceResolve;
    }

    public async Task<bool> ChangeAggregation(string viewId, string userId, string columnId, AggregateType aggregateType)
    {
        var view = await Get(viewId, userId);
        var i = view.Columns.FindIndex(c => c.Id == columnId);
        if (i >= 0)
        {
            var col = view.Columns[i];
            col.Settings.AggregateType = aggregateType;
            var SerializedView = System.Text.Json.JsonSerializer.Serialize(view);
            await uISettings.Set(userId, "View." + viewId, SerializedView);
            return true;
        }
        return false;
    }

    public async Task<bool> ChangeSorting(string viewId, string userId, string columnId, SortingType sortingType)
    {
        var view = await Get(viewId, userId);
        var i = view.Columns.FindIndex(c => c.Id == columnId);
        if (i >= 0)
        {
            var col = view.Columns[i];
            col.Settings.SortType = sortingType;
            var SerializedView = System.Text.Json.JsonSerializer.Serialize(view);
            await uISettings.Set(userId, "View." + viewId, SerializedView);
            return true;
        }
        return false;
    }

    public async Task<bool> ChangeVisibility(string viewId, string userId, string columnId, bool show)
    {
        var view = await Get(viewId, userId);
        var i = view.Columns.FindIndex(c => c.Id == columnId);
        if (i >= 0)
        {
            var col = view.Columns[i];
            col.Settings.Visible = show;
            var SerializedView = System.Text.Json.JsonSerializer.Serialize(view);
            await uISettings.Set(userId, "View." + viewId, SerializedView);
            return true;
        }
        return false;
    }

    public async Task<bool> ChangeWidth(string viewId, string userId, string columnId, string width)
    {
        var view = await Get(viewId, userId);
        var i = view.Columns.FindIndex(c => c.Id == columnId);
        if (i >= 0)
        {
            var col = view.Columns[i];
            col.Settings.Width = width;
            var SerializedView = System.Text.Json.JsonSerializer.Serialize(view);
            await uISettings.Set(userId, "View." + viewId, SerializedView);
            return true;
        }
        return false;
    }

    public async Task<ViewProperties> Get(string viewId, string userId)
    {
        var service = await serviceResolver.GetSearchService(viewId);
        var view = await service.GetDefaultViewProperties(viewId, await userService.GetUserProfile(userId));

        var SerializedView = await uISettings.Get(userId, "View." + viewId);
        if (String.IsNullOrEmpty(SerializedView))
            SerializedView = await uISettings.Get(SpecialUser.SystemUser, "View." + viewId);

        ViewProperties viewSettings = new();
        if (!String.IsNullOrEmpty(SerializedView))
        {
            viewSettings = System.Text.Json.JsonSerializer.Deserialize<ViewProperties>(SerializedView);
        }
        else
            viewSettings = view;
        for (int i = 0; i < viewSettings.Columns.Count; i++)
        {
            var col = viewSettings.Columns[i];
            var colIndex = view.Columns.FindIndex(c => c.Id == col.Id);

            //var v = view.Columns[i];
            //var colIndex = viewSettings.Columns.FindIndex(c => c.Id == v.Id);
            //if (colIndex <  0)
            //{
            //    viewSettings.Columns.Add(v);
            //}
            //else
            if (colIndex >=  0)
            {
                var v = view.Columns[colIndex];
                v.Settings = col.Settings;
                viewSettings.Columns[i] = v;
            }

        }
        return viewSettings;
    }



    public async Task<bool> MoveColumn(string viewId, string userId, string fromColumnId, string toColumnId)
    {
        var view = await Get(viewId, userId);

        int FromIndex = view.Columns.FindIndex(c => c.Id == fromColumnId);
        int ToIndex = view.Columns.FindIndex(c => c.Id == toColumnId);

        if (FromIndex >=0 && FromIndex != ToIndex)
        {
            var FromColumn = view.Columns[FromIndex];
            view.Columns[FromIndex] = view.Columns[ToIndex];
            view.Columns[ToIndex] = FromColumn;
            var SerializedView = System.Text.Json.JsonSerializer.Serialize(view);
            await uISettings.Set(userId, "View." + viewId, SerializedView);
            return true;
        }
        return false;
    }

    public async Task<ViewProperties> Reset(string viewId, string userId)
    {
        await uISettings.Set(userId, "View." + viewId, "");
        return await Get(viewId, userId);
    }

    public async Task<bool> GroupBy(string viewId, string userId, string columnId)
    {
        var view = await Get(viewId, userId);
        var i = view.Columns.FindIndex(c => c.Id == columnId);
        if (i >= 0)
        {
            var col = view.Columns[i];
            if (col.IsGroupable)
            {
                view.GroupByColumn = columnId;
                var SerializedView = System.Text.Json.JsonSerializer.Serialize(view);
                await uISettings.Set(userId, "View." + viewId, SerializedView);
                return true;
            } 
        }
        return false;
    }

    public async Task<bool> Save(ViewProperties viewProperties, string userId)
    {
        var SerializedView = System.Text.Json.JsonSerializer.Serialize(viewProperties);
        await uISettings.Set(userId, "View." + viewProperties.ViewId, SerializedView);
        return true;
    }
}