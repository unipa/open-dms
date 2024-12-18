using OpenDMS.Domain.Entities.Schemas;

namespace Web.BL.Interface
{
    public interface IPermessiBL
    {
        Task<bool> ACLHasBind(string ACLId);
        Task AddAuthorization(ACLPermission aclauth);
        Task Delete(string Id);
        Task<int> DeleteAuthorization(ACLPermission aclauth);
        Task<ACL> GetACL(string Id);
        Task<IEnumerable<ACL>> GetAllACL();
        Task<IEnumerable<ACLPermission>> GetAuthorizations(string Id);
        Task<string> Insert(ACL acl);
        Task Update(ACL acl);
        Task<int> UpdateAuthorization(ACLPermission permission);
    }
}