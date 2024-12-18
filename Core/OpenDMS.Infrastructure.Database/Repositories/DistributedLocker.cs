using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Services;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;


namespace OpenDMS.Infrastructure.Database.Repositories
{
    public class DistributedLocker : IDistributedLocker
    {
        private readonly IApplicationDbContextFactory contextFactory;
        private readonly ApplicationDbContext DS;

        public DistributedLocker(IApplicationDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
            this.DS = (ApplicationDbContext)contextFactory.GetDbContext();

        }

        public bool Acquire(string objectId, string recordId, string serviceId, DateTime expirationTime)
        {
            var l = DS.DistributedLocks.AsNoTracking().FirstOrDefault(l => l.ObjectId == objectId && l.RecordId == recordId);
            if (l != null && l.ExpirationDate < DateTime.UtcNow)
            {
                DS.DistributedLocks.Remove(l);
                l = null;
            }
            if (l == null)
            {
                try
                {
                    l = new DistributedLock() { ServiceId = serviceId, ObjectId = objectId, RecordId = recordId, ExpirationDate = expirationTime };
                    DS.DistributedLocks.Add(l);
                    DS.SaveChanges();
                    DS.Entry<DistributedLock>(l).State = EntityState.Unchanged;
                    return true;
                }
                catch (Exception ex)
                {
                }
            }
            return false;
        }

        public bool Release(string objectId, string recordId, string serviceId)
        {
            var l = DS.DistributedLocks.AsNoTracking().FirstOrDefault(l => l.ServiceId == serviceId && l.ObjectId == objectId && l.RecordId == recordId);
            if (l != null)
            {
                try
                {
                    DS.Entry<DistributedLock>(l).State = EntityState.Deleted;
                    DS.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                }
            }
            return false;
        }

 
        public bool Update(string objectId, string recordId, string serviceId, DateTime expirationTime)
        {
            var l = DS.DistributedLocks.AsNoTracking().FirstOrDefault(l => l.ServiceId == serviceId && l.ObjectId == objectId && l.RecordId == recordId);
            if (l != null)
            {
                try
                {
                    l.ExpirationDate = expirationTime;
                    DS.DistributedLocks.Update(l);
                    DS.SaveChanges();
                    DS.Entry<DistributedLock>(l).State = EntityState.Unchanged;
                    return true;
                }
                catch (Exception ex)
                {
                }
            }
            return false;
        }
    }
}
