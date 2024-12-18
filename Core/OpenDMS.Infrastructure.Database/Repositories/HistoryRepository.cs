using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Events.Types;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.Infrastructure.Database.Repositories
{
    public class HistoryRepository : IHistoryRepository
    {
        private readonly ApplicationDbContext DS;
        private readonly IApplicationDbContextFactory contextFactory;

        public HistoryRepository(IApplicationDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
            this.DS = (ApplicationDbContext)contextFactory.GetDbContext();
        }

        private IQueryable<HistoryEntry> ApplyFilters(HistoryFilters filters) 
        {
            var H = DS.Histories.OrderByDescending(o=>o.CreationDate).AsQueryable();
            if (filters != null)
            {
                if (filters.Events.Count > 0)
                    H = H.Where(f => filters.Events.Contains(f.EventType));
                if (!String.IsNullOrEmpty(filters.Search))
                    H = H.Where(f => f.Description.Contains(filters.Search));
                if (!String.IsNullOrEmpty(filters.UserId))
                    H = H.Where(f => f.UserId == filters.UserId);
                if (!String.IsNullOrEmpty(filters.RecipientId))
                    H = H.Where(f => f.Recipients.Any(r => r.ProfileId == filters.RecipientId && r.ProfileType == filters.RecipientType));
                if (filters.DocumentId > 0)
                    H = H.Where(f => f.Documents.Any(d => d.DocumentId == filters.DocumentId));
                if (filters.PageSize > 0)
                {
                    if (filters.PageIndex > 0)
                        H = H.Skip(filters.PageIndex * filters.PageSize);
                    if (filters.PageIndex >= 0)
                        H = H.Take(filters.PageSize);
                }
            }
            return H;        
        }


        public async Task<int> Count(HistoryFilters filters = null)
        {

            return await ApplyFilters(filters).CountAsync();
        }

        public async Task<IList<HistoryEntry>> GetByFilters(HistoryFilters filters = null)
        {
            return await ApplyFilters(filters).AsNoTracking().ToListAsync(); 
        }

        public async Task<HistoryEntry> GetById(int entryId)
        {
            return await DS.Histories.AsNoTracking().FirstOrDefaultAsync(d=>d.Id==entryId);
        }

        public async Task<int> Insert(HistoryEntry entry)
        {
            HistoryEntry h = null;
            // Solo per la visualizzazione, mantengo la prima che viene registrata nell'arco di 5 min.
            if (entry.EventType == EventType.View)
            {
                var dt = DateTime.UtcNow.AddMinutes(-5);
                var d = entry.Documents[0].DocumentId;
                h = await DS.Histories.AsNoTracking().FirstOrDefaultAsync(h =>
                    h.EventType == entry.EventType &&
                    h.UserId == entry.UserId &&
                    h.CreationDate > dt &&
                    h.Documents.Any(a => a.DocumentId == d)
                );
            }
            if (h == null)
            {
                try { 
                DS.Histories.Add(entry);
                return await DS.SaveChangesAsync();
                }
                finally
                {
                    DS.Entry<HistoryEntry>(entry).State = EntityState.Detached;
                }

            }
            return 0;
        }


        public bool Any(int documentId, string EventId)
        {
            return DS.Histories.AsNoTracking().Any(h => (h.Documents.Any(d=>d.DocumentId == documentId)) && (h.EventType == EventId));
        }

    }
}
