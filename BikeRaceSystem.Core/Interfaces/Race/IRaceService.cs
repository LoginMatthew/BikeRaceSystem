namespace BikeRaceSystem.Core.Interfaces.Race
{
    public interface IRaceService
    {
        Task StartAsync(CancellationToken cancellationToken);
        double GetSpeed(int racerIndex);
        Task StopAsync();
    }
}
