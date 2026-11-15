using BikeRaceSystem.Core.Interfaces.Data;
using BikeRaceSystem.Core.Models;
using Microsoft.Data.Sqlite;

namespace BikeRaceSystem.Services.Data
{
    public class RaceResultRepository : IRaceResultRepository
    {
        private readonly string _dbPath;
        private readonly IDatabaseService _database;

        public RaceResultRepository(string dbPath, IDatabaseService db)
        {
            _dbPath = dbPath;
            _database = db;
        }

        public async Task<int> SaveRaceAsync(RaceResultModel race)
        {
            await _database.InitializeAsync();
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            await connection.OpenAsync();

            var raceCmd = connection.CreateCommand();
            raceCmd.CommandText =
            @"
                INSERT INTO RaceResults (Date, RaceName, Distance)
                VALUES ($date, $name, $distance);
                SELECT last_insert_rowid();
            ";

            raceCmd.Parameters.AddWithValue("$date", race.Date.ToString("yyyy-MM-dd HH:mm:ss"));
            raceCmd.Parameters.AddWithValue("$name", race.RaceName);
            raceCmd.Parameters.AddWithValue("$distance", race.Distance);

            var result = await raceCmd.ExecuteScalarAsync()
             ?? throw new InvalidOperationException("Failed to get inserted RaceResult ID.");

            var raceId = Convert.ToInt32(result);

            foreach (var p in race.Participants)
            {
                var pCmd = connection.CreateCommand();
                pCmd.CommandText =
                @"
                    INSERT INTO RaceParticipantResults
                    (RaceResultId, Name, UniqueCode, Time, Place)
                    VALUES ($raceId, $name, $code, $time, $place);
                ";

                pCmd.Parameters.AddWithValue("$raceId", raceId);
                pCmd.Parameters.AddWithValue("$name", p.Name);
                pCmd.Parameters.AddWithValue("$code", p.UniqueCode);
                pCmd.Parameters.AddWithValue("$time", p.Time);
                pCmd.Parameters.AddWithValue("$place", p.Place);

                await pCmd.ExecuteNonQueryAsync();
            }

            return (int)raceId;
        }

        public async Task<List<RaceResultModel>> GetAllRacesAsync()
        {
            await _database.InitializeAsync();
            var results = new List<RaceResultModel>();

            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            await connection.OpenAsync();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM RaceResults ORDER BY Date DESC";

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(new RaceResultModel
                {
                    Id = reader.GetInt32(0),
                    Date = DateTime.Parse(reader.GetString(1)),
                    RaceName = reader.GetString(2),
                    Distance = reader.GetInt32(3)
                });
            }

            return results;
        }

        public async Task<List<RaceParticipantResultModel>> GetParticipantsAsync(int raceId)
        {
            await _database.InitializeAsync();
            var list = new List<RaceParticipantResultModel>();

            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            await connection.OpenAsync();

            var cmd = connection.CreateCommand();
            cmd.CommandText =
            @"
                SELECT Id, Name, UniqueCode, Time, Place
                FROM RaceParticipantResults
                WHERE RaceResultId = $id
                ORDER BY Place ASC;
            ";

            cmd.Parameters.AddWithValue("$id", raceId);

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new RaceParticipantResultModel
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    UniqueCode = reader.GetString(2),
                    Time = reader.GetDouble(3),
                    Place = reader.GetInt32(4)
                });
            }

            return list;
        }

        public async Task<List<RaceParticipantResultModel>> GetByUniqueCodeAsync(string code)
        {
            await _database.InitializeAsync();
            var list = new List<RaceParticipantResultModel>();

            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            await connection.OpenAsync();

            var cmd = connection.CreateCommand();
            cmd.CommandText =
            @"
                SELECT Id, RaceResultId, Name, UniqueCode, Time, Place
                FROM RaceParticipantResults
                WHERE UniqueCode = $code
                ORDER BY Time ASC;
            ";

            cmd.Parameters.AddWithValue("$code", code);

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new RaceParticipantResultModel
                {
                    Id = reader.GetInt32(0),
                    RaceResultId = reader.GetInt32(1),
                    Name = reader.GetString(2),
                    UniqueCode = reader.GetString(3),
                    Time = reader.GetDouble(4),
                    Place = reader.GetInt32(5)
                });
            }

            return list;
        }

        public async Task<List<RaceResultModel>> GetRacesByDateAsync(DateOnly date)
        {
            await _database.InitializeAsync();
            var list = new List<RaceResultModel>();

            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            await connection.OpenAsync();

            var cmd = connection.CreateCommand();
            cmd.CommandText =
                    @"
                SELECT Id, Date, RaceName, Distance
                FROM RaceResults
                WHERE Date(date) = $date
                ORDER BY Date DESC;
            ";

            cmd.Parameters.AddWithValue("$date", date.ToString("yyyy-MM-dd"));

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new RaceResultModel
                {
                    Id = reader.GetInt32(0),
                    Date = DateTime.Parse(reader.GetString(1)),
                    RaceName = reader.GetString(2),
                    Distance = reader.GetInt32(3)
                });
            }

            return list;
        }
    }
}
