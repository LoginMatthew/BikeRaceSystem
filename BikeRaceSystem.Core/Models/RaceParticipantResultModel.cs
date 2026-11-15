
namespace BikeRaceSystem.Core.Models
{
    public class RaceParticipantResultModel
    {
        public int Id { get; set; }
        public int RaceResultId { get; set; }
        /// <summary>
        /// there may be several with the same name
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// In seconds
        /// </summary>
        public double Time { get; set; }
        /// <summary>
        /// Competition ranking
        /// </summary>
        public int Place { get; set; }
        public string UniqueCode { get; set; } = string.Empty;
        public double AverageSpeed(double distance) => distance > 0 ? distance / Time : 0;
    }
}
