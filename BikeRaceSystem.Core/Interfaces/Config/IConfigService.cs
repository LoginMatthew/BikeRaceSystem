using BikeRaceSystem.Core.Models;

namespace BikeRaceSystem.Core.Interfaces.Config
{
    public interface IConfigService
    {
        void Set(string key, string value);
        string? Get(string key);
    }
}
