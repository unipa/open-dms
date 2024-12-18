using OpenDMS.Domain.Entities.Users;

namespace OpenDMS.Domain.Repositories;


public interface IUISettingsRepository
{
    Task<string> Get(string userId, string Name);
    Task<string> Get(int Company, string userId, string Name);

    Task Set(string userId, string Name, string Value);
    Task Set(int Company, string userId, string Name, string Value);

    Task<List<UISetting>> GetAll(int Company, string userId);
}

