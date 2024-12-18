using OpenDMS.Core.DTOs;
using OpenDMS.Domain.Entities.Settings;

namespace DigitalSignService.Interfaces
{
    public interface IDMSWrapper
    {
        IAppSettingService AppSetting { get; }

        Task<string> SignFile(string Host, int documentId, int imageId, string fileName, byte[] file);
        Task<int> AddFile(string Host, int documentId, string fileName, byte[] file);
        Task<DocumentInfo> GetDocument(string Host, int documentId);
        Task<byte[]> GetFile(string Host, int documentId, int imageId);
        Task<List<FileProperty>> GetFiles(string Host, string Documents, bool postback);
        Task<List<LookupTable>> GetFileSignature(string Host, FileProperty document);
        Task<string> GetSettings(string Host, string Setting);
        Task SetSettings(string Host, string Setting, string Value);
        Task<byte[]> CheckOut(string Host, int imageId);
    }
}