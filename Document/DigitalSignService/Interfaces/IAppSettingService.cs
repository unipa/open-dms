using DigitalSignService.Model;

namespace DigitalSignService.Interfaces
{
    public interface IAppSettingService
    {
        Task<DMSConfig> GetConfig(string host);
        Task SaveConfig(string host, DMSConfig config);
        Task<string> GetSecret(string host, bool Reload = false);
        string normalizeHost(string host);
        Task<string> SetSecret(string host, string ClientSecret);
    }
}