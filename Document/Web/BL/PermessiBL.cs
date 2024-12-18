using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.Schemas;
using Web.BL.Interface;

namespace Web.BL
{
    public class PermessiBL : IPermessiBL
    {
        private readonly IConfiguration _config;
        private readonly string Host;
        private IHttpContextAccessor _accessor;
        private readonly ILoggedUserProfile _userContext;
        private readonly IACLService _aclService;
        private readonly IDocumentTypeService _doctypeService;

        public PermessiBL(IConfiguration config, IHttpContextAccessor accessor, IDocumentTypeService doctypeService, IACLService aclService, ILoggedUserProfile userContext)
        {
            _config = config;
            Host = (string)_config.GetValue(typeof(string), "Endpoint:AdminService");
            _accessor = accessor;
            _doctypeService = doctypeService;
            _aclService = aclService;
            _userContext = userContext;
        }

        public async Task<IEnumerable<ACL>> GetAllACL()
        {
            return await _aclService.GetAll();
        }

        //public async Task<IEnumerable<ACL>> GetAllACL()
        //{
        //    String URL = Host + "/ACL";
        //    try
        //    {
        //        return await _httpHandler.GetAsyncCall<List<ACL>>(URL, null, null);
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

        public async Task<ACL> GetACL(string Id)
        {
            var result = new ACL();
            return await _aclService.GetById(Id);
        }

        //public async Task<ACL> GetACL(string Id)
        //{
        //    String URL = Host + "/ACL/" + Id;
        //    try
        //    {
        //        return await _httpHandler.GetAsyncCall<ACL>(URL, null, null);
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

        public async Task<string> Insert(ACL acl)
        {
            if (await _aclService.GetById(acl.Id) == null)
            {
                ACL A = await _aclService.Insert(new OpenDMS.Core.DTOs.CreateOrUpdateACL() { Id = acl.Id, Name = acl.Name });
                return A != null ? A.Id : throw new Exception("L'inserimento non è andato a buon fine.");
            }
            else throw new Exception("Non può essere inserita una ACL con un id già presente.");
        }

        //public async Task<string> Insert(ACL bd)
        //{
        //    String URL = Host + "/ACL";
        //    String body = System.Text.Json.JsonSerializer.Serialize(bd);
        //    StringContent content = new StringContent(body, Encoding.UTF8, "application/json");
        //    try
        //    {
        //        var inserted = await _httpHandler.PostAsyncCall<ACL>(URL, content, null, null);
        //        return inserted.Id;
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

        public async Task Update(ACL acl)
        {
            var exist = await _aclService.GetById(acl.Id);
            if (exist != null)
            {
                await _aclService.Update(new OpenDMS.Core.DTOs.CreateOrUpdateACL() { Id = acl.Id, Name = acl.Name });
            }
            else throw new Exception("L'ACL selezionata non è stata trovata");
        }

        //public async Task Update(ACL bd)
        //{
        //    String URL = Host + "/ACL";
        //    String body = System.Text.Json.JsonSerializer.Serialize(bd);
        //    StringContent content = new StringContent(body, Encoding.UTF8, "application/json");
        //    try
        //    {
        //        await _httpHandler.PutAsyncCall<ACL>(URL, content, null, null);
        //    }
        //    catch (HttpCallException ex)
        //    {
        //        if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        //            throw new Exception("Accesso non autorizzato.");
        //        else
        //            throw ex;
        //    }
        //}

        public async Task Delete(string Id)
        {
            var exist = await _aclService.GetById(Id);
            if (exist != null)
            {
                await _aclService.Delete(Id);
            }
            else throw new Exception("la ACL selezionata non è stata trovata");
        }
        //public async Task Delete(string Id)
        //{
        //    String URL = Host + "/ACL/" + Id;
        //    try
        //    {
        //        await _httpHandler.DeleteAsyncCall<ACL>(URL, null, null);
        //    }
        //    catch (HttpCallException ex)
        //    {
        //        if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        //            throw new Exception("Accesso non autorizzato.");
        //        else
        //            throw ex;
        //    }
        //}

        public async Task<IEnumerable<ACLPermission>> GetAuthorizations(string Id)
        {
            return await _aclService.GetAllPermissions(Id);
        }

