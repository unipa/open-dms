using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Entities.Users;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;


namespace OpenDMS.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext DS;
    private readonly IApplicationDbContextFactory contextFactory;

    public UserRepository(IApplicationDbContextFactory contextFactory)
    {
        this.contextFactory = contextFactory;
        this.DS = (ApplicationDbContext)contextFactory.GetDbContext();
    }
    public async Task<User> GetById(string userId)
    {
        return await DS.Users.Include(u => u.Contact).AsNoTracking().FirstOrDefaultAsync(r => r.Id == userId);
    }
    public async Task<User> AddOrUpdate(string userId, string userName)
    {
        var user = await DS.Users.Include(u => u.Contact).FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            user = new User() { Id = userId, Contact = new Contact() { FullName = userName, FriendlyName = userName, SearchName = userName, CreationUser = userId, ContactType = Domain.Enumerators.ContactType.Contact } };
            DS.Users.Add(user);
        }
        else
        {
            user.Deleted = false;
            user.Contact.FriendlyName = userName;
            user.Contact.SearchName = userName;
            DS.Users.Update(user);
        }
        var r = await DS.SaveChangesAsync();
        return r > 0 ? user : null;
    }
    public async Task<User> AddOrUpdate(User U)
    {
        var user = await DS.Users.Include(u => u.Contact).AsNoTracking().FirstOrDefaultAsync(u => u.Id == U.Id);
        if (user == null)
        {
            DS.Users.Add(U);
            user = U;
        }
        else
        {
            user.Deleted = false;
            //user.Contact.FriendlyName = U.Contact.FriendlyName;
            //user.Contact.SearchName = U.Contact.SearchName;
            //user.Contact.FullName = U.Contact.FullName;
            user.Contact = U.Contact;
            user.ContactId = U.ContactId;
            DS.Users.Update(user);
        }
        var r = await DS.SaveChangesAsync();
        DS.Entry<User>(user).State = EntityState.Detached;
        DS.Entry<Contact>(user.Contact).State = EntityState.Detached;
        return r > 0 ? user : null;
    }
    public async Task<int> Delete(string userId)
    {
        var user = await DS.Users.FirstOrDefaultAsync(r => r.Id == userId);
        if (user == null) return 0;
        DS.Entry<User>(user).State = EntityState.Modified;
        user.Deleted = true;
        var r = await DS.SaveChangesAsync();
        return r;
    }
    public async Task<int> Restore(string userId)
    {
        var user = await DS.Users.FirstOrDefaultAsync(r => r.Id == userId);
        if (user == null) return 0;
        DS.Entry<User>(user).State = EntityState.Modified;
        user.Deleted = false;
        var r = await DS.SaveChangesAsync();
        return r;
    }
    public async Task<User> GetByName(string userName)
    {
        return await DS.Users.Include(u => u.Contact).AsNoTracking().FirstOrDefaultAsync(r => r.Contact.SearchName == userName);
    }
    public async Task<List<User>> GetAll(bool IncludeDeleted = false)
    {
        return await DS.Users.Include(u => u.Contact).AsNoTracking().Where(r => IncludeDeleted || !r.Deleted).OrderBy(u => u.Contact.FriendlyName).ToListAsync();
    }
    public async Task<List<User>> GetByFilter(UserFilter filter)
    {
        var now = int.Parse( DateTime.UtcNow.Date.ToString("yyyyMMdd"));
        var users = DS.Users.Include(u => u.Contact).AsQueryable();
        if (filter.IncludeDeletes)
            users = users.Where(u => u.Deleted);
        if (filter.userGroupId != "*")
            users = users.Where (u => u.UserGroups.Any(g => g.UserId == u.Id && g.StartISODate <= now && g.EndISODate >= now && g.UserGroupId == filter.userGroupId));
        //users = users.Include(u => u.UserGroups.Where(g => g.UserId == u.Id && g.StartISODate <= now && g.EndISODate >= now && g.UserGroupId == filter.userGroupId));
        if (filter.roleId != "*")
            users = users.Where (u => u.UserGroups.Any(g => g.UserId == u.Id && g.StartISODate <= now && g.EndISODate >= now && g.RoleId == filter.roleId));
        if (!String.IsNullOrEmpty(filter.filter))
            users = users.Where(u => u.Contact.SearchName.Contains(filter.filter) || u.Contact.FiscalCode.StartsWith(filter.filter) || u.Contact.LicTradNum.StartsWith(filter.filter) || u.Contact.DigitalAddresses.Any(d=>d.Name.Contains(filter.filter) || d.Address.Contains(filter.filter)));
        var list = await users.AsNoTracking().ToListAsync();
        return list;
        //return await users.OrderBy(u => u.Contact.FriendlyName).ToListAsync();
    }


    public async Task<List<User>> Find(string SearchText, int MaxResults = 0)
    {
        var items = DS.Users.Include(u => u.Contact).AsNoTracking().Where(r => r.Contact.SearchName.Contains(SearchText) && !r.Deleted);
        if (MaxResults > 0) items = items.Take(MaxResults);
        return await items.OrderBy(u => u.Contact.FriendlyName).ToListAsync();
    }





    public async Task<ContactDigitalAddress> FindMailAddress(string searchName, string address)
    {
        return await DS.ContactDigitalAddresses.AsNoTracking().FirstOrDefaultAsync(a => a.Address.Equals(address) && a.SearchName.Equals(searchName));
    }
    public async Task<List<ContactDigitalAddress>> FindMailAddresses(string SearchText, int MaxResults = 0)
    {
        return await DS.ContactDigitalAddresses.Where(c =>
                (c.DigitalAddressType == Domain.Enumerators.DigitalAddressType.Email || c.DigitalAddressType == Domain.Enumerators.DigitalAddressType.Pec) &&
                (c.Address.Contains(SearchText)) &&
                (!c.Deleted)
                ).AsNoTracking().Take(MaxResults).ToListAsync();
    }

    //public async Task<List<Contact>> FindContactsByDigitalAddress(string address)
    //{
    //    return await DS.Contacts.Where(c =>c.DigitalAddresses.Any(a=>
    //            (a.DigitalAddressType == Domain.Enumerators.DigitalAddressType.Email || a.DigitalAddressType == Domain.Enumerators.DigitalAddressType.Pec) &&
    //            (a.Address.Equals(address))
    //            )).AsNoTracking().ToListAsync();

    //}


    public async Task<int> AddOrUpdateAddress(ContactDigitalAddress bd,string executor)
    {
        var found = await DS.ContactDigitalAddresses.AsNoTracking().FirstOrDefaultAsync(a => a.Address.Equals(bd.Address) && a.SearchName.Equals(bd.SearchName));
        //      var contactDigAdd = await DS.ContactDigitalAddresses.AsNoTracking().FirstOrDefaultAsync(a => a.Address.Equals(bd.Address) && a.SearchName.Equals(bd.SearchName));
        if (bd.Id <= 0) 
        {
            // verifico che non esista un altro contatto con lo stesso indirizzo e nome
            if (found != null) return 0;
            bd.CreationUser = executor;
            bd.CreationDate = DateTime.UtcNow;
            bd.LastUpdateUser = string.Empty;
            DS.ContactDigitalAddresses.Add(bd);
        }
        else
        {
            // verifico che non esista un altro contatto (id differente) con lo stesso indirizzo e nome
            if (found != null && found.Id != bd.Id) return 0;

            bd.LastUpdateUser = executor;
            bd.LastUpdate = DateTime.UtcNow;

            DS.ContactDigitalAddresses.Update(bd);
        }
        var r = await DS.SaveChangesAsync();
        return r;
    }
    public async Task<int> DeleteAddress(int ContactDigitalAddressId, string executor)
    {
        var contactDigAdd = await DS.ContactDigitalAddresses.FirstOrDefaultAsync(u => u.Id == ContactDigitalAddressId);
        if(contactDigAdd == null) return 0;

        contactDigAdd.LastUpdateUser = executor;
        contactDigAdd.LastUpdate = DateTime.UtcNow;
        contactDigAdd.Deleted = true;

        DS.ContactDigitalAddresses.Update(contactDigAdd);
        var r = await DS.SaveChangesAsync();

        DS.Entry<ContactDigitalAddress>(contactDigAdd).State = EntityState.Detached;
        return r;
    }
    public async Task<List<ContactDigitalAddress>> GetAllAddresses(string userId)
    {
        var u = (await GetById(userId));
        if (u == null) return null; 
        var ContactId = u.ContactId;
        return await DS.ContactDigitalAddresses.AsNoTracking().Where(r => (r.ContactId == ContactId && r.Deleted==false)).OrderBy(o => o.Name).ToListAsync();
    }

    public async Task<ContactDigitalAddress> GetDigitalAddressById(int DigitalAddressId)
    {
        return await DS.ContactDigitalAddresses.AsNoTracking().FirstOrDefaultAsync(r => (r.Id == DigitalAddressId));
    }
    public async Task<List<ContactDigitalAddress>> GetAllDeletedContactDigitalAddress(string userId)
    {
        var u = (await GetById(userId));
        var ContactId = (u != null ? u.ContactId : "");
        return await DS.ContactDigitalAddresses.AsNoTracking().Where(r => (r.ContactId == ContactId && r.Deleted == true)).OrderBy(o => o.Id).ToListAsync();
    }

    public async Task<List<User>> GetFilteredAndPaginatedUsers(string searchName, int pageSize, int pageNumber, bool includesDeleted = false)
    {
        var users = DS.Users.Include(u => u.Contact).AsQueryable();
        if (includesDeleted)
            users = users.Where(u => u.Deleted);

        if (!String.IsNullOrEmpty(searchName))
            users = users.Where(u => u.Contact.SearchName.Contains(searchName) || u.Contact.FiscalCode.StartsWith(searchName) || u.Contact.LicTradNum.StartsWith(searchName) || u.Contact.DigitalAddresses.Any(d => d.Name.Contains(searchName) || d.Address.Contains(searchName)));
        
        var list = await users
            .AsNoTracking()
            .Skip((pageNumber -1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return list;
    }

    public async Task<int> GetTotalCountOfFilteredUsers(string searchName, bool includesDeleted = false)
    {
        var users = DS.Users.Include(u => u.Contact).AsQueryable();
        if (includesDeleted)
            users = users.Where(u => u.Deleted);

        if (!String.IsNullOrEmpty(searchName))
            users = users.Where(u => u.Contact.SearchName.Contains(searchName) || u.Contact.FiscalCode.StartsWith(searchName) || u.Contact.LicTradNum.StartsWith(searchName) || u.Contact.DigitalAddresses.Any(d => d.Name.Contains(searchName) || d.Address.Contains(searchName)));

        return await users.CountAsync();
    }
}