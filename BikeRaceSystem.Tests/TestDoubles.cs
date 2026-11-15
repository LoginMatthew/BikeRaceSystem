using BikeRaceSystem.Core.Interfaces.Config;
using BikeRaceSystem.Core.Interfaces.Race;
using BikeRaceSystem.Core.Models;

namespace BikeRaceSystem.Tests;

internal sealed class TestConfigService(string? raceMode = null) : IConfigService
{
    public string? Get(string key) => key == "RaceMode" ? raceMode : null;
    public void Set(string key, string value) { }
}

internal sealed class TestAppConfigService : IAppConfigService
{
    public AppConfig Load() => new();
    public void Save(AppConfig config) { }
}

internal sealed class UnusedSensorTransportFactory : ISensorTransportFactory
{
    public Task<Stream> ConnectAsync(SensorEndpointConfig endpoint, CancellationToken cancellationToken) =>
        throw new NotSupportedException("This test does not open sensor transports.");
}

internal sealed class TestRaceService(Func<int, double>? speed = null, Exception? startError = null) : IRaceService
{
    public TaskCompletionSource Stopped { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

    public Task StartAsync(CancellationToken cancellationToken) => startError is null
        ? Task.CompletedTask
        : Task.FromException(startError);

    public double GetSpeed(int racerIndex) => speed?.Invoke(racerIndex) ?? 0;

    public Task StopAsync()
    {
        Stopped.TrySetResult();
        return Task.CompletedTask;
    }
}

