using BikeRaceSystem.Core.Interfaces.Race;

namespace BikeRaceSystem.Services.Race
{
    public class RaceEngineService : IRaceEngineService
    {
        public event Action<double, double, double, double, double, bool, bool>? ProgressChanged;
        public event Action<Exception>? Failed;

        private CancellationTokenSource? _cts;
        private readonly SemaphoreSlim _runLock = new(1, 1);

        public void Start(IRaceService raceService, int targetDistance)
        {
            _cts?.Cancel();
            _cts?.Dispose();
            var currentCts = new CancellationTokenSource();
            _cts = currentCts;
            _ = Task.Run(() => RunQueuedAsync(raceService, targetDistance, currentCts.Token));
        }

        public void Stop()
        {
            _cts?.Cancel();
        }

        private async Task RunQueuedAsync(IRaceService raceService, int targetDistance, CancellationToken token)
        {
            try { await _runLock.WaitAsync(token); }
            catch (OperationCanceledException) { return; }
            try { await RunAsync(raceService, targetDistance, token); }
            finally { _runLock.Release(); }
        }

        private async Task RunAsync(IRaceService raceService, int targetDistance, CancellationToken token)
        {
            try
            {
                await raceService.StartAsync(token);
                await LoopAsync(raceService, targetDistance, token);
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested) { }
            catch (Exception ex)
            {
                Failed?.Invoke(ex);
            }
            finally
            {
                try { await raceService.StopAsync(); }
                catch (Exception ex) { Failed?.Invoke(ex); }
            }
        }

        private async Task LoopAsync(IRaceService raceService, int targetDistance, CancellationToken token)
        {
            double d1 = 0;
            double d2 = 0;
            double elapsed = 0;

            while (!token.IsCancellationRequested)
            {
                var s1 = raceService.GetSpeed(0);
                var s2 = raceService.GetSpeed(1);

                d1 += s1 * 0.2;
                d2 += s2 * 0.2;

                elapsed += 0.2;

                bool r1Finished = d1 >= targetDistance;
                bool r2Finished = d2 >= targetDistance;

                ProgressChanged?.Invoke(d1, d2, s1, s2, elapsed, r1Finished, r2Finished);

                if (r1Finished && r2Finished)
                    break;

                await Task.Delay(200);
            }
        }
    }
}
