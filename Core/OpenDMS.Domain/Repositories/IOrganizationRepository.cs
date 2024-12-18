using OpenDMS.Domain.Entities.OrganizationUnits;
using OpenDMS.Domain.Entities.Users;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;

namespace OpenDMS.Domain.Repositories;

public interface IOrganizationRepository
{
    Task<int> Insert(OrganizationNode node);
    Task<int> Update(OrganizationNode node);
    Task<int> Delete(OrganizationNode node);
    Task<int> Move(OrganizationNode OldNode, OrganizationNode NewNode);
    Task<List<OrganizationNode>> GetAll(int startISODate);
    Task<List<string>> GetUsersInRole(string roleId);
    Task<OrganizationNode> GetById(int startISODate, string GroupId);
    //Task<List<UserGroup>> GetAll(string userId);
    Task<OrganizationNode> GetByExternalId(string ExternalId, int StartISODate = 0);

    Task<List<UserGroupRole>> GetUsersInGroup(int startISODate, string groupId);
    Task<List<UserGroupRole>> GetUsersInGroup(string groupId);
    Task<List<UserGroupRole>> GetGroupsByUser(string userId);
    Task<UserGroupRole> GetUser(int startISODate, string groupId, string userId);
    Task<UserGroupRole> GetUser(string groupId, string userId);

    Task<int> AddUser(UserGroupRole acl);
    Task<int> RemoveUser(UserGroupRole acl);
    Task<int> UpdateUser(UserGroupRole acl);
    Task<List<UserGroupRole>> GetGroupsByRole(UserFilter filter);
    Task<OrganizationNode> GetByName(string shortName, int StartISODate = 0);


    Task<List<User>> GetInactiveUsers();
    Task<List<OrganizationNode>> GetInactiveNodes();
}