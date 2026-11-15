
namespace BikeRaceSystem.Core.Interfaces.Data
{
    public interface IDatabaseService
    {
        Task InitializeAsync();
        Task<string> ResetDatabaseAsync();
    }
}
