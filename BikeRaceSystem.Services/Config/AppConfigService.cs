using BikeRaceSystem.Core.Interfaces.Config;
using BikeRaceSystem.Core.Models;
using System.Text.Json;

namespace BikeRaceSystem.Services.Config
{
    public class AppConfigService : IAppConfigService
    {
        private readonly string _filePath;

        public AppConfigService()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _filePath = Path.Combine(folder, "appconfig.json");

            if (!File.Exists(_filePath))
            {
                Save(new AppConfig());
            }
        }

        public AppConfig Load()
        {
            if (!File.Exists(_filePath))
                return new AppConfig();

            var json = File.ReadAllText(_filePath);
            var config = JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
            if (config.Sensor.Racer1.Ip == "192.168.1.50" && config.Sensor.Racer1.TcpPort == 5000 &&
                (config.Sensor.Ip != "192.168.1.50" || config.Sensor.Port != 5000))
            {
                config.Sensor.Racer1.Ip = config.Sensor.Ip;
                config.Sensor.Racer1.TcpPort = config.Sensor.Port;
            }
            return config;
        }

        public void Save(AppConfig config)
        {
            var folder = Path.GetDirectoryName(_filePath)!;
            Directory.CreateDirectory(folder);

            var json = JsonSerializer.Serialize(config, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(_filePath, json);
        }
    }
}
