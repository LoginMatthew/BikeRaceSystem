using BikeRaceSystem.Core.Models;
using BikeRaceSystem.Services.Data;
using Microsoft.Data.Sqlite;

namespace BikeRaceSystem.Tests;

public sealed class RaceResultRepositoryTests
{
    /// <summary>SQLite integration test: saves a race and reads its participants and date entry back.</summary>
    [Fact]
    public async Task SaveRaceAsync_PersistsRaceAndParticipants()
    {
        using var folder = new TemporaryDirectory();
        var path = Path.Combine(folder.Path, "results.db");
        var database = new DatabaseService(path);
        var repository = new RaceResultRepository(path, database);
        var date = new DateTime(2026, 4, 12, 14, 30, 0);
        var race = new RaceResultModel
        {
            Date = date,
            RaceName = "Afternoon race",
            Distance = 250,
            Participants = new List<RaceParticipantResultModel>
            {
                new() { Name = "Alex", UniqueCode = "A-01", Time = 18.4, Place = 1 },
                new() { Name = "Sam", UniqueCode = "B-02", Time = 21.7, Place = 2 }
            }
        };

        var raceId = await repository.SaveRaceAsync(race);
        var racesOnDate = await repository.GetRacesByDateAsync(DateOnly.FromDateTime(date));
        var participants = await repository.GetParticipantsAsync(raceId);
        var participantByCode = await repository.GetByUniqueCodeAsync("B-02");

        var savedRace = Assert.Single(racesOnDate);
        Assert.Equal("Afternoon race", savedRace.RaceName);
        Assert.Equal(250, savedRace.Distance);
        Assert.Equal(new[] { "Alex", "Sam" }, participants.Select(person => person.Name));
        Assert.Equal(2, Assert.Single(participantByCode).Place);
    }

    /// <summary>Database integration test: reset points to a newly initialized, empty database file.</summary>
    [Fact]
    public async Task ResetDatabaseAsync_CreatesAnEmptyDatabaseWithSchema()
    {
        using var folder = new TemporaryDirectory();
        var oldPath = Path.Combine(folder.Path, "results.db");
        var database = new DatabaseService(oldPath);
        var oldRepository = new RaceResultRepository(oldPath, database);
        await oldRepository.SaveRaceAsync(new RaceResultModel
        {
            Date = DateTime.Now,
            RaceName = "Old race",
            Distance = 100
        });

        var newPath = await database.ResetDatabaseAsync();
        var newDatabase = new DatabaseService(newPath);
        var newRepository = new RaceResultRepository(newPath, newDatabase);

        Assert.NotEqual(oldPath, newPath);
        Assert.Empty(await newRepository.GetAllRacesAsync());
    }

    private sealed class TemporaryDirectory : IDisposable
    {
        public string Path { get; } = Directory.CreateTempSubdirectory("BikeRaceSystem.Tests-").FullName;

        public void Dispose()
        {
            SqliteConnection.ClearAllPools();
            if (Directory.Exists(Path))
                Directory.Delete(Path, recursive: true);
        }
    }
}
