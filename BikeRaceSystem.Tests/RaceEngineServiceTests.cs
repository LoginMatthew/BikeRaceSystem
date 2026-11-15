using BikeRaceSystem.Services.Race;

namespace BikeRaceSystem.Tests;

public sealed class RaceEngineServiceTests
{
    /// <summary>Race-engine test: verifies progress values, both finish flags, and service shutdown.</summary>
    [Fact]
    public async Task Start_ReportsProgressAndStopsWhenBothRacersReachTheTarget()
    {
        var engine = new RaceEngineService();
        var race = new TestRaceService(_ => 10);
        var finished = new TaskCompletionSource<(double Distance1, double Distance2, bool Racer1Finished, bool Racer2Finished)>(
            TaskCreationOptions.RunContinuationsAsynchronously);

        engine.ProgressChanged += (d1, d2, _, _, _, r1Finished, r2Finished) =>
        {
            if (r1Finished && r2Finished)
                finished.TrySetResult((d1, d2, r1Finished, r2Finished));
        };

        engine.Start(race, targetDistance: 1);

        var result = await finished.Task.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);
        await race.Stopped.Task.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);

        Assert.True(result.Distance1 >= 1);
        Assert.True(result.Distance2 >= 1);
        Assert.True(result.Racer1Finished);
        Assert.True(result.Racer2Finished);
    }

    /// <summary>Race-engine test: a source failure is reported and still followed by StopAsync.</summary>
    [Fact]
    public async Task Start_ReportsSourceFailureAndStopsTheService()
    {
        var engine = new RaceEngineService();
        var expected = new InvalidOperationException("Test source failure");
        var race = new TestRaceService(startError: expected);
        var failure = new TaskCompletionSource<Exception>(TaskCreationOptions.RunContinuationsAsynchronously);
        engine.Failed += exception => failure.TrySetResult(exception);

        engine.Start(race, targetDistance: 100);

        var actual = await failure.Task.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);
        await race.Stopped.Task.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);

        Assert.Same(expected, actual);
    }
}
