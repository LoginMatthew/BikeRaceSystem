using BikeRaceSystem.Core.Interfaces.Data;
using BikeRaceSystem.Core.Interfaces.Localization;
using BikeRaceSystem.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BikeRaceSystem.UI.Maui.ViewModels.Race
{
    public partial class ExtendedStatisticsViewModel : ObservableObject
    {
        private readonly IRaceResultRepository _repo;
        private readonly ILocalizationService _loc;

        [ObservableProperty]
        private List<RaceResultModel> todayRaces = new();

        public string PageTitle => _loc["Statistics"];

        public ExtendedStatisticsViewModel(IRaceResultRepository repo, ILocalizationService loc)
        {
            _repo = repo;
            _loc = loc;
            _loc.LanguageChanged += () => OnPropertyChanged(nameof(PageTitle));
            LoadTodayRaces();
        }

        private async void LoadTodayRaces()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            TodayRaces = await _repo.GetRacesByDateAsync(today);
        }
    }
}
