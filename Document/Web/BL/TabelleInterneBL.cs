using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Settings;
using Web.BL.Interface;

namespace Web.BL
{
    public class TabelleInterneBL : ITabelleInterneBL
    {
        private readonly IConfiguration _config;
        private readonly string Host;
        private ILookupTableService _lookupTableRepo;

        public TabelleInterneBL(IConfiguration config, ILookupTableService ltService = null)
        {
            _config = config;
            Host = (string)_config.GetValue(typeof(string), "Endpoint:AdminService");
            _lookupTableRepo = ltService;
        }

        public async Task<List<LookupTable>> GetTables()
        {
            return await _lookupTableRepo.GetAll();
        }

        public async Task<List<LookupTable>> GetLookupTables(string TableId)
        {
            if (string.IsNullOrEmpty(TableId)) throw new Exception(nameof(TableId) + " non può essere vuoto");

            TableId = TableId.ToUpper();

            var result = await _lookupTableRepo.GetAll(TableId);
            return result.Count == 0 ? new List<LookupTable>() : result;
        }

        public async Task<LookupTable> GetLookupTable(string TableId, string Id)
        {
            if (string.IsNullOrEmpty(Id)) throw new Exception(nameof(Id) + " non può essere vuoto");
            if (string.IsNullOrEmpty(TableId)) throw new Exception(nameof(TableId) + " non può essere vuoto");

            TableId = TableId.ToUpper();
            Id = Id.ToUpper();

            var result = await _lookupTableRepo.GetById(TableId, Id);
            return result == null ? throw new Exception("La LookupTable indicata non è stata trovata.") : result;
        }

        public async Task Insert(LookupTable bd)
        {
            bd.Id = bd.Id.ToUpper();
            bd.TableId = bd.TableId.ToUpper();

            var exist = await _lookupTableRepo.GetById(bd.TableId, bd.Id);
            if (exist.Description.Equals("#" + bd.Id))
            {

                int res = await _lookupTableRepo.Insert(bd);
                if (res == 0) throw new Exception("L'inserimento non è andato a buon fine.");
            }
            else throw new Exception("Non può essere inserita una LookupTable con un TableId già esistente, usa un altro TableId oppure se vuoi modificarla usa il metodo PUT.");
        }

        public async Task Update(LookupTable bd)
        {
            bd.Id = bd.Id.ToUpper();
            bd.TableId = bd.TableId.ToUpper();

            var exist = await _lookupTableRepo.GetById(bd.TableId, bd.Id);
            if (exist.Description == null || !exist.Description.Equals("#" + bd.Id))
            {
                exist.Annotation = bd.Annotation;
                exist.Description = bd.Description;
                int res = await _lookupTableRepo.Update(exist);
                if (res == 0) throw new Exception("L'aggiornamento non è andato a buon fine.");
            }
            else throw new Exception("La LookupTable indicata non è stata trovata");
        }

        public async Task Delete(LookupTable bd)
        {
            if (string.IsNullOrEmpty(bd.Id)) throw new Exception(nameof(bd.Id) + " non può essere vuoto");
            if (string.IsNullOrEmpty(bd.TableId)) throw new Exception(nameof(bd.TableId) + " non può essere vuoto");

            bd.TableId = bd.TableId.ToUpper();
            bd.Id = bd.Id.ToUpper();

            var exist = await _lookupTableRepo.GetById(bd.TableId, bd.Id);
            if (exist.Description == null || !exist.Description.Equals("#" + bd.Id))
            {
                var result = await _lookupTableRepo.Delete(exist);
                if (result == 0) throw new Exception("Eliminazione non è andata a buon fine.");
            }
            else throw new Exception("La LookupTable indicata non è stata trovata");
        }

    }
}
