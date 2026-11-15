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
        public string AppNameLabel => _loc["AppTitle"];
        public string RaceSettingsLabel => _loc["RaceSettingsTitle"];
        public string ReadyLabel => _loc["HomeRaceTag"];
        public string WelcomeSubtitleLabel => _loc["HomeWelcomeSubtitle"];
        public string StatisticsDescriptionLabel => _loc["HomeStatisticsDescription"];
        public string SettingsDescriptionLabel => _loc["HomeSettingsDescription"];
        public string FooterLabel => _loc["HomeFooter"];
        public string WindowsEditionLabel => _loc["WindowsEdition"];


        public HomeViewModel(ILocalizationService loc)
        {
            _loc = loc;

            _loc.LanguageChanged += () =>
            {
                OnPropertyChanged(nameof(StartLabel));
                OnPropertyChanged(nameof(StatisticsLabel));
                OnPropertyChanged(nameof(SettingsLabel));
                OnPropertyChanged(nameof(AppNameLabel));
                OnPropertyChanged(nameof(RaceSettingsLabel));
                OnPropertyChanged(nameof(ReadyLabel));
                OnPropertyChanged(nameof(WelcomeSubtitleLabel));
                OnPropertyChanged(nameof(StatisticsDescriptionLabel));
                OnPropertyChanged(nameof(SettingsDescriptionLabel));
                OnPropertyChanged(nameof(FooterLabel));
                OnPropertyChanged(nameof(WindowsEditionLabel));
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
