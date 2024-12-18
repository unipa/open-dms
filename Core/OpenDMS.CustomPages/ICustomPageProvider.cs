using OpenDMS.CustomPages.Models;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Models;

namespace OpenDMS.CustomPages
{
    public interface ICustomPageProvider
    {
        Task Register (CustomPage page);
        Task<List<CustomPageDTO>> GetPages(UserProfile userInfo, string parentId = "");
        Task<CustomPageDTO> GetPage(UserProfile userInfo, string parentId = "");
        Task Remove(string pageId);
    }
}