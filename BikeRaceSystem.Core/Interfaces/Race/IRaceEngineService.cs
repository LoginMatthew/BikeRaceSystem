namespace BikeRaceSystem.Core.Interfaces.Race
{
public interface IRaceEngineService
{
    event Action<double, double, double, double, double, bool, bool> ProgressChanged;
    event Action<Exception>? Failed;
    void Start(IRaceService raceService, int targetDistance);
    void Stop();
}

}
