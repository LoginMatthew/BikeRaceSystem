using BikeRaceSystem.Services.Race;

namespace BikeRaceSystem.Tests;

public sealed class CountdownServiceTests
{
    /// <summary>Countdown test: one second produces one tick followed by one completion event.</summary>
    [Fact]
    public async Task StartAsync_EmitsTicksAndFinishesOnce()
    {
        var countdown = new CountdownService();
        var ticks = new List<int>();
        var finishedCount = 0;
        countdown.Tick += ticks.Add;
        countdown.Finished += () => finishedCount++;

        await countdown.StartAsync(1, TestContext.Current.CancellationToken)
            .WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);

        Assert.Equal(new[] { 1 }, ticks);
        Assert.Equal(1, finishedCount);
    }

    /// <summary>Countdown test: cancellation on the first tick prevents further ticks and completion.</summary>
    [Fact]
    public async Task StartAsync_DoesNotFinishAfterCancellation()
    {
        var countdown = new CountdownService();
        using var cancellation = new CancellationTokenSource();
        var ticks = 0;
        var finished = false;
        countdown.Tick += _ =>
        {
            ticks++;
            cancellation.Cancel();
        };
        countdown.Finished += () => finished = true;

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => countdown.StartAsync(5, cancellation.Token));

        Assert.Equal(1, ticks);
        Assert.False(finished);
    }
}
