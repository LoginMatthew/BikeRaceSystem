using BikeRaceSystem.Core.Interfaces.Config;
using BikeRaceSystem.Core.Interfaces.Race;
using BikeRaceSystem.Core.Models;
using System.Globalization;

namespace BikeRaceSystem.Services.Race;

public sealed class SensorRaceService : IRaceService
{
    private readonly IAppConfigService _configService;
    private readonly ISensorTransportFactory _transportFactory;
    private readonly object _speedLock = new();
    private readonly double[] _speeds = new double[2];
    private readonly List<Stream> _streams = new();
    private CancellationTokenSource? _readerCancellation;
    private Task[] _readerTasks = Array.Empty<Task>();

    public SensorRaceService(IAppConfigService configService, ISensorTransportFactory transportFactory)
    {
        _configService = configService;
        _transportFactory = transportFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await StopAsync();
        var config = _configService.Load().Sensor;
        _readerCancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        try
        {
            _streams.Add(await _transportFactory.ConnectAsync(config.Racer1, _readerCancellation.Token));
            _streams.Add(await _transportFactory.ConnectAsync(config.Racer2, _readerCancellation.Token));
            _readerTasks = _streams.Select((stream, index) => ReadSpeedsAsync(stream, index, _readerCancellation.Token)).ToArray();
        }
        catch
        {
            await StopAsync();
            throw;
        }
    }

    public double GetSpeed(int racerIndex)
    {
        var failedReader = _readerTasks.FirstOrDefault(task => task.IsFaulted);
        if (failedReader?.Exception is { } error)
            throw error.GetBaseException();
        lock (_speedLock)
            return _speeds[racerIndex];
    }

    public async Task StopAsync()
    {
        _readerCancellation?.Cancel();
        foreach (var stream in _streams)
            await stream.DisposeAsync();
        _streams.Clear();

        try { await Task.WhenAll(_readerTasks); }
        catch (OperationCanceledException) { }
        catch (ObjectDisposedException) { }
        catch (Exception) { }

        _readerTasks = Array.Empty<Task>();
        _readerCancellation?.Dispose();
        _readerCancellation = null;
        lock (_speedLock)
        {
            _speeds[0] = 0;
            _speeds[1] = 0;
        }
    }

    private async Task ReadSpeedsAsync(Stream stream, int racerIndex, CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(stream, leaveOpen: true);
        while (!cancellationToken.IsCancellationRequested)
        {
            var line = await reader.ReadLineAsync(cancellationToken);
            if (line is null)
                throw new EndOfStreamException($"The sensor stream for racer {racerIndex + 1} was closed.");
            if (!double.TryParse(line, NumberStyles.Float, CultureInfo.InvariantCulture, out var speed) || speed < 0)
                throw new FormatException($"Invalid speed value from racer {racerIndex + 1} sensor: '{line}'.");

            lock (_speedLock)
                _speeds[racerIndex] = speed;
        }
    }
}
