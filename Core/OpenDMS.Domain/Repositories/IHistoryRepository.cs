using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Events.Types;
using OpenDMS.Domain.Models;

namespace OpenDMS.Domain.Repositories;


/// <summary>
/// Descrizione di riepilogo per BancheDatiDAO
/// </summary>
public interface IHistoryRepository
{
    Task<HistoryEntry> GetById(int entryId);
    Task<int> Insert(HistoryEntry entry);
    Task<IList<HistoryEntry>> GetByFilters(HistoryFilters filters = null);
    Task<int> Count(HistoryFilters filters = null);
    bool Any(int documentId, string EventId);
}


