using OpenDMS.Domain.Entities.Settings;

namespace OpenDMS.Domain.Repositories
{
    public interface ICustomPagesRepository
    {
        List<CustomPage> GetPages(string parentPageId);
        CustomPage GetPage(string pageId);
        void Register(CustomPage page);
        void Remove(CustomPage page);
    }
}