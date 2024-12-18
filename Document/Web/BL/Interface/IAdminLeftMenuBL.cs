using OpenDMS.Domain.Models;
using Web.Model.Admin;

namespace Web.BL.Interface
{
    public interface IAdminLeftMenuBL
    {
        Task<List<Area>> GetAreas(UserProfile UserId);
    }
}