        //public async Task<IEnumerable<ACLPermission>> GetAuthorizations(string Id)
        //{
        //    String URL = Host + "/ACL/GetAllPermissions?aclId=" + Id;
        //    try
        //    {
        //        var res = await _httpHandler.GetAsyncCall<List<ACLPermission>>(URL, null, null);
        //        return res;
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

        public async Task AddAuthorization(ACLPermission aclauth)
        {
            if (!await _aclService.HasPermission(aclauth.ACLId, aclauth.ProfileId, aclauth.ProfileType, aclauth.PermissionId))
            {
                int res = await _aclService.AddPermission(aclauth);
                if (res == 0) throw new Exception("L'inserimento non è andato a buon fine.");
            }
            else throw new Exception("Non può essere inserito un permesso con un id già presente");
        }

        //public async Task<ACLPermission> AddAuthorization(ACLPermission permission)
        //{
        //    String URL = Host + "/ACL/AddPermission";
        //    String body = System.Text.Json.JsonSerializer.Serialize(permission);
        //    StringContent content = new StringContent(body, Encoding.UTF8, "application/json");
        //    try
        //    {
        //        var inserted = await _httpHandler.PostAsyncCall<ACLPermission>(URL, content, null, null);
        //        return inserted;
        //    }
        //    catch (HttpCallException ex)
        //    {
        //        if (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
        //            throw new Exception("Conflitto. Non può essere inserita una entità dati identificativi già esistenti.");
        //        if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        //            throw new Exception("Accesso non autorizzato.");
        //        else
        //            throw ex;
        //    }
        //}

        public async Task<int> UpdateAuthorization(ACLPermission permission)
        {
            return await _aclService.ChangePermission(permission);
        }

        //public async Task<int> UpdateAuthorization(ACLPermission permission)
        //{
        //    String URL = Host + "/ACL/changePermission";
        //    String body = System.Text.Json.JsonSerializer.Serialize(permission);
        //    StringContent content = new StringContent(body, Encoding.UTF8, "application/json");
        //    try
        //    {
        //        return await _httpHandler.PutAsyncCall<int>(URL, content, null, null);
        //    }
        //    catch (HttpCallException ex)
        //    {
        //        if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        //            throw new Exception("Accesso non autorizzato.");
        //        else
        //            throw ex;
        //    }
        //}

        public async Task<int> DeleteAuthorization(ACLPermission aclauth/*string ACLId, string ProfileId, ProfileType profileType, string PermissionId*/)
        {
            var exist = await _aclService.GetPermission(aclauth.ACLId, aclauth.ProfileId, aclauth.ProfileType, aclauth.PermissionId);
            if (exist != null)
            {
                var result = await _aclService.RemovePermission(exist);
                return result > 0 ? result : throw new Exception("Eliminazione non riuscita");
            }
            else throw new Exception("L'ACL selezionato non è stato trovato");
        }

        //public async Task<int> DeleteAuthorization(ACLPermission permission/*string ACLId, string ProfileId, ProfileType profileType, string PermissionId*/)
        //{
        //    //String URL = Host + "/ACL/RemovePermission?ACLId="+ACLId+"&ProfileId="+ProfileId+"&profileType="+profileType+"&PermissionId="+PermissionId;
        //    String URL = Host + "/ACL/RemovePermission";
        //    String body = System.Text.Json.JsonSerializer.Serialize(permission);
        //    StringContent content = new StringContent(body, Encoding.UTF8, "application/json");
        //    try
        //    {
        //        return await _httpHandler.DeleteAsyncCall<int>(URL, content, null);
        //    }
        //    catch (HttpCallException ex)
        //    {
        //        if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        //            throw new Exception("Accesso non autorizzato.");
        //        else
        //            throw ex;
        //    }
        //}

        public async Task<bool> ACLHasBind(string ACLId)
        {
            var userInfo = _userContext.Get();
            var result = await _doctypeService.GetByPermission(userInfo, PermissionType.CanView);
            return result.Any(dt => dt.ACLId == ACLId);
        }

        //public async Task<bool> ACLHasBind(string ACLId)
        //{
        //    String URL = Host + "/DocumentType";
        //    try
        //    {
        //        var res = await _httpHandler.GetAsyncCall<List<DocumentType>>(URL, null, null);
        //        return res.Any(dt => dt.ACLId == ACLId);
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
