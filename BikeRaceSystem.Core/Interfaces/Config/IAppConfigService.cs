using BikeRaceSystem.Core.Models;

namespace BikeRaceSystem.Core.Interfaces.Config
{
    public interface IAppConfigService
    {
        AppConfig Load();
        void Save(AppConfig config);
    }
}