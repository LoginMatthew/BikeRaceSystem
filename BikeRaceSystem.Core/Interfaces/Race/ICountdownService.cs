namespace BikeRaceSystem.Core.Interfaces.Race
{
    public interface ICountdownService
    {
        event Action<int> Tick;
        event Action Finished;
        Task StartAsync(int seconds, CancellationToken token);
    }
}
