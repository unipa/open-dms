using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenDMS.Domain.Entities.Users;
using OpenDMS.Domain.Repositories;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.Infrastructure.Repositories;

public class UISettingRepository : IUISettingsRepository
{
    private readonly ILogger<UISetting> log;
    private readonly IApplicationDbContextFactory contextProvider;
    private readonly ApplicationDbContext dbContext;
    private Dictionary<string, string> Setting = new Dictionary<string, string>();

    public UISettingRepository(ILogger<UISetting> log, IApplicationDbContextFactory contextProvider)
    {
        this.log = log;
        this.contextProvider = contextProvider;
        this.dbContext = (ApplicationDbContext)contextProvider.GetDbContext();
    }


    public async Task<string> Get(string userId, string Name)
    {
        return await Get(0, userId, Name);
    }
    public async Task<string> Get(int Company, string userId, string Name)
    {
        string ret = "";
        try
        {
            var Key = Company + "." + Name.ToLower()+"."+userId;
            if (!Setting.ContainsKey(Key))
            {
                var v = dbContext.UISettings.FirstOrDefault(a=>a.CompanyId==Company && a.UserId==userId && a.Name == Name);
                if (v != null)
                {
                    ret = v.Value;
                    Setting[Key] = ret;
                }
                else
                {
                    if (Company != 0)
                    {
                        Key = "0." + Name.ToLower() + "." + userId;
                        if (!Setting.ContainsKey(Key))
                        {
                            v = dbContext.UISettings.FirstOrDefault(a => a.CompanyId == 0 && a.UserId == userId && a.Name == Name);
                            if (v != null)
                            {
                                ret = v.Value;
                                Setting[Key] = ret;
                            }
                        }
                        else ret = Setting[Key];
                    }
                }
            }
            else ret = Setting[Key];
        }
        catch (Exception ex)
        {
            log.LogError("UserSettings: " + ex.Message);
        };
        return ret;
    }

    public async Task Set(string userId, string Name, string Value)
    {
        await Set(0, userId, Name, Value);
    }
    public async Task Set(int Company, string userId, string name, string value)
    {
        name = name.ToLower();
        var Key = Company + "." + name + "." + userId;
        try
        {
            var v = dbContext.UISettings.FirstOrDefault(a => a.CompanyId == Company && a.UserId == userId && a.Name == name);
            if (v != null)
            {
                v.Value = value;
                dbContext.Entry<UISetting>(v).State = string.IsNullOrEmpty(value)
                    ? EntityState.Deleted
                    : EntityState.Modified;
            }
            else
            {
                v = new UISetting() { Value = value, CompanyId = Company, UserId = userId, Name = name };
                dbContext.UISettings.Add(v);
            }
            await dbContext.SaveChangesAsync();
             Setting[Key] = value;
        }
        catch (Exception ex)
        {
            log.LogError("Settings: " + ex.Message);
        };
    }

    public async Task<List<UISetting>> GetAll(int Company, string userID)
    {
        return await dbContext.UISettings.AsNoTracking().Where(s => s.CompanyId == Company && s.UserId == userID).ToListAsync();
    }

    Task<List<UISetting>> IUISettingsRepository.GetAll(int Company, string userId)
    {
        throw new NotImplementedException();
    }
}
