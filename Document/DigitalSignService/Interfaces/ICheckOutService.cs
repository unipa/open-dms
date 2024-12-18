namespace DigitalSignService.Interfaces
{
    public interface ICheckOutService
    {
        IAppSettingService AppSettingService { get; }
        IDMSWrapper Wrapper { get; }

        Task<string> AddFile(string Host, int documentId);
        Task<bool> Alive(string guid);
        Task<bool> Changed(string guid);
    }
}