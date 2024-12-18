using A3Synch.Models;

namespace A3Synch.Interfacce
{
    public interface IUserGroupsDAO
    {
        //DAO per UserGroups
        Task<int> UpdateUserGroups(List<UserGroups> userGroupsList);
        Task<List<UserGroups>> GetAllUserGroups();
        string GetUserGroupsId(string externalId);
        string GetExternalId(string id);
        DateTime? GetClosingDate(string externalId);
        DateTime? GetCreationDate(string externalId);
        Task<UserGroups> GetUserGroupsById(string UserGroupsId);
        Task<List<string>> GetAllExternalIds();



    }
}
