namespace OpenDMS.Infrastructure.Services.BusinessLogic
{
    public interface IPreviewGenerator
    {
        Task Generate(int imageId);
        Task<bool> HasPreview(int imageId);
    }
}