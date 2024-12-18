using OpenDMS.Domain.Entities.Users;
using OpenDMS.Domain.Models;

namespace OpenDMS.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> AddOrUpdate(string userId, string userName);
        Task<User> AddOrUpdate(User U);
        Task<int> Delete(string userId);
        Task<int> Restore(string userId);
        Task<List<User>> GetAll(bool IncludeDeleted = false);
        Task<User> GetById(string useriId);
        Task<User> GetByName(string useriId);
        Task<List<User>> Find(string SearchText, int MaxResults = 0);



        Task<ContactDigitalAddress> FindMailAddress(string searchName, string address);

        Task<List<ContactDigitalAddress>> FindMailAddresses(string SearchText, int MaxResults = 0);
        //Task<List<Contact>> FindContactsByDigitalAddress(string address);

        Task<List<ContactDigitalAddress>> GetAllAddresses(string userId);

        Task<int> AddOrUpdateAddress(ContactDigitalAddress bd, string executor);
        Task<int> DeleteAddress(int ContactDigitalAddressId, string executor);
        Task<List<ContactDigitalAddress>> GetAllDeletedContactDigitalAddress(string userId);
        Task<ContactDigitalAddress> GetDigitalAddressById(int DigitalAddressId);
        Task<List<User>> GetByFilter(UserFilter filter);
        Task<List<User>> GetFilteredAndPaginatedUsers(string searchName, int pageSize, int pageNumber, bool includesDeleted = false);
        Task<int> GetTotalCountOfFilteredUsers(string searchName, bool includesDeleted = false);
    }

}
