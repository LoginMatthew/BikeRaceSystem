using BikeRaceSystem.Core.Interfaces.Config;
using System.Text.Json;

namespace BikeRaceSystem.Services.Config
{
    public class ConfigService : IConfigService
    {
        private readonly string _filePath;
        private Dictionary<string, string> _store = new();

        public ConfigService()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _filePath = Path.Combine(folder, "appsettings.json");
            Load();
        }

        public void Set(string key, string value)
        {
            _store[key] = value;
            Save();
        }

        public string? Get(string key)
        {
            return _store.TryGetValue(key, out var value)
                ? value
                : null;
        }

        private void Load()
        {
            if (!File.Exists(_filePath))
            {
                _store = new Dictionary<string, string>();
                Save();

                return;
            }

            var json = File.ReadAllText(_filePath);
            _store = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
        }

        private void Save()
        {
            var folder = Path.GetDirectoryName(_filePath)!;
            Directory.CreateDirectory(folder);

            var json = JsonSerializer.Serialize(_store, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(_filePath, json);
        }

    }
}
