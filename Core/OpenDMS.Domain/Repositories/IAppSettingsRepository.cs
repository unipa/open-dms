using OpenDMS.Domain.Entities.Settings;

namespace OpenDMS.Domain.Repositories;

public interface IAppSettingsRepository
{

    Task<string> Get(string Name);
    Task<string> Get(int Company, string Name);

    Task Set(string Name, string Value);
    Task Set(int Company, string Name, string Value);

    Task<List<AppSetting>> GetAll(int Company);


}

