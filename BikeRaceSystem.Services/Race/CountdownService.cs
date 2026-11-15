using BikeRaceSystem.Core.Interfaces.Race;

namespace BikeRaceSystem.Services.Race
{
    public class CountdownService : ICountdownService
    {
        public event Action<int> Tick;
        public event Action Finished;

        public async Task StartAsync(int seconds, CancellationToken token)
        {
            int value = seconds;

            while (value > 0)
            {
                if (token.IsCancellationRequested)
                    return;

                Tick?.Invoke(value);
                await Task.Delay(1000, token);
                value--;
            }

            if (!token.IsCancellationRequested)
                Finished?.Invoke();

        }
    }
}
