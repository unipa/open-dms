using DigitalSignService.Interfaces;
using DigitalSignService.Model;
using System.Text.Json;

namespace DigitalSignService.Services
{
    public class AppSettingService : IAppSettingService
    {
        private const string configFileName = "dms.config";
        private const string configFileDirectory = "elmi";

        private Dictionary<string, DMSConfig> _configs = new Dictionary<string, DMSConfig>();

        public AppSettingService()
        {
        }

        public  string normalizeHost(string host)
        {
            return host.ToLower().Replace("https://", "").Replace("http://", "").Replace(":", "").Replace("/", "-").Replace("\\", "-");
        }

        public async Task<DMSConfig> GetConfig(string host)
        {
            var clientPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), configFileDirectory, normalizeHost(host));
            if (!Directory.Exists(clientPath)) Directory.CreateDirectory(clientPath);
            var clientFile = Path.Combine(clientPath, configFileName);
            if (System.IO.File.Exists(clientFile))
            {
                DMSConfig c = JsonSerializer.Deserialize<DMSConfig>(File.ReadAllText( clientFile));
                _configs[host] = c;
                return c;
            }
            return new DMSConfig();
        }
        public async Task SaveConfig(string host, DMSConfig config)
        {
            var clientPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), configFileDirectory, normalizeHost(host));
            if (!Directory.Exists(clientPath)) Directory.CreateDirectory(clientPath);
            var clientFile = Path.Combine(clientPath, configFileName);
            var json = JsonSerializer.Serialize(config);
            File.WriteAllText(clientFile, json);
            _configs[host] = config;
        }
        public async Task<string> GetSecret(string host, bool Reload = false)
        {
            if (_configs.ContainsKey(host) && !Reload)
            {
                return _configs[host].ClientSecret;
            }
            var config = await GetConfig(host);
            if (config != null) {
                return config.ClientSecret;
            }
            return "";
        }

        public async Task<string> SetSecret(string host, string clientSecret)
        {
            DMSConfig c = null;
            if (_configs.ContainsKey(host))
                c = _configs[host];
            else
                c = await GetConfig(host);

            c.ClientSecret = clientSecret;
            await SaveConfig(host, c);
            return clientSecret;
        }

    }


}
