
using Azure;
using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.Infrastructure.Database.Repositories;

public class CustomPagesRepository : ICustomPagesRepository
{
    private readonly ApplicationDbContext ds;
    private readonly IApplicationDbContextFactory contextFactory;

    public CustomPagesRepository(IApplicationDbContextFactory contextFactory)
    {
        this.contextFactory = contextFactory;
        this.ds = (ApplicationDbContext)contextFactory.GetDbContext();
    }


    public CustomPage GetPage(string pageId)
    {
        if (string.IsNullOrEmpty(pageId)) return null;
        return ds.CustomPages.AsNoTracking().FirstOrDefault(m => m.PageId == pageId);
    }

    public List<CustomPage> GetPages(string parentPageId)
    {
        if (parentPageId == "")
            return ds.CustomPages.AsNoTracking().Where(m => m.ParentPageId == parentPageId || m.ParentPageId == null).OrderBy(o => o.Alignment).ThenBy(o => o.Position).ToList();

        return ds.CustomPages.AsNoTracking().Where(m => m.ParentPageId == parentPageId).OrderBy(o=>o.Alignment).ThenBy(o=>o.Position).ToList();
    }

    public void Register(CustomPage menuItem)
    {
        var m = GetPage(menuItem.PageId);
        if (m == null)
            ds.CustomPages.Add(menuItem);
        else
        {
            ds.CustomPages.Update(menuItem);
        }
        ds.SaveChanges();
    }
    public void Remove(CustomPage menuItem)
    {
        ds.CustomPages.Remove(menuItem);
        ds.SaveChanges();
    }


}
