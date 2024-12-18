using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Settings;
using Web.BL.Interface;

namespace Web.BL
{
    public class BancheDatiBL : IBancheDatiBL
    {
        private readonly IConfiguration _config;
        private readonly string Host;
        private readonly ICompanyService companyRepo;


        public BancheDatiBL(IConfiguration config, ICompanyService companyRepo = null)
        {
            _config = config;
            Host = (string)_config.GetValue(typeof(string), "Endpoint:AdminService");
            this.companyRepo = companyRepo;
        }

        public async Task<Company> GetById(int Id)
        {
            var result = await companyRepo.GetById(Id);
            return result == null ? throw new Exception("La Company indicata non è stata trovata.") : result;
        }

        //public async Task<Company> GetById(int cdb)
        //{
        //    String URL = Host + "/Company/" + cdb;
        //    try
        //    {
        //        return await _httpHandler.GetAsyncCall<Company>(URL, null, null);
        //    }
        //    catch (HttpCallException ex)
        //    {
        //        if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        //            return null;
        //        if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        //            throw new Exception("Accesso non autorizzato.");
        //        else
        //            throw ex;
        //    }
        //}

        public async Task<IEnumerable<Company>> GetAll()
        {
            return await companyRepo.GetAll();
        }
        //public async Task<IEnumerable<Company>> GetAll()
        //{
        //    String URL = Host + "/Company";
        //    try
        //    {
        //        return await _httpHandler.GetAsyncCall<List<Company>>(URL, null, null);
        //    }
        //    catch (HttpCallException ex)
        //    {
        //        if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        //            return null;
        //        if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        //            throw new Exception("Accesso non autorizzato.");
        //        else
        //            throw ex;
        //    }
        //}

        public async Task Insert(Company bd)
        {
            //if (await companyRepo.GetById(bd.Id) == null)
            //{
            //    bd.Id = 0;
            //    await companyRepo.Create(bd);
            //}
            //else throw new Exception("Non può essere inserita una Company con un Id già esistente, usa un altro Id oppure se vuoi modificarla usa il metodo PUT.");
            throw new NotImplementedException();
        }

        //public async Task Insert(Company bd)
        //{
        //    String URL = Host + "/Company";
        //    String body = System.Text.Json.JsonSerializer.Serialize(bd);
        //    StringContent content = new StringContent(body, Encoding.UTF8, "application/json");
        //    try
        //    {
        //        await _httpHandler.PostAsyncCall<Company>(URL, content, null, null);
        //    }
        //    catch (HttpCallException ex)
        //    {
        //        if (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
        //            throw new Exception("Conflitto. Non può essere inserita una entità con un Id già esistente.");
        //        if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        //            throw new Exception("Accesso non autorizzato.");
        //        else
        //            throw ex;
        //    }
        //}

        public async Task Update(Company bd)
        {
            var exist = await companyRepo.GetById(bd.Id);
            if (exist != null)
            {
                exist.Description = bd.Description;
                exist.Theme = bd.Theme;
                exist.Logo = bd.Logo;
                exist.ERP = bd.ERP;
                exist.AOO = bd.AOO;
                exist.OffLine = bd.OffLine;
                await companyRepo.Update(exist);
            }
            else throw new Exception("La Company indicata non è stata trovata");
        }

        //public async Task Update(Company bd)
        //{
        //    String URL = Host + "/Company";
        //    String body = System.Text.Json.JsonSerializer.Serialize(bd);
        //    StringContent content = new StringContent(body, Encoding.UTF8, "application/json");
        //    try
        //    {
        //        await _httpHandler.PutAsyncCall<Company>(URL, content, null, null);
        //    }
        //    catch (HttpCallException ex)
        //    {
        //        if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        //            throw new Exception("Entità non trovata.");
        //        if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        //            throw new Exception("Accesso non autorizzato.");
        //        else
        //            throw ex;
        //    }
        //}

        public async Task Delete(Company bd)
        {
            throw new NotImplementedException();
        }

        //public async Task Delete(Company bd)
        //{
        //    String URL = Host + "/Company/" + bd.Id;
        //    try
        //    {
        //        await _httpHandler.DeleteAsyncCall<int?>(URL, null, null);
        //    }
        //    catch (HttpCallException ex)
        //    {
        //        if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        //            throw new Exception("Accesso non autorizzato.");
        //        else
        //            throw ex;
        //    }
        //}
    }
}
