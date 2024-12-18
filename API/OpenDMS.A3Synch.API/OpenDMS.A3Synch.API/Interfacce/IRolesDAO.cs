using A3Synch.Models;

namespace A3Synch.Interfacce
{
    public interface IRolesDAO
    {
        Task<int> UpdateRoles(List<Roles> roles);
        Task<List<Roles>> GetAllRoles();
    }
}
