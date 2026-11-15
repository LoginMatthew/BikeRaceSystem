using BikeRaceSystem.Core.Interfaces.Data;
using BikeRaceSystem.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BikeRaceSystem.UI.Maui.ViewModels.Race
{
    public partial class ExtendedStatisticsViewModel : ObservableObject
    {
        private readonly IRaceResultRepository _repo;

        [ObservableProperty]
        private List<RaceResultModel> todayRaces;

        public ExtendedStatisticsViewModel(IRaceResultRepository repo)
        {
            _repo = repo;
            LoadTodayRaces();
        }

        private async void LoadTodayRaces()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            TodayRaces = await _repo.GetRacesByDateAsync(today);
        }
    }

}
