using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Entities.Users;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using System.Security.Claims;

namespace OpenDMS.Domain.Services;


public interface IUserService
{
    IUserSettingsRepository Settings { get; }


    Task<UserProfile> GetUserProfile(string userId);

    #region Avatar
    Task<byte[]> GetAvatar(string userId);
    Task<byte[]> GetAvatar(string profileId, ProfileType profileType);
    Task<byte[]> GetDefaultAvatar(string profileId, ProfileType profileType);
    Task<List<Tuple<string, byte[]>>> GetUploadedAvatars(string profileId, ProfileType profileType);
    Task SetAvatar(string profileId, ProfileType profileType, byte[] imageBytes);
    Task SetAvatar(string profileId, ProfileType profileType, string guid);
    Task UploadAvatar(string profileId, ProfileType profileType, byte[] imageBytes);
    Task DeleteAvatar(string profileId, ProfileType profileType, string guid);

    #endregion

    #region UserStamp
    Task<byte[]> GetUserStamp(string profileId, ProfileType profileType, string stampId);
    Task SetUserStamp(string profileId, ProfileType profileType, string stampId, byte[] signBytes);

    #endregion



    Task<List<LookupTable>> GetGroups(string userId);
    Task<List<LookupTable>> GetRoles(string userId);

    Task<string> GetName(string userId);


    Task<string> GetAttribute(string userId, string attributeId, int companyId = 0);
    Task SetAttribute(string userId, string attributeId, string value, int companyId = 0);

    Task<User> GetById(string userId);
    Task<List<User>> Find(string SearchText, int MaxResults = 0);

    Task<User> UpdateUserInfo(ClaimsPrincipal Claim);
    Task<User> AddOrUpdate(User new_user);

    Task<string> GetProfileName(string Profile);
    Task<string> GetProfileName(string ProfileId, ProfileType ProfileType);


    #region ContactDigitalAddress

    Task<int> AddOrUpdateAddress(ContactDigitalAddress bd, string executor);
    Task<int> DeleteContactDigitalAddress(int ContactDigitalAddressId, string executor);

    Task<List<ContactDigitalAddress>> GetAllContactDigitalAddress(string userId);
    Task<ContactDigitalAddress> GetDigitalAddressById(int DigitalAddressId);
    Task<List<ContactDigitalAddress>> GetAllDeletedContactDigitalAddress(string userId);
    Task<List<ContactDigitalAddress>> FindMailAddresses(string SearchText, int MaxResults = 0);
    Task<ContactDigitalAddress> FindMailAddress(string searchName, string address);
    //Task<List<Contact>> FindContactsByDigitalAddress(string address);

    #endregion

    Task<ContactDigitalAddress> GetOrCreateAddress(string SearchName, string address, DigitalAddressType addressType);
    Task<List<User>> GetAll(bool IncludesDeleted);
    Task<List<User>> GetFilteredAndPaginatedUsers(string SearchName, int pageSize, int pageNumber, bool IncludesDeleted = false);
    Task<int> GetTotalCountOfFilteredUsers(string SearchName, bool IncludesDeleted = false);
    Task<List<User>> GetAllDeleted(string filter);
    Task<List<User>> GetAllByRole(string roleId, string filter);
    Task<List<User>> GetAllByGroup(string userGroupId, string filter);
}