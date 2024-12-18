
using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Repositories;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;


namespace OpenDMS.Infrastructure.Repositories
{
    public class ACLRepository : IACLRepository
    {
        private readonly ApplicationDbContext DS;
        private readonly IApplicationDbContextFactory contextFactory;

        public ACLRepository(IApplicationDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
            this.DS = (ApplicationDbContext)contextFactory.GetDbContext();
        }



        public async Task<int> Insert(ACL bd)
        {
            if (String.IsNullOrEmpty(bd.Name)) throw new ArgumentNullException(nameof(bd.Name));

            DS.ACLs.Add(bd);
            var r = await DS.SaveChangesAsync();
            DS.Entry<ACL>(bd).State = EntityState.Detached;
            return r;
        }

        public async Task<int> Update(ACL bd)
        {
            DS.Entry<ACL>(bd).State = EntityState.Modified;
            var r = await DS.SaveChangesAsync();
            DS.Entry<ACL>(bd).State = EntityState.Detached;
            return r;
        }
        public async Task<int> Delete(ACL bd)
        {
            DS.Entry<ACL>(bd).State = EntityState.Deleted;
            var r =  await DS.SaveChangesAsync();
            DS.Entry<ACL>(bd).State = EntityState.Detached;
            return r;
        }

        public async Task<ACL> GetById(string acl)
        {
            return await DS.ACLs.AsNoTracking().FirstOrDefaultAsync(a => a.Id == acl);
        }

        public async Task<List<ACL>> GetAll()
        {
            return await DS.ACLs.AsNoTracking().ToListAsync();
        }

        public async Task<List<ACLPermission>> GetAllPermissions(string aclId)
        {
            return await DS.ACLPermissions.AsNoTracking().Where(a=>a.ACLId== aclId).ToListAsync();
        }




        public async Task<int> AddPermission(ACLPermission bd)
        {
            if (String.IsNullOrEmpty(bd.ProfileId)) throw new ArgumentNullException(nameof(bd.ProfileId));
            DS.ACLPermissions.Add(bd);
            return await DS.SaveChangesAsync();
        }

        public async Task<int> ChangePermission(ACLPermission bd)
        {
            DS.Entry<ACLPermission>(bd).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return await DS.SaveChangesAsync();
        }
        public async Task<int> RemovePermission(ACLPermission bd)
        {
            DS.Entry<ACLPermission>(bd).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            return await DS.SaveChangesAsync();
        }
        public async Task<AuthorizationType> GetAuthorization(string acl, string userId, ProfileType userType, string permissionId)
        {
            return (await DS.ACLPermissions.AsNoTracking().FirstOrDefaultAsync(a => a.ACLId == acl && a.ProfileId == userId && a.ProfileType == userType && a.PermissionId == permissionId))?.Authorization??AuthorizationType.None;
        }

        public async Task<ACLPermission> GetPermission(string acl, string userId, ProfileType userType, string permissionId)
        {
            return await DS.ACLPermissions.AsNoTracking().FirstOrDefaultAsync(a => a.ACLId == acl && a.ProfileId == userId && a.ProfileType == userType && a.PermissionId == permissionId);
        }

        public async Task<List<ACL>> GetByProfile(ProfileType profileType, string profileId)
        {
            return await DS.ACLs.AsNoTracking().Where(a => a.Permissions.Any(p=>p.ProfileType==profileType && p.ProfileId==profileId)).ToListAsync();
        }

        public async Task<List<ACLPermission>> GetByProfile(string acl, ProfileType profileType, string profileId)
        {
            return await DS.ACLPermissions.AsNoTracking().Where(a => a.ACLId == acl && a.ProfileType==profileType && a.ProfileId==profileId).ToListAsync();
        }
    }

}
