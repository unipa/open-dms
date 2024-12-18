using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.Domain.Repositories;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.Infrastructure.Repositories
{
    public class MailServerRepository : IMailServerRepository
    {
        private readonly ApplicationDbContext ds;
        private readonly IApplicationDbContextFactory contextFactory;

        public MailServerRepository(IApplicationDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
            this.ds = (ApplicationDbContext)contextFactory.GetDbContext();
        }

        public async Task<IList<MailServer>> GetAll()
        {

            return ds.MailServers.AsNoTracking().ToList();
        }

        public async Task<MailServer> GetById(int MailServerId)
        {
            return await ds.MailServers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == MailServerId);
        }

        public async Task<MailServer> GetByDomain(string MailDomain)
        {
            return await ds.MailServers.AsNoTracking().FirstOrDefaultAsync(c => c.Domain == MailDomain);
        }


        public async Task<int> Insert(MailServer mailServer)
        {
            if (String.IsNullOrEmpty(mailServer.Domain)) throw new ArgumentNullException(nameof(mailServer.Domain));
            ds.MailServers.Add(mailServer);
            var r = await ds.SaveChangesAsync();
            ds.Entry<MailServer>(mailServer).State = EntityState.Detached;
            return r;
        }



        public async Task<int> Update(MailServer mailServer)
        {
            ds.MailServers.Update(mailServer);
            var r = await ds.SaveChangesAsync();
            ds.Entry<MailServer>(mailServer).State = EntityState.Detached;
            return r;
        }

        public async Task<int> Delete(int mailServerId)
        {
            var c = await ds.MailServers.FirstOrDefaultAsync(c => c.Id == mailServerId);
            ds.MailServers.Remove(c);
            var r = await ds.SaveChangesAsync();
            ds.Entry<MailServer>(c).State = EntityState.Detached;
            return r;
        }
    }


}
