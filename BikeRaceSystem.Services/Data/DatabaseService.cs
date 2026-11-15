using BikeRaceSystem.Core.Interfaces.Data;
using Microsoft.Data.Sqlite;

namespace BikeRaceSystem.Services.Data
{
    public class DatabaseService : IDatabaseService
    {
        private readonly string _dbPath;
        private readonly SemaphoreSlim _initializeLock = new(1, 1);
        private bool _isInitialized;

        public DatabaseService(string dbPath)
        {
            _dbPath = dbPath;
        }

        public async Task InitializeAsync()
        {
            if (_isInitialized)
                return;

            await _initializeLock.WaitAsync();
            try
            {
                if (_isInitialized)
                    return;

                EnsureFolder();
                using var connection = new SqliteConnection($"Data Source={_dbPath}");
                await connection.OpenAsync();
                await ExecuteSchemaCreationAsync(connection);
                _isInitialized = true;
            }
            finally
            {
                _initializeLock.Release();
            }
        }

        /// <summary>
        /// RESET DATABASE
        /// </summary>
        public async Task<string> ResetDatabaseAsync()
        {
            var newDbPath = Path.Combine(
                Path.GetDirectoryName(_dbPath)!,
                $"results_{Guid.NewGuid()}.db"
            );

            var newDb = new DatabaseService(newDbPath);
            await newDb.InitializeAsync();

            return newDbPath;
        }



        // PRIVATE HELPERS

        private void EnsureFolder()
        {
            var folder = Path.GetDirectoryName(_dbPath);

            if (string.IsNullOrWhiteSpace(folder))
                return;

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
        }

        private async Task ExecuteSchemaCreationAsync(SqliteConnection connection)
        {
            using var raceCmd = connection.CreateCommand();
            raceCmd.CommandText = RaceTableSql;
            await raceCmd.ExecuteNonQueryAsync();

            using var participantCmd = connection.CreateCommand();
            participantCmd.CommandText = ParticipantTableSql;
            await participantCmd.ExecuteNonQueryAsync();
        }

        // SQL CONSTANTS

        private const string RaceTableSql =
        @"
            CREATE TABLE IF NOT EXISTS RaceResults (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT NOT NULL,
                RaceName TEXT NOT NULL,
                Distance INTEGER NOT NULL
            );
        ";

        private const string ParticipantTableSql =
        @"
            CREATE TABLE IF NOT EXISTS RaceParticipantResults (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                RaceResultId INTEGER NOT NULL,
                Name TEXT NOT NULL,
                UniqueCode TEXT NOT NULL,
                Time REAL NOT NULL,
                Place INTEGER NOT NULL,
                FOREIGN KEY (RaceResultId) REFERENCES RaceResults(Id)
            );
        ";
    }
}
