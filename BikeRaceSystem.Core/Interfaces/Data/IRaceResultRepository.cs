using BikeRaceSystem.Core.Models;
namespace BikeRaceSystem.Core.Interfaces.Data
{
    public interface IRaceResultRepository
    {
        Task<int> SaveRaceAsync(RaceResultModel race);
        Task<List<RaceResultModel>> GetAllRacesAsync();
        Task<List<RaceParticipantResultModel>> GetParticipantsAsync(int raceId);
        Task<List<RaceParticipantResultModel>> GetByUniqueCodeAsync(string code);
        Task<List<RaceResultModel>> GetRacesByDateAsync(DateOnly date);
    }
}
