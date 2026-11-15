using BikeRaceSystem.Core.Interfaces.Race;

namespace BikeRaceSystem.Services.Race
{
    public class RandomRaceService : IRaceService
    {
        private readonly Random _rnd = new();

        public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public double GetSpeed(int racerIndex)
        {
            return _rnd.NextDouble() * 100; // MaxSpeed (100km/h)
        }

        public Task StopAsync() => Task.CompletedTask;
    }

}
