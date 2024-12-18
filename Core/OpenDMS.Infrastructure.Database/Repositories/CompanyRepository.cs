using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Repositories;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.Infrastructure.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext ds;
        private readonly IApplicationDbContextFactory contextFactory;

        public CompanyRepository(IApplicationDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
            this.ds = (ApplicationDbContext)contextFactory.GetDbContext();
        }

        public async Task<IList<Company>> GetAll()
        {

            return await ds.Companies.AsNoTracking().ToListAsync();
        }

        public async Task<Company> GetById(int CompanyId)
        {
            return await ds.Companies.AsNoTracking().FirstOrDefaultAsync(c=>c.Id == CompanyId);
        }


        public async Task<int> Insert(Company Company)
        {
            if (String.IsNullOrEmpty(Company.Description)) throw new ArgumentNullException(nameof(Company.Description));
            ds.Companies.Add(Company);
            var r = await ds.SaveChangesAsync();
            ds.Entry<Company>(Company).State = EntityState.Detached;
            return r;
        }



        public async Task<int> Update(Company Company)
        {
            ds.Companies.Update(Company);
            var r = await ds.SaveChangesAsync();
            ds.Entry<Company>(Company).State = EntityState.Detached;
            return r;
        }

        public async Task<int> Delete(int CompanyId)
        {
            var c = await ds.Companies.FirstOrDefaultAsync(c => c.Id == CompanyId);
            ds.Companies.Remove(c);
            var r = await ds.SaveChangesAsync();
            ds.Entry<Company>(c).State = EntityState.Detached;
            return r;
        }
    }


}
