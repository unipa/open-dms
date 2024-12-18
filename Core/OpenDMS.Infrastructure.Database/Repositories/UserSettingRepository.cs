using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenDMS.Domain.Entities.Users;
using OpenDMS.Domain.Repositories;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.Infrastructure.Repositories;

public class UserSettingRepository : IUserSettingsRepository
{
    private readonly ILogger<UserSetting> log;
    private readonly IApplicationDbContextFactory contextProvider;
    private readonly ApplicationDbContext dbContext;
    private Dictionary<string, string> Setting = new Dictionary<string, string>();

    public UserSettingRepository(ILogger<UserSetting> log, IApplicationDbContextFactory contextProvider)
    {
        this.log = log;
        this.contextProvider = contextProvider;
        this.dbContext = (ApplicationDbContext)contextProvider.GetDbContext();
    }


    public async Task<string> Get(string contactId, string attributeId)
    {
        return await Get(0, contactId, attributeId);
    }
    public async Task<string> Get(int companyId, string contactId, string attributeId)
    {
        string ret = "";
        try
        {
            var Key = companyId + "." + attributeId.ToLower()+"."+contactId;
            if (!Setting.ContainsKey(Key))
            {
                var v = dbContext.UserSettings.FirstOrDefault(a=>a.CompanyId==companyId && a.ContactId==contactId && a.AttributeId == attributeId);
                if (v != null)
                {
                    ret = v.Value;
                    Setting[Key] = ret;
                }
                else
                {
                    if (companyId != 0)
                    {
                        Key = "0." + attributeId.ToLower() + "." + contactId;
                        if (!Setting.ContainsKey(Key))
                        {
                            v = dbContext.UserSettings.FirstOrDefault(a => a.CompanyId == 0 && a.ContactId == contactId && a.AttributeId == attributeId);
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

    public async Task Set(string contactId, string attributeId, string Value)
    {
        await Set(0, contactId, attributeId, Value);
    }
    public async Task Set(int companyId, string contactId, string attributeId, string value)
    {
        attributeId = attributeId.ToLower();
        var Key = companyId + "." + attributeId + "." + contactId;
        try
        {
            var v = dbContext.UserSettings.FirstOrDefault(a => a.CompanyId == companyId && a.ContactId == contactId && a.AttributeId == attributeId);
            if (v != null)
            {

                if (string.IsNullOrEmpty(value))          
                    dbContext.UserSettings.Remove(v);
                else
                {
                    v.Value = value;
                    dbContext.UserSettings.Update(v);
                }
            }
            else
            {
                v = new UserSetting() { Value = value, CompanyId = companyId, ContactId = contactId, AttributeId = attributeId };
                dbContext.UserSettings.Add(v);
            }
            await dbContext.SaveChangesAsync();
            dbContext.Entry<UserSetting>(v).State = EntityState.Detached; //aggiunto da GB
           Setting[Key] = value;
        }
        catch (Exception ex)
        {
            log.LogError("Settings: " + ex.Message);
        };
    }

    public async Task<List<UserSetting>> GetAll(int companyId, string contactId)
    {
        return await dbContext.UserSettings.AsNoTracking().Where(s => s.CompanyId == companyId && s.ContactId == contactId).ToListAsync();
    }



}
