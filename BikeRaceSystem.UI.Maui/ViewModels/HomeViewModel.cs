using BikeRaceSystem.Core.Interfaces.Localization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BikeRaceSystem.UI.Maui.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly ILocalizationService _loc;

        public string StartLabel => _loc["StartCompetition"];
        public string StatisticsLabel => _loc["Statistics"];
        public string SettingsLabel => _loc["Settings"];


        public HomeViewModel(ILocalizationService loc)
        {
            _loc = loc;

            _loc.LanguageChanged += () =>
            {
                OnPropertyChanged(nameof(StartLabel));
                OnPropertyChanged(nameof(StatisticsLabel));
                OnPropertyChanged(nameof(SettingsLabel));
            };
        }

        [RelayCommand]
        private async Task NavigateRace()
        {
            await Shell.Current.GoToAsync("race");
        }

        [RelayCommand]
        private async Task NavigateStatistics()
        {
            await Shell.Current.GoToAsync("statistics");
        }

        [RelayCommand]
        private async Task NavigateSettings()
        {
            await Shell.Current.GoToAsync("settings");
        }
    }
}
