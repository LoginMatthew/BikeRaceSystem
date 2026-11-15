using BikeRaceSystem.Core.Interfaces.Config;
using BikeRaceSystem.Core.Interfaces.Race;
using BikeRaceSystem.Services.Race;
using Microsoft.Extensions.DependencyInjection;

namespace BikeRaceSystem.Services.Factories
{
    public class RaceServiceFactory : IRaceServiceFactory
    {
        private readonly IServiceProvider _services;
        private readonly IConfigService _config;

        public RaceServiceFactory(IServiceProvider services, IConfigService config)
        {
            _services = services;
            _config = config;
        }

        public IRaceService GetService()
        {
            var mode = _config.Get("RaceMode") ?? "Random";

            return mode switch
            {
                "Sensor" => _services.GetRequiredService<SensorRaceService>(),
                _ => _services.GetRequiredService<RandomRaceService>()
            };
        }
    }
}
