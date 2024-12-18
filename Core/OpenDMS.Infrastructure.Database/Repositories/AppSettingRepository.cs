using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Repositories;

using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.Infrastructure.Repositories;

public class AppSettingRepository : IAppSettingsRepository
{
 
        private readonly ILogger<AppSetting> log;
        private readonly IApplicationDbContextFactory contextProvider;
        private readonly ApplicationDbContext dbContext;
        private Dictionary<string, string> Setting = new Dictionary<string, string>();

        public AppSettingRepository(ILogger<AppSetting> log, IApplicationDbContextFactory contextProvider)
        {
            this.log = log;
            this.contextProvider = contextProvider;
            this.dbContext = (ApplicationDbContext)contextProvider.GetDbContext();
        }


        public async Task<string> Get(string Name)
        {
            return await Get(0, Name);
        }
        public async Task<string> Get(int Company, string Name)
        {
            string ret = "";
            try
            {
                var Key = Company + "." + Name.ToLower() ;
                if (!Setting.ContainsKey(Key))
                {
                    var v = dbContext.AppSettings.FirstOrDefault(a => a.CompanyId == Company && a.Name == Name);
                    if (v != null)
                    {
                        ret = v.Value;
                        Setting[Key] = ret;
                    }
                    else
                    {
                        if (Company != 0)
                        {
                            Key = "0." + Name.ToLower();
                            if (!Setting.ContainsKey(Key))
                            {
                                v = dbContext.AppSettings.FirstOrDefault(a => a.CompanyId == 0 && a.Name == Name);
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
                log.LogError("AppSettings: " + ex.Message);
            };
            return ret;
        }

        public async Task Set(string Name, string Value)
        {
            await Set(0, Name, Value);
        }
        public async Task Set(int Company, string name, string value)
        {
            name = name.ToLower();
            var Key = Company + "." + name;
            try
            {
                var v = dbContext.AppSettings.FirstOrDefault(a => a.CompanyId == Company && a.Name == name);
                if (v != null)
                {
                    dbContext.Entry<AppSetting>(v).State = string.IsNullOrEmpty(value)
                        ? EntityState.Deleted
                        : EntityState.Modified;
                    v.Value = value;
                }
                else
                {
                    v = new AppSetting() { Value = value, CompanyId = Company, Name = name };
                    dbContext.AppSettings.Add(v);
                }
                await dbContext.SaveChangesAsync();
                Setting[Key] = value;
            }
            catch (Exception ex)
            {
                log.LogError("Settings: " + ex.Message);
            };
        }

        public async Task<List<AppSetting>> GetAll(int Company)
        {
            return await dbContext.AppSettings.AsNoTracking().Where(s => s.CompanyId == Company).ToListAsync();
        }

    }