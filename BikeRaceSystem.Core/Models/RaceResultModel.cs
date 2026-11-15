
namespace BikeRaceSystem.Core.Models
{
    public class RaceResultModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string RaceName { get; set; } = string.Empty;
        public int Distance { get; set; }
        public List<RaceParticipantResultModel> Participants { get; set; } = new();
    }
}
