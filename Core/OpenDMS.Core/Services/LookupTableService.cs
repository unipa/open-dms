using Microsoft.Extensions.Logging;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Repositories;

/// <summary>
/// Summary description for TabellaLookup
/// </summary>

namespace OpenDMS.Core.BusinessLogic
{

    public class LookupTableService : ILookupTableService
    {
        private readonly ILookupTableRepository _repository;
        private readonly ILogger<ILookupTableService> logger;

        public LookupTableService(ILookupTableRepository lookupRepo, ILogger<ILookupTableService> logger)
        {
            this._repository = lookupRepo;
            this.logger = logger;
        }



        public async Task<LookupTable> GetById(string tabella, string codice, bool ReturnDefault = true)
        {
            return await _repository.GetById(tabella, codice,ReturnDefault);
        }

        public async Task<List<LookupTable>> GetAll(string id)
        {
            return await _repository.GetAll(id);
        }
        public async Task<List<LookupTable>> GetAll()
        {
            return await _repository.GetAll("$TABLES$");
        }

        public async Task<int> Insert(LookupTable bd)
        {
            return await _repository.Insert(bd);
        }
        public async Task<int> Update(LookupTable bd)
        {
            return await _repository.Update(bd);
        }
        public async Task<int> Delete(LookupTable bd)
        {
            return await _repository.Delete(bd);
        }

    }
}