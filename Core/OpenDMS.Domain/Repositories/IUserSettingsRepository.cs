using OpenDMS.Domain.Entities.Users;

namespace OpenDMS.Domain.Repositories;


public interface IUserSettingsRepository
{
    Task<string> Get(string contactId, string Name);
    Task<string> Get(int Company, string contactId, string Name);

    Task Set(string contactId, string Name, string Value);
    Task Set(int Company, string contactId, string Name, string Value);


    Task<List<UserSetting>> GetAll(int Company, string contactId);

    //    Task Clone (string FromUser, string ToUser);
}